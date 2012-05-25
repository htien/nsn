using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NewSocialNetwork.Website.Models;
using NSN.Common.Utilities;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoController : ApplicationController
    {
        public IPhotoRepository photoRepo { private get; set; }

        public PhotoController()
        {
            ViewBag.PageTitle = "NSN: Photo";
        }

        //
        // GET: /Photo/

        public ActionResult Show()
        {
            Domain.User user = sessionManager.GetUser();
            IList<Photo> photos = this.photoRepo.GetPhotosByUser(user.UserId);
            ViewBag.Photos = photos;
            return View();
        }

        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertSave()
        {
            return RedirectToAction("Insert");
        }

        [HttpPost]
        public ActionResult InsertSave(InsertPhotoModel model)
        {
            try
            {
                string test = model.Test;
                if (test.Equals("abc"))
                {
                    ViewBag.Message = "Insert Successfully";
                }
                else
                {
                    ViewBag.Message = "Insert Failed";
                }
            }
            catch
            {
                ViewBag.Message = "Insert Error";
            }
            return View("Insert");
        }

        [HttpPost]
        public JsonResult UploadSave()
        {
            //ResponseMessage msg = new ResponseMessage("PhotoAlbumUploader", RAction.ADD, RStatus.FAIL,
            //    "Error when processing your request.");
            UploadedPhotoModel[] photosModel = null;
            try
            {
                IList<ImageInfo> images = frontendService.SaveImages(Request.Files);
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
            catch// (Exception e)
            {
                //msg.Message = e.Message;
            }
            return Json(photosModel);
        }

        public JsonResult CancelUpload()
        {
            ResponseMessage msg = new ResponseMessage("PhotoAlbumUploader", RAction.REMOVE, RStatus.FAIL,
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
