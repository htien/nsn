using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoDAO : DAO<Photo>, IPhotoRepository
    {
        public PhotoDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}