using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFriendRequestRepository : IRepository<FriendRequest>
    {
        IList<FriendRequest> List(int userId);
        IList<FriendRequest> List(int userId, bool isIgnore);
        int Count(int userId);
        int Count(int userId, bool isIgnore);
        bool IsConfirmingFriendRequest(int userId, int friendUserId);
    }
}