using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.PhotoAlbumInfo]", "dbo", Lazy = true)]
    public class PhotoAlbumInfo : ActiveRecordValidationBase<PhotoAlbumInfo>, IEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Foreign, "AlbumId")]
        public virtual int AlbumId { get; set; }

        [OneToOne]
        public virtual PhotoAlbum Album { get; set; }

        [Property("Description", NotNull = true)]
        public virtual string Description { get; set; }

        #endregion

        public PhotoAlbumInfo() { }
    }
}
