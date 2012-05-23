using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendDAO : DAO<Friend>, IFriendRepository
    {
        public FriendDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        /// <summary>
        /// Kiểm tra friendUser có phải là bạn của user.
        /// </summary>
        /// <returns>true nếu là bạn hoặc ngược lại</returns>
        public bool IsFriend(int userId, int friendUserId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(f.FriendId) from Friend f
                  where f.User.UserId = :userId and f.FriendUser.UserId = :friendUserId")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .UniqueResult()) == 1;
        }
    }
}