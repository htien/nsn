using System.Web.Mvc;
using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Website.Controllers
{
    public class ProfileController : AbstractController
    {
        private User loggedUser;

        public ProfileController()
        {
            this.loggedUser = null; // lấy thực thể User từ SessionManager
        }

        //
        // GET: /Profile/

        public ActionResult Info()
        {
            ViewBag.PageTitle = "NSN: Profile Info";
            return View();
        }

        public ActionResult Posts()
        {
            ViewBag.PageTitle = "NSN: Posts";
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
