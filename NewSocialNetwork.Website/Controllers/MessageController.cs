using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class MessageController : AbstractDefaultController
    {
        //
        // GET: /Message/

        public ActionResult List()
        {
            return View();
        }
    }
}
