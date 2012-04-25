using System.Web.Configuration;
using System.Web.Mvc;
using NewSocialNetwork.Website.Main;

namespace NewSocialNetwork.Website.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected internal AbstractController()
        {
            ViewBag.Charset = WebConfigurationManager.AppSettings[CfgKeys.GLOBAL_CHARSET];
            ViewBag.PageTitle = "NSN: NoTitle";
        }
    }
}
