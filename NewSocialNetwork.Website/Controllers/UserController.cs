using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
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
