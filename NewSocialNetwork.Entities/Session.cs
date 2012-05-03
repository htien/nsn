using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Session]", "dbo", Lazy = true)]
    public class Session : ActiveRecordValidationBase<Session>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Foreign, "UserId")]
        public virtual int UserId { get; set; }

        [Property("Ip", NotNull = false)]
        public virtual string Ip { get; set; }

        [Property("Start", NotNull = false)]
        public virtual int? Start { get; set; }

        [Property("LastAccess", NotNull = false)]
        public virtual int? LastAccess { get; set; }

        [Property("LastVisit", NotNull = false)]
        public virtual int? LastVisit { get; set; }

        #endregion

        #region Relationship

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        #endregion

        public Session() { }
    }
}
