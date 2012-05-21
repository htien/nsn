using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class CommentController : ApplicationController
    {
        public ICommentRepository commentRepo { private get; set; }

        //
        // GET: /Comment/

        [HttpPost]
        public ActionResult AddSave(long fid, string c)
        {
            Domain.User myUser = sessionManager.GetUser();
            long commentId = frontendService.AddComment(fid, myUser.UserId, c);
            Comment comment = commentRepo.FindById(commentId);
            if (comment != null)
            {
                ViewBag.Comment = comment;
                return View();
            }
            return null;
        }

    }
}
