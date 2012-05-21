using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ILikeRepository : IRepository<Like>
    {
        long Add(string typeId, int itemId, int userId, int timestamp);
        bool Remove(string typeId, int itemId, int userId);
        bool Exists(string typeId, int itemId, int userId);
        int GetTotalLike(string typeId, int itemId);
    }
}