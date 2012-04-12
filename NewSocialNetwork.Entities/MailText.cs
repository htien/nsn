using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.MailText]", "dbo", Lazy = true)]
    public class MailText : ActiveRecordValidationBase<MailText>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Foreign, "MailId")]
        public virtual int MailId { get; set; }

        [OneToOne]
        public virtual Mail Mail { get; set; }

        [Property("Text", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string Text { get; set; }

        [Property("TextParsed", ColumnType = "StringClob", SqlType = "NTEXT", NotNull = false)]
        public virtual string TextParsed { get; set; }

        #endregion

        public MailText() { }
    }
}
