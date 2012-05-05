using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class LikeCacheDAO : DAO<LikeCache>, ILikeCacheRepository
    {
        public LikeCacheDAO() { }        
    }
}