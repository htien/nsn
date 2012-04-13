using System.Collections.Generic;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.FriendList]", "dbo", Lazy = true)]
    public class FriendList : ActiveRecordValidationBase<FriendList>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "ListId")]
        public virtual int ListId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User UserId { get; set; }

        [Property("Name", Length = 100, NotNull = true)]
        public virtual string Name { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        #endregion

        #region Relationship

        [HasMany(typeof(FriendListData), ColumnKey = "ListId", Lazy = true)]
        public virtual IList<FriendListData> FriendListData { get; set; }

        #endregion

        public FriendList() { }
    }
}
