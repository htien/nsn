using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Common.Utilities;
using NSN.Common;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : ApplicationController
    {
        public IFeedRepository feedRepo { private get; set; }
        public IUserRepository userRepo { private get; set; }

        public HomeController()
        {
            ViewBag.PageTitle = "NSN: Stream";
        }

        //
        // GET: /Home/

        public ActionResult Stream()
        {
            User myUser = sessionManager.GetUser();
            IList<Feed> feeds = feedRepo.ListStreamByUser(myUser.UserId);
            ViewBag.Feeds = feeds;
            ViewBag.LastFeedId = feeds[0].FeedId;
            return View();
        }

        public ActionResult FindFriends()
        {
            return View();
        }
    }
}
