using System;
using System.Web.Mvc;
using NSN.Common;
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
            string uid = (string)filterContext.RouteData.Values["uid"];
            ViewBag.IsMyProfile = uid == null || uid.Equals;

            Domain.User myProfile = sessionManager.GetUser();
            Domain.User userProfile = frontendService.GetUserProfile(uid);
            if (!ViewBag.IsMyProfile && userProfile == null)
            {
                filterContext.Result = new HttpNotFoundResult("Profile not found!");
            }
            else
            {
                if (userProfile == null)
                {
                    userProfile = myProfile;
                }
                ViewBag.UserSession = sessionManager.GetUserSession();
                ViewBag.MyProfile = myProfile;
                ViewBag.UProfile = userProfile;
                ViewBag.MyProfileHasUsername = myProfile.Username != null && myProfile.Username.Length > 0;
                ViewBag.MyLoginId = ViewBag.MyProfileHasUsername ? myProfile.Username : myProfile.Email;
                ViewBag.MyDisplayId = ViewBag.MyProfileHasUsername ? myProfile.Username : Convert.ToString(myProfile.UserId);
                ViewBag.MyAvatarFileName = GeneralUtils.UserImage(myProfile.UserImage, myProfile.Gender);
                ViewBag.UProfileHasUsername = userProfile.Username != null && userProfile.Username.Length > 0;
                ViewBag.ULoginId = ViewBag.UProfileHasUsername ? userProfile.Username : userProfile.Email;
                ViewBag.UDisplayId = ViewBag.UProfileHasUsername ? userProfile.Username : Convert.ToString(userProfile.UserId);
                ViewBag.UAvatarFileName = GeneralUtils.UserImage(userProfile.UserImage, userProfile.Gender);
                ViewBag.ContextPath = Globals.ApplicationPath;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
