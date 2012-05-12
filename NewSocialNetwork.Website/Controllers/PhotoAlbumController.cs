using System.Collections.Generic;
using System.Web.Mvc;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Manager;

namespace NewSocialNetwork.Website.Controllers
{
    public class PhotoAlbumController : ApplicationController
    {
        public IPhotoAlbumRepository photoAlbumRepo { private get; set; }
        public IFriendRepository friendRepo { private get; set; }

        public PhotoAlbumController()
        {
            ViewBag.PageTitle = "NSN: Photo Album";
        }

        //
        // GET: /PhotoAlbum/

        public ActionResult List()
        {
            User user = sessionManager.GetUser();
            IList<PhotoAlbum> albums = photoAlbumRepo.GetPhotoAlbumByUser(user.UserId);
            int totalAlbum = photoAlbumRepo.GetTotalPhotoAlbumByUser(user.UserId);
            //int totalUsers = photoAlbumRepo.GetNotMutualFriends(1, 2).Count;
            //ViewBag.Total = totalUsers;
            //IList<Friend> u = friendRepo.GetNotMutualFriends(1, 2);
            //ViewBag.user = u;
            int totalfriends = photoAlbumRepo.GetTotalFriendsByUser(1);
            ViewBag.user = totalfriends;
            IList<User> listfriend = photoAlbumRepo.GetListFriendByUser(1);
            ViewBag.listfriend = listfriend;
            IList<Photo> listphotoalbum = photoAlbumRepo.GetPhotoByAlbum(2, 1);
            ViewBag.listphotoalbum = listphotoalbum;
            IList<User> friendsearch = photoAlbumRepo.SearchFriendByName("l", 1);
            ViewBag.friendsearch = friendsearch;
            int totalcomment = photoAlbumRepo.GetTotalComment(1, "Photo", 1);
            ViewBag.totalcomment = totalcomment;
            IList<Comment> allc = photoAlbumRepo.GetAllComment(1, "photo", 1);
            ViewBag.allc = allc;
            int friendRequest = photoAlbumRepo.GetTotalFriendRequestPending(1);
            ViewBag.friendRequest = friendRequest;
            int message = photoAlbumRepo.GetTotalMessagePending(1);
            ViewBag.message = message;
            int activity = photoAlbumRepo.GetTotalActivityPendingRelativeUser(1);
            ViewBag.activity = activity;
            ViewBag.friendRequest = friendRequest;

            IList<FriendList> fl = photoAlbumRepo.GetAllFriendListByUser(1);
            ViewBag.friendList = fl;

            IList<User> userFriendList = photoAlbumRepo.GetFriendInListByUser(1);
            ViewBag.userFriendList = userFriendList;
            IList<CustomRelation> customRelation = photoAlbumRepo.GetRelationshipBetweenUsers(1, 2);
            ViewBag.customRelation = customRelation;
            ViewBag.Albums = albums;

            IList<User> NotMutualFriend = photoAlbumRepo.GetNotMutualFriend(2,1);
            ViewBag.notMutualFriend = NotMutualFriend;

            IList<User> MutualFriend = photoAlbumRepo.GetMutualFriend(2, 1);
            ViewBag.mutualFriend = MutualFriend;
            return View();
        }
    }
}
