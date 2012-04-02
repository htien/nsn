using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using Castle.Core.Logging;

using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ILogger Logger { get; set; }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            string iso = Country.Find("VN").Name;

            ViewBag.Welcome = "New Social Network";
            ViewBag.iso = iso;
            return View();
        }
    }
}