using NewSocialNetwork.Entities;
using NewSocialNetwork.Website.Modules.SSO;

namespace NewSocialNetwork.Website.Services
{
    public class LoginService
    {
        private ILoginAuthenticator loginValidator;

        public LoginService(ILoginAuthenticator loginValidator)
        {
            this.loginValidator = loginValidator;
        }

        public User Authenticate(string usernameOrEmail, string password)
        {
            return loginValidator.ValidateUser(usernameOrEmail, password);
        }
    }
}