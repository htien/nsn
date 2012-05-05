using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Emotion]", "dbo", Lazy = true)]
    public class Emotion : ActiveRecordValidationBase<Emotion>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "EmotionId")]
        public virtual short EmotionId { get; set; }

        [BelongsTo("PackagePath", NotNull = true)]
        public virtual EmotionPackage PackagePath { get; set; }

        [Property("Title", Length = 100, NotNull = true)]
        public virtual string Title { get; set; }

        [Property("Text", Length = 20, NotNull = true)]
        public virtual string Text { get; set; }

        [Property("Image", Length = 100, NotNull = true)]
        public virtual string Image { get; set; }

        [Property("Ordering", NotNull = true, Default = "0")]
        public virtual short Ordering { get; set; }

        #endregion

        public Emotion() { }
    }
}
