using System.Web.Mvc;
using Castle.Core.Logging;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserController : Controller
    {
        public ILogger Logger { get; set; }

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string returnUrl)
        {
            if (ModelState.IsValid)
            {
            }

            return View();
        }

    }
}
