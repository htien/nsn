using System;
using System.Web;
using System.Web.Mvc;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Kernel;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class AuthController : AbstractController
    {
        public AuthService authService { private get; set; }

        public AuthController()
        {
            RedirectToHomePage(System.Web.HttpContext.Current);
            ViewBag.PageTitle = "NSN: Sign In";
        }

        //
        // GET: /Auth/

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string nsnId, string nsnPasswd)
        {
            ResponseMessage msg = new ResponseMessage("Login", RStatus.FAIL, "Wrong ID or password. Access denied.");
            try
            {
                Domain.User user = authService.Login(nsnId, nsnPasswd);
                if (user != null)
                {
                    msg.SetStatusAndMessage(RStatus.SUCCESS, "Authenticated!");
                }
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

        public ActionResult Logout()
        {
            authService.Logout();
            return RedirectToRoute("Root");
        }

        private static void RedirectToHomePage(HttpContext context)
        {
            if (NSNContext.Current.SessionManager.GetUserSession().IsLogged())
            {
                context.Response.RedirectToRoute("Root");
            }
        }
    }
}
