using System;
using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Manager;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserTweetController : AbstractDefaultController
    {
        public ISessionManager sessionManager { private get; set; }
        public IUserTweetRepository userTweetRepo { private get; set; }
        public FrontendService frontService { private get; set; }

        public UserTweetController()
        {
            ViewBag.PageTitle = "NSN: Posts";
        }

        //
        // GET: /UserTweet/

        [HttpPost]
        public JsonResult Post(string inputText)
        {
            ResponseMessage msg = new ResponseMessage("UserPostTweet", RAction.ADD, RStatus.FAIL,
                "<p>Gửi thất bại.</p>");
            try
            {
                Domain.User user = sessionManager.GetUser();
                userTweetRepo.Add(user, inputText.Trim());
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Gửi thành công.");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

        public ActionResult Posts()
        {
            return View();
        }

    }
}
