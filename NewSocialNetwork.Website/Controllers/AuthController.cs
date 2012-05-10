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
            ResponseMessage msg = new ResponseMessage("Login", RStatus.FAIL,
                @"<p class='nsn-popup-msg ui-state-error'>Incorrect ID or password. Access denied.</p>
                  <p>The username you entered does not belong to any account.</p>
                  <p>You can login using any email or username associated with your account.
                     Make sure that it is typed correctly.</p>");
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
