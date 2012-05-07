using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NSN.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : AbstractDefaultController
    {
        public IUserRepository userRepo { private get; set; }
        public ISessionManager sessionManager { private get; set; }

        public HomeController()
        {
            ViewBag.PageTitle = "NSN: Stream";
        }

        //
        // GET: /Home/

        public ActionResult Stream(string userid)
        {
            ViewBag.IsLogged = sessionManager.GetUserSession().IsLogged();
            ViewBag.LoggedUser = sessionManager.GetUser();
            return View();
        }

        public ActionResult FindFriends()
        {
            return View();
        }
    }
}
