using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : AbstractController
    {
        public HomeController()
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
