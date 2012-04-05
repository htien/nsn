﻿using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Friend]", "dbo", Lazy = true)]
    public class Friend : ActiveRecordBase<Friend>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "FriendId")]
        public virtual int FriendId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUser { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        public Friend() { }
    }
}