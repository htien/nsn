using System.Web.Mvc;
using SaberLily.Web.Filter.Optimizer;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class AdminController : AbstractController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
