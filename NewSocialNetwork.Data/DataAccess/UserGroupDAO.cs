using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class UserGroupDAO : DAO<UserGroup>, IUserGroupRepository
    {
        public UserGroupDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}