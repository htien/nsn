using System.Web.Mvc;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class FriendController : ApplicationController
    {
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }
        public FriendController()
        {
            ViewBag.PageTitle = "NSN: Friends";
        }

        //
        // GET: /Friend/

        public ActionResult List()
        {
            int totalfriends = photoAlbumRepo.GetTotalFriendsByUser(this.sessionManager.GetUser().UserId);
            ViewBag.totalFriends = totalfriends;
            IList<User> listfriend = photoAlbumRepo.GetListFriendByUser(this.sessionManager.GetUser().UserId);
            ViewBag.listfriend = listfriend;
            return View();
        }

    }
}
