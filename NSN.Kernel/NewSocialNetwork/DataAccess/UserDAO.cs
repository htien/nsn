using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class UserDAO : DAO<User>, IUserRepository
    {
        public UserDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region UserRepository Members

        public User GetUserByEmail(string email)
        {
            User[] users = ActiveRecordMediator<User>.FindAllByProperty("Email", email);
            return users.Length > 0 ? users[0] : null;
        }

        public User GetUserByUsername(string username)
        {
            User[] users = ActiveRecordMediator<User>.FindAllByProperty("Username", username);
            return users.Length > 0 ? users[0] : null;
        }

        public string GetPasswordByEmail(string email)
        {
            return this.GetUserByEmail(email).Password;
        }

        public string GetPasswordByUsername(string username)
        {
            return this.GetUserByUsername(username).Password;
        }

        public bool IsExistUsername(string username)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(Email) from User where Username = :username")
                .SetString("username", username)
                .UniqueResult()) > 0;
        }

        public bool IsExistEmail(string email)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(Email) from User where Email = :email")
                .SetString("email", email)
                .UniqueResult()) > 0;
        }

        public IList<User> ListFriends(int userId)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f where f.User.UserId = :userId")
                .SetInt32("userId", userId)
                .List<User>();
        }

        public IList<User> ListFriendsInList(int listId)
        {
            return this.Session().CreateQuery(
                @"select fld.FriendUser from FriendListData fld where fld.FriendList = :listId")
                .SetInt32("listId", listId)
                .List<User>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">ID đăng nhập của người dùng</param>
        /// <param name="friendUserId">Tìm kiếm những friend dựa trên ID này</param>
        /// <returns></returns>
        public IList<User> ListNotMutualFriends(int userId, int friendUserId, int size = 5)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f
                  where f.User.UserId = :friendUserId and
                        f.FriendUser.UserId <> :userId and
                        f.FriendUser not in (select ff.FriendUser from Friend ff
                                             where ff.User.UserId = :userId)
                  order by rand()")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .SetMaxResults(size)
                .List<User>();
        }

        public IList<User> ListMutualFriends(int userId, int friendUserId, int size = 10)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f
                  where f.User.UserId = :friendUserId and
                        f.FriendUser.UserId <> :userId and
                        f.FriendUser in (select ff.FriendUser from Friend ff
                                         where ff.User.UserId=:userId)")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .SetMaxResults(size)
                .List<User>();
        }

        public IList<User> SearchFriendByName(string friendName, int userId)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f
                  where f.FriendUser.FullName like :name and f.User.UserId = :userId")
                .SetString("name", "%" + friendName + "%")
                .SetInt32("userId", userId)
                .List<User>();
        }

        public int GetTotalFriendsByUser(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(f.FriendId) from Friend f where f.User.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public IList<int> ListFriendUserIds(int userId)
        {
            return this.Session().CreateSQLQuery(
                @"select f.FriendUserId from [NSN.Friend] f where f.UserId = :userId")
                .SetInt32("userId", userId)
                .List<int>();
        }

        #endregion
    }
}