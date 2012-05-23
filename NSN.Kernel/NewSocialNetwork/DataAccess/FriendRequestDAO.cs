using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendRequestDAO : DAO<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public bool IsConfirmingFriendRequest(int userId, int friendUserId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(fr.RequestId) from FriendRequest fr
                  where fr.User.UserId = :userId and fr.FriendUser.UserId = :friendUserId")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .UniqueResult()) == 1;
        }
    }
}