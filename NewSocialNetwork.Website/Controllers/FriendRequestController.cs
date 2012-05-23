using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using System;

namespace NewSocialNetwork.Website.Controllers
{
    public class FriendRequestController : ApplicationController
    {
        public IUserRepository userRepo { get; set; }
        public IFriendRepository friendRepo { get; set; }
        public IFriendRequestRepository friendRequestRepo { get; set; }

        //
        // GET: /FriendRequest/

        [HttpPost]
        public ActionResult Add(int friendUserId)
        {
            Domain.User myUser = sessionManager.GetUser();
            if (friendUserId == myUser.UserId
                || friendRepo.IsFriend(myUser.UserId, friendUserId)
                || friendRequestRepo.IsConfirmingFriendRequest(myUser.UserId, friendUserId))
            {
                return View();
            }
            Domain.User requestingFriend = userRepo.FindById(friendUserId);
            ViewBag.RequestingFriend = requestingFriend;
            return View();
        }

        [HttpPost]
        public JsonResult AddSave(int friendUserId, string message)
        {
            ResponseMessage msg = new ResponseMessage("RequestFriend", RAction.ADD, RStatus.FAIL,
                "Cannot process your request friend.");
            try
            {
                // Add new request
                frontendService.AddRequestFriend(friendUserId, message);
                // Add pending
                frontendService.IncreaseCountOfFriendRequest(friendUserId);
                // Returns
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Request friend sent.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
