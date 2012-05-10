using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendDAO : DAO<Friend>, IFriendRepository
    {
        public FriendDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public System.Collections.Generic.IList<Friend> GetNotMutualFriends(int userId, int friendUserId)
        {
            string e = "and f.FriendUserId not in (select a.FriendUserId from Friend as a where a.UserId=1)";
            return this.Session().CreateSQLQuery(@" 
select f.FriendUserId from Friend as f where f.UserId=2")
           .List<Friend>();
        }
    }
}