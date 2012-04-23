using System.Web.Mvc;
using SaberLily.Web.Filter.Optimizer;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class AuthController : AbstractController
    {
        public AuthController()
        {
            ViewBag.Title = "NSN: Sign In";
        }

        //
        // GET: /Auth/

        public ActionResult Register()
        {
            return View();
        }

    }
}
