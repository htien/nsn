using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
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
        private readonly object _AddSyncLock = new object();
        private readonly object _RemoveSyncLock = new object();

        public NSNConfig config { private get; set; }
        public IUserRepository userRepo { private get; set; }
        public ISessionRepository sessionRepo { private get; set; }

        private SessionManager() { }

        #region ISessionManager Members

        /// <summary>
        /// Register a new UserSession.
        /// </summary>
        /// <param name="userSession">The user session to add</param>
        public void Add(UserSession userSession)
        {
            lock (_AddSyncLock)
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
                        this._AnonymousSessions.Add(userSession.SessionId, userSession);
                    else
                    {
                        UserSession existing = this.GetExistingUserSession(userSession.User.UserId);
                        if (existing != null)
                        {
                            userSession.LastVisit = existing.LastVisit;
                            this.Remove(existing.SessionId);
                        }
                        else
                        {
                            Session session = this.sessionRepo.FindById(userSession.SessionId);
                            if (session != null && session.LastVisit != null)
                                userSession.LastVisit = session.LastVisit.Value;
                        }
                        this._LoggedSessions.Add(userSession.SessionId, userSession);
                    }
                }
            }
        }

        /// <summary>
        /// Remove an entry for the session map.
        /// </summary>
        /// <param name="sessionId">The session id to remove</param>
        public void Remove(string sessionId)
        {
            lock (_RemoveSyncLock)
            {
                if (this._LoggedSessions.ContainsKey(sessionId))
                    this._LoggedSessions.Remove(sessionId);
                else
                    this._AnonymousSessions.Remove(sessionId);
            }
        }

        /// <summary>
        /// Persist the user session to the database.
        /// </summary>
        /// <param name="sessionId">The id of the session to persist</param>
        public void StoreSession(string sessionId)
        {
            UserSession us = this.GetUserSession(sessionId);
            if (us != null && us.User.UserId != this.config.GetInt(CfgKeys.ANONYMOUS_USER_ID))
            {
                Session s = us.AsSession();
                s.LastVisit = s.LastAccess;
                this.sessionRepo.Create(s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public UserSession RefreshSession(HttpSessionState session)
        {
            bool isSSOAuthentication = CfgKeys.TYPE_SSO.Equals(this.config[CfgKeys.AUTHENTICATION_TYPE]);
            return null;
        }

        public UserSession GetUserSession()
        {
            return this.GetUserSession(HttpContext.Current.Session.SessionID);
        }

        public UserSession GetUserSession(string sessionId)
        {
            UserSession us = this._AnonymousSessions[sessionId];
            return us != null ? us : this._LoggedSessions[sessionId];
        }

        public UserSession GetExistingUserSession(int userId)
        {
            foreach (UserSession us in this._LoggedSessions.Values)
            {
                if (us.User.UserId == userId)
                    return us;
            }
            return null;
        }

        public User GetUser()
        {
            return this.GetUserSession().User;
        }

        public int TotalUsers()
        {
            return _AnonymousSessions.Count + _LoggedSessions.Count;
        }

        public int TotalLoggedUsers()
        {
            return _LoggedSessions.Count;
        }

        public int TotalAnonymousUsers()
        {
            return _AnonymousSessions.Count;
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