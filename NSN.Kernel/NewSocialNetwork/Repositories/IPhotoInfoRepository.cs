using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IPhotoInfoRepository : IRepository<PhotoInfo>
    {
        int Remove(int photoId);
    }
}