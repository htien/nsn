using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using NSN.Kernel.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    public class FeedController : ApplicationController
    {
        public FeedController()
        {
            ViewBag.PageTitle = "NSN: Posts";
        }

        //
        // GET: /Feed/

        public ActionResult Feeds()
        {
            Domain.User userProfile = ViewBag.UProfile;
            IList<FeedItem> feedItems = frontendService.LoadFeedItems(userProfile.UserId, 0, 20);
            ViewBag.FeedItems = feedItems;
            return View();
        }

    }
}
