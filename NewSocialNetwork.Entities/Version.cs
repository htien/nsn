

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Version]", "dbo", Lazy = true)]
    public class Version : ActiveRecordBase<Version>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "VersionId")]
        public virtual int VersionId { get; set; }

        [Property("Ordering", NotNull = true)]
        public virtual short Ordering { get; set; }

        public Version() { }
    }
}
