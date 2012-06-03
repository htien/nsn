using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class SearchController : ApplicationController
    {
        public IUserRepository userRepo { private get; set; }

        public SearchController()
        {
            ViewBag.PageTitle = "NSN: Search";
        }

        //
        // GET: /Search/

        public ActionResult Result(string type, string q, int friendUid = 0)
        {
            string viewName = null;
            dynamic results = null;
            switch (type)
            {
                case NSNSearchType.USERS:
                    results = SearchForUsers(q);
                    viewName = "UsersResult";
                    break;
                case NSNSearchType.FRIENDS:
                    results = SearchFriends();
                    viewName = "UsersResult";
                    break;
                case NSNSearchType.NOTMUTUALFRIENDS:
                    results = SearchNotMutualFriends(friendUid);
                    viewName = "UsersResult";
                    break;
            }
            if (viewName != null)
            {
                ViewBag.Results = results;
                ViewBag.SearchType = type;
                ViewBag.FriendUserId = friendUid;
                return View(viewName);
            }
            else
            {
                return View();
            }
        }

        private IList<User> SearchForUsers(string q)
        {
            if (String.IsNullOrWhiteSpace(q))
            {
                return null;
            }
            return userRepo.SearchUserByName(q.Trim());
        }

        private IList<User> SearchFriends()
        {
            User myUser = sessionManager.GetUser();
            return userRepo.ListFriends(myUser.UserId);
        }

        private IList<User> SearchNotMutualFriends(int friendUserId)
        {
            User myUser = sessionManager.GetUser();
            return userRepo.ListNotMutualFriends(myUser.UserId, friendUserId, 20);
        }
    }
}
