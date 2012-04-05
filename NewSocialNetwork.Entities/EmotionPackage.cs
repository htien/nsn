using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.EmotionPackage]", "dbo", Lazy = true)]
    public class EmotionPackage : ActiveRecordBase<EmotionPackage>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "PackagePath", Length = 50)]
        public virtual string PackagePath { get; set; }

        [Property("PackageName", Length = 100, NotNull = true)]
        public virtual string PackageName { get; set; }

        [Property("IsActive", NotNull = true, Default = "1")]
        public virtual bool IsActive { get; set; }

        public EmotionPackage() { }
    }
}
