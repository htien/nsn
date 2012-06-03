using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Kernel.Manager;

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
            IList<FeedItem> feedItems = frontendService.LoadStreamItems(myUser.UserId, 0, 40);
            if (feedItems != null && feedItems.Count > 0)
            {
                ViewBag.FeedItems = feedItems;
                ViewBag.LastFeedId = feedItems[0]._Feed.FeedId;
            }
            return View();
        }

        public ActionResult FindFriends()
        {
            return View();
        }
    }
}
