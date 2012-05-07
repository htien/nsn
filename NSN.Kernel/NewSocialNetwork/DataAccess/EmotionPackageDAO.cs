using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class EmotionPackageDAO : DAO<EmotionPackage>, IEmotionPackageRepository
    {
        public EmotionPackageDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}