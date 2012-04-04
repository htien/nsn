using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Comment]", "dbo")]
    public class Comment : ActiveRecordBase<Comment>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "CommentId")]
        public virtual long CommentId { get; set; }

        [Property("TypeId", Length = 75, NotNull = true)]
        public virtual string TypeId { get; set; }

        [Property("ItemId", NotNull = true)]
        public virtual string Itemid { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("OwnerUserId", NotNull = true)]
        public virtual User OwnerUser { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("IpAddress", Length = 15, NotNull = true)]
        public virtual string IpAddress { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        public Comment() { }
    }
}
