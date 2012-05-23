using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Security.Application;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Common;
using NSN.Common.Utilities;
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
        #region Injects

        public ILoginAuthenticator loginAuthenticator { private get; set; }
        public ISessionManager sessionManager { private get; set; }
        public INSNConfig config { private get; set; }
        public IUserCountRepository userCountRepo { get; set; }
        public IUserRepository userRepo { private get; set; }
        public IUserGroupRepository userGroupRepo { private get; set; }
        public IFeedRepository feedRepo { private get; set; }
        public IFriendRepository friendRepo { private get; set; }
        public IFriendRequestRepository friendRequestRepo { get; set; }
        public IUserTweetRepository userTweetRepo { private get; set; }
        public ICommentRepository commentRepo { private get; set; }
        public ICommentTextRepository commentTextRepo { private get; set; }
        public ILikeRepository likeRepo { private get; set; }
        public ILikeCacheRepository likeCacheRepo { private get; set; }
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }

        #endregion

        public FrontendService() { }

        #region Register, Login, Logout

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

        #endregion

        #region Create default value

        public UserCount CreateUserCount(User user)
        {
            return userCountRepo.Create(new UserCount()
            {
                User = user,
                FriendRequest = 0,
                CommentPending = 0,
                MailNew = 0
            });
        }

        #endregion

        #region Generals

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

        public FriendRequest AddRequestFriend(int friendUserId, string message)
        {
            User friendUser = userRepo.FindById(friendUserId);
            if (friendUser == null)
            {
                throw new Exception("Cannot request not existed user.");
            }
            User myUser = sessionManager.GetUser();
            if (friendUserId == myUser.UserId
                || friendRepo.IsFriend(myUser.UserId, friendUserId)
                || friendRequestRepo.IsConfirmingFriendRequest(myUser.UserId, friendUserId))
            {
                throw new Exception("Error when processing your request friend.");
            }
            string originMessage = HttpUtility.UrlDecode(message.Trim(), System.Text.Encoding.GetEncoding("ISO-8859-1"));
            int timestamp = DateTimeUtils.UnixTimestamp;
            FriendRequest friendRequest = new FriendRequest()
            {
                User = myUser,
                FriendUser = friendUser,
                Message = Encoder.HtmlEncode(message),
                Timestamp = timestamp
            };
            return friendRequestRepo.Create(friendRequest);
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
                    case NSNType.PHOTO_ALBUM:
                        entity = photoAlbumRepo.FindById(feed.ItemId);
                        break;
                }
                feedManager.AddFeedItem(feed, entity);
            }
            return feedManager.GetItems();
        }

        public IList<ImageInfo> SaveImages(HttpFileCollectionBase imageCollection)
        {
            IList<ImageInfo> images = new List<ImageInfo>();
            foreach (string upload in imageCollection)
            {
                HttpPostedFileBase file = imageCollection[upload];
                try
                {
                    ImageInfo image = Globals.SaveImageInPlace(file, "/static/images/photos/");
                    images.Add(image);
                }
                catch
                {
                    continue;
                }
            }
            return images;
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
            string originContent = HttpUtility.UrlDecode(content.Trim(), System.Text.Encoding.GetEncoding("ISO-8859-1"));

            if (String.IsNullOrEmpty(originContent))
            {
                throw new Exception("<p>Invalid content.</p>");
            }
            if (receiver.UserId == sender.UserId)
            {
                userTweetRepo.Add(sender.UserId, 0, Encoder.HtmlEncode(originContent), timestamp);
            }
            else
            {
                userTweetRepo.Add(sender.UserId, receiver.UserId, Encoder.HtmlEncode(originContent), timestamp);
            }
        }

        public void IncreaseCountOfFriendRequest(int userId)
        {
            UserCount userCount = userCountRepo.FindById(userId);
            userCount.FriendRequest++;
            userCountRepo.Save(userCount);
        }

        public void IncreaseCountOfCommentPending(int userId)
        {
            UserCount userCount = userCountRepo.FindById(userId);
            userCount.CommentPending++;
            userCountRepo.Save(userCount);
        }

        public void IncreaseCountOfMailNew(int userId)
        {
            UserCount userCount = userCountRepo.FindById(userId);
            userCount.MailNew++;
            userCountRepo.Save(userCount);
        }

        public int SaveImagesInSession(HttpSessionStateBase session, IList<ImageInfo> images)
        {
            int userId = sessionManager.GetUser().UserId;
            string sesKey = Globals.SESSIONKEY_UPLOADED_PHOTOS + userId.ToString();
            if (session[sesKey] == null)
            {
                session[sesKey] = images;
            }
            else
            {
                IList<ImageInfo> _images = session[Globals.SESSIONKEY_UPLOADED_PHOTOS + userId.ToString()] as IList<ImageInfo>;
                foreach (ImageInfo image in images)
                {
                    _images.Add(image);
                }
            }
            return images.Count;
        }

        public void RemoveImagesFromSession(HttpSessionStateBase session)
        {
            int userId = sessionManager.GetUser().UserId;
            string sesKey = Globals.SESSIONKEY_UPLOADED_PHOTOS + userId.ToString();
            if (session[sesKey] != null)
            {
                session.Remove(sesKey);
            }
        }

        public void RemoveImagesFromDisk(HttpSessionStateBase session)
        {
            int userId = sessionManager.GetUser().UserId;
            string sesKey = Globals.SESSIONKEY_UPLOADED_PHOTOS + userId.ToString();
            if (session[sesKey] == null)
            {
                return;
            }
            IList<ImageInfo> images = session[sesKey] as IList<ImageInfo>;
            foreach (ImageInfo image in images)
            {
                try
                {
                    System.IO.File.Delete(image.LinkAccess);
                }
                catch
                {
                    continue;
                }
            }
            session.Remove(sesKey);
        }

        #endregion

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