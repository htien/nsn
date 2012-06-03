using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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
        public ICountryRepository countryRepo { private get; set; }
        public ILikeRepository likeRepo { private get; set; }
        public ILikeCacheRepository likeCacheRepo { private get; set; }
        public ILinkRepository linkRepo { private get; set; }
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }
        public IPhotoRepository photoRepo { get; set; }
        public IPhotoInfoRepository photoInfoRepo { get; set; }

        #endregion

        public FrontendService() { }

        #region Register, Login, Logout

        public User RegisterNewUser(string firstName, string lastName, byte gender,
            string regEmail, string regPassword, string confirmPassword,
            string birthday, int joinDate, string ipAddr)
        {
            if (userRepo.IsExistEmail(regEmail.Trim()))
            {
                throw new Exception("Cannot accept dupplicated email.");
            }
            DateTime birthDay = DateTime.Parse(birthday);
            if (gender != 1 && gender != 2)
            {
                throw new Exception("Please choose your gender.");
            }
            UserGroup group = userGroupRepo.FindById(UserGroupLevel.RegisteredUser);
            User user = new User()
            {
                UserGroup = group,
                Email = regEmail.Trim(),
                Password = PasswordCryptor.Hash(regPassword, 690),
                FullName = firstName.Trim() + " " + lastName.Trim(),
                Gender = gender,
                Birthday = String.Format("{0}{1}{2}",
                                birthDay.Year,
                                birthDay.Month < 10 ? "0" + birthDay.Month.ToString() : birthDay.Month.ToString(),
                                birthDay.Day < 10 ? "0" + birthDay.Day.ToString() : birthDay.Day.ToString()),
                Joined = joinDate,
                LastLogin = sessionManager.GetUserSession().LastVisit,
                LastActivity = sessionManager.GetUserSession().LastAccessedTime,
                LastIpAddress = ipAddr
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

        public IList<FeedItem> LoadFeedItems(int userId, int start, int size)
        {
            if (start < 0)
                start = 0;
            if (size < 1)
                size = 5;
            IList<Feed> feeds = feedRepo.GetUserFeeds(userId, start, size);
            return FetchFeedItems(feeds);
        }

        public IList<FeedItem> LoadStreamItems(int userId, int start, int size)
        {
            if (start < 0)
                start = 0;
            if (size < 1)
                size = 5;
            IList<Feed> feeds = feedRepo.ListStreamByUser(userId, start, size);
            return FetchFeedItems(feeds);
        }

        public IList<FeedItem> LoadStreamNewestItems(int userId, long feedId, int start = 0, int size = 5)
        {
            IList<Feed> newestFeeds = feedRepo.ListNewestFeed(userId, feedId);
            return FetchFeedItems(newestFeeds);
        }

        public IList<FeedItem> FetchFeedItems(IList<Feed> feeds)
        {
            FeedManager feedManager = new FeedManager();
            foreach (Feed feed in feeds)
            {
                IEntity entity = null;
                switch (feed.TypeId)
                {
                    case NSNType.USER_TWEET:
                        entity = userTweetRepo.FindById(feed.ItemId);
                        break;
                    case NSNType.LINK:
                        entity = linkRepo.FindById(feed.ItemId);
                        break;
                    case NSNType.PHOTO_ALBUM:
                        entity = photoAlbumRepo.FindById(feed.ItemId);
                        break;
                    case NSNType.PHOTO_ALBUM_MORE:
                        entity = photoAlbumRepo.FindById(feed.ItemId);
                        break;
                    case NSNType.FRIEND:
                        entity = friendRepo.FindById(feed.ItemId);
                        break;
                }
                feedManager.AddFeedItem(feed, entity);
            }
            return feedManager.GetItems();
        }

        public IList<ImageInfo> SaveImagesFromHttp(HttpFileCollectionBase imageCollection)
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

        public IList<FriendRequest> ListFriendRequests(int userId)
        {
            return friendRequestRepo.List(userId);
        }

        public int PostUserTweet(int userId, string content, int timestamp)
        {
            User receiver = userRepo.FindById(userId);
            User sender = sessionManager.GetUser();
            string originContent = Globals.UrlDecode(content.Trim());

            if (String.IsNullOrEmpty(originContent))
            {
                throw new Exception("<p>Invalid content.</p>");
            }
            if (receiver.UserId == sender.UserId)
            {
                return userTweetRepo.Add(sender.UserId, 0, Globals.HtmlEncode(originContent), timestamp);
            }
            else
            {
                return userTweetRepo.Add(sender.UserId, receiver.UserId, Globals.HtmlEncode(originContent), timestamp);
            }
        }

        public int PostLink(int userId, string content, string linkUrl, string imageUrl, string title, string description, int timestamp)
        {
            User receiver = userRepo.FindById(userId);
            User sender = sessionManager.GetUser();
            string parsedContent = Globals.HtmlEncode(Globals.UrlDecode(content.Trim()));
            string parsedTitle = Globals.HtmlEncode(Globals.UrlDecode(title.Trim()));
            string parsedDescription = Globals.HtmlEncode(Globals.UrlDecode(description.Trim()));
            LinkInfo linkInfo = Globals.GetLinkInfo(linkUrl);

            if (String.IsNullOrEmpty(parsedTitle))
            {
                parsedTitle = linkInfo.Title.Trim();
            }
            if (String.IsNullOrEmpty(parsedDescription))
            {
                parsedDescription = linkInfo.Description.Trim();
            }
            parsedContent = Globals.TruncateString(parsedContent, 255);
            parsedTitle = Globals.TruncateString(parsedTitle, 255);
            parsedDescription = Globals.TruncateString(parsedDescription, 255);
            if (receiver.UserId == sender.UserId)
            {
                return linkRepo.Add(sender.UserId, 0, parsedContent, linkUrl, imageUrl, parsedTitle, parsedDescription, timestamp);
            }
            else
            {
                return linkRepo.Add(sender.UserId, receiver.UserId, parsedContent, linkUrl, imageUrl, parsedTitle, parsedDescription, timestamp);
            }
        }

        public long AddCommentOnFeed(long feedId, int userId, int timestamp, string commentText, string ipAddr)
        {
            Feed feed = feedRepo.FindById(feedId);
            if (feed == null)
            {
                throw new Exception("Cannot comment on the feed does not exist.");
            }
            int ownerUserId = (userId == feed.User.UserId) ? 0 : userId;
            return this.AddComment(feed.TypeId, feed.ItemId, feed.User.UserId, ownerUserId, commentText, ipAddr, timestamp);
        }

        public long AddCommentOnPhoto(int photoId, int userId, int timestamp, string commentText, string ipAddr)
        {
            Photo photo = photoRepo.FindById(photoId);
            if (photo == null)
            {
                throw new Exception("Cannot comment on the photo does not exist.");
            }
            int ownerUserId = (userId == photo.User.UserId) ? 0 : userId;
            return this.AddComment(NSNType.PHOTO, photoId, photo.User.UserId, ownerUserId, commentText, ipAddr, timestamp);
        }

        public long AddComment(string type, int itemId, int userId, int ownerUserId, string commentText, string ipAddr, int timestamp)
        {
            long commentId = commentRepo.Add(type, itemId, userId, ownerUserId, commentText, ipAddr, timestamp);
            string originCommentText = Globals.UrlDecode(commentText);
            return commentTextRepo.Add(commentId, originCommentText, Globals.HtmlEncode(originCommentText));
        }

        public User UpdateProfileInfo(string username, string fullName, string email, byte gender,
            string birthday, string countryiso)
        {
            User user = sessionManager.GetUser();

            // Validate
            if (String.IsNullOrWhiteSpace(user.Username))
            {
                username = username.Trim();
                if (!String.IsNullOrEmpty(username))
                {
                    if (userRepo.IsExistUsername(username.Trim()))
                    {
                        throw new Exception("Username is exists. Please choose another.");
                    }
                    else
                    {
                        if (!Regex.IsMatch(username, Globals.glbUserNameRegEx))
                        {
                            throw new Exception("Username is invalid. Allow A-Z, a-z, 0-9. First letter must be alphabet character. At least 5 characters.");
                        }
                        user.Username = username.Trim();
                    }
                }
            }
            if (!Regex.IsMatch(email.Trim(), Globals.glbEmailRegEx))
            {
                throw new Exception("Email is invalid format.");
            }
            DateTime birthDay = DateTime.Parse(birthday);
            if (gender != 1 && gender != 2)
            {
                throw new Exception("Please choose your gender.");
            }
            Country country = countryRepo.FindById(countryiso);
            if (country == null)
            {
                throw new Exception("Country is unavailable.");
            }

            // Update
            user.Email = email.Trim();
            user.FullName = fullName.Trim();
            user.Gender = gender;
            user.Birthday = String.Format("{0}{1}{2}",
                                birthDay.Year,
                                birthDay.Month < 10 ? "0" + birthDay.Month.ToString() : birthDay.Month.ToString(),
                                birthDay.Day < 10 ? "0" + birthDay.Day.ToString() : birthDay.Day.ToString());
            user.Country = country;
            return userRepo.Save(user);
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
            string originMessage = Globals.UrlDecode(message.Trim());
            int timestamp = DateTimeUtils.UnixTimestamp;
            FriendRequest friendRequest = new FriendRequest()
            {
                User = myUser,
                FriendUser = friendUser,
                Message = Globals.HtmlEncode(message),
                Timestamp = timestamp
            };
            return friendRequestRepo.Create(friendRequest);
        }

        public Friend AcceptFriendRequest(int requestId, int timestamp)
        {
            FriendRequest friendRequest = friendRequestRepo.FindById(requestId);
            if (friendRequest == null)
            {
                throw new Exception("Friend request is not exists.");
            }
            // Thêm vào bạn mới
            User myUser = sessionManager.GetUser(); // người accepted
            User friendUser = friendRequest.User; // người requested
            Friend newFriend1 = new Friend()
            {
                User = friendUser,
                FriendUser = myUser,
                Timestamp = timestamp
            };
            Friend newFriend2 = new Friend()
            {
                User = myUser, // User
                FriendUser = friendUser, // ParentUser
                Timestamp = timestamp
            };
            Friend newFriend = null;
            try
            {
                friendRepo.Create(newFriend1);
                newFriend = friendRepo.Create(newFriend2);
            }
            catch { }
            // Nếu chấp nhận làm bạn, đặt IsIgnore = true
            friendRequest.IsIgnore = true;
            friendRequest.IsSeen = true;
            friendRequestRepo.Save(friendRequest);
            return newFriend;
        }

        public void CancelFriendRequest(int requestId, int userId)
        {
            FriendRequest friendRequest = friendRequestRepo.FindById(requestId);
            if (friendRequest == null)
            {
                throw new Exception("Friend request does not exists.");
            }
            if (friendRequest.FriendUser.UserId != userId)
            {
                throw new Exception("Wrong friend request ID.");
            }
            friendRequest.IsIgnore = true;
            friendRequest.IsSeen = true;
            friendRequestRepo.Save(friendRequest);
        }

        public PhotoAlbum AddPhotoAlbum(HttpSessionStateBase session, int timestamp,
            string albumTitle = "Untitled Album", byte privacy = 0)
        {
            string privacyRegex = @"^(0|1|10)$";
            if (String.IsNullOrWhiteSpace(albumTitle)
                || !Regex.IsMatch(privacy.ToString(), privacyRegex))
            {
                throw new Exception("Album title is unavailable.");
            }
            IList<ImageInfo> uploadedImages = (IList<ImageInfo>)session[Globals.SESSIONKEY_UPLOADED_PHOTOS + sessionManager.GetUser().UserId];
            if (uploadedImages == null || uploadedImages.Count == 0)
            {
                throw new Exception("Have no any uploaded photo. Please add at least a photo or more.");
            }
            // Insert new photo album
            string orginAlbumTitle = Globals.UrlDecode(albumTitle);
            PhotoAlbum photoAlbum = new PhotoAlbum()
            {
                Name = Globals.HtmlEncode(orginAlbumTitle),
                User = sessionManager.GetUser(),
                ProfileId = 0,
                Privacy = privacy,
                PrivacyComment = NSNPrivacyCommentMode.PUBLIC,
                Timestamp = timestamp
            };
            return photoAlbumRepo.Create(photoAlbum);
        }

        public void AddPhotosToAlbum(HttpSessionStateBase session, int albumId, int timestamp,
            byte privacy = NSNPrivacyMode.PUBLIC)
        {
            // Validate
            PhotoAlbum photoAlbum = null;
            if (photoAlbumRepo.IsAlbumOfUser(sessionManager.GetUser().UserId, albumId))
            {
                photoAlbum = photoAlbumRepo.FindById(albumId);
            }
            if (photoAlbum == null)
            {
                throw new Exception("You cannot upload to this or it does not exist.");
            }
            IList<ImageInfo> uploadedImages = (IList<ImageInfo>)session[Globals.SESSIONKEY_UPLOADED_PHOTOS + sessionManager.GetUser().UserId];
            if (uploadedImages == null || uploadedImages.Count == 0)
            {
                throw new Exception("Have no any uploaded photo. Please add at least a photo or more.");
            }

            // Add photos
            this.AddPhotosFromSession(session, photoAlbum, timestamp, privacy);
        }

        public void AddPhotosFromSession(HttpSessionStateBase session, PhotoAlbum photoAlbum, int timetamp,
            byte privacy = NSNPrivacyMode.PUBLIC)
        {
            IList<ImageInfo> uploadedImages = (IList<ImageInfo>)session[Globals.SESSIONKEY_UPLOADED_PHOTOS + sessionManager.GetUser().UserId];
            foreach (ImageInfo imageInfo in uploadedImages)
            {
                Photo photo = new Photo()
                {
                    Album = photoAlbum,
                    User = sessionManager.GetUser(),
                    Privacy = privacy,
                    Image = imageInfo.FileName,
                    AllowComment = true,
                    Timestamp = timetamp
                };
                Photo newPhoto = photoRepo.Create(photo);
                this.AddPhotoInfo(newPhoto, imageInfo);
            }
        }

        public PhotoInfo AddPhotoInfo(Photo photo, ImageInfo imageInfo)
        {
            Image sImg = Image.FromFile(imageInfo.LinkAccess);
            PhotoInfo photoInfo = new PhotoInfo()
            {
                Photo = photo,
                FileName = imageInfo.FileName,
                FileSize = imageInfo.FileSize,
                MimeType = imageInfo.MimeType,
                Extension = Path.GetExtension(imageInfo.FileName),
                Description = "",
                Width = sImg.Width,
                Height = sImg.Height
            };
            return photoInfoRepo.Create(photoInfo);
        }

        public int RemoveFeed_OnPhotoAlbumEmpty(int albumId)
        {
            int removedCount = 0;
            if (photoAlbumRepo.Exists(albumId))
            {
                IList<Feed> listF1 = feedRepo.ListFeedsByItem(NSNType.PHOTO_ALBUM_MORE, albumId);
                if (listF1.Count > 0)
                {
                    foreach (Feed f in listF1)
                    {
                        if (!photoAlbumRepo.HasPhotosByTimestamp(albumId, f.Timestamp))
                        {
                            removedCount += feedRepo.Remove(f.FeedId);
                        }
                    }
                }
                IList<Feed> listF2 = feedRepo.ListFeedsByItem(NSNType.PHOTO_ALBUM, albumId);
                if (listF2.Count > 0)
                {
                    foreach (Feed f in listF2)
                    {
                        if (!photoAlbumRepo.HasPhotosByTimestamp(albumId, f.Timestamp))
                        {
                            removedCount += feedRepo.Remove(f.FeedId);
                        }
                    }
                }
            }
            return removedCount;
        }

        public void RemoveAlbum(int albumId)
        {
            User user = sessionManager.GetUser();
            if (!photoAlbumRepo.Exists(albumId)
                || !photoAlbumRepo.IsAlbumOfUser(user.UserId, albumId))
            {
                throw new Exception("This album does not exist or you do not have permission to remove it.");
            }
            PhotoAlbum album = photoAlbumRepo.FindById(albumId);
            // Xóa tất cả photos và xóa ảnh từ disk
            IList<Photo> photos = album.Photos;
            foreach (Photo photo in photos)
            {
                string imageFileName = photo.Image;
                photoRepo.Remove(photo.PhotoId);
                this.RemovePhotoFromDisk(imageFileName);
            }
            // Xóa các feed liên quan
            this.RemoveFeed_OnPhotoAlbumEmpty(albumId);

            // Xóa album
            photoAlbumRepo.Remove(albumId);
        }

        public int RemovePhoto(int albumId, int photoId)
        {
            User user = sessionManager.GetUser();
            bool allowRemove = photoRepo.IsPhotoOfUser(photoId, user.UserId)
                || photoAlbumRepo.IsAlbumOfUser(user.UserId, albumId);
            if (allowRemove)
            {
                string imageFileName = photoRepo.ImageFileName(photoId);
                int removedCount = photoRepo.Remove(photoId);
                this.RemovePhotoFromDisk(imageFileName);
                return removedCount;
            }
            return 0;
        }

        public int RemoveComment(string typeId, int itemId, int commentId)
        {
            User user = sessionManager.GetUser();
            bool allowRemove = commentRepo.IsCommentOfUser(commentId, user.UserId)
                || feedRepo.IsItemOfUser(typeId, itemId, user.UserId);
            return allowRemove ? commentRepo.Remove(commentId) : 0;
        }

        public bool RemoveFeed(long feedId, int userId)
        {
            Feed feed = feedRepo.FindById(feedId);
            if (feed == null)
            {
                throw new Exception("Feed does not exists.");
            }
            // kiểm tra feedId của user mới dc phép xóa
            if (!feedRepo.IsItemOfUser(feed.TypeId, feed.ItemId, userId))
            {
                throw new Exception("You do not permission to delete post.");
            }

            return feedRepo.Remove(feedId) > 0;
        }

        public long LikeForFeed(long feedId, int userId, int timestamp)
        {
            return this.Like("on_feed", feedId, userId, timestamp);
        }

        public void UnlikeForFeed(long feedId, int userId)
        {
            this.Unlike("on_feed", feedId, userId);
        }

        public long LikeForPhoto(int photoId, int userId, int timestamp)
        {
            return this.Like("on_photo", photoId, userId, timestamp);
        }

        public void UnlikeForPhoto(int photoId, int userId)
        {
            this.Unlike("on_photo", photoId, userId);
        }

        public long Like(string where, long id, int userId, int timestamp)
        {
            string typeId = null;
            int itemId = 0;
            switch (where)
            {
                case "on_feed":
                    Feed feed = feedRepo.FindById(id);
                    typeId = feed.TypeId;
                    itemId = feed.ItemId;
                    break;
                case "on_photo":
                    Photo photo = photoRepo.FindById(Convert.ToInt32(id));
                    typeId = NSNType.PHOTO;
                    itemId = photo.PhotoId;
                    break;
            }
            if (!likeRepo.Exists(typeId, itemId, userId))
            {
                long likeId = likeRepo.Add(typeId, itemId, userId, timestamp);
                likeCacheRepo.Add(typeId, itemId, userId);
                return likeId;
            }
            else
            {
                return -1;
            }
        }

        public void Unlike(string where, long id, int userId)
        {
            string typeId = null;
            int itemId = 0;
            switch (where)
            {
                case "on_feed":
                    Feed feed = feedRepo.FindById(id);
                    typeId = feed.TypeId;
                    itemId = feed.ItemId;
                    break;
                case "on_photo":
                    Photo photo = photoRepo.FindById(Convert.ToInt32(id));
                    typeId = NSNType.PHOTO;
                    itemId = photo.PhotoId;
                    break;
            }
            if (likeRepo.Exists(typeId, itemId, userId))
            {
                likeRepo.Remove(typeId, itemId, userId);
                likeCacheRepo.Remove(typeId, itemId, userId);
            }
        }

        public int TotalLike(string typeId, int itemId)
        {
            if (!sessionManager.GetUserSession().IsLogged())
            {
                throw new Exception("Cannot process for get total likes. Please login.");
            }
            return likeCacheRepo.TotalLike(typeId, itemId);
        }

        public void IncreaseCountOfFriendRequest(int userId, int count = 1)
        {
            UserCount userCount = userCountRepo.FindById(userId);
            userCount.FriendRequest += count;
            userCountRepo.Save(userCount);
        }

        public void IncreaseCountOfCommentPending(int userId, int count = 1)
        {
            UserCount userCount = userCountRepo.FindById(userId);
            userCount.CommentPending += count;
            userCountRepo.Save(userCount);
        }

        public void IncreaseCountOfMailNew(int userId, int count = 1)
        {
            UserCount userCount = userCountRepo.FindById(userId);
            userCount.MailNew += count;
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

        public void RemovePhotoFromDisk(string fileName)
        {
            try
            {
                string imageUrl = Globals.ApplicationMapPath + "/static/images/photos/" + fileName;
                Globals.RemoveImageInPlace(imageUrl);
            }
            catch { }
        }

        public void RemoveAvatarFromDisk(string fileName)
        {
            try
            {
                string imageUrl = Globals.ApplicationMapPath + "/static/images/avatars/" + fileName;
                Globals.RemoveImageInPlace(imageUrl);
            }
            catch { }
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