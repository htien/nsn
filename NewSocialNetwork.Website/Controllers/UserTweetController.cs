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
        public ActionResult Post(int uid, string inputText)
        {
            try
            {
                int timestamp = DateTimeUtils.UnixTimestamp;
                // Post tweet
                int tweetId = frontendService.PostUserTweet(uid, inputText, timestamp);
                // Send to feed
                UserTweet newTweet = userTweetRepo.FindById(tweetId);
                long newFeedId = 0;
                if (newTweet.FriendUser == null || newTweet.FriendUser.UserId == 0)
                {
                    newFeedId = feedRepo.Add(NSNType.USER_TWEET, tweetId, newTweet.User.UserId, 0, timestamp);
                }
                else
                {
                    newFeedId = feedRepo.Add(NSNType.USER_TWEET, tweetId, newTweet.User.UserId, newTweet.FriendUser.UserId, timestamp);
                    // Increase user count
                    frontendService.IncreaseCountOfCommentPending(newTweet.FriendUser.UserId);
                }
                if (newFeedId > 0)
                {
                    Feed newFeed = feedRepo.GetFeed(newFeedId);
                    ViewBag.NewFeed = newFeed;
                    ViewBag.NewPost = frontendService.FromFeed(newFeed);
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
