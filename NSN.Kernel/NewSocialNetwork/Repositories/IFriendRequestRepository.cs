using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFriendRequestRepository : IRepository<FriendRequest>
    {
        bool IsConfirmingFriendRequest(int userId, int friendUserId);
    }
}