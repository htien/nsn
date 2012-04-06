using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.User]", "dbo", Lazy = true)]
    public class User : ActiveRecordValidationBase<User>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "UserId")]
        public virtual int UserId { get; set; }

        [BelongsTo("UserGroupId", NotNull = true)]
        public virtual UserGroup UserGroup { get; set; }

        [Property("Email", Unique = true, Length = 255, NotNull = false)]
        public virtual string Email { get; set; }

        [Property("Username", Unique = true, Length = 100, NotNull = false)]
        public virtual string Username { get; set; }

        [Property("Passwd", Length = 1024, NotNull = false)]
        public virtual string Password { get; set; }

        [Property("FullName", Length = 100, NotNull = true)]
        public virtual string FullName { get; set; }

        [Property("Gender", NotNull = true, Default = "0")]
        public virtual byte Gender { get; set; }

        [Property("Birthday", Length = 10, NotNull = false)]
        public virtual string Birthday { get; set; }

        [BelongsTo("CountryIso", NotNull = false)]
        public virtual Country Country { get; set; }

        [Property("Joined", NotNull = true)]
        public virtual int Joined { get; set; }

        [Property("LastLogin", NotNull = true)]
        public virtual int LastLogin { get; set; }

        [Property("LastActivity", NotNull = true)]
        public virtual int LastActivity { get; set; }

        [Property("UserImage", Length = 75, NotNull = false)]
        public virtual string UserImage { get; set; }

        [Property("LastIpAddress", Length = 15, NotNull = false)]
        public virtual string LastIpAddress { get; set; }

        public User() { }
    }
}
