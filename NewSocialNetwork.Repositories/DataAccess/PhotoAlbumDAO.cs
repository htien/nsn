using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumDAO : DAO<PhotoAlbum>, PhotoAlbumRepository
    {
        public PhotoAlbumDAO() { }
    }
}