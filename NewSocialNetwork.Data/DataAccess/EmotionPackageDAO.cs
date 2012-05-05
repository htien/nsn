using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class EmotionPackageDAO : DAO<EmotionPackage>, IEmotionPackageRepository
    {
        public EmotionPackageDAO() { }
    }
}