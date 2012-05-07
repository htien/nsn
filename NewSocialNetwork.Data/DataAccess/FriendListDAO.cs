using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendListDAO : DAO<FriendList>, IFriendListRepository
    {
        public FriendListDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}