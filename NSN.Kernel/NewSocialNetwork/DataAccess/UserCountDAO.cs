using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;
using NHibernate.Criterion;

namespace NewSocialNetwork.DataAccess
{
    public class UserCountDAO : DAO<UserCount>, IUserCountRepository
    {
        public UserCountDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public UserCount Get(int userId)
        {
            return this.Session().CreateCriteria<UserCount>()
                .Add(Restrictions.Eq("UserId", userId))
                .SetLockMode(LockMode.Write)
                .UniqueResult<UserCount>();
        }
    }
}