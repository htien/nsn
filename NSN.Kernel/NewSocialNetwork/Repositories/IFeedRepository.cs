using System.Collections;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFeedRepository : IRepository<Feed>
    {
        long Add(string typeId, int itemId, int userId, int parentUserId, int timestamp);
        Feed GetFeed(long feedId);
        IList<Feed> GetUserFeeds(int userId);
        IList<Feed> GetUserFeeds(int userId, bool onlyme);
        IList<Feed> GetUserFeeds(int userId, int start, int size);
        IList<Feed> GetUserFeeds(int userId, int start, int size, bool onlyme);
    }
}