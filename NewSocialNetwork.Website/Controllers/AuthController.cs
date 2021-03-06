﻿using System;
using System.Web.Mvc;
using NewSocialNetwork.Repositories;
using NewSocialNetwork.Website.Controllers.Helper;
using NSN.Service.BusinessService;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class AuthController : AbstractController
    {
        public FrontendService frontendService { private get; set; }
        public IUserRepository userRepo { private get; set; }

        public AuthController()
        {
            FrontendService.RequireLoggedOut();
            ViewBag.PageTitle = "NSN: Sign In";
        }

        //
        // GET: /Auth/

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(string firstname, string lastname, byte gender,
            string reg_email, string reg_password, string confirm_password,
            short birthday_year, byte birthday_month, byte birthday_day)
        {
            ResponseMessage msg = new ResponseMessage("Register", RAction.ADD, RStatus.FAIL,
                @"<p>Quá trình đăng ký gặp trắc trở và đã thất bại. Ôi thê thảm quá!</p>");
            try
            {
                int joinDate = DateTimeUtils.UnixTimestamp;
                string ipAddr = Request.UserHostAddress;
                // Add new user
                Domain.User user = frontendService.RegisterNewUser(firstname, lastname, gender, reg_email, reg_password, confirm_password,
                    (birthday_year + "/" + birthday_month + "/" + birthday_day), joinDate, ipAddr);
                // Init user count pending
                frontendService.CreateUserCount(user);
                // Returns
                msg.SetStatusAndMessage(RStatus.SUCCESS,
                    String.Format(@"<p>Register successfully! Welcome to New Social Network.</p>
                                    <p>Your login ID is <strong>{0}</strong>.</p>", user.Email));
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Login(string nsnId, string nsnPasswd)
        {
            ResponseMessage msg = new ResponseMessage("Login", RAction.LOGIN, RStatus.FAIL,
                @"<p class='nsn-popup-msg ui-state-error'>Incorrect ID or password. Access denied.</p>
                  <p>The username you entered does not belong to any account.</p>
                  <p>You can login using any email or username associated with your account.
                     Make sure that it is typed correctly.</p>");
            try
            {
                Domain.User user = frontendService.Login(nsnId, nsnPasswd);
                if (user != null)
                {
                    msg.SetStatusAndMessage(RStatus.SUCCESS, "Authenticated!");
                }
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }

        [HttpPost]
        public JsonResult Logout()
        {
            ResponseMessage msg = new ResponseMessage("Logout", RAction.LOGOUT, RStatus.FAIL, "Cannot logout.");
            try
            {
                frontendService.Logout();
                msg.SetStatusAndMessage(RStatus.SUCCESS, "Logged Out.");
                msg.ReturnedPath = Url.RouteUrl("Root");
            }
            catch (Exception e)
            {
                msg.Message = e.Message;
            }
            return Json(msg);
        }
    }
}
