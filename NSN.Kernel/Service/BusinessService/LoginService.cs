using NewSocialNetwork.Domain;
using NSN.Service.SSO;

namespace NSN.Service.BusinessService
{
    public class LoginService : IBusinessService
    {
        public ILoginAuthenticator loginAuthenticator { private get; set; }

        public LoginService() { }

        public User Authenticate(string usernameOrEmail, string password)
        {
            return loginAuthenticator.AuthenticateUser(usernameOrEmail, password);
        }
    }
}