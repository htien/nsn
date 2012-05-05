using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Mail]", "dbo", Lazy = true)]
    public class Mail : ActiveRecordValidationBase<Mail>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "MailId")]
        public virtual int MailId { get; set; }

        /// <summary>
        /// Thuộc tính chỉ định mail này là dạng mail reply.
        /// </summary>
        [Property("ParentMailId", NotNull = true, Default = "0")]
        public virtual int ParentMailId { get; set; }

        /// <summary>
        /// Người gửi.
        /// </summary>
        [BelongsTo("OwnerUserId", NotNull = true)]
        public virtual User OwnerUser { get; set; }

        [BelongsTo("OwnerFolderId", NotNull = true)]
        public virtual MailFolder OwnerFolder { get; set; }

        [Property("OwnerTypeId", NotNull = true, Default = "0")]
        public virtual byte OwnerTypeId { get; set; }

        /// <summary>
        /// Người xem/người nhận.
        /// </summary>
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

        #endregion

        #region Relationship

        [OneToOne(Fetch = FetchEnum.Join)]
        public virtual MailText MailText { get; set; }

        #endregion

        public Mail() { }
    }
}
