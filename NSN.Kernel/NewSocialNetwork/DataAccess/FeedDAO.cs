using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FeedDAO : DAO<Feed>, IFeedRepository
    {
        public FeedDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region IFeedRepository Members

        public IList<Feed> GetUserFeeds(int userId)
        {
            string hql = @"select f from Feed f where f.User.UserId = :userId";
            return this.Session().CreateQuery(hql)
                .SetInt32("userId", userId)
                .List<Feed>();
        }

        #endregion
    }
}