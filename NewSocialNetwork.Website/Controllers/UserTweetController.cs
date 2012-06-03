using System;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserTweetController : ApplicationController
    {
        public IFeedRepository feedRepo { private get; set; }
        public IUserCountRepository userCountRepo { private get; set; }
        public IUserTweetRepository userTweetRepo { private get; set; }

        public UserTweetController()
        {
            ViewBag.PageTitle = "NSN: UserTweet";
        }

        //
        // GET: /UserTweet/

        [HttpPost]
        public JsonResult Post(int uid, string inputText)
        {
            ResponseMessage msg = new ResponseMessage("UserPostTweet", RAction.ADD, RStatus.FAIL,
                "<p>Incomplete post.</p>");
            try
            {
                int timestamp = DateTimeUtils.UnixTimestamp;
                // Post tweet
                int tweetId = frontendService.PostUserTweet(uid, inputText, timestamp);
                // Send to feed
                UserTweet newTweet = userTweetRepo.FindById(tweetId);
                if (newTweet.FriendUser == null || newTweet.FriendUser.UserId == 0)
                {
                    feedRepo.Add(NSNType.USER_TWEET, tweetId, newTweet.User.UserId, 0, timestamp);
                }
                else
                {
                    feedRepo.Add(NSNType.USER_TWEET, tweetId, newTweet.User.UserId, newTweet.FriendUser.UserId, timestamp);
                    // Increase user count
                    frontendService.IncreaseCountOfCommentPending(newTweet.FriendUser.UserId);
                }
                msg.SetStatusAndMessage(RStatus.SUCCESS, "<p>Posted.</p>");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
