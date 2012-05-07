using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FeedDAO : DAO<Feed>, IFeedRepository
    {
        public FeedDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}