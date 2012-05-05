using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class SessionDAO : DAO<Session>, ISessionRepository
    {
        public SessionDAO() { }
    }
}
