using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.UserGroup]", "dbo", Lazy = true)]
    public class UserGroup : ActiveRecordValidationBase<UserGroup>, IEntity
    {
        [PrimaryKey(PrimaryKeyType.Identity, "UserGroupId")]
        public virtual byte UserGroupId { get; set; }

        [Property("Title", Length = 255, NotNull = false)]
        public virtual string Title { get; set; }

        public UserGroup() { }
    }
}
