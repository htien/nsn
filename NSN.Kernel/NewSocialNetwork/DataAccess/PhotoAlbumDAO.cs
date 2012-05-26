using System;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoAlbumDAO : DAO<PhotoAlbum>, IPhotoAlbumRepository
    {
        public PhotoAlbumDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public IList<PhotoAlbum> GetPhotoAlbumByUser(int userId)
        {
            return this.Session().CreateQuery(@"from PhotoAlbum a where a.User.UserId = :userId")
                .SetInt32("userId", userId)
                .List<PhotoAlbum>();
        }

        public int GetTotalPhotoAlbumByUser(int userId)
        {
            return Convert.ToInt32(this.Session()
                .CreateQuery(@"select count(a.AlbumId) from PhotoAlbum a where a.User.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public IList<Photo> GetPhotoByAlbum(int albumId)
        {
            return this.Session().CreateQuery(
                @"select p from Photo p where p.Album.AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .List<Photo>();
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


        public bool IsAlbumOfUser(int userId, int albumId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(a.AlbumId) from PhotoAlbum a where a.AlbumId = :albumId and a.User.UserId = :userId")
                .SetInt32("albumId", albumId)
                .SetInt32("userId", userId)
                .UniqueResult()) > 0;
        }
    }
}