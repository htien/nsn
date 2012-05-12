using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Manager;
using System.Collections.Generic;

namespace NewSocialNetwork.Website.Controllers
{
    public class ProfileController : ApplicationController
    {        
        public IPhotoAlbumRepository albumRepo { private get; set; }
        
        public ProfileController()
        {
            ViewBag.PageTitle = "NSN: Profile";
        }

        //
        // GET: /Profile/

        public ActionResult Info()
        {
            ViewBag.PageTitle += " Info";
            //Get User Info            
            Domain.User userLogin = sessionManager.GetUser();
            ViewBag.UserLogin = userLogin;
            //Get List Frieng By User            
            IList<User> listFriend = albumRepo.GetListFriendByUser(userLogin.UserId);
            ViewBag.ListFriend = listFriend;
            return View();
        }
    }
}
