using System.Web.Mvc;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoAlbumController : AbstractController
    {
        public IPhotoAlbumRepository photoAlbumRepo { get; set; }

        public PhotoAlbumController()
        {
            ViewBag.Title = "NSN: Photo Album";
        }
        //
        // GET: /PhotoAlbum/

        public ActionResult Index()
        {
            return View();
        }

    }
}
