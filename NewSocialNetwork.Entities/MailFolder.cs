using System.Collections.Generic;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.MailFolder]", "dbo", Lazy = true)]
    public class MailFolder : ActiveRecordValidationBase<MailFolder>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "FolderId")]
        public virtual int FolderId { get; set; }

        [Property("Name", Length = 255, NotNull = true)]
        public virtual string Name { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        #endregion

        #region Relationship

        [HasMany(typeof(Mail), ColumnKey = "OwnerFolderId", Lazy = true)]
        public virtual IList<Mail> OwnerMails { get; set; }

        [HasMany(typeof(Mail), ColumnKey = "ViewerFolderId", Lazy = true)]
        public virtual IList<Mail> ViewerMails { get; set; }

        #endregion

        public MailFolder() { }
    }
}
