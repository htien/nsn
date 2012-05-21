using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IUserTweetRepository : IRepository<UserTweet>
    {
        int Add(int userId, int friendUserId, string content, int timestamp);
    }
}