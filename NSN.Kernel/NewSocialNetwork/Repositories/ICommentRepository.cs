using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        long Add(string typeId, int itemId, int userId, int ownerUserId, string commentText, string ipAddr, int timestamp);
        int GetTotalComment(int ownerUserId, string typeId, int itemId);
        IList<Comment> GetCommentsByFeed(int ownerUserId, string typeId, int itemId);
    }
}