using System.Collections.Generic;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.EmotionPackage]", "dbo", Lazy = true)]
    public class EmotionPackage : ActiveRecordValidationBase<EmotionPackage>
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Assigned, "PackagePath", Length = 50)]
        public virtual string PackagePath { get; set; }

        [Property("PackageName", Unique = true, Length = 100, NotNull = true)]
        public virtual string PackageName { get; set; }

        [Property("IsActive", NotNull = true, Default = "1")]
        public virtual bool IsActive { get; set; }

        #endregion

        #region Relationship

        [HasMany(typeof(Emotion), ColumnKey = "PackagePath", Lazy = false)]
        public virtual IList<Emotion> Emotions { get; set; }

        #endregion

        public EmotionPackage() { }
    }
}
