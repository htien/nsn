using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NewSocialNetwork.Website.Models;
using NSN.Common;
using NSN.Common.Utilities;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoController : ApplicationController
    {
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }
        public IPhotoRepository photoRepo { private get; set; }
        public IFeedRepository feedRepo { private get; set; }

        public PhotoController()
        {
            ViewBag.PageTitle = "NSN: Photo";
        }

        //
        // GET: /Photo/
        [HttpPost]
        public ActionResult ShowZoom(int photoId)
        {
            Photo photo = photoRepo.FindById(photoId);
            if (photo == null)
            {
                return HttpNotFound("The photo does not exists.");
            }
            if ((photo.Privacy == NSNPrivacyMode.PUBLIC)
                || (photo.Privacy == NSNPrivacyMode.FRIENDS && ViewBag.IsFriend)
                || (photo.Privacy == NSNPrivacyMode.ONLYME && ViewBag.IsMyProfile))
            {
                ViewBag.Photo = photo;
            }
            return View();
        }

        public ActionResult Uploader(int albumId)
        {
            frontendService.RemoveImagesFromDisk(this.Session);

            PhotoAlbum photoAlbum = null;
            if (photoAlbumRepo.IsAlbumOfUser(sessionManager.GetUser().UserId, albumId))
            {
                photoAlbum = photoAlbumRepo.FindById(albumId);
            }
            if (photoAlbum == null)
            {
                return HttpNotFound("You cannot upload to this or it does not exist.");
            }

            ViewBag.PhotoAlbum = photoAlbum;
            return View();
        }

        [HttpPost]
        public JsonResult SaveUploadedPhotos(int albumId, byte privacy = 0)
        {
            ResponseMessage msg = new ResponseMessage("Photo", RAction.ADD, RStatus.FAIL,
                "Error when uploading your photos. Please try again.");
            try
            {
                int timestamp = DateTimeUtils.UnixTimestamp;
                // Add more photo images
                frontendService.AddPhotosToAlbum(this.Session, albumId, timestamp, privacy);
                // Remove photo images from session
                frontendService.RemoveImagesFromSession(this.Session);
                // Insert to feed
                feedRepo.Add(NSNType.PHOTO, albumId, sessionManager.GetUser().UserId, 0, timestamp);
                // Returns
                msg.SetStatusAndMessage(RStatus.SUCCESS, String.Format("Uploaded your photos to album: <strong>{0}</strong>", ""));
                msg.ReturnedPath = Url.RouteUrl("PhotoAlbumAction", new { uid = Globals.GetDisplayId(sessionManager.GetUser()), albumid = albumId, action = "ListPhotos" });
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult UploadSave()
        {
            UploadedPhotoModel[] photosModel = null;
            try
            {
                IList<ImageInfo> images = frontendService.SaveImagesFromHttp(Request.Files);
                if (images.Count == 0)
                {
                    throw new Exception("Cannot process your request.");
                }
                // Save list of uploaded images to HttpSession
                frontendService.SaveImagesInSession(this.Session, images);
                //msg.SetStatusAndMessage(RStatus.SUCCESS, "Uploaded.");
                photosModel = new UploadedPhotoModel[images.Count];
                int i = 0;
                foreach (ImageInfo image in images)
                {
                    UploadedPhotoModel model = new UploadedPhotoModel()
                    {
                        FileName = image.FileName,
                        FileSize = image.FileSize,
                        MimeType = image.MimeType,
                        UploadTimestamp = image.UploadTimestamp
                    };
                    photosModel[i++] = model;
                }
            }
            catch { }
            return Json(photosModel);
        }

        [HttpPost]
        public JsonResult CancelUpload()
        {
            ResponseMessage msg = new ResponseMessage("ImageUploader", RAction.REMOVE, RStatus.FAIL,
                "Error when processing your request.");
            try
            {
                frontendService.RemoveImagesFromDisk(this.Session);
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Canceled.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
