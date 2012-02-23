using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using Castle.Core.Logging;

namespace OlympiaNet.Website.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ILogger Logger { get; set; }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Welcome = "Olympia!";
            Logger.WarnFormat("OK!");
            return View();
        }
    }
}