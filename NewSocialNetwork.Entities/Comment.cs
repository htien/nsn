using System.Collections.Generic;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Comment]", "dbo", Lazy = true)]
    public class Comment : ActiveRecordValidationBase<Comment>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "CommentId")]
        public virtual long CommentId { get; set; }

        [Property("TypeId", Length = 75, NotNull = true)]
        public virtual string TypeId { get; set; }

        [Property("ItemId", NotNull = true)]
        public virtual string Itemid { get; set; }

        /// <summary>
        /// Người dùng nhận được comment.
        /// </summary>
        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        /// <summary>
        /// Người dùng đã tạo ra comment.
        /// </summary>
        [BelongsTo("OwnerUserId", NotNull = true)]
        public virtual User OwnerUser { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("IpAddress", Length = 15, NotNull = true)]
        public virtual string IpAddress { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        #endregion

        #region Relationship

        [OneToOne(Fetch = FetchEnum.Join)]
        public virtual CommentText CommentText { get; set; }

        #endregion

        public Comment() { }
    }
}
