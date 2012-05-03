using System.Web;
using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Website.Core
{
    public interface ISessionManager
    {
        void Add(UserSession userSession);
        void Remove(string sessionId);
        void StoreSession(string sessionId);
        UserSession RefreshSession(HttpRequest request, HttpResponse response);
        UserSession GetUserSession(string sessionId);
        UserSession IsUserSession(int userId);
        User GetUser();
        int TotalUsers();
    }
}