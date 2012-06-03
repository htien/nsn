using System.Web.Mvc;

namespace NewSocialNetwork.Website.Controllers
{
    public class AjaxController : ApplicationController
    {
        //
        // GET: /Ajax/

        public ActionResult Feeds()
        {
            return RedirectToRoute("Go", new { controller = "Feed", action = "Feeds" });
        }

        public ActionResult Like(string where, long id)
        {
            return RedirectToRoute("Go", new { controller = "Like", action = "Like", where = where, id = id });
        }

        public ActionResult Unlike(string where, long id)
        {
            return RedirectToRoute("Go", new { controller = "Like", action = "Unlike", where = where, id = id });
        }

        public ActionResult PhotoAlbumUploader()
        {
            return RedirectToRoute("Go", new { controller = "PhotoAlbum", action = "Uploader" });
        }

        public ActionResult PhotoUploader(int albumId)
        {
            return RedirectToRoute("Go", new { controller = "Photo", action = "Uploader", albumId = albumId });
        }
    }
}
