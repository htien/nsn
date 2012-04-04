using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Photo]", "dbo", Lazy = true)]
    public class Photo : ActiveRecordBase<Photo>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "PhotoId")]
        public virtual int PhotoId { get; set; }

        public virtual int AlbumId { get; set; }

        public virtual int UserId { get; set; }

        public virtual int FriendUserId { get; set; }

        [Property("Privacy", NotNull = true, Default = "0")]
        public virtual byte Privacy { get; set; }

        [Property("Title", Length = 255, NotNull = true)]
        public virtual string Title { get; set; }

        [Property("Image", Length = 255, NotNull = false)]
        public virtual string Image { get; set; }

        [Property("AllowComment", NotNull = true, Default = "true")]
        public virtual bool AllowComment { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("TotalComment", NotNull = true, Default = "0")]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        [Property("Ordering", NotNull = true, Default = "0")]
        public virtual int Ordering { get; set; }

        public Photo() { }
    }
}
