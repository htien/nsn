using System.Web.Mvc;
using Castle.ActiveRecord;
using Castle.Core.Logging;
using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserController : Controller
    {
        public ILogger Logger { get; set; }

        //
        // GET: /User/

        public ActionResult Index()
        {
            ViewBag.User = ActiveRecordMediator<User>.FindByPrimaryKey(2);
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string returnUrl)
        {
            if (ModelState.IsValid)
            {
            }

            return View();
        }

    }
}
