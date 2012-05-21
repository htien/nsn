using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ILikeCacheRepository : IRepository<LikeCache>
    {
        void Add(string typeId, int itemId, int userId);
        bool Remove(string typeId, int itemId, int userId);
    }
}