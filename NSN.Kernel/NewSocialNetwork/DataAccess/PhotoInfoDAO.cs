using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoInfoDAO : DAO<PhotoInfo>, IPhotoInfoRepository
    {
        public PhotoInfoDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}