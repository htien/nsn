using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IUserCountRepository : IRepository<UserCount>
    {
        UserCount Get(int userId);
    }
}