using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class UserTweetDAO : DAO<UserTweet>, IUserTweetRepository
    {
        public UserTweetDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}