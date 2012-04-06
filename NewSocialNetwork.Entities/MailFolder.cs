using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.MailFolder]", "dbo", Lazy = true)]
    public class MailFolder : ActiveRecordValidationBase<MailFolder>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "FolderId")]
        public virtual int FolderId { get; set; }

        [Property("Name", Length = 255, NotNull = true)]
        public virtual string Name { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        public MailFolder() { }
    }
}
