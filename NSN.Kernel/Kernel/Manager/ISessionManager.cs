using System.Web.SessionState;
using NewSocialNetwork.Entities;

namespace NSN.Kernel.Manager
{
    public interface ISessionManager
    {
        void Add(UserSession userSession);
        void Remove(string sessionId);
        void StoreSession(string sessionId);
        UserSession RefreshSession(HttpSessionState session);
        UserSession GetUserSession();
        UserSession GetUserSession(string sessionId);
        UserSession GetExistingUserSession(int userId);
        User GetUser();
        int TotalUsers();
        int TotalLoggedUsers();
        int TotalAnonymousUsers();
    }
}