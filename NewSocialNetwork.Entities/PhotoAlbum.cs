using System.Collections.Generic;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("NSN.PhotoAlbum", "dbo", Lazy = true)]
    public class PhotoAlbum : ActiveRecordBase<PhotoAlbum>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "AlbumId")]
        public virtual int AlbumId { get; set; }

        public virtual int UserId { get; set; }

        public virtual int ProfileId { get; set; }

        [Property("Privacy", NotNull = true)]
        public virtual int Privacy { get; set; }

        [Property("PrivacyComment", NotNull = true)]
        public virtual int PrivacyComment { get; set; }

        [Property("Name", Length = 255, NotNull = true)]
        public virtual string Name { get; set; }

        [Property("Timestamp", NotNull = true)]
        public virtual int Timestamp { get; set; }

        [Property("TotalPhoto", NotNull = true)]
        public virtual int TotalPhoto { get; set; }

        [Property("TotalComment", NotNull = true)]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true)]
        public virtual int TotalLike { get; set; }

        public PhotoAlbum() { }
    }
}
