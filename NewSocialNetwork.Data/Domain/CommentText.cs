using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.CommentText]", "dbo", Lazy = true)]
    public class CommentText : ActiveRecordValidationBase<CommentText>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Foreign, "CommentId")]
        public virtual long CommentId { get; set; }

        [OneToOne]
        public virtual Comment Comment { get; set; }

        [Property("Text", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string Text { get; set; }

        [Property("TextParsed", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string TextParsed { get; set; }

        #endregion

        public CommentText() { }
    }
}
