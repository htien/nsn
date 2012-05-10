using System.Web.Mvc;
using NewSocialNetwork.Domain;

namespace NewSocialNetwork.Website.Controllers
{
    public class ProfileController : AbstractDefaultController
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
            ViewBag.Uid = uid;
            return View();
        }
    }
}
