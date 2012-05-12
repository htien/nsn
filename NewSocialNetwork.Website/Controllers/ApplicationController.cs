using System.Web.Mvc;
using NSN.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    public class ApplicationController : AbstractDefaultController
    {
        public ISessionManager sessionManager { protected get; set; }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.UserSession = sessionManager.GetUserSession();
            ViewBag.UserLogged = sessionManager.GetUser();
            base.OnActionExecuted(filterContext);
        }
    }
}
