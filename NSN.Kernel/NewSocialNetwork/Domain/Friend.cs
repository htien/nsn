using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Friend]", "dbo", Lazy = true)]
    public class Friend : ActiveRecordValidationBase<Friend>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "FriendId")]
        public virtual int FriendId { get; set; }

        [BelongsTo("UserId", UniqueKey = "UQ_NSN.Friend_User", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("FriendUserId", UniqueKey = "UQ_NSN.Friend_User", NotNull = true)]
        public virtual User FriendUser { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        #endregion

        public Friend() { }
    }
}
