using System;
using System.Web.Mvc;
using NewSocialNetwork.Website.Controllers.Helper;
using SaberLily.Utils;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class LikeController : ApplicationController
    {
        public IFeedRepository feedRepo { private get; set; }

        //
        // GET: /Like/

        public JsonResult Like(string where, long id)
        {
            ResponseMessage msg = new ResponseMessage("Like", RAction.ADD, RStatus.FAIL,
                "Cannot process for this like. Please try again.");
            try
            {
                int timestamp = DateTimeUtils.UnixTimestamp;
                Domain.User myUser = sessionManager.GetUser();
                long likeId = -1;
                switch (where)
                {
                    case "on_feed":
                        likeId = frontendService.LikeForFeed(id, myUser.UserId, timestamp);
                        break;
                    case "on_photo":
                        likeId = frontendService.LikeForPhoto(Convert.ToInt32(id), myUser.UserId, timestamp);
                        break;
                }
                if (likeId == -1)
                {
                    throw new Exception("Only once for like.");
                }
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Liked.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unlike(string where, long id)
        {
            ResponseMessage msg = new ResponseMessage("Like", RAction.ADD, RStatus.FAIL,
                "Cannot process for this like. Please try again.");
            try
            {
                Domain.User myUser = sessionManager.GetUser();
                switch (where)
                {
                    case "on_feed":
                        frontendService.UnlikeForFeed(id, myUser.UserId);
                        break;
                    case "on_photo":
                        frontendService.UnlikeForPhoto(Convert.ToInt32(id), myUser.UserId);
                        break;
                }
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Unliked.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TotalLike(string typeId, int itemId)
        {
            ResponseMessage msg = new ResponseMessage("TotalLike", RAction.TOTAL, RStatus.FAIL,
                "Error occurs when total the likes.");
            try
            {
                int total = frontendService.TotalLike(typeId, itemId);
                msg.SetStatusAndMessage(RStatus.SUCCESS, total.ToString());
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TotalLikeOnFeed(long feedId)
        {
            Feed feed = feedRepo.FindById(feedId);
            if (feed == null)
            {
                return null;
            }
            return TotalLike(feed.TypeId, feed.ItemId);
        }
    }
}
