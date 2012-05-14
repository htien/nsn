using System.Web.Script.Serialization;
using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.UserTweet]", "dbo", Lazy = true)]
    public class UserTweet : ActiveRecordValidationBase<UserTweet>, IEntity
    {
        [PrimaryKey(PrimaryKeyType.Identity, "TweetId")]
        public virtual int TweetId { get; set; }

        [ScriptIgnore]
        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [ScriptIgnore]
        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUser { get; set; }

        [Property("Privacy", NotNull = true, Default = "0")]
        public virtual byte Privacy { get; set; }

        [Property("Content", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string Content { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("TotalComment", NotNull = true, Default = "0")]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        public UserTweet() { }
    }
}
