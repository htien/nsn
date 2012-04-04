

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.UserCount]", "dbo", Lazy = true)]
    public class UserCount : ActiveRecordBase<UserCount>
    {
        [BelongsTo("UserId", NotNull = true)]
        public virtual User UserId { get; set; }

        [Property("MailNew", NotNull = true)]
        public virtual int MailNew { get; set; }

        [Property("CommentPending", NotNull = true)]
        public virtual int CommentPending { get; set; }

        [Property("FriendRequest", NotNull = true)]
        public virtual int FriendRequest { get; set; }

        public UserCount() { }
    }
}
