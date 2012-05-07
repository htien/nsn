using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Version]", "dbo", Lazy = true)]
    public class NSNVersion : ActiveRecordBase<NSNVersion>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "VersionId", Length = 50)]
        public virtual string VersionId { get; set; }

        [Property("Ordering", NotNull = true, Default = "0")]
        public virtual short Ordering { get; set; }

        #endregion

        public NSNVersion() { }
    }
}
