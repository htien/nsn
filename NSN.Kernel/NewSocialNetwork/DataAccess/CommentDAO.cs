using System;
using System.Collections.Generic;
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
                  values (:typeId, :itemId, :userId, :ownerUserId, :timestamp, :ipAddr);
                  select scope_identity()")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .SetInt32("ownerUserId", ownerUserId)
                .SetInt32("timestamp", timestamp)
                .SetString("ipAddr", ipAddr)
                .UniqueResult());
        }

        public int GetTotalComment(string typeId, int itemId, int ownerUserId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(c.CommentId) from Comment c
                  where c.OwnerUser.UserId = :ownnerUserId and c.TypeId = :typeId and c.ItemId = :itemId")
                .SetInt32("ownnerUserId", ownerUserId)
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .UniqueResult());
        }

        public IList<Comment> GetCommentsByFeed(string typeId, int itemId, int ownerUserId)
        {
            return this.Session().CreateQuery(
                @"from Comment c inner join fetch c.CommentText
                  where c.TypeId = :typeId and c.ItemId = :itemId and c.OwnerUser.UserId = :ownerUserId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("ownerUserId", ownerUserId)
                .List<Comment>();
        }

        #endregion
    }
}