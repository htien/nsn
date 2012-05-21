﻿using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Security.Application;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Common;
using NSN.Framework;
using NSN.Kernel;
using NSN.Kernel.Manager;
using NSN.Manager;
using NSN.Service.SSO;
using SaberLily.Security.Crypto;
using SaberLily.Utils;

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
        public ICommentRepository commentRepo { private get; set; }
        public ICommentTextRepository commentTextRepo { private get; set; }
        public ILikeRepository likeRepo { private get; set; }
        public ILikeCacheRepository likeCacheRepo { private get; set; }

        public FrontendService() { }

        public User RegisterNewUser(string firstName, string lastName, byte gender,
            string regEmail, string regPassword, string confirmPassword,
            string birthday)
        {
            DateTime birthDay = DateTime.Parse(birthday);
            UserGroup group = userGroupRepo.FindById(UserGroupLevel.RegisteredUser);
            User user = new User()
            {
                Email = regEmail.Trim(),
                Password = PasswordCryptor.Hash(regPassword, 690),
                FullName = firstName.Trim() + " " + lastName.Trim(),
                Gender = gender,
                Birthday = String.Format("{0}{1}{2}",
                                birthDay.Year,
                                birthDay.Month < 10 ? "0" + birthDay.Month.ToString() : birthDay.Month.ToString(),
                                birthDay.Day < 10 ? "0" + birthDay.Day.ToString() : birthDay.Day.ToString()),
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
                //if (userId != userLogged.UserId)
                    userProfile = userRepo.FindById(userId);
            }
            return userProfile;
        }

        public IList<FeedItem> LoadFeedItems(int userId, int start, int size)
        {
            if (start < 0)
                start = 0;
            if (size < 1)
                size = 5;
            IList<Feed> feeds = feedRepo.GetUserFeeds(userId, start, size);
            FeedManager feedManager = new FeedManager();
            foreach (Feed feed in feeds)
            {
                IEntity entity = null;
                switch (feed.TypeId)
                {
                    case NSNType.USER_TWEET:
                        
                        entity = userTweetRepo.FindById(feed.ItemId);
                        break;
                    case NSNType.PHOTO:
                        break;
                }
                feedManager.AddFeedItem(feed, entity);
            }
            return feedManager.GetItems();
        }

        public long AddComment(long feedId, int userId, string commentText)
        {
            Feed feed = feedRepo.FindById(feedId);
            string ipAddr = HttpContext.Current.Request.UserHostAddress;
            int timestamp = DateTimeUtils.UnixTimestamp;
            long commentId = commentRepo.Add(feed.TypeId, feed.ItemId, userId, feed.User.UserId, commentText, ipAddr, timestamp);
            string originCommentText = HttpUtility.UrlDecode(commentText, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            return commentTextRepo.Add(commentId, originCommentText, Encoder.HtmlEncode(originCommentText));
        }

        public long LikeForFeed(long feedId, int userId)
        {
            int timestamp = DateTimeUtils.UnixTimestamp;
            Feed feed = feedRepo.FindById(feedId);
            if (!likeRepo.Exists(feed.TypeId, feed.ItemId, userId))
            {
                long likeId = likeRepo.Add(feed.TypeId, feed.ItemId, userId, timestamp);
                likeCacheRepo.Add(feed.TypeId, feed.ItemId, userId);
                return likeId;
            }
            else
            {
                return -1;
            }
        }

        public void UnlikeForFeed(long feedId, int userId)
        {
            Feed feed = feedRepo.FindById(feedId);
            if (likeRepo.Exists(feed.TypeId, feed.ItemId, userId))
            {
                likeRepo.Remove(feed.TypeId, feed.ItemId, userId);
                likeCacheRepo.Remove(feed.TypeId, feed.ItemId, userId);
            }
        }

        public void PostUserTweet(int userId, string content)
        {
            int timestamp = DateTimeUtils.UnixTimestamp;
            User receiver = userRepo.FindById(userId);
            User sender = sessionManager.GetUser();
            content = content.Trim();

            if (String.IsNullOrEmpty(content))
            {
                throw new Exception("<p>Invalid content.</p>");
            }
            if (receiver.UserId == sender.UserId)
            {
                userTweetRepo.Add(sender.UserId, 0, content, timestamp);
            }
            else
            {
                userTweetRepo.Add(sender.UserId, receiver.UserId, content, timestamp);
            }
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