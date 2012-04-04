using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CommentText]", "dbo")]
    public class CommentText : ActiveRecordBase<CommentText>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "CommentId")]
        public virtual long CommentId { get; set; }

        [Property("Text", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string Text { get; set; }

        [Property("TextParsed", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string TextParsed { get; set; }

        public CommentText() { }
    }
}
