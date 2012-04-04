

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.LikeCache]", "dbo", Lazy = true)]
    public class LikeCache : ActiveRecordBase<LikeCache>
    {
        [Property("TypeId", NotNull = true)]
        public virtual string TypeId { get; set; }

        public virtual int ItemId { get; set; }

        public virtual User UserId { get; set; }

        public LikeCache() { }
    }
}
