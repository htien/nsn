using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class FriendController : AbstractDefaultController
    {
        public FriendController()
        {
            ViewBag.PageTitle = "NSN: Friends";
        }

        //
        // GET: /Friend/

        public ActionResult List()
        {
            return View();
        }

    }
}
