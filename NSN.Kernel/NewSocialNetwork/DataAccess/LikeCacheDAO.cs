using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class LikeCacheDAO : DAO<LikeCache>, ILikeCacheRepository
    {
        public LikeCacheDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }        
    }
}