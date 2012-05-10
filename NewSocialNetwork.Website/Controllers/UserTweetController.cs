using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserTweetController : AbstractDefaultController
    {
        public UserTweetController()
        {
            ViewBag.PageTitle = "NSN: Posts";
        }

        //
        // GET: /UserTweet/

        public ActionResult Posts()
        {
            return View();
        }

    }
}
