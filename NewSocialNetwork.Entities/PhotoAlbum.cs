using System.Collections.Generic;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.PhotoAlbum]", "dbo", Lazy = true)]
    public class PhotoAlbum : ActiveRecordValidationBase<PhotoAlbum>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "AlbumId")]
        public virtual int AlbumId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User User { get; set; }

        [Property("ProfifeId", NotNull = true, Default = "0")]
        public virtual int ProfileId { get; set; }

        [Property("Privacy", NotNull = true, Default = "0")]
        public virtual byte Privacy { get; set; }

        [Property("PrivacyComment", NotNull = true, Default = "0")]
        public virtual byte PrivacyComment { get; set; }

        [Property("Name", Length = 255, NotNull = true)]
        public virtual string Name { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("TotalPhoto", NotNull = true, Default = "0")]
        public virtual int TotalPhoto { get; set; }

        [Property("TotalComment", NotNull = true, Default = "0")]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        #endregion

        #region Relationship

        [OneToOne(Fetch = FetchEnum.Join)]
        public virtual PhotoAlbumInfo AlbumInfo { get; set; }

        [HasMany(typeof(Photo), ColumnKey = "AlbumId", Lazy = true)]
        public virtual IList<Photo> Photos { get; set; }

        #endregion

        public PhotoAlbum() { }
    }
}
