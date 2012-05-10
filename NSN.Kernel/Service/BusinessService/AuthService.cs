using System.Web;
using NewSocialNetwork.Domain;
using NSN.Common;
using NSN.Framework;
using NSN.Kernel;
using NSN.Manager;
using NSN.Service.SSO;

namespace NSN.Service.BusinessService
{
    public class AuthService : IBusinessService
    {
        public ILoginAuthenticator loginAuthenticator { private get; set; }
        public ISessionManager sessionManager { private get; set; }
        public INSNConfig config { private get; set; }

        public AuthService() { }

        public User Login(string usernameOrEmail, string password)
        {
            User user = loginAuthenticator.AuthenticateUser(usernameOrEmail, password);
            if (user != null)
            {
                UserSession userSession = sessionManager.GetUserSession();
                userSession.User = user;
                userSession.BecomesLogged();

                // TODO: Check autologin

                sessionManager.Add(userSession);
            }
            return user;
        }

        public void Logout()
        {
            UserSession us = sessionManager.GetUserSession();
            sessionManager.StoreSession(us.SessionId);
            us.BecomesAnonymous(config.GetInt(Globals.ANONYMOUS_USER_ID));
            sessionManager.Remove(us.SessionId);
            sessionManager.Add(us);
        }

        public static void RequireLoggedIn()
        {
            if (!NSNContext.Current.SessionManager.GetUserSession().IsLogged())
            {
                HttpContext.Current.Response.Redirect("/auth");
            }
        }

        public static void RequireLoggedOut()
        {
            if (NSNContext.Current.SessionManager.GetUserSession().IsLogged())
            {
                HttpContext.Current.Response.RedirectToRoute("Root");
            }
        }
    }
}