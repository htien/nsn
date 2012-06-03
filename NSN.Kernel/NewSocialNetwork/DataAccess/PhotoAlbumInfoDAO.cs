using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumInfoDAO : DAO<PhotoAlbumInfo>, IPhotoAlbumInfoRepository
    {
        public PhotoAlbumInfoDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public int Remove(int albumId)
        {
            return this.Session().CreateQuery(
                @"delete from PhotoAlbumInfo where AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .ExecuteUpdate();
        }
    }
}