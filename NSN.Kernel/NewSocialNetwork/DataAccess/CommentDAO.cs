using System;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CommentDAO : DAO<Comment>, ICommentRepository
    {
        public ICommentTextRepository commentTextRepo { private get; set; }

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

        public int Remove(int commentId)
        {
            commentTextRepo.Remove(commentId);
            return this.Session().CreateQuery(
                @"delete from Comment where CommentId = :commentId")
                .SetInt32("commentId", commentId)
                .ExecuteUpdate();
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

        public IList<Comment> ListComments(string typeId, int itemId, int userId)
        {
            return this.Session().CreateQuery(
                @"from Comment c inner join fetch c.CommentText
                  where c.TypeId = :typeId and c.ItemId = :itemId and c.User.UserId = :userId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .List<Comment>();
        }

        public IList<Comment> ListComments(string typeId, int itemId)
        {
            return this.Session().CreateQuery(
                @"from Comment c inner join fetch c.CommentText
                  where c.TypeId = :typeId and c.ItemId = :itemId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .List<Comment>();
        }

        public IList<Comment> ListCommentsByPhotoAlbum(int albumId)
        {
            return this.ListComments(NSNType.PHOTO_ALBUM, albumId);
        }

        public IList<Comment> ListCommentsByPhoto(int photoId)
        {
            return this.ListComments(NSNType.PHOTO, photoId);
        }

        public IList<Comment> ListCommentsByUserTweet(int tweetId)
        {
            return this.ListComments(NSNType.USER_TWEET, tweetId);
        }

        public bool IsCommentOfUser(int commentId, int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(CommentId) from Comment
                  where CommentId = :commentId and (OwnerUser.UserId = :userId or User.UserId = :userId)")
                .SetInt32("commentId", commentId)
                .SetInt32("userId", userId)
                .UniqueResult()) > 0;
        }

        #endregion
    }
}