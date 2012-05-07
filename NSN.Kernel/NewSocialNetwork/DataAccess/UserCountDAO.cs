using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class UserCountDAO : DAO<UserCount>, IUserCountRepository
    {
        public UserCountDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}