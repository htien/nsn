using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CustomRelationDAO : DAO<CustomRelation>, ICustomRelationRepository
    {
        public CustomRelationDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}