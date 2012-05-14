using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class AjaxController : ApplicationController
    {
        //
        // GET: /Ajax/

        public ActionResult Feeds()
        {
            return RedirectToRoute("Go", new { controller = "Feed", action = "Feeds" });
        }

    }
}
