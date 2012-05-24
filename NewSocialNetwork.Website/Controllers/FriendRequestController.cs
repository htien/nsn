using System;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    public class FriendRequestController : ApplicationController
    {
        public IUserRepository userRepo { get; set; }
        public IFriendRepository friendRepo { get; set; }
        public IFriendRequestRepository friendRequestRepo { get; set; }
        public IFeedRepository feedRepo { private get; set; }

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

        [HttpPost]
        public JsonResult Accept(int requestId)
        {
            ResponseMessage msg = new ResponseMessage("Friend", RAction.ADD, RStatus.FAIL,
                "Cannot accept this friend request.");
            try
            {
                int timestamp = DateTimeUtils.UnixTimestamp;
                // Chấp nhận làm bạn
                Friend friend = frontendService.AcceptFriendRequest(requestId, timestamp);
                // Giảm số FriendRequest trong bảng UserCount
                frontendService.IncreaseCountOfFriendRequest(sessionManager.GetUser().UserId, -1);
                // Thông báo lên Feed
                feedRepo.Add(NSNType.FRIEND, friend.FriendId, friend.User.UserId, friend.FriendUser.UserId, timestamp);

                msg.SetStatusAndMessage(RStatus.SUCCESS, "Accepted.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
