using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Models;
using NSN.Manager;

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
    }
}
