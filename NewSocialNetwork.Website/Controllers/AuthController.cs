using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class AuthController : AbstractController
    {
        public AuthController()
        {
            ViewBag.Title = "NSN: Sign In";
        }

        //
        // GET: /Auth/

        public ActionResult Login()
        {
            return View();
        }

    }
}
