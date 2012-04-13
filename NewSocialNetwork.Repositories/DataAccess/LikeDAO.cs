using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class LikeDAO : DAO<Like>, LikeRepository
    {
        public LikeDAO() { }
    }
}