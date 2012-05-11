using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;
using SaberLily.Utils;

namespace NewSocialNetwork.DataAccess
{
    public class UserTweetDAO : DAO<UserTweet>, IUserTweetRepository
    {
        public UserTweetDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region IUserTweetRepository Members

        public void Add(User user, string content)
        {
            string sql = @"insert into [NSN.UserTweet] (UserId, FriendUserId, [Content], Timestamp)
                           values (:userId, :friendUserId, :content, :timestamp)";
            this.Session().CreateSQLQuery(sql)
                .SetInt32("userId", user.UserId)
                .SetInt32("friendUserId", 0)
                .SetString("content", content)
                .SetInt32("timestamp", DateTimeUtils.UnixTimestamp)
                .ExecuteUpdate();
        }

        #endregion
    }
}