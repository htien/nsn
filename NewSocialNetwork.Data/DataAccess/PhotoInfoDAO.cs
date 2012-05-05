using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoInfoDAO : DAO<PhotoInfo>, IPhotoInfoRepository
    {
        public PhotoInfoDAO() { }
    }
}