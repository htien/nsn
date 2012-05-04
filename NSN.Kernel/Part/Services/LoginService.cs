using NewSocialNetwork.Entities;
using NSN.Security.SSO;

namespace NSN.Kernel.Part.Services
{
    public class LoginService : IBusinessService
    {
        public ILoginAuthenticator loginAuthenticator { private get; set; }

        public LoginService() { }

        public User Authenticate(string usernameOrEmail, string password)
        {
            return loginAuthenticator.authenticateUser(usernameOrEmail, password);
        }
    }
}