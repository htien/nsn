using System;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class FriendRequestDAO : DAO<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public IList<FriendRequest> List(int userId)
        {
            return List(userId, false);
        }

        public IList<FriendRequest> List(int userId, bool isIgnore)
        {
            return this.Session().CreateQuery(
                @"from FriendRequest fr where fr.FriendUser.UserId = :userId and fr.IsIgnore = :isIgnore")
                .SetInt32("userId", userId)
                .SetBoolean("isIgnore", isIgnore)
                .List<FriendRequest>();
        }

        public int Count(int userId)
        {
            return Count(userId, false);
        }

        public int Count(int userId, bool isIgnore)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(fr.RequestId) from FriendRequest fr
                  where fr.FriendUser.UserId = :userId and fr.IsIgnore = :isIgnore")
                .SetInt32("userId", userId)
                .SetBoolean("isIgnore", isIgnore)
                .UniqueResult());
        }

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