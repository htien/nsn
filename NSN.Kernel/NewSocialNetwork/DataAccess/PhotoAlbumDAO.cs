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


        public int GetTotalFriendsByUser(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(f.FriendId) from Friend f where f.User.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }


        public IList<User> GetListFriendByUser(int userId)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f where f.User.UserId = :userId")
                .SetInt32("userId", userId)
                .List<User>();
        }


        public IList<Photo> GetPhotoByAlbum(int albumId)
        {
            return this.Session().CreateQuery(
                @"select p from Photo p where p.Album.AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .List<Photo>();
        }


        public IList<User> SearchFriendByName(string friendName, int userId)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f
                  where f.FriendUser.FullName like :name and f.User.UserId = :userId")
                .SetString("name", "%" + friendName + "%")
                .SetInt32("userId", userId)
                .List<User>();
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


        public IList<User> GetFriendInListByUser(int listId)
        {
            return this.Session().CreateQuery(
                @"select fld.FriendUser from FriendListData fld where fld.FriendList = :listId")
                .SetInt32("listId", listId)
                .List<User>();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">ID đăng nhập của người dùng</param>
        /// <param name="friendUserId">Tìm kiếm những friend dựa trên ID này</param>
        /// <returns></returns>
        public IList<User> GetNotMutualFriend(int userId, int friendUserId)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f
                  where f.User.UserId = :friendUserId and
                        f.FriendUser.UserId <> :userId and
                        f.FriendUser not in (select ff.FriendUser from Friend ff
                                             where ff.User.UserId = :userId)")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .List<User>();
        }


        public IList<User> GetMutualFriend(int userId, int friendUserId)
        {
            return this.Session().CreateQuery(
                @"select f.FriendUser from Friend f
                  where f.User.UserId = :friendUserId and
                        f.FriendUser.UserId <> :userId and
                        f.FriendUser in (select ff.FriendUser from Friend ff
                                         where ff.User.UserId=:userId)")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .List<User>();
        }
    }
}