using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IPhotoAlbumRepository : IRepository<PhotoAlbum>
    {
        IList<PhotoAlbum> GetPhotoAlbumByUser(int userId);
        int GetTotalPhotoAlbumByUser(int userId);
        int GetTotalFriendsByUser(int userId);
        IList<User> GetListFriendByUser(int userId);
        IList<Photo> GetPhotoByAlbum(int userId, int albumId);
        IList<User> SearchFriendByName(string friendName, int userId);
        int GetTotalComment(int ownerUserId, string typeId, int itemId);
        IList<Comment> GetAllComment(int ownerUserId, string typeId, int itemId);
        int GetTotalFriendRequestPending(int userId);
        int GetTotalMessagePending(int userId);
        int GetTotalActivityPendingRelativeUser(int userId);
        IList<FriendList> GetAllFriendListByUser(int userId);
        IList<User> GetFriendInListByUser(int listId);
        IList<CustomRelation> GetRelationshipBetweenUsers(int userId, int withUserId);
        IList<User> GetNotMutualFriend(int userId, int friendUserId);
        IList<User> GetMutualFriend(int userId, int friendUserId);
    }
}