using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class FriendRequestDAO : DAO<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestDAO() { }
    }
}