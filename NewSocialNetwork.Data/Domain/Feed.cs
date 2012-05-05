using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Feed]", "dbo", Lazy = true)]
    public class Feed : ActiveRecordValidationBase<Feed>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "FeedId")]
        public virtual long FeedId { get; set; }

        [Property("Privacy", NotNull = true, Default = "0")]
        public virtual byte Privacy { get; set; }

        [Property("TypeId", Length = 75, NotNull = true)]
        public virtual string TypeId { get; set; }

        [Property("ItemId", NotNull = true)]
        public virtual int ItemId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("ParentUserId", NotNull = true)]
        public virtual User ParentUser { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        #endregion

        public Feed() { }
    }
}
