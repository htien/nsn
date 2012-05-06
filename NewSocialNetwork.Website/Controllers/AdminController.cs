using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class AdminController : AbstractDefaultController
    {
        public AdminController()
        { }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
