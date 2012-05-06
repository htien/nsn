using System.Web.Mvc;
using NSN.Common;
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
            charset = config[Globals.GLOBAL_CHARSET];
            ViewBag.NSNAuthorName = config[Globals.NSN_AUTHOR_NAME];
            ViewBag.NSNAuthorEmail = config[Globals.NSN_AUTHOR_EMAIL];
            ViewBag.NSNCodeName = config[Globals.NSN_CODENAME];
            ViewBag.NSNVersion = config[Globals.NSN_VERSION];
            ViewBag.NSNName = config[Globals.NSN_NAME];
            ViewBag.NSNDescription = config[Globals.NSN_DESCRIPTION];
            ViewBag.Charset = charset;
            ViewBag.PageName = config[Globals.PAGE_NAME];
            ViewBag.PageTitle = config[Globals.PAGE_TITLE];
            ViewBag.PageKeywords = config[Globals.PAGE_METATAG_KEYWORDS];
            ViewBag.PageDescription = config[Globals.PAGE_METATAG_DESCRIPTION];
            ViewBag.PageGenerator = config[Globals.PAGE_METATAG_GENERATOR];
        }
    }
}
