using System.Collections;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFeedRepository : IRepository<Feed>
    {
        long Add(string typeId, int itemId, int userId, int parentUserId, int timestamp);
        int Remove(long feedId);
        int Remove(string typeId, int itemId);
        IList<Feed> ListStreamByUser(int userId, int start = 0, int size = 25);
        IList<Feed> ListNewestFeed(int userId, long lastFeedId, int start = 0, int size = 5);
        Feed GetFeed(long feedId);
        IList<Feed> ListFeedsByItem(string typeId, int itemId);
        IList<Feed> GetUserFeeds(int userId);
        IList<Feed> GetUserFeeds(int userId, bool onlyme);
        IList<Feed> GetUserFeeds(int userId, int start, int size);
        IList<Feed> GetUserFeeds(int userId, int start, int size, bool onlyme);
        bool IsItemOfUser(string typeId, int itemId, int userId);
    }
}