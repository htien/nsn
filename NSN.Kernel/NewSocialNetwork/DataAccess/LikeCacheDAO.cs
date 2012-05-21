using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class LikeCacheDAO : DAO<LikeCache>, ILikeCacheRepository
    {
        public LikeCacheDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region ILikeCacheRepository Members

        public void Add(string typeId, int itemId, int userId)
        {
            this.Session().CreateSQLQuery(
                @"insert into [NSN.LikeCache](TypeId, ItemId, UserId)
                  values (:typeId, :itemId, :userId)")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .ExecuteUpdate();
        }

        public bool Remove(string typeId, int itemId, int userId)
        {
            return Convert.ToInt32(this.Session().CreateSQLQuery(
                @"delete from [NSN.LikeCache]
                  where TypeId = :typeId and ItemId = :itemId and UserId = :userId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .ExecuteUpdate()) > 0;
        }

        #endregion
    }
}