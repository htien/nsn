using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CustomRelationDataDAO : DAO<CustomRelationData>, ICustomRelationDataRepository
    {
        public CustomRelationDataDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}