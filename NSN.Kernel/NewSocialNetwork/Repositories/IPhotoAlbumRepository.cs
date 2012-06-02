using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IPhotoAlbumRepository : IRepository<PhotoAlbum>
    {
        IList<PhotoAlbum> GetPhotoAlbumByUser(int userId);
        IList<Photo> GetPhotoByAlbum(int albumId);
        int Remove(int albumId);
        int GetTotalPhotoAlbumByUser(int userId);
        int GetTotalFriendRequestPending(int userId);
        int GetTotalMessagePending(int userId);
        int GetTotalActivityPendingRelativeUser(int userId);
        IList<FriendList> GetAllFriendListByUser(int userId);
        IList<CustomRelation> GetRelationshipBetweenUsers(int userId, int withUserId);
        bool Exists(int albumId);
        bool HasPhotos(int albumId);
        bool HasPhotosByTimestamp(int albumId, int timestamp);
        bool IsAlbumOfUser(int userId, int albumId);
    }
}