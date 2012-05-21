using System;
using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserTweetController : ApplicationController
    {
        public IUserTweetRepository userTweetRepo { private get; set; }

        public UserTweetController()
        {
            ViewBag.PageTitle = "NSN: UserTweet";
        }

        //
        // GET: /UserTweet/

        [HttpPost]
        public JsonResult Post(int uid, string inputText)
        {
            ResponseMessage msg = new ResponseMessage("UserPostTweet", RAction.ADD, RStatus.FAIL,
                "<p>Incomplete post.</p>");
            try
            {
                frontendService.PostUserTweet(uid, inputText);
                msg.SetStatusAndMessage(RStatus.SUCCESS, "<p>Posted.</p>");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
