using System;
using System.Web.Mvc;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class LikeController : ApplicationController
    {
        //
        // GET: /Like/

        public JsonResult ForFeed(long feedId)
        {
            ResponseMessage msg = new ResponseMessage("LikeForFeed", RAction.ADD, RStatus.FAIL,
                "Cannot process for this like. Please try again.");
            try
            {
                Domain.User myUser = sessionManager.GetUser();
                long likeId = frontendService.LikeForFeed(feedId, myUser.UserId);
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

        public JsonResult UnlikeForFeed(long feedId)
        {
            ResponseMessage msg = new ResponseMessage("LikeForFeed", RAction.ADD, RStatus.FAIL,
                "Cannot process for this like. Please try again.");
            try
            {
                Domain.User myUser = sessionManager.GetUser();
                frontendService.UnlikeForFeed(feedId, myUser.UserId);
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Unliked.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
