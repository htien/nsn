using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ILikeCacheRepository : IRepository<LikeCache>
    {
        void Add(string typeId, int itemId, int userId);
        int Remove(string typeId, int itemId);
        bool Remove(string typeId, int itemId, int userId);
        int TotalLike(string typeId, int itemId);
    }
}