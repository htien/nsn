using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Common;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoAlbumController : ApplicationController
    {
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }
        public IFeedRepository feedRepo { private get; set; }

        public PhotoAlbumController()
        {
            ViewBag.PageTitle = "NSN: Photo Albums";
        }

        //
        // GET: /PhotoAlbum/
        
        public ActionResult List()
        {
            User user = ViewBag.UProfile;
            IList<PhotoAlbum> albums = photoAlbumRepo.GetPhotoAlbumByUser(user.UserId);
            ViewBag.Albums = albums;
            return View();
        }

        public ActionResult ListPhotos(int albumid)
        {
            PhotoAlbum album = photoAlbumRepo.FindById(albumid);
            if (album == null)
            {
                return HttpNotFound("Album not found.");
            }
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
                int timestamp = DateTimeUtils.UnixTimestamp;
                PhotoAlbum newPhotoAlbum = frontendService.AddPhotoAlbum(this.Session, timestamp, albumTitle, privacy);
                // Insert new photo images
                frontendService.AddPhotosFromSession(this.Session, newPhotoAlbum, timestamp);
                // Remove photo images from session
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
