

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.FriendRequest]", "dbo", Lazy = true)]
    public class FriendRequest : ActiveRecordBase<FriendRequest>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RequestId")]
        public virtual int RequestId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User UserId { get; set; }

        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUserId { get; set; }

        [BelongsTo("ListId", NotNull = true)]
        public virtual FriendList ListId { get; set; }

        [Property("IsSeen", NotNull = true)]
        public virtual bool IsSeen { get; set; }

        [Property("IsIgnore", NotNull = true)]
        public virtual bool IsIgnore { get; set; }

        [Property("Message", Length = 255, NotNull = false)]
        public virtual string Message { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        public FriendRequest() { }
    }
}
