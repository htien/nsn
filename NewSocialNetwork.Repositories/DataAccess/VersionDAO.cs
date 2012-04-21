using NewSocialNetwork.Repositories;
using NSNEntities = NewSocialNetwork.Entities;

namespace NewSocialNetwork.DataAccess
{
    public class VersionDAO : DAO<NSNEntities.NSNVersion>, INSNVersionRepository
    {
        public VersionDAO() { }
    }
}