using System;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Common;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class CommentController : ApplicationController
    {
        public ICommentRepository commentRepo { private get; set; }
        public IFeedRepository feedRepo { private get; set; }

        //
        // GET: /Comment/

        [HttpPost]
        public ActionResult AddSave(string where, long targetId, string c)
        {
            int timestamp = DateTimeUtils.UnixTimestamp;
            string ipAddr = Request.UserHostAddress;
            Domain.User myUser = sessionManager.GetUser();
            long commentId = 0;
            switch (where)
            {
                case "on_stream":
                case "on_feed":
                    commentId = frontendService.AddCommentOnFeed(targetId, myUser.UserId, timestamp, c, ipAddr);
                    break;
                case "on_photo":
                    commentId = frontendService.AddCommentOnPhoto(Convert.ToInt32(targetId), myUser.UserId, timestamp, c, ipAddr);
                    break;
            }
            Comment comment = commentRepo.FindById(commentId);
            if (comment != null)
            {
                User author = (comment.OwnerUser == null || comment.OwnerUser.UserId == 0) ? comment.User : comment.OwnerUser;
                ViewBag.Comment = comment;
                ViewBag.Author = author;
                ViewBag.AuthorProfileUrl = Url.RouteUrl("Profile", new { uid = NSN.Common.Globals.GetDisplayId(author) });
                ViewBag.AuthorImageUrl = Url.Content(Globals.RESOURCE_AVATARS + Globals.UserImage(author));

                switch (where)
                {
                    case "on_stream": return View("AddSaveOnStream");
                    case "on_feed": return View("AddSaveOnFeed");
                    case "on_photo": return View("AddSaveOnPhoto");
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult Remove(string typeId, int itemId, int commentId)
        {
            ResponseMessage msg = new ResponseMessage("Comment", RAction.REMOVE, RStatus.FAIL,
                "Cannot delete this item because it's not exists or you do not have permission.");
            try
            {
                if (frontendService.RemoveComment(typeId, itemId, commentId) > 0)
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

        [HttpPost]
        public JsonResult RemoveOnFeed(long feedId, int commentId)
        {
            Feed feed = feedRepo.FindById(feedId);
            if (feed == null)
            {
                throw new Exception("Cannot delete this item because it's not exists.");
            }
            return Remove(feed.TypeId, feed.ItemId, commentId);
        }
    }
}
