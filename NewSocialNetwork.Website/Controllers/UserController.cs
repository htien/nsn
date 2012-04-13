using System.Web.Mvc;
using Castle.ActiveRecord;
using Castle.Core.Logging;
using NewSocialNetwork.Entities;
using NewSocialNetwork.Website.Services;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserController : Controller
    {
        public ILogger Logger { get; set; }
        public LoginService loginService { get; set; }

        //
        // GET: /User/

        public ActionResult Index()
        {
            ViewBag.User = ActiveRecordMediator<User>.FindByPrimaryKey(2);
            return View();
        }

        [HttpPost]
        public ActionResult Index(string returnUrl)
        {
            string uname= Request["username"];
            string passwd = Request["password"];
            User user = loginService.Authenticate(uname, passwd);

            ViewBag.LoginOK = user != null ? "Đăng nhập thành công!" : "Sai username hoặc password!";
            return View();
        }

    }
}
