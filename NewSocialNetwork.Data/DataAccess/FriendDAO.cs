using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendDAO : DAO<Friend>, IFriendRepository
    {
        public FriendDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}