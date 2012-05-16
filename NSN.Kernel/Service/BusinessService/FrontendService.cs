using System;
using System.Collections.Generic;
using System.Web;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Common;
using NSN.Framework;
using NSN.Kernel;
using NSN.Kernel.Manager;
using NSN.Manager;
using NSN.Service.SSO;
using SaberLily.Security.Crypto;

namespace NSN.Service.BusinessService
{
    public class FrontendService : IBusinessService
    {
        public ILoginAuthenticator loginAuthenticator { private get; set; }
        public ISessionManager sessionManager { private get; set; }
        public INSNConfig config { private get; set; }
        public IUserRepository userRepo { private get; set; }
        public IUserGroupRepository userGroupRepo { private get; set; }
        public IFeedRepository feedRepo { private get; set; }
        public IUserTweetRepository userTweetRepo { private get; set; }

        public FrontendService() { }

        public User RegisterNewUser(string firstName, string lastName, byte gender,
            string regEmail, string regPassword, string confirmPassword,
            string birthday)
        {
            DateTime.Parse(birthday);
            UserGroup group = userGroupRepo.FindById(UserGroupLevel.RegisteredUser);
            User user = new User()
            {
                Email = regEmail.Trim(),
                Password = PasswordCryptor.Hash(regPassword, 690),
                FullName = firstName.Trim() + " " + lastName.Trim(),
                Gender = gender,
                Birthday = birthday.Trim(),
                UserGroup = group
            };
            userRepo.Create(user);
            return user;
        }

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

        public User GetUserProfile(string uid)
        {
            if (String.IsNullOrEmpty(uid))
                return null;
            else
                uid = uid.Trim();

            int userId = 0;
            bool isUsername = !Int32.TryParse(uid, out userId);

            User userLogged = sessionManager.GetUser();
            User userProfile = null;

            if (isUsername) // uid is username
            {
                if (!uid.Equals(userLogged.Username, StringComparison.OrdinalIgnoreCase))
                    userProfile = userRepo.GetUserByUsername(uid);
            }
            else // uid is userId
            {
                if (userId != userLogged.UserId)
                    userProfile = userRepo.FindById(userId);
            }
            userProfile = userProfile ?? userLogged;
            return userProfile;
        }

        public IList<FeedItem> LoadFeedItems(int start, int size)
        {
            if (start < 0)
                start = 0;
            if (size < 1)
                size = 5;
            User user = sessionManager.GetUser();
            IList<Feed> feeds = feedRepo.GetUserFeeds(user.UserId, start, size);
            FeedManager feedManager = new FeedManager();
            foreach (Feed feed in feeds)
            {
                IEntity entity = null;
                switch (feed.TypeId)
                {
                    case NSNType.USER_TWEET:
                        entity = userTweetRepo.Get(feed.ItemId, user.UserId);
                        break;
                    case NSNType.PHOTO:
                        break;
                }
                feedManager.AddFeedItem(feed, entity);
            }
            return feedManager.GetItems();
        }

        #region Static Method

        public static void RequireLoggedIn()
        {
            if (!NSNContext.Current.SessionManager.GetUserSession().IsLogged())
            {
                HttpContext.Current.Response.Redirect("~/auth");
            }
        }

        public static void RequireLoggedOut()
        {
            if (NSNContext.Current.SessionManager.GetUserSession().IsLogged())
            {
                HttpContext.Current.Response.RedirectToRoute("Root");
            }
        }

        #endregion
    }
}