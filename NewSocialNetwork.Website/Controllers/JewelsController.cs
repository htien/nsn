using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;

namespace NewSocialNetwork.Website.Controllers
{
    public class JewelsController : ApplicationController
    {
        //
        // GET: /Jewels/

        [HttpPost]
        public ActionResult ShowFriendRequests()
        {
            Domain.User myUser = sessionManager.GetUser();
            // Lấy ra danh sách các requests
            IList<FriendRequest> friendRequests = frontendService.ListFriendRequests(myUser.UserId);

            ViewBag.FriendRequests = friendRequests;
            return View();
        }

    }
}
