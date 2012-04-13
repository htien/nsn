using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using Castle.ActiveRecord;
using Castle.Core.Logging;
using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ILogger Logger { get; set; }
        public UserRepository userRepo { get; set; }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            bool exist = userRepo != null;
            IList<User> Users = ActiveRecordMediator<User>.FindAll();

            ViewBag.Users = Users;
            return View();
        }
    }
}