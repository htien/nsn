using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IPhotoAlbumRepository : IRepository<PhotoAlbum>
    {
        IList<PhotoAlbum> GetPhotoAlbumByUser(int userId);
        int GetTotalPhotoAlbumByUser(int userId);
        IList<Photo> GetPhotoByAlbum(int albumId);
        int GetTotalFriendRequestPending(int userId);
        int GetTotalMessagePending(int userId);
        int GetTotalActivityPendingRelativeUser(int userId);
        IList<FriendList> GetAllFriendListByUser(int userId);
        IList<CustomRelation> GetRelationshipBetweenUsers(int userId, int withUserId);
        bool IsAlbumOfUser(int userId, int albumId);
    }
}