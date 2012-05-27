using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class UserTweetDAO : DAO<UserTweet>, IUserTweetRepository
    {
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
            return tweetId;
        }

        #endregion
    }
}