using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class UserTweetDAO : DAO<UserTweet>, IUserTweetRepository
    {
        public UserTweetDAO() { }
    }
}