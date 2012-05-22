using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NewSocialNetwork.Website.Models;
using NSN.Common;
using SaberLily.Utils;

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
            ResponseMessage msg = new ResponseMessage("PhotoAlbumUploader", RAction.ADD, RStatus.FAIL,
                "Error when processing your request.");
            try
            {
                foreach (string upload in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[upload];
                    if (file.ContentLength == 0)
                    {
                        continue;
                    }
                    int sizeInKB = file.ContentLength / 1024;
                    if (sizeInKB > 2048)
                    {
                        continue;
                    }
                    string fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    string folderUpload = Globals.ApplicationMapPath + "\\static\\images\\photos\\";
                    string linkAccess = Path.Combine(folderUpload, fileName);
                    int uploadTimestamp = DateTimeUtils.UnixTimestamp;
                    file.SaveAs(linkAccess);
                }
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Uploaded.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
