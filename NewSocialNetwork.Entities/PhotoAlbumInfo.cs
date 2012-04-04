

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.PhotoAlbumInfo]", "dbo", Lazy = true)]
    public class PhotoAlbumInfo : ActiveRecordBase<PhotoAlbumInfo>
    {
        [Property("AlbumId", Unique = true, NotNull = true)]
        public virtual int AlbumId { get; set; }

        [Property("Description", NotNull = true)]
        public virtual string Description { get; set; }
        public PhotoAlbumInfo() { }
    }
}
