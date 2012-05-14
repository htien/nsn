using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IUserTweetRepository : IRepository<UserTweet>
    {
        int Add(User user, string content);
        UserTweet Get(int tweetId, int userId);
    }
}