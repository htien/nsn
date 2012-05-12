using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NSN.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : ApplicationController
    {
        public IUserRepository userRepo { private get; set; }

        public HomeController()
        {
            ViewBag.PageTitle = "NSN: Stream";
        }

        //
        // GET: /Home/

        public ActionResult Stream()
        {
            return View();
        }

        public ActionResult FindFriends()
        {
            return View();
        }
    }
}
