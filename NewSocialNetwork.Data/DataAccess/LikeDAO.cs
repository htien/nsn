using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class LikeDAO : DAO<Like>, ILikeRepository
    {
        public LikeDAO() { }
    }
}