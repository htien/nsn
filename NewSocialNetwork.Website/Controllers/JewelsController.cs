using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class JewelsController : ApplicationController
    {
        //
        // GET: /Jewels/

        [HttpPost]
        public ActionResult ShowFriendRequests()
        {
            return View();
        }

    }
}
