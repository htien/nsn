using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Security.Application;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Common;
using NSN.Common.Utilities;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoAlbumController : ApplicationController
    {
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }
        public IPhotoRepository photoRepo { private get; set; }
        public IPhotoInfoRepository photoInfoRepo { private get; set; }
        public IFriendRepository friendRepo { private get; set; }
        public ICommentRepository commentRepo { private get; set; }
        public IFeedRepository feedRepo { private get; set; }

        public PhotoAlbumController()
        {
            ViewBag.PageTitle = "NSN: Photo Albums";
        }

        //
        // GET: /PhotoAlbum/
        
        public ActionResult List()
        {
            User user = sessionManager.GetUser();
            IList<PhotoAlbum> albums = photoAlbumRepo.GetPhotoAlbumByUser(user.UserId);
            int totalAlbum = photoAlbumRepo.GetTotalPhotoAlbumByUser(user.UserId);
            //int totalUsers = photoAlbumRepo.GetNotMutualFriends(1, 2).Count;
            //ViewBag.Total = totalUsers;
            //IList<Friend> u = friendRepo.GetNotMutualFriends(1, 2);
            //ViewBag.user = u;
            int totalfriends = photoAlbumRepo.GetTotalFriendsByUser(1);
            ViewBag.user = totalfriends;
            IList<User> listfriend = photoAlbumRepo.GetListFriendByUser(1);
            ViewBag.listfriend = listfriend;
            IList<Photo> listphotoalbum = photoAlbumRepo.GetPhotoByAlbum(2);
            ViewBag.listphotoalbum = listphotoalbum;
            IList<User> friendsearch = photoAlbumRepo.SearchFriendByName("l", 1);
            ViewBag.friendsearch = friendsearch;
            int totalcomment = commentRepo.GetTotalComment("photo", 1, 1);
            ViewBag.totalcomment = totalcomment;
            IList<Comment> allc = commentRepo.GetCommentsByFeed("photo", 1, 1);
            ViewBag.allc = allc;
            int friendRequest = photoAlbumRepo.GetTotalFriendRequestPending(1);
            ViewBag.friendRequest = friendRequest;
            int message = photoAlbumRepo.GetTotalMessagePending(1);
            ViewBag.message = message;
            int activity = photoAlbumRepo.GetTotalActivityPendingRelativeUser(1);
            ViewBag.activity = activity;
            ViewBag.friendRequest = friendRequest;

            IList<FriendList> fl = photoAlbumRepo.GetAllFriendListByUser(1);
            ViewBag.friendList = fl;

            IList<User> userFriendList = photoAlbumRepo.GetFriendInListByUser(1);
            ViewBag.userFriendList = userFriendList;
            IList<CustomRelation> customRelation = photoAlbumRepo.GetRelationshipBetweenUsers(1, 2);
            ViewBag.customRelation = customRelation;
            ViewBag.Albums = albums;

            IList<User> NotMutualFriend = photoAlbumRepo.GetNotMutualFriend(2,1);
            ViewBag.notMutualFriend = NotMutualFriend;

            IList<User> MutualFriend = photoAlbumRepo.GetMutualFriend(2, 1);
            ViewBag.mutualFriend = MutualFriend;

            return View();
        }

        public ActionResult ListPhotos(int albumid)
        {
            PhotoAlbum album = photoAlbumRepo.FindById(albumid);
            IList<Photo> photos = album.Photos;
            ViewBag.Album = album;
            ViewBag.Photos = photos;
            return View();
        }

        public ActionResult Uploader()
        {
            frontendService.RemoveImagesFromDisk(this.Session);
            return View();
        }

        [HttpPost]
        public JsonResult SaveUploadedPhotos(string albumTitle, byte privacy = 0)
        {
            ResponseMessage msg = new ResponseMessage("PhotoAlbum", RAction.ADD, RStatus.FAIL,
                "Error when creating your album. Please try again.");
            try
            {
                string privacyRegex = @"^(0|1|10)$";
                if (String.IsNullOrWhiteSpace(albumTitle)
                    || !Regex.IsMatch(privacy.ToString(), privacyRegex))
                {
                    throw new Exception("You are hacking.");
                }
                IList<ImageInfo> uploadedImages = (IList<ImageInfo>)Session[Globals.SESSIONKEY_UPLOADED_PHOTOS + sessionManager.GetUser().UserId];
                if (uploadedImages == null || uploadedImages.Count == 0)
                {
                    throw new Exception("Have no any uploaded photo. Please add at least a photo or more.");
                }
                // Insert new photo album
                string orginAlbumTitle = HttpUtility.UrlDecode(albumTitle, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                int timestamp = DateTimeUtils.UnixTimestamp;
                PhotoAlbum photoAlbum = new PhotoAlbum()
                {
                    Name = Encoder.HtmlEncode(orginAlbumTitle),
                    User = sessionManager.GetUser(),
                    ProfileId = 0,
                    Privacy = privacy,
                    PrivacyComment = NSNPrivacyCommentMode.PUBLIC,
                    Timestamp = timestamp
                };
                PhotoAlbum newPhotoAlbum = photoAlbumRepo.Create(photoAlbum);
                // Insert new photo images
                foreach (ImageInfo image in uploadedImages)
                {
                    Photo photo = new Photo()
                    {
                        Album = newPhotoAlbum,
                        User = sessionManager.GetUser(),
                        Privacy = NSNPrivacyMode.PUBLIC,
                        Image = image.FileName,
                        AllowComment = true,
                        Timestamp = image.UploadTimestamp
                    };
                    Photo newPhoto = photoRepo.Create(photo);
                    Image sImg = Image.FromStream(image.ImageStream);
                    PhotoInfo photoInfo = new PhotoInfo()
                    {
                        Photo = newPhoto,
                        FileName = image.FileName,
                        FileSize = image.FileSize,
                        MimeType = image.MimeType,
                        Extension = Path.GetExtension(image.FileName),
                        Description = "",
                        Width = sImg.Width,
                        Height = sImg.Height
                    };
                    photoInfoRepo.Create(photoInfo);
                }
                // Remove session for photo images
                frontendService.RemoveImagesFromSession(this.Session);
                // Insert to feed
                feedRepo.Add(NSNType.PHOTO_ALBUM, newPhotoAlbum.AlbumId, sessionManager.GetUser().UserId, 0, timestamp);
                msg.SetStatusAndMessage(RStatus.SUCCESS, String.Format("Uploaded your photos to album: <strong>{0}</strong>", albumTitle));
                msg.ReturnedPath = Url.RouteUrl("PhotoAlbumAction", new { uid = Globals.GetDisplayId(sessionManager.GetUser()), albumid = newPhotoAlbum.AlbumId, action = "ListPhotos" });
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
