using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.LikeCache]", "dbo", Lazy = true)]
    public class LikeCache : ActiveRecordBase<LikeCache>
    {
        [Property("TypeId", NotNull = true)]
        public virtual string TypeId { get; set; }

        [Property("ItemId", NotNull = true)]
        public virtual int ItemId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        public LikeCache() { }
    }
}
