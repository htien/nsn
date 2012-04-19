using System.Web.Mvc;
using SaberLily.Web.Filter.Optimizer;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class UserController : AbstractController
    {
        public UserController()
        {
            ViewBag.Title = "NSN: Stream";
        }

        //
        // GET: /User/

        public ActionResult Stream()
        {
            ViewBag.kq = Request["name"];
            return View();
        }

    }
}
