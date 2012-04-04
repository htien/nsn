using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Link]", "dbo", Lazy = true)]
    public class Link : ActiveRecordBase<Link>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "LinkId")]
        public virtual int LinkId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User UserId { get; set; }

        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUserId { get; set; }

        [Property("Privacy", NotNull = true, Default = "0")]
        public virtual byte Privacy { get; set; }

        [Property("Url", Length = 255, NotNull = true)]
        public virtual string Url { get; set; }

        [Property("Image", Length = 255, NotNull = false)]
        public virtual string Image { get; set; }

        [Property("Title", Length = 255, NotNull = false)]
        public virtual string Title { get; set; }

        [Property("Description", Length = 255, NotNull = false)]
        public virtual string Description { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("TotalComment", NotNull = true, Default = "0")]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        public Link() { }
    }
}
