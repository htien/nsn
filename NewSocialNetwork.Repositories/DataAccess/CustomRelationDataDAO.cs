using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CustomRelationDataDAO : DAO<CustomRelationData>, ICustomRelationDataRepository
    {
        public CustomRelationDataDAO() { }
    }
}