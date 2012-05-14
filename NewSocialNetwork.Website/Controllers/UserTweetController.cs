using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Kernel.Manager;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class UserTweetController : ApplicationController
    {
        public IUserTweetRepository userTweetRepo { private get; set; }

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
                "<p>Incomplete post.</p>");
            try
            {
                Domain.User user = sessionManager.GetUser();
                inputText = inputText.Trim();
                if (String.IsNullOrEmpty(inputText))
                {
                    throw new Exception("<p>Invalid content.</p>");
                }
                else
                {
                    userTweetRepo.Add(user, inputText.Trim());
                    msg.SetStatusAndMessage(RStatus.SUCCESS, "<p>Posted.</p>");
                }
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

        public ActionResult Posts()
        {
            IList<FeedItem> feedItems = frontendService.LoadFeedItems(0, 20);
            ViewBag.FeedItems = feedItems;
            return View();
        }

    }
}
