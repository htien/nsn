using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace OlympiaNet.Website.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.abc = "Xin Chao!";
            return View();
        }
    }
}
