using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByEmail(string email);
        User GetUserByUsername(string username);
        string GetPasswordByEmail(string email);
        string GetPasswordByUsername(string username);
        bool IsExistUsername(string username);
        bool IsExistEmail(string email);
        IList<User> ListFriends(int userId);
        IList<User> ListFriendsInList(int listId);
        IList<User> ListNotMutualFriends(int userId, int friendUserId, int size = 5);
        IList<User> ListMutualFriends(int userId, int friendUserId, int size = 10);
        IList<User> SearchFriendByName(string friendName, int userId);
        int GetTotalFriendsByUser(int userId);
        IList<int> ListFriendUserIds(int userId);
    }
}