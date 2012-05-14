using System.Collections;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFeedRepository : IRepository<Feed>
    {
        long AddForUserTweet(int itemId, int userId, int timestamp);
        IList<Feed> GetUserFeeds(int userId);
        IList<Feed> GetUserFeeds(int userId, int start, int size);
    }
}