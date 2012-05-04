using NewSocialNetwork.Entities;
using NewSocialNetwork.Website.Modules.SSO;

namespace NewSocialNetwork.Website.Services
{
    public class LoginService : IBusinessService
    {
        private ILoginAuthenticator loginAuthenticator;

        public LoginService(ILoginAuthenticator loginAuthenticator)
        {
            this.loginAuthenticator = loginAuthenticator;
        }

        public User Authenticate(string usernameOrEmail, string password)
        {
            return loginAuthenticator.authenticateUser(usernameOrEmail, password);
        }
    }
}