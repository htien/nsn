using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        long Add(string typeId, int itemId, int userId, int ownerUserId, string commentText, string ipAddr, int timestamp);
        int Remove(int commentId);
        int GetTotalComment(string typeId, int itemId, int ownerUserId);
        IList<Comment> ListComments(string typeId, int itemId, int ownerUserId);
        IList<Comment> ListComments(string typeId, int itemId);
        IList<Comment> ListCommentsByPhotoAlbum(int albumId);
        IList<Comment> ListCommentsByPhoto(int photoId);
        IList<Comment> ListCommentsByUserTweet(int tweetId);
        bool IsCommentOfUser(int commentId, int userId);
    }
}