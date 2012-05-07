using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendListDataDAO : DAO<FriendListData>, IFriendListDataRepository
    {
        public FriendListDataDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}