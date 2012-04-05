using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Version]", "dbo", Lazy = true)]
    public class Version : ActiveRecordBase<Version>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "VersionId", Length = 50)]
        public virtual string VersionId { get; set; }

        [Property("Ordering", NotNull = true, Default = "0")]
        public virtual short Ordering { get; set; }

        public Version() { }
    }
}
