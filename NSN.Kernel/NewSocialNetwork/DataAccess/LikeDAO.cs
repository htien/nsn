using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class LikeDAO : DAO<Like>, ILikeRepository
    {
        public LikeDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region ILikeRepository Members

        public long Add(string typeId, int itemId, int userId, int timestamp)
        {
            return Convert.ToInt64(this.Session().CreateSQLQuery(
                @"insert into [NSN.Like](TypeId, ItemId, UserId, [Timestamp])
                  values (:typeId, :itemId, :userId, :timestamp);
                  select scope_identity()")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
        }

        public bool Remove(string typeId, int itemId, int userId)
        {
            return Convert.ToInt32(this.Session().CreateSQLQuery(
                @"delete from [NSN.Like]
                  where TypeId = :typeId and ItemId = :itemId and UserId = :userId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .ExecuteUpdate()) > 0;
        }

        public bool Exists(string typeId, int itemId, int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(l.LikeId) from Like l
                  where l.TypeId = :typeId and l.ItemId = :itemId and l.User.UserId = :userId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .UniqueResult()) > 0;
        }

        public int GetTotalLike(string typeId, int itemId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(lc.User) from LikeCache lc where lc.Key.TypeId = :typeId and lc.Key.ItemId = :itemId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .UniqueResult());
        }

        #endregion
    }
}