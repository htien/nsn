using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Models;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserCountController : ApplicationController
    {
        public IUserCountRepository userCountRepo { get; set; }

        //
        // GET: /UserCount/

        [HttpPost]
        public JsonResult CountPending()
        {
            Domain.User myUser = sessionManager.GetUser();
            UserCount userCount = userCountRepo.FindById(myUser.UserId);
            CountPendingModel model = this.ExtractCountValue(userCount);
            return Json(model);
        }

        private CountPendingModel ExtractCountValue(UserCount userCount)
        {
            return new CountPendingModel()
            {
                MailNew = userCount.MailNew,
                CommentPending = userCount.CommentPending,
                FriendRequest = userCount.FriendRequest
            };
        }
    }
}
