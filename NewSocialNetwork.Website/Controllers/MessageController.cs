using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class MessageController : AbstractController
    {
        //
        // GET: /Message/

        public ActionResult List()
        {
            return View();
        }
    }
}
