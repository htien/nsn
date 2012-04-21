using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumInfoDAO : DAO<PhotoAlbumInfo>, IPhotoAlbumInfoRepository
    {
        public PhotoAlbumInfoDAO() { }
    }
}