﻿using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Feed]", "dbo", Lazy = true)]
    public class Feed : ActiveRecordBase<Feed>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "FeedId")]
        public virtual int FeedId { get; set; }

        [Property("Privacy", NotNull = true, Default = "0")]
        public virtual byte Privacy { get; set; }

        [Property("TypeId", Length = 75, NotNull = true)]
        public virtual string TypeId { get; set; }

        [Property("ItemId", NotNull = true)]
        public virtual int ItemId { get; set; }

        [Property("UserId", NotNull = true)]
        public virtual int UserId { get; set; }

        [Property("ParentUserId", NotNull = true)]
        public virtual int ParentUserId { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        public Feed() { }
    }
}