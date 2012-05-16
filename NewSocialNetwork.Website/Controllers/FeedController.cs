using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using NSN.Kernel.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    public class FeedController : ApplicationController
    {
        //
        // GET: /Feed/

        public ActionResult Feeds()
        {
            IList<FeedItem> feedItems = frontendService.LoadFeedItems(0, 20);
            ViewBag.FeedItems = feedItems;
            return View();
        }

    }
}
