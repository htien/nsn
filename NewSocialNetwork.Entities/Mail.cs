using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Mail]", "dbo", Lazy = true)]
    public class Mail : ActiveRecordBase<Mail>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "MailId")]
        public virtual int MailId { get; set; }

        [Property("ParentMailId", NotNull = true, Default = "0")]
        public virtual int ParentMailId { get; set; }

        [BelongsTo("OwnerUserId", NotNull = true)]
        public virtual User OwnerUser { get; set; }

        [BelongsTo("OwnerFolderId", NotNull = true)]
        public virtual MailFolder OwnerFolder { get; set; }

        [Property("OwnerTypeId", NotNull = true, Default = "0")]
        public virtual byte OwnerTypeId { get; set; }

        [BelongsTo("ViewerUserId", NotNull = true)]
        public virtual User ViewerUser { get; set; }

        [BelongsTo("ViewerFolderId", NotNull = true)]
        public virtual MailFolder ViewerFolder { get; set; }

        [Property("ViewerTypeId", NotNull = true, Default = "0")]
        public virtual byte ViewerTypeId { get; set; }

        [Property("ViewerIsNew", NotNull = true, Default = "true")]
        public virtual bool ViewerIsNew { get; set; }

        [Property("Subject", Length = 255, NotNull = false)]
        public virtual string Subject { get; set; }

        [Property("Preview", Length = 255, NotNull = false)]
        public virtual string Preview { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        public Mail() { }
    }
}
