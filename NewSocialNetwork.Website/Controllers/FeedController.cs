using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using NSN.Kernel.Manager;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class FeedController : ApplicationController
    {
        //
        // GET: /Feed/

        public JsonResult Feeds()
        {
            IList<FeedItem> feedItems = frontendService.LoadFeedItems(0, 5);
            return Json(feedItems, JsonRequestBehavior.AllowGet);
        }

    }
}
