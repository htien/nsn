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
        public int Add(int userId, int friendUserId, string content, int timestamp)
        {
            int tweetId = Convert.ToInt32(this.Session().CreateSQLQuery(
                    @"insert into [NSN.UserTweet] (UserId, FriendUserId, [Content], Timestamp)
                      values (:userId, :friendUserId, :content, :timestamp); select scope_identity()")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .SetString("content", content)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
            feedRepo.Add(NSNType.USER_TWEET, tweetId, userId, friendUserId, timestamp);
            return tweetId;
        }

        #endregion
    }
}