using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumInfoDAO : DAO<PhotoAlbumInfo>, IPhotoAlbumInfoRepository
    {
        public PhotoAlbumInfoDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}