using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class EmotionPackageDAO : DAO<EmotionPackage>, EmotionPackageRepository
    {
        public EmotionPackageDAO() { }
    }
}