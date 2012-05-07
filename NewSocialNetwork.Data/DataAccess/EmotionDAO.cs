using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class EmotionDAO : DAO<Emotion>, IEmotionRepository
    {
        public EmotionDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}