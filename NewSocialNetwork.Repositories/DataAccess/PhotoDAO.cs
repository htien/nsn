using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class PhotoDAO : DAO<Photo>, IPhotoRepository
    {
        public PhotoDAO() { }
    }
}