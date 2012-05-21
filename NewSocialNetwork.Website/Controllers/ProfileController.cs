using System.Web.Mvc;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class ProfileController : ApplicationController
    {
        public ProfileController()
        {
            ViewBag.PageTitle = "NSN: Profile";
        }

        //
        // GET: /Profile/

        public ActionResult Info(string uid)
        {
            ViewBag.PageTitle += " Info";
            return View();
        }
    }
}
