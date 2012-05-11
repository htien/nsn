﻿using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class AbstractDefaultController : AbstractController
    {
        protected internal AbstractDefaultController()
        {
            FrontendService.RequireLoggedIn();
        }
    }
}
