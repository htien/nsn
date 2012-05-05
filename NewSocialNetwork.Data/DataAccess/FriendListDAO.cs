using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class FriendListDAO : DAO<FriendList>, IFriendListRepository
    {
        public FriendListDAO() { }
    }
}