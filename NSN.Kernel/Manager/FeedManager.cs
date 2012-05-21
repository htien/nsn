using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NSN.Kernel.Manager
{
    /// <summary>
    /// Quản lý danh sách Feed.
    /// </summary>
    public class FeedManager
    {
        private IList<FeedItem> feedItems;

        public FeedManager()
        {
            this.feedItems = new List<FeedItem>();
        }

        public void AddFeedItem(Feed feed, IEntity entity)
        {
            FeedItem item = new FeedItem(feed, entity);
            this.feedItems.Add(item);
        }

        public IList<FeedItem> GetItems()
        {
            return this.feedItems;
        }

        public void ClearItems()
        {
            this.feedItems.Clear();
        }
    }
}