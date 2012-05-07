using NewSocialNetwork.Repositories;
using NSNEntities = NewSocialNetwork.Domain;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class VersionDAO : DAO<NSNEntities.NSNVersion>, INSNVersionRepository
    {
        public VersionDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}