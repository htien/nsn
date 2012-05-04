using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Core;
using NewSocialNetwork.Website.Main;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : AbstractController
    {
        public IUserRepository userRepo { private get; set; }

        public HomeController()
        {
            ViewBag.PageTitle = "NSN: Stream";
        }

        //
        // GET: /Home/

        public ActionResult Stream(string userid)
        {
            ViewBag.Author = config[CfgKeys.ANONYMOUS_USER_ID];
            return View();
        }

        public ActionResult FindFriends()
        {
            return View();
        }
    }
}
