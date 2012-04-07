using System;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.LikeCache]", "dbo", Lazy = true)]
    public class LikeCache : ActiveRecordValidationBase<LikeCache>
    {
        #region Properties

        [CompositeKey]
        public virtual LikeCachePK Key { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        #endregion

        public LikeCache() { }
    }

    [Serializable]
    public class LikeCachePK
    {
        [KeyProperty(Column = "TypeId", Length = 75)]
        public string TypeId { get; set; }

        [KeyProperty(Column = "ItemId")]
        public int ItemId { get; set; }

        [KeyProperty(Column = "UserId")]
        public int UserId { get; set; }

        public override int GetHashCode()
        {
            return TypeId.GetHashCode() ^ ItemId ^ UserId;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            LikeCachePK key = obj as LikeCachePK;
            if (key == null)
            {
                return false;
            }
            if (key.TypeId != TypeId || key.ItemId != ItemId || key.UserId == UserId)
            {
                return false;
            }
            return true;
        }
    }
}
