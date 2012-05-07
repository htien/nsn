using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendRequestDAO : DAO<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}