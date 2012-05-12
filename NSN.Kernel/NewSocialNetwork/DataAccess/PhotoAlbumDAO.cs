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
            return this.Session().CreateQuery("from PhotoAlbum a where a.User.UserId = :userId")
                .SetInt32("userId", userId)
                .List<PhotoAlbum>();
        }

        public int GetTotalPhotoAlbumByUser(int userId)
        {
            return Convert.ToInt32(this.Session()
                .CreateQuery("select count(a.AlbumId) from PhotoAlbum a where a.User.UserId = :userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }


        public int GetTotalFriendsByUser(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery("select count(f.FriendId) from Friend f where f.User.UserId=:userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }


        public IList<User> GetListFriendByUser(int userId)
        {
            string sql = "select u from User u inner join (select f.FriendUserId from Friends f where f.User.UserId=:userId) t on u.User.UserId=t.Friends.FriendUserId";
            string root = "select u from User u inner join u.Friends f where f.User.UserId=:userId";
            string trueSql = "select f.FriendUser from Friend f where f.User.UserId = :userId";
            return this.Session().CreateQuery(trueSql)
                .SetInt32("userId", userId)
                .List<User>();
        }


        public IList<Photo> GetPhotoByAlbum(int userId, int albumId)
        {
            return this.Session().CreateQuery("select p from Photo p where p.User.UserId=:userId and p.Album.AlbumId=:albumId ")
                .SetInt32("userId", userId)
                .SetInt32("albumId", albumId)
                .List<Photo>();
        }


        public IList<User> SearchFriendByName(string friendName, int userId)
        {
            return this.Session().CreateQuery("select f.FriendUser from Friend f where f.FriendUser.FullName like :name and f.User.UserId=:userId")
                .SetString("name", "%" + friendName + "%")
                .SetInt32("userId", userId)
                .List<User>();
        }


        public int GetTotalComment(int ownerUserId, string typeId, int itemId)
        {
            return Convert.ToInt32(this.Session().CreateQuery("select count(c.CommentId) from Comment c where c.OwnerUser.UserId=:ownnerUserId and c.TypeId=:typeId and c.ItemId=:itemId")
                .SetInt32("ownnerUserId", ownerUserId)
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .UniqueResult());
        }


        public IList<Comment> GetAllComment(int ownerUserId, string typeId, int itemId)
        {
            return this.Session().CreateQuery(
                    @"from Comment c inner join fetch c.CommentText
                      where c.TypeId = :typeId and c.ItemId = :itemId
                      and c.OwnerUser.UserId = :ownerUserId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("ownerUserId", ownerUserId)
                .List<Comment>();
        }


        public int GetTotalFriendRequestPending(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery("select uc.FriendRequest from UserCount uc where uc.UserId=:userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public int GetTotalMessagePending(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery("select uc.MailNew from UserCount uc where uc.UserId=:userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }

        public int GetTotalActivityPendingRelativeUser(int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery("select uc.CommentPending from UserCount uc where uc.UserId=:userId")
                .SetInt32("userId", userId)
                .UniqueResult());
        }


        public IList<FriendList> GetAllFriendListByUser(int userId)
        {
            return this.Session().CreateQuery("from FriendList fl where fl.UserId=:userId")
                .SetInt32("userId", userId)
                .List<FriendList>();
        }


        public IList<User> GetFriendInListByUser(int listId)
        {
            return this.Session().CreateQuery("select fld.FriendUser from FriendListData fld where fld.FriendList=:listId")
                .SetInt32("listId", listId)
                .List<User>();
        }


        public IList<CustomRelation> GetRelationshipBetweenUsers(int userId, int withUserId)
        {
            return this.Session().CreateQuery("select crd.Relation from CustomRelationData crd where crd.User=:userId and crd.WithUser=:withUserId")
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
            string trueSql = @"select f.FriendUser from Friend f
                where f.User.UserId = :friendUserId and
                      f.FriendUser.UserId <> :userId and
                      f.FriendUser not in (select ff.FriendUser from Friend ff
                                           where ff.User.UserId=:userId)";
            return this.Session().CreateQuery(trueSql)
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .List<User>();
        }


        public IList<User> GetMutualFriend(int userId, int friendUserId)
        {
            string trueSql = @"select f.FriendUser from Friend f
                where f.User.UserId = :friendUserId and
                      f.FriendUser.UserId <> :userId and
                      f.FriendUser in (select ff.FriendUser from Friend ff
                                           where ff.User.UserId=:userId)";
            return this.Session().CreateQuery(trueSql)
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .List<User>();
        }
    }
}