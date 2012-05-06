using System.Web.Mvc;
using NewSocialNetwork.Domain;

namespace NewSocialNetwork.Website.Controllers
{
    public class ProfileController : AbstractDefaultController
    {
        public ProfileController() { }

        //
        // GET: /Profile/

        public ActionResult Info()
        {
            ViewBag.PageTitle = "NSN: Profile Info";
            return View();
        }

        public ActionResult Posts(string userid)
        {
            ViewBag.PageTitle = "NSN: Posts";
            ViewBag.abc = userid;
            return View();
        }

        public ActionResult Friends()
        {
            ViewBag.PageTitle = "NSN: Friends";
            return View();
        }

        public ActionResult Links()
        {
            ViewBag.PageTitle = "NSN: Links";
            return View();
        }
    }
}
