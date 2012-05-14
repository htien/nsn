using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;
using SaberLily.Utils;
using System;

namespace NewSocialNetwork.DataAccess
{
    public class UserTweetDAO : DAO<UserTweet>, IUserTweetRepository
    {
        public IFeedRepository feedRepo { private get; set; }

        public UserTweetDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region IUserTweetRepository Members

        /// <summary>
        /// Add new tweet of user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int Add(User user, string content)
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
            feedRepo.AddForUserTweet(tweetId, user.UserId, timestamp);
            return tweetId;
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