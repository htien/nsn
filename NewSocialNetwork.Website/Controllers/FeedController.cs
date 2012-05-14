using System.Collections;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Framework;
using NSN.Kernel.Manager;
using NSN.NewSocialNetwork.Domain;
using SaberLily.Web.Mvc;
using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class FeedController : ApplicationController
    {
        public IFeedRepository feedRepo { private get; set; }
        public IUserTweetRepository userTweetRepo { private get; set; }

        //
        // GET: /Feed/

        public JsonResult Posts()
        {
            Domain.User user = sessionManager.GetUser();
            IList<Feed> feeds = feedRepo.GetUserFeeds(user.UserId, 0, 5);
            FeedManager feedManager = new FeedManager();

            foreach (Feed feed in feeds)
            {
                IEntity entity = null;
                switch (feed.TypeId)
                {
                    case NSNType.USER_TWEET:
                        entity = userTweetRepo.Get(feed.ItemId, user.UserId);
                        break;
                    case NSNType.PHOTO:
                        break;
                }
                feedManager.AddFeedItem(feed, entity);
            }

            return Json(feedManager.GetItems(), JsonRequestBehavior.AllowGet);
        }

    }
}
