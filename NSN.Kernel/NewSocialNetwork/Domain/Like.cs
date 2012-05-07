using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Like]", "dbo", Lazy = true)]
    public class Like : ActiveRecordValidationBase<Like>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "LikeId")]
        public virtual int LikeId { get; set; }

        [Property("TypeId", Length = 75, NotNull = true)]
        public virtual string TypeId { get; set; }

        [Property("ItemId", NotNull = true)]
        public virtual int ItemId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        #endregion

        public Like() { }
    }
}
