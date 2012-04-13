using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class UserCountDAO : DAO<UserCount>, UserCountRepository
    {
        public UserCountDAO() { }
    }
}