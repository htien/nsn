using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Photo]", "dbo", Lazy = true)]
    public class Photo : ActiveRecordValidationBase<Photo>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "PhotoId")]
        public virtual int PhotoId { get; set; }

        [BelongsTo("AlbumId", NotNull = true)]
        public virtual PhotoAlbum Album { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUser { get; set; }

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

        #endregion

        #region Relationship

        [OneToOne(Fetch = FetchEnum.Join)]
        public virtual PhotoInfo PhotoInfo { get; set; }

        #endregion

        public Photo() { }
    }
}
