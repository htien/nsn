using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.UserGroup]", "dbo")]
    public class UserGroup : ActiveRecordBase<UserGroup>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "UserGroupId")]
        public virtual byte UserGroupId { get; set; }

        [Property("Title", Length = 255, NotNull = false)]
        public virtual string Title { get; set; }

        public UserGroup() { }
    }
}
