using System;
using System.Web.Mvc;
using NSN.Common;
using NSN.Manager;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class ApplicationController : AbstractDefaultController
    {
        public ISessionManager sessionManager { protected get; set; }
        public FrontendService frontendService { protected get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string uid = (string)filterContext.RouteData.Values["uid"];

            Domain.User myProfile = sessionManager.GetUser();
            Domain.User userProfile = frontendService.GetUserProfile(uid);

            ViewBag.IsMyProfile = uid == null
                || uid.Equals(myProfile.UserId.ToString())
                || uid.Equals(myProfile.Username, StringComparison.OrdinalIgnoreCase);

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
                ViewBag.MyProfileHasUsername = Globals.HasUsername(myProfile);
                ViewBag.MyLoginId = Globals.GetLoginId(myProfile);
                ViewBag.MyDisplayId = Globals.GetDisplayId(myProfile);
                ViewBag.MyAvatarFileName = Globals.UserImage(myProfile.UserImage, myProfile.Gender);
                ViewBag.UProfileHasUsername = Globals.HasUsername(userProfile);
                ViewBag.ULoginId = Globals.GetLoginId(userProfile);
                ViewBag.UDisplayId = Globals.GetDisplayId(userProfile);
                ViewBag.UAvatarFileName = Globals.UserImage(userProfile.UserImage, userProfile.Gender);
                ViewBag.ContextPath = Globals.ApplicationPath;
                ViewBag.CountPending = Globals.GetUserCount(myProfile.UserId);
                if (myProfile.UserId != userProfile.UserId)
                {
                    ViewBag.IsFriend = Globals.IsFriend(myProfile.UserId, userProfile.UserId);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
