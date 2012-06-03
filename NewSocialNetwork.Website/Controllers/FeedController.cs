using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Kernel.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    public class FeedController : ApplicationController
    {
        public IFeedRepository feedRepo { private get; set; }

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

        public ActionResult NewestFeeds(long feedId)
        {
            User myUser = sessionManager.GetUser();
            IList<FeedItem> newestItems = frontendService.LoadStreamNewestItems(myUser.UserId, feedId);
            if (newestItems == null || newestItems.Count == 0)
            {
                return View("Empty");
            }
            ViewBag.NewestFeedItems = newestItems;
            return View();
        }

        [HttpPost]
        public JsonResult Remove(long feedId)
        {
            ResponseMessage msg = new ResponseMessage("Feed", RAction.REMOVE, RStatus.FAIL,
                "An error occurs when removing this feed.");
            try
            {
                User user = sessionManager.GetUser();
                if (frontendService.RemoveFeed(feedId, user.UserId))
                {
                    msg.SetStatusAndMessage(RStatus.SUCCESS, "Removed.");
                }
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

    }
}
