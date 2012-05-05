using System.Web.Mvc;
using NSN.Framework;
using NSN.Kernel;

namespace NewSocialNetwork.Website.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected internal INSNConfig config;
        protected internal string charset;

        protected internal AbstractController()
        {
            config = NSNConfig.Instance;
            charset = config[CfgKeys.GLOBAL_CHARSET];
            ViewBag.NSNAuthorName = config[CfgKeys.NSN_AUTHOR_NAME];
            ViewBag.NSNAuthorEmail = config[CfgKeys.NSN_AUTHOR_EMAIL];
            ViewBag.NSNCodeName = config[CfgKeys.NSN_CODENAME];
            ViewBag.NSNVersion = config[CfgKeys.NSN_VERSION];
            ViewBag.NSNName = config[CfgKeys.NSN_NAME];
            ViewBag.NSNDescription = config[CfgKeys.NSN_DESCRIPTION];
            ViewBag.Charset = charset;
            ViewBag.PageName = config[CfgKeys.PAGE_NAME];
            ViewBag.PageTitle = config[CfgKeys.PAGE_TITLE];
            ViewBag.PageKeywords = config[CfgKeys.PAGE_METATAG_KEYWORDS];
            ViewBag.PageDescription = config[CfgKeys.PAGE_METATAG_DESCRIPTION];
            ViewBag.PageGenerator = config[CfgKeys.PAGE_METATAG_GENERATOR];
        }
    }
}
