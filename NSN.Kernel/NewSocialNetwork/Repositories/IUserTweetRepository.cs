using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IUserTweetRepository : IRepository<UserTweet>
    {
        void Add(User user, string content);
    }
}