using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Session]", "dbo", Lazy = true)]
    public class Session : ActiveRecordValidationBase<Session>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Assigned, "UserId")]
        public virtual int UserId { get; set; }

        [Property("Ip", Length = 15, NotNull = false)]
        public virtual string Ip { get; set; }

        [Property("Start", NotNull = false)]
        public virtual int? Start { get; set; }

        [Property("LastAccess", NotNull = false)]
        public virtual int? LastAccess { get; set; }

        [Property("LastVisit", NotNull = false)]
        public virtual int? LastVisit { get; set; }

        #endregion

        #region Relationship

        [BelongsTo("UserId", NotNull = true, Insert = false, Update = false)]
        public virtual User User { get; set; }

        #endregion

        public Session() { }
    }
}
