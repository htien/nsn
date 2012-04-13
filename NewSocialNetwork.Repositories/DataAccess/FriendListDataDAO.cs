using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class FriendListDataDAO : DAO<FriendListData>, FriendListDataRepository
    {
        public FriendListDataDAO() { }
    }
}