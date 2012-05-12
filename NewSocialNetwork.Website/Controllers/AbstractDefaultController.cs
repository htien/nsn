using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public abstract class AbstractDefaultController : AbstractController
    {
        protected internal AbstractDefaultController()
        {
            FrontendService.RequireLoggedIn();
        }
    }
}
