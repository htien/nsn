using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Service.BusinessService;

namespace NewSocialNetwork.Website.Controllers
{
    public class CommentController : ApplicationController
    {
        public FrontendService frontService { private get; set; }
        public ICommentRepository commentRepo { private get; set; }

        //
        // GET: /Comment/

        [HttpPost]
        public ActionResult AddSave(long fid, string c)
        {
            Domain.User myUser = sessionManager.GetUser();
            long commentId = frontService.AddComment(fid, myUser.UserId, c);
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
