﻿using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class LinkController : AbstractDefaultController
    {
        public LinkController()
        {
            ViewBag.PageTitle = "NSN: Links";
        }

        //
        // GET: /Link/

        public ActionResult List()
        {
            return View();
        }

    }
}
