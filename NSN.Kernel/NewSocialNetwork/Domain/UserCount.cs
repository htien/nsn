﻿using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.UserCount]", "dbo", Lazy = true)]
    public class UserCount : ActiveRecordValidationBase<UserCount>, IEntity
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "UserId")]
        public virtual int UserId { get; set; }

        [OneToOne]
        public virtual User User { get; set; }

        [Property("MailNew", NotNull = true, Default = "0")]
        public virtual int MailNew { get; set; }

        [Property("CommentPending", NotNull = true, Default = "0")]
        public virtual int CommentPending { get; set; }

        [Property("FriendRequest", NotNull = true, Default = "0")]
        public virtual int FriendRequest { get; set; }

        public UserCount() { }
    }
}
