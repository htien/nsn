using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.FriendListData]", "dbo", Lazy = true)]
    public class FriendListData : ActiveRecordBase<FriendListData>
    {
        [Property("ListId", UniqueKey = "UQ_NSN.FriendListData_List", NotNull = true)]
        public virtual int ListId { get; set; }

        [BelongsTo("FriendUserId", UniqueKey = "UQ_NSN.FriendListData_List", NotNull = true)]
        public virtual User FriendUserId { get; set; }

        [Property("Ordering", NotNull = true)]
        public virtual int Ordering { get; set; }

        public FriendListData() { }
    }
}
