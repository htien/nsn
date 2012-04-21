using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByEmail(string email);

        User GetUserByUsername(string username);

        string GetPasswordByEmail(string email);

        string GetPasswordByUsername(string username);
    }
}