using System;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumDAO : DAO<PhotoAlbum>, IPhotoAlbumRepository
    {
        public IPhotoAlbumInfoRepository photoAlbumRepo { private get; set; }

        public PhotoAlbumDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public IList<PhotoAlbum> GetPhotoAlbumByUser(int userId)
        {
            return this.Session().CreateQuery(@"from PhotoAlbum a where a.User.UserId = :userId")
                .SetInt32("userId", userId)
                .List<PhotoAlbum>();
        }

        public IList<Photo> GetPhotoByAlbum(int albumId)
        {
            return this.Session().CreateQuery(
                @"select p from Photo p where p.Album.AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .List<Photo>();
        }

        public int Remove(int albumId)
        {
            photoAlbumRepo.Remove(albumId);
            return this.Session().CreateQuery(
                @"delete from PhotoAlbum where AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .ExecuteUpdate();
        }

        public int GetTotalPhotoAlbumByUser(int userId)
        {
            return Convert.ToInt32(this.Session()
                .CreateQuery(@"select count(a.AlbumId) from PhotoAlbum a where a.User.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public int GetTotalFriendRequestPending(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select uc.FriendRequest from UserCount uc where uc.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public int GetTotalMessagePending(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select uc.MailNew from UserCount uc where uc.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public int GetTotalActivityPendingRelativeUser(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select uc.CommentPending from UserCount uc where uc.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }


        public IList<FriendList> GetAllFriendListByUser(int userId)
        {
            return this.Session().CreateQuery(@"from FriendList fl where fl.UserId = :userId")
                .SetInt32("userId", userId)
                .List<FriendList>();
        }

        public IList<CustomRelation> GetRelationshipBetweenUsers(int userId, int withUserId)
        {
            return this.Session().CreateQuery(
                @"select crd.Relation from CustomRelationData crd
                  where crd.User = :userId and crd.WithUser = :withUserId")
                .SetInt32("userId", userId)
                .SetInt32("withUserId", withUserId)
                .List<CustomRelation>();
        }

        public bool Exists(int albumId)
        {
            return Convert.ToUInt32(this.Session().CreateQuery(
                @"select count(AlbumId) from PhotoAlbum where AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .UniqueResult()) > 0;
        }

        public bool HasPhotos(int albumId)
        {
            return Convert.ToUInt32(this.Session().CreateQuery(
                @"select count(PhotoId) from Photo where Album.AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .UniqueResult()) > 0;
        }


        public bool HasPhotosByTimestamp(int albumId, int timestamp)
        {
            return Convert.ToUInt32(this.Session().CreateQuery(
                @"select count(PhotoId) from Photo
                  where Album.AlbumId = :albumId and Timestamp = :timestamp")
                .SetInt32("albumId", albumId)
                .SetInt32("timestamp", timestamp)
                .UniqueResult()) > 0;
        }

        public bool IsAlbumOfUser(int userId, int albumId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(AlbumId) from PhotoAlbum
                  where AlbumId = :albumId and User.UserId = :userId")
                .SetInt32("albumId", albumId)
                .SetInt32("userId", userId)
                .UniqueResult()) > 0;
        }
    }
}