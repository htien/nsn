using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class UserController : AbstractController
    {
        public UserController()
        {
            ViewBag.PageTitle = "NSN: Stream";
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
