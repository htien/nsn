using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CommentDAO : DAO<Comment>, ICommentRepository
    {
        public CommentDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region ICommentRepository Members

        public long Add(string typeId, int itemId, int userId, int ownerUserId, string commentText, string ipAddr, int timestamp)
        {
            return Convert.ToInt32(this.Session().CreateSQLQuery(
                @"insert into [NSN.Comment](TypeId, ItemId, UserId, OwnerUserId, [Timestamp], IpAddress)
                  values (:typeId, :itemId, :userId, :ownerUserId, :timestamp, :ipAddr); select scope_identity()")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .SetInt32("ownerUserId", ownerUserId)
                .SetInt32("timestamp", timestamp)
                .SetString("ipAddr", ipAddr)
                .UniqueResult());
        }

        #endregion
    }
}