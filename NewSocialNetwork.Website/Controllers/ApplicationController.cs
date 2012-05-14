using System;
using System.Web.Mvc;
using NSN.Manager;
using NSN.Service.BusinessService;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class ApplicationController : AbstractDefaultController
    {
        public ISessionManager sessionManager { protected get; set; }
        public FrontendService frontendService { protected get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object _uid = filterContext.RouteData.Values["uid"];
            string uid = _uid != null ? _uid.ToString() : null;

            Domain.User userLogged = sessionManager.GetUser();
            Domain.User userProfile = frontendService.GetUserProfile(uid) ?? userLogged;

            ViewBag.UserSession = sessionManager.GetUserSession();
            ViewBag.UserLogged = userLogged;
            ViewBag.UserProfile = userProfile;
            ViewBag.IsMyProfile = userProfile.UserId == userLogged.UserId;
            ViewBag.HasUsername = userProfile.Username != null && userProfile.Username.Length > 0;
            ViewBag.LoginId = ViewBag.HasUsername ? userProfile.Username : userProfile.Email;
            ViewBag.DisplayId = ViewBag.HasUsername ? userProfile.Username : Convert.ToString(userProfile.UserId);
            ViewBag.AvatarFileName = GeneralUtils.UserImage(userProfile.UserImage, userProfile.Gender);
            base.OnActionExecuting(filterContext);
        }
    }
}
