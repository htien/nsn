using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class SessionDAO : DAO<Session>, ISessionRepository
    {
        public SessionDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}
