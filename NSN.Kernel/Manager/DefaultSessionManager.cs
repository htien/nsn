using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using log4net;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Common;
using NSN.Framework;
using NSN.Kernel;
using SaberLily.Utils;

namespace NSN.Manager
{
    public class DefaultSessionManager : ISessionManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultSessionManager));
        private IDictionary<string, UserSession> _LoggedSessions = new Dictionary<string, UserSession>();
        private IDictionary<string, UserSession> _AnonymousSessions = new Dictionary<string, UserSession>();
        private readonly object _AddSyncLock = new object();
        private readonly object _RemoveSyncLock = new object();

        public INSNConfig config { private get; set; }
        public IUserRepository userRepo { private get; set; }
        public ISessionRepository sessionRepo { private get; set; }

        public DefaultSessionManager() { }

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
                    if (userSession.User.UserId == this.config.GetInt(Globals.ANONYMOUS_USER_ID))
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
                            try
                            {
                                Session session = this.sessionRepo.FindById(userSession.User.UserId);
                                if (session != null && session.LastVisit != null)
                                    userSession.LastVisit = session.LastVisit.Value;
                            }
                            catch// (ObjectNotFoundException e)
                            {
                                // LOG
                            }
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
            if (us != null && us.User.UserId != this.config.GetInt(Globals.ANONYMOUS_USER_ID))
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
        public UserSession RefreshSession(HttpContext context)
        {
            HttpSessionState session = context.Session;
            if (session == null || session.SessionID == null)
                return null;

            bool isSSOAuthentication = Globals.TYPE_SSO.Equals(this.config[Globals.AUTHENTICATION_TYPE]);
            context.Items[Globals.TYPE_SSO] = isSSOAuthentication;
            context.Items[Globals.SSO_LOGOUT_URL] = config[Globals.SSO_LOGOUT_URL];

            UserSession userSession = this.GetUserSession(session.SessionID);
            int anonymousUserId = this.config.GetInt(Globals.ANONYMOUS_USER_ID);

            if (userSession == null)
            {
                userSession = new UserSession()
                {
                    SessionId = session.SessionID,
                    CreationTime = DateTimeUtils.UnixTimestamp
                };
                if (true)
                {
                    if (isSSOAuthentication)
                    {
                        //CheckSSO
                    }
                    else
                    {
                        bool autoLoginEnabled = this.config.GetBoolean(Globals.AUTO_LOGIN_ENABLED);
                        bool autoLoginSuccess = autoLoginEnabled && this.CheckAutoLogin(userSession);
                        if (!autoLoginSuccess)
                        {
                            userSession.BecomesAnonymous(anonymousUserId);
                            userSession.User = this.userRepo.FindById(anonymousUserId);
                        }
                    }
                }
                this.Add(userSession);
                if (log.IsInfoEnabled)
                {
                    log.Info("Registered user userSession: " + session.SessionID);
                }
            }
            else if (isSSOAuthentication)
            {
                // TODO
            }
            else
            {
                // FIXME: Force a reload of the user instance, because if it's kept in the usersession,
                // changes made to the group (like permissions) won't be seen.
                userSession.User = this.userRepo.FindById(userSession.User.UserId);
            }

            userSession.ping();

            if (userSession.User == null || userSession.User.UserId == 0)
            {
                if (log.IsWarnEnabled)
                {
                    log.Warn("After userSession.Ping() -> userSession.GetUser returned null or user.UserId is zero. " +
                             "User is null? " + (userSession.User == null) + ". user.UserId is: " +
                             (userSession.User == null ? "GetUser() returned null" : userSession.User.UserId.ToString()) +
                             ". As we have a problem, will force the user to become anonymous. Session ID: " +
                             session.SessionID.ToString());
                }
                userSession.BecomesAnonymous(anonymousUserId);

                User anonymousUser = this.userRepo.FindById(userSession.User.UserId);
                if (anonymousUser == null)
                {
                    if (log.IsWarnEnabled)
                        log.Warn("Could not find the anonymous user in the database. Tried using id " + anonymousUserId);
                }
                else
                    userSession.User = anonymousUser;
            }

            // TODO: SetRoleManager()
            return userSession;
        }

        public UserSession GetUserSession()
        {
            return this.GetUserSession(HttpContext.Current.Session.SessionID);
        }

        public UserSession GetUserSession(string sessionId)
        {
            return this._AnonymousSessions.ContainsKey(sessionId)
                ? this._AnonymousSessions[sessionId]
                : this._LoggedSessions.ContainsKey(sessionId)
                ? this._LoggedSessions[sessionId]
                : null;
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
        /// Checks for user authentication using some SSO implementation.
        /// </summary>
        /// <param name="userSession"></param>
        /// <param name="session"></param>
        private void CheckSSO(UserSession userSession, HttpSessionState session)
        {
            try
            {
                // TODO
            }
            catch (Exception e)
            {
                throw new NSNException("Error while executing SSO actions: " + e.Message, e);
            }
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