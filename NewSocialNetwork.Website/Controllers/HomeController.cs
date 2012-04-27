using System;
using System.Web;
using System.Web.Mvc;
using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;
using NHibernate.Criterion;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : AbstractController
    {
        public IUserRepository userRepo { private get; set; }

        public HomeController()
        {
            ViewBag.PageTitle = "NSN: Stream";
        }

        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Stream(string userid)
        {
            if (userid == null || userid.Length == 0)
                userid = ""; // Lấy userid của người đã đăng nhập!!!

            DetachedCriteria criteria = DetachedCriteria.For<User>()
                .Add(Restrictions.Eq(true ? "UserId" : "Username", Convert.ToInt32(userid)));
            if (!userRepo.Exists(criteria))
            {
                throw new HttpException(404, "Not found");
            }

            return View();
        }
    }
}
