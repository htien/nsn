using System.Collections.Generic;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Castle.Core.Logging;

using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Website.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ILogger Logger { get; set; }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            IList<Country> Countries = Country.FindAll();
            CountryChild Alaska = CountryChild.Find(2);
            Entities.User user = Entities.User.Find(1);
            Comment.FindAll();

            ViewBag.Welcome = "New Social Network";
            ViewBag.Countries = Countries;
            ViewBag.Alaska = Alaska;
            ViewBag.User = user;

            return View();
        }
    }
}