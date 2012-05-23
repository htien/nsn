using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFriendRepository : IRepository<Friend>
    {
        bool IsFriend(int userId, int friendUserId);
    }
}