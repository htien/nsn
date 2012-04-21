using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class UserGroupDAO : DAO<UserGroup>, IUserGroupRepository
    {
        public UserGroupDAO() { }
    }
}