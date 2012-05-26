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

        public ActionResult LikeForFeed(long feedId)
        {
            return RedirectToRoute("Go", new { controller = "Like", action = "ForFeed", feedId = feedId });
        }

        public ActionResult UnlikeForFeed(long feedId)
        {
            return RedirectToRoute("Go", new { controller = "Like", action = "UnlikeForFeed", feedId = feedId });
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
