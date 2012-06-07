using System;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Common;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class LinkController : ApplicationController
    {
        public ILinkRepository linkRepo { get; set; }
        public IFeedRepository feedRepo { get; set; }

        public LinkController()
        {
            ViewBag.PageTitle = "NSN: Links";
        }

        //
        // GET: /Link/

        [HttpPost]
        public JsonResult GetRemote(string remoteUrl)
        {
            if (String.IsNullOrWhiteSpace(remoteUrl))
            {
                throw new Exception("Invalid remote url.");
            }
            LinkInfo remoteInfo = null;
            try
            {
                remoteInfo = Globals.GetLinkInfo(remoteUrl);
            }
            catch
            {
                return Json(null);
            }
            return Json(remoteInfo);
        }

        [HttpPost]
        public ActionResult Post(int uid, string inputText, string inputLink, string imageUrl, string title, string description)
        {
            try
            {
                int timestamp = DateTimeUtils.UnixTimestamp;
                // Post tweet
                int linkId = frontendService.PostLink(uid, inputText, inputLink, imageUrl, title, description, timestamp);
                // Send to feed
                Link newLink = linkRepo.FindById(linkId);
                long newFeedId = 0;
                if (newLink.FriendUser == null || newLink.FriendUser.UserId == 0)
                {
                    newFeedId = feedRepo.Add(NSNType.LINK, linkId, newLink.User.UserId, 0, timestamp);
                }
                else
                {
                    newFeedId = feedRepo.Add(NSNType.LINK, linkId, newLink.User.UserId, newLink.FriendUser.UserId, timestamp);
                    // Increase user count
                    frontendService.IncreaseCountOfCommentPending(newLink.FriendUser.UserId);
                }
                if (newFeedId > 0)
                {
                    Feed newFeed = feedRepo.GetFeed(newFeedId);
                    ViewBag.NewFeed = newFeed;
                    ViewBag.NewPost = newLink;
                }
            }
            catch
            {
                return View("Empty");
            }
            return View("PostResult");
        }

    }
}
