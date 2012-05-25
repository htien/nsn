using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        IList<Photo> GetPhotosByUser(int userId);
        IList<Photo> GetPhotosByTimestamp(int timestamp, int size);
        Photo GetFirstPhotoByAlbum(int albumId);
        int GetTotalPhoto(int albumId);
    }
}