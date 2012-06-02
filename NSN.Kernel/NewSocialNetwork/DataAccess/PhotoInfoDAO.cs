using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoInfoDAO : DAO<PhotoInfo>, IPhotoInfoRepository
    {
        public PhotoInfoDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public int Remove(int photoId)
        {
            return this.Session().CreateQuery(
                @"delete from PhotoInfo where PhotoId = :photoId")
                .SetInt32("photoId", photoId)
                .ExecuteUpdate();
        }
    }
}