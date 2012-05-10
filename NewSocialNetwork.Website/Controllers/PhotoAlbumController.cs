﻿using System.Web.Mvc;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoAlbumController : AbstractDefaultController
    {
        public IPhotoAlbumRepository photoAlbumRepo { get; set; }

        public PhotoAlbumController()
        {
            ViewBag.PageTitle = "NSN: Photo Album";
        }

        //
        // GET: /PhotoAlbum/

        public ActionResult List()
        {
            return View();
        }

    }
}
