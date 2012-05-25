using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class FriendController : ApplicationController
    {
        public IUserRepository userRepo { private get; set; }

        public FriendController()
        {
            ViewBag.PageTitle = "NSN: Friends";
        }

        //
        // GET: /Friend/

        public ActionResult List()
        {
            Domain.User userProfile = ViewBag.UProfile;
            int totalFriends = userRepo.GetTotalFriendsByUser(userProfile.UserId);
            IList<User> friends = userRepo.ListFriends(userProfile.UserId);

            ViewBag.totalFriends = totalFriends;
            ViewBag.listfriend = friends;
            return View();
        }

    }
}
