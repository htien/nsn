using System.Collections.Generic;
using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Exceptions;
using NewSocialNetwork.Website.Main;

namespace NewSocialNetwork.Website.Core
{
    public class SessionManager : ISessionManager
    {
        private IDictionary<string, UserSession> _LoggedSessions = new Dictionary<string, UserSession>();
        private IDictionary<string, UserSession> _AnonymousSessions = new Dictionary<string, UserSession>();
        private readonly object _SyncLock = new object();

        public NSNConfig config { private get; set; }
        public IUserRepository userRepo { private get; set; }
        public ISessionRepository sessionRepo { private get; set; }

        private SessionManager() { }

        #region ISessionManager Members

        public void Add(UserSession userSession)
        {
            lock (_SyncLock)
            {
                string sessionId = userSession.SessionId;
                if (sessionId == null || sessionId.Length == 0)
                {
                    throw new NSNException("An UserSession instance must have a session ID.");
                }

                if (!userSession.IsBot())
                {
                    this.PreventDupplicates(userSession);
                    if (userSession.User.UserId == this.config.GetInt(CfgKeys.ANONYMOUS_USER_ID))
                    {
                        //this._AnonymousSessions
                    }
                }
            }
        }

        public void Remove(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        public void StoreSession(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        public UserSession RefreshSession(System.Web.HttpRequest request, System.Web.HttpResponse response)
        {
            throw new System.NotImplementedException();
        }

        public UserSession GetUserSession(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        public UserSession IsUserSession(int userId)
        {
            throw new System.NotImplementedException();
        }

        public User GetUser()
        {
            throw new System.NotImplementedException();
        }

        public int TotalUsers()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Checks user credentials / automatic login.
        /// </summary>
        /// <param name="userSession">The UserSession instance associated to the user's session</param>
        /// <returns>true if auto login was enabled and the user was successfully logged in</returns>
        private bool CheckAutoLogin(UserSession userSession)
        {
            return false;
        }

        /// <summary>
        /// Setup options and values for the user's session if authentication was ok.
        /// </summary>
        /// <param name="userSession">The UserSession instance of the user</param>
        /// <param name="user">The User instance of the authenticated user</param>
        private void ConfigureUserSession(UserSession userSession, User user)
        {
            userSession.User = user;
            userSession.BecomesLogged();
        }

        /// <summary>
        /// Make sure we'll not add a session that was already registered.
        /// </summary>
        /// <param name="userSession"></param>
        private void PreventDupplicates(UserSession userSession)
        {
            if (this.GetUserSession(userSession.SessionId) != null)
            {
                this.Remove(userSession.SessionId);
            }
        }

        #endregion
    }
}