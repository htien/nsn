using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IPhotoAlbumInfoRepository : IRepository<PhotoAlbumInfo>
    {
        int Remove(int albumId);
    }
}