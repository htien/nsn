using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class RedirectorController : Controller
    {
        //
        // GET: /Redirector/

        public ActionResult Redirect(string controllerName, string actionName)
        {
            string newUrl = Server.UrlDecode(Url.Action(actionName, controllerName));
            return Redirect(newUrl);
        }

    }
}
