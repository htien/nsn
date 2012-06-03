using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ILinkRepository : IRepository<Link>
    {
        int Add(int userId, int friendUserId, string content, string url, string image, string title, string description, int timestamp);
    }
}