using Castle.ActiveRecord;
using NHibernate.SqlTypes;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.UserGroup]", "dbo", Lazy = true)]
    public class UserGroup : ActiveRecordValidationBase<UserGroup>, INSNEntity
    {
        [PrimaryKey(PrimaryKeyType.Identity, "UserGroupId")]
        public virtual byte UserGroupId { get; set; }

        [Property("Title", Length = 255, NotNull = false)]
        public virtual string Title { get; set; }

        public UserGroup() { }
    }
}
