﻿using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.UserTweet]", "dbo", Lazy = true)]
    public class UserTweet : ActiveRecordBase<UserTweet>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "TweetId")]
        public virtual int TweetId { get; set; }

        public virtual int UserId { get; set; }

        public virtual int FriendUserId { get; set; }

        [Property("Privacy", NotNull = true)]
        public virtual int Privacy { get; set; }

        [Property("Content", ColumnType="StringClob", SqlType="NTEXT", NotNull = false)]
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