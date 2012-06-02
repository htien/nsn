using System;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoDAO : DAO<Photo>, IPhotoRepository
    {
        public IPhotoInfoRepository photoInfoRepo { private get; set; }

        public PhotoDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public IList<Photo> GetPhotosByUser(int userId)
        {
            return this.Session()
                .CreateQuery("from Photo p where p.User.UserId = :userId")
                .SetInt32("userId", userId)
                .List<Photo>();
        }

        public IList<Photo> GetPhotosByTimestamp(int timestamp, int size)
        {
            return this.Session().CreateQuery(
                @"from Photo p where p.Timestamp = :timestamp order by p.Timestamp desc")
                .SetInt32("timestamp", timestamp)
                .SetMaxResults(size)
                .List<Photo>();
        }

        public Photo GetFirstPhotoByAlbum(int albumId)
        {
            return this.Session().CreateQuery(
                @"from Photo p
                  where p.Album = :albumId
                        and p.PhotoId = (select max(f.PhotoId) from Photo f where f.Album = :albumId group by f.Album)")
                .SetInt32("albumId", albumId)
                .UniqueResult<Photo>();
        }

        public int Remove(int photoId)
        {
            photoInfoRepo.Remove(photoId);
            return this.Session().CreateQuery(
                @"delete from Photo where PhotoId = :photoId")
                .SetInt32("photoId", photoId)
                .ExecuteUpdate();
        }

        public int GetTotalPhoto(int albumId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(p.PhotoId) from Photo p where p.Album.AlbumId = :albumId")
                .SetInt32("albumId", albumId)
                .UniqueResult());
        }

        public int TotalPhotoByTimestamp(int albumId, int timetamp)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(p.PhotoId) from Photo p where p.Album.AlbumId = :albumId and Timestamp = :timestamp")
                .SetInt32("albumId", albumId)
                .SetInt32("timestamp", timetamp)
                .UniqueResult());
        }

        public string ImageFileName(int photoId)
        {
            return Convert.ToString(this.Session().CreateQuery(
                @"select Image from Photo where PhotoId = :photoId")
                .SetInt32("photoId", photoId)
                .UniqueResult());
        }

        public bool IsPhotoOfUser(int photoId, int userId)
        {
            return Convert.ToInt32(this.Session().CreateQuery(
                @"select count(PhotoId) from Photo
                  where PhotoId = :photoId and (FriendUser.UserId = :userId or User.UserId = :userId)")
                .SetInt32("photoId", photoId)
                .SetInt32("userId", userId)
                .UniqueResult()) > 0;
        }
    }
}