using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.FriendRequest]", "dbo", Lazy = true)]
    public class FriendRequest : ActiveRecordValidationBase<FriendRequest>
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "RequestId")]
        public virtual int RequestId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUser { get; set; }

        [BelongsTo("ListId", NotNull = true)]
        public virtual FriendList List { get; set; }

        [Property("IsSeen", NotNull = true, Default = "false")]
        public virtual bool IsSeen { get; set; }

        [Property("IsIgnore", NotNull = true, Default = "false")]
        public virtual bool IsIgnore { get; set; }

        [Property("Message", Length = 255, NotNull = false)]
        public virtual string Message { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        #endregion

        public FriendRequest() { }
    }
}
