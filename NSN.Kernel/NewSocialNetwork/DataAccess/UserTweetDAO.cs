using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;
using SaberLily.Utils;
using System;

namespace NewSocialNetwork.DataAccess
{
    public class UserTweetDAO : DAO<UserTweet>, IUserTweetRepository
    {
        public UserTweetDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region IUserTweetRepository Members

        public void Add(User user, string content)
        {
            int timestamp = DateTimeUtils.UnixTimestamp;
            int tweetId = Convert.ToInt32(this.Session().CreateSQLQuery(
                    @"insert into [NSN.UserTweet] (UserId, FriendUserId, [Content], Timestamp)
                      values (:userId, :friendUserId, :content, :timestamp); select scope_identity()")
                .SetInt32("userId", user.UserId)
                .SetInt32("friendUserId", 0)
                .SetString("content", content)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
            long feedId = Convert.ToInt32(this.Session().CreateSQLQuery(
                    @"insert into [NSN.Feed] (TypeId, ItemId, UserId, Timestamp)
                      values (:typeId, :itemId, :userId, :timestamp); select scope_identity()")
                .SetString("typeId", "user_tweet")
                .SetInt32("itemId", tweetId)
                .SetInt32("userId", user.UserId)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
        }

        public UserTweet Get(int tweetId, int userId)
        {
            return this.Session().CreateQuery(
                @"from UserTweet tw where tw.TweetId = :tweetId and tw.User.UserId = :userId")
                .SetInt32("tweetId", tweetId)
                .SetInt32("userId", userId)
                .UniqueResult<UserTweet>();
        }

        #endregion
    }
}