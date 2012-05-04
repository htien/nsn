using System.Collections.Generic;
using System.Web.Script.Serialization;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.User]", "dbo", Lazy = true)]
    public class User : ActiveRecordValidationBase<User>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "UserId")]
        public virtual int UserId { get; set; }

        [BelongsTo("UserGroupId", NotNull = true)]
        public virtual UserGroup UserGroup { get; set; }

        [Property("Email", Unique = true, Length = 255, NotNull = false)]
        public virtual string Email { get; set; }

        [Property("Username", Unique = true, Length = 100, NotNull = false)]
        public virtual string Username { get; set; }

        [Property("Passwd", Length = 1024, NotNull = false)]
        public virtual string Password { get; set; }

        [Property("FullName", Length = 100, NotNull = true)]
        public virtual string FullName { get; set; }

        [Property("Gender", NotNull = true, Default = "0")]
        public virtual byte Gender { get; set; }

        [Property("Birthday", Length = 10, NotNull = false)]
        public virtual string Birthday { get; set; }

        [BelongsTo("CountryIso", NotNull = false)]
        public virtual Country Country { get; set; }

        [Property("Joined", NotNull = true)]
        public virtual int Joined { get; set; }

        [Property("LastLogin", NotNull = true)]
        public virtual int LastLogin { get; set; }

        [Property("LastActivity", NotNull = true)]
        public virtual int LastActivity { get; set; }

        [Property("UserImage", Length = 75, NotNull = false)]
        public virtual string UserImage { get; set; }

        [Property("LastIpAddress", Length = 15, NotNull = false)]
        public virtual string LastIpAddress { get; set; }

        #endregion

        #region Relationship

        /// <summary>
        /// Kiểm tra 1 sự kiện nhanh chóng bằng lấy giá trị trong bảng này.
        /// Dùng cho việc hiển thị notification phía trên thanh topbar.
        /// </summary>
        [OneToOne(Fetch = FetchEnum.Join)]
        public virtual UserCount UserCount { get; set; }

        /// <summary>
        /// Lấy ra danh sách comment của user đã nhận từ người dùng khác.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Comment), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Comment> Comments { get; set; }

        /// <summary>
        /// Lấy ra danh sách comment mà user đã comment, nghĩa là truy vấn
        /// chủ nhân của các comment.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Comment), ColumnKey = "OwnerUserId", Lazy = true)]
        public virtual IList<Comment> OwnerComments { get; set; }

        /// <summary>
        /// Bảng Feed liệt kê tất cả những sự kiện trên 1 bức tường (wall)
        /// của người dùng.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Feed), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Feed> Feeds { get; set; }

        /// <summary>
        /// Danh sách những người đã kết bạn.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Friend), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Friend> Friends { get; set; }

        /// <summary>
        /// Danh sách định nghĩa các thể loại friend, ví dụ: Close Friend,
        /// Family, Acquaintance.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(FriendList), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<FriendList> FriendLists { get; set; }

        /// <summary>
        /// Danh sách các yêu cầu kết bạn của người dùng.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(FriendRequest), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<FriendRequest> FriendRequests { get; set; }

        /// <summary>
        /// Danh sách tất cả những mục đã like của người dùng.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Like), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Like> Likes { get; set; }

        [ScriptIgnore]
        [HasMany(typeof(LikeCache), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<LikeCache> LikeCaches { get; set; }

        /// <summary>
        /// Liệt kê các link người dùng đã post.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Link), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Link> Links { get; set; }

        /// <summary>
        /// Liệt kê các link người bạn đã post.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Link), ColumnKey = "FriendUserId", Lazy = true)]
        public virtual IList<Link> FriendLinks { get; set; }

        /// <summary>
        /// Danh sách các mail đã gửi.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Mail), ColumnKey = "OwnerUserId", Lazy = true)]
        public virtual IList<Mail> OwnerMails { get; set; }

        /// <summary>
        /// Danh sách các mail của người nhận.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Mail), ColumnKey = "ViewerUserId", Lazy = true)]
        public virtual IList<Mail> ViewerMails { get; set; }

        /// <summary>
        /// Danh sách các folder/phân loại mail của người dùng.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(MailFolder), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<MailFolder> MailFolders { get; set; }
        
        /// <summary>
        /// Danh sách các photo người dùng đã post.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Photo), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Photo> Photos { get; set; }

        /// <summary>
        /// Danh sách các photo người dùng đã nhận từ người khác.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Photo), ColumnKey = "FriendUserId", Lazy = true)]
        public virtual IList<Photo> FriendPhotos { get; set; }

        /// <summary>
        /// Danh sách các Photo Album người dùng đã tạo.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(PhotoAlbum), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<PhotoAlbum> PhotoAlbums { get; set; }

        /// <summary>
        /// Danh sách các Session.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(Session), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<Session> Sessions { get; set; }

        /// <summary>
        /// Danh sách các tweet của người dùng đã post.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(UserTweet), ColumnKey = "UserId", Lazy = true)]
        public virtual IList<UserTweet> UserTweets { get; set; }

        /// <summary>
        /// Danh sách các tweet của người bạn đã post cho người dùng.
        /// </summary>
        [ScriptIgnore]
        [HasMany(typeof(UserTweet), ColumnKey = "FriendUserId", Lazy = true)]
        public virtual IList<UserTweet> FriendUserTweets { get; set; }

        #endregion

        public User() { }
    }
}
