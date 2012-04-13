using System.Web.Mvc;
using Castle.ActiveRecord;
using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            ViewBag.Users = ActiveRecordMediator<User>.FindAll();
            return View();
        }

    }
}
