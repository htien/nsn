using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumInfoDAO : DAO<PhotoAlbumInfo>, PhotoAlbumInfoRepository
    {
        public PhotoAlbumInfoDAO() { }
    }
}