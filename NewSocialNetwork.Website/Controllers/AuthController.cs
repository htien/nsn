using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Kernel;
using NSN.Manager;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class AuthController : AbstractController
    {
        public LoginService loginService { private get; set; }
        public ISessionManager sessionManager { private get; set; }
        public IUserRepository userRepo { private get; set; }

        public AuthController()
        {
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
            //try
            //{
                Domain.User user = this.loginService.Authenticate(nsnId, nsnPasswd);
                if (user == null)
                {
                    ViewBag.LoginFail = true;
                }
                else
                {
                    UserSession userSession = this.sessionManager.GetUserSession();
                    userSession.User = user;
                    userSession.BecomesLogged();

                    // TODO: Check autologin

                    this.sessionManager.Add(userSession);
                    msg.SetStatusAndMessage(RStatus.SUCCESS, "Authenticated!");
                }
            //}
            //catch (Exception e)
            //{
            //    msg.Message = e.Message;
            //}
            return Json(msg);
        }
    }
}
