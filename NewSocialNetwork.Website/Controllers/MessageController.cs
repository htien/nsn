using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class MessageController : ApplicationController
    {
        //
        // GET: /Message/

        public ActionResult List()
        {
            return View();
        }
    }
}
