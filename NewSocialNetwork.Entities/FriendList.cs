

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.FriendList]", "dbo", Lazy = true)]
    public class FriendList : ActiveRecordBase<FriendList>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "ListId")]
        public virtual int ListId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User UserId { get; set; }

        [Property("Name", Length = 100, NotNull = true)]
        public virtual string Name { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        public FriendList() { }
    }
}
