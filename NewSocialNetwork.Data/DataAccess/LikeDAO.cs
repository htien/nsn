using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class LikeDAO : DAO<Like>, ILikeRepository
    {
        public LikeDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}