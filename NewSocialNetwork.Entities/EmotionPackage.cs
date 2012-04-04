using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.EmotionPackage]", Lazy = true)]
    public class EmotionPackage : ActiveRecordBase<EmotionPackage>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "PackagePath")]
        public virtual string PackagePath { get; set; }

        [Property("PackageName", Length = 100, NotNull = true)]
        public virtual string PackageName { get; set; }

        [Property("IsActive", NotNull = true)]
        public virtual bool IsActive { get; set; }

        public EmotionPackage() { }
    }
}
