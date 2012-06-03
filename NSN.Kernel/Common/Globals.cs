using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Common.Utilities;
using NSN.Kernel;
using SaberLily.Utils;

namespace NSN.Common
{
    public sealed class Globals
    {
        /**************************************************************************/

        public const string CONFIG_FOLDER_NAME = "Config";
        public const string CONFIG_FOLDER_PATH = CONFIG_FOLDER_NAME + "/";
        public static string CONFIG_PHYSICAL_PATH = HttpContext.Current.Server.MapPath(CONFIG_FOLDER_PATH);
        public const string RESOURCE = "~/static/";
        public const string RESOURCE_IMAGES = "~/static/images/";
        public const string RESOURCE_AVATARS = RESOURCE_IMAGES + "avatars/";
        public const string RESOURCE_PHOTOS = RESOURCE_IMAGES + "photos/";
        public const string RESOURCE_SMILES = "~/static/smiles/";

        public const string CTX_NSNCONTAINER = "__NSNContainer";
        public const string CTX_NSNCONFIG = "__NSNConfig";

        public const string ASSEMBLY_NSN_ENTITIES = "Lien.NSN.Kernel";
        public const string ASSEMBLY_NSN_WEBSITE = "Lien.NewSocialNetwork.Website";

        public const string CONNECTION_REMOTE_NAME = "remote";
        public const string CONNECTION_LOCAL_NAME = "local";
        public const string DB_DATASOURCE = "db.datasource";
        public const string DB_PORT = "db.port";
        public const string DB_NAME = "db.name";
        public const string DB_USER = "db.user";
        public const string DB_PASSWORD = "db.passwd";

        public const string NHIBERNATE_SESSION_KEY = "nsn.nhibernate.session";
        public const string USER_SESSION = "userSession";

        public const string SSO_LOGGED = "sso.logged";
        public const string TYPE_SSO = "sso";

        /**************************************************************************/

        public const string glbEmailRegEx = @"^\s*[a-zA-Z0-9_%+#&'*/=^`{|}~-](?:\.?[a-zA-Z0-9_%+#&'*/=^`{|}~-])*@(?:[a-zA-Z0-9_](?:(?:\.?|-*)[a-zA-Z0-9_])*\.[a-zA-Z]{2,9}|\[(?:2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?:2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?:2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?:2[0-4]\d|25[0-5]|[01]?\d\d?)])\s*$";
        public const string glbUserNameRegEx = @"^[A-Za-z][A-Za-z0-9_]{4,30}$";
        public const string glbScriptFormat = "<script type=\"text/javascript\" src=\"{0}\"></script>";

        /**************************************************************************/

        public const string SESSIONKEY_UPLOADED_PHOTOS = "uploaded_images_for_";

        /*** SystemGlobals.properties *********************************************/

        public const string DEBUG = "debug";
        public const string ISREMOTE = "isRemote";

        public const string GLOBAL_VERSION = "webpages:Version";
        public const string GLOBAL_CHARSET = "webpages:Charset";
        public const string GLOBAL_ACTIVERECORD_ISWEBAPP = "active-record:isWebapp";
        public const string GLOBAL_ACTIVERECORD_DEBUG = "active-record:debug";
        public const string GLOBAL_MELEZEMINIFIER_AGGRESSIVE = "meleze-minifier:Aggressive";
        public const string GLOBAL_MELEZEMINIFIER_COMMENTS = "meleze-minifier:Comments";
        public const string GLOBAL_MELEZEMINIFIER_JAVASCRIPT = "meleze-minifier:Javascript";

        public const string NSN_AUTHOR_NAME = "author.name";
        public const string NSN_AUTHOR_EMAIL = "author.email";

        public const string NSN_CODENAME = "nsn.codename";
        public const string NSN_VERSION = "nsn.version";
        public const string NSN_NAME = "nsn.name";
        public const string NSN_DESCRIPTION = "nsn.description";

        public const string PAGE_NAME = "page.name";
        public const string PAGE_TITLE = "page.title";
        public const string PAGE_METATAG_KEYWORDS = "page.metatag.keywords";
        public const string PAGE_METATAG_DESCRIPTION = "page.metatag.description";
        public const string PAGE_METATAG_GENERATOR = "page.metatag.generator";

        public const string DATETIME_FORMAT = "datetime.format";
        public const string DATETIME_DATE_FORMAT = "datetime.date.format";
        public const string DATETIME_TIME_FORMAT = "datetime.time.format";

        public const string I18N_INTERNAL = "i18n.internal";
        public const string I18N_DEFAULT = "i18n.default";

        public const string AUTHENTICATION_TYPE = "authentication.type";
        public const string LOGIN_AUTHENTICATOR = "login.authenticator";
        public const string SSO_IMPLEMENTATION = "sso.implementation";
        public const string SSO_LOGOUT_URL = "sso.logOutUrl";

        public const string ANONYMOUS_USER_ID = "anonymous.userId";
        public const string AUTO_LOGIN_ENABLED = "auto.login.enabled";

        /**************************************************************************/

        private static string _applicationPath;
        private static string _applicationMapPath;
        private static string _imagePath;

        public static string ApplicationPath
        {
            get
            {
                if (_applicationPath == null && HttpContext.Current != null)
                {
                    _applicationPath = HttpContext.Current.Request.ApplicationPath.ToLowerInvariant();
                }
                return _applicationPath;
            }
        }

        public static string ApplicationMapPath
        {
            get
            {
                return _applicationMapPath ??
                    (_applicationMapPath = System.AppDomain.CurrentDomain.BaseDirectory.Substring(0, System.AppDomain.CurrentDomain.BaseDirectory.Length - 1).Replace("/", "\\"));
            }
        }

        public static string ImagePath
        {
            get
            {
                if (_imagePath == null)
                    _imagePath = ApplicationPath + "/static/images/";
                return _imagePath;
            }
        }

        public static void Redirect(string url, bool endResponse)
        {
            try
            {
                HttpContext.Current.Response.Redirect(url, endResponse);
            }
            catch { }
        }

        /// <summary>
        /// Gets the absolute server path.
        /// </summary>
        /// <param name="Request">The request.</param>
        /// <returns>absolute server path</returns>
        public static string GetAbsoluteServerPath(HttpRequest Request)
        {
            string strServerPath = Request.MapPath(Request.ApplicationPath);
            if (!strServerPath.EndsWith("\\"))
            {
                strServerPath += "\\";
            }
            return strServerPath;
        }

        public static bool HasUsername(User user)
        {
            return user.Username != null && user.Username.Length > 0;
        }

        public static string GetLoginId(User user)
        {
            return HasUsername(user) ? user.Username : user.Email;
        }

        public static string GetDisplayId(User user)
        {
            return HasUsername(user) ? user.Username : Convert.ToString(user.UserId);
        }

        public static DateTime GetBirthday(string birthday)
        {
            if (birthday == null || birthday.Trim().Length != 8)
            {
                throw new Exception("Invalid birthday string.");
            }
            int year = Int32.Parse(birthday.Substring(0, 4));
            int month = Int32.Parse(birthday.Substring(4, 2));
            int day = Int32.Parse(birthday.Substring(6, 2));
            return new DateTime(year, month, day);
        }

        public static string ShowBirthday(string birthday)
        {
            return ShowBirthday(GetBirthday(birthday));
        }

        public static string ShowBirthday(DateTime birthday)
        {
            return birthday.ToString("MMM dd, yyyy");
        }

        public static DateTime GetDateTime(int timestamp)
        {
            TimeZoneInfo utc7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime dt = DateTimeUtils.ConvertFromUnixTimestamp(timestamp);
            return TimeZoneInfo.ConvertTimeFromUtc(dt, utc7);
        }

        public static string ShowDate(int timestamp)
        {
            return GetDateTime(timestamp).ToString("MMMM dd, yyyy");
        }

        public static string ShowDateTime(int timestamp)
        {
            DateTime dt = GetDateTime(timestamp);
            return String.Format("{0} at {1}", dt.ToString("MMMM dd, yyyy"), dt.ToString("hh:mmtt").ToLower());
        }

        public static string ShowFullDateTime(int timestamp)
        {
            DateTime dt = GetDateTime(timestamp);
            return String.Format("{0} at {1}", dt.ToString("dddd, MMMM dd, yyyy"), dt.ToString("hh:mm:sstt").ToLower());
        }

        public static string Gender(int gender)
        {
            switch (gender)
            {
                case 1: return "Male";
                case 2: return "Female";
                default: return "Unspecified";
            }
        }

        public static string UserImage(User user)
        {
            return UserImage(user.UserImage, user.Gender);
        }

        public static string UserImage(string userImage, int gender)
        {
            if (userImage == null || userImage.Length == 0)
            {
                switch (gender)
                {
                    case GenderType.MALE: return "default_medium_male.gif";
                    case GenderType.FEMALE: return "default_medium_female.gif";
                    default: return "";
                }
            }
            else
            {
                return userImage;
            }
        }

        public static bool IsFriend(int userId, int friendUserId)
        {
            IFriendRepository friendRepo = NSNContext.Current.Container.Resolve<IFriendRepository>();
            return friendRepo.IsFriend(userId, friendUserId);
        }

        public static bool IsLikeForFeed(string typeId, int itemId, int userId)
        {
            ILikeRepository likeRepo = NSNContext.Current.Container.Resolve<ILikeRepository>();
            return likeRepo.Exists(typeId, itemId, userId);
        }

        public static bool IsLikeForPhoto(int photoId, int userId)
        {
            ILikeRepository likeRepo = NSNContext.Current.Container.Resolve<ILikeRepository>();
            return likeRepo.Exists(NSNType.PHOTO, photoId, userId);
        }

        public static bool IsConfirmingFriendRequest(int userId, int friendUserId)
        {
            IFriendRequestRepository friendRequestRepo = NSNContext.Current.Container.Resolve<IFriendRequestRepository>();
            return friendRequestRepo.IsConfirmingFriendRequest(userId, friendUserId);
        }

        public static IList<Comment> ListComments(string typeId, int itemId, int userId)
        {
            ICommentRepository commentRepo = NSNContext.Current.Container.Resolve<ICommentRepository>();
            return commentRepo.ListComments(typeId, itemId, userId);
        }

        public static IList<Photo> GetNewPhotosByTimestamp(int timestamp, int size)
        {
            IPhotoRepository photoRepo = NSNContext.Current.Container.Resolve<IPhotoRepository>();
            return photoRepo.GetPhotosByTimestamp(timestamp, size);
        }

        public static IList<User> GetNotMutualFriends(int userId, int size = 5)
        {
            IUserRepository userRepo = NSNContext.Current.Container.Resolve<IUserRepository>();
            IList<int> friendUserIds = userRepo.ListFriendUserIds(userId);
            Random rnd = new Random();
            int chosenFriendUserId = friendUserIds[rnd.Next(friendUserIds.Count)];
            return userRepo.ListNotMutualFriends(userId, chosenFriendUserId);
        }

        public static IList<User> GetMutualFriends(int userId, int friendUserId, int size = 10)
        {
            IUserRepository userRepo = NSNContext.Current.Container.Resolve<IUserRepository>();
            return userRepo.ListMutualFriends(userId, friendUserId, size);
        }

        public static int GetTotalPhoto(int albumId)
        {
            IPhotoRepository photoRepo = NSNContext.Current.Container.Resolve<IPhotoRepository>();
            return photoRepo.GetTotalPhoto(albumId);
        }

        public static int TotalPhotoByTimestamp(int albumId, int timestamp)
        {
            IPhotoRepository photoRepo = NSNContext.Current.Container.Resolve<IPhotoRepository>();
            return photoRepo.TotalPhotoByTimestamp(albumId, timestamp);
        }

        public static int GetTotalLike(string typeId, int itemId)
        {
            ILikeRepository likeRepo = NSNContext.Current.Container.Resolve<ILikeRepository>();
            return likeRepo.TotalLike(typeId, itemId);
        }

        public static Photo GetAlbumAvatar(int albumId)
        {
            IPhotoRepository photoRepo = NSNContext.Current.Container.Resolve<IPhotoRepository>();
            return photoRepo.GetFirstPhotoByAlbum(albumId);
        }

        public static UserCount GetUserCount(int userId)
        {
            IUserCountRepository userCountRepo = NSNContext.Current.Container.Resolve<IUserCountRepository>();
            return userCountRepo.FindById(userId);
        }

        public static LinkInfo GetLinkInfo(string url, int size = 10)
        {
            SiteReader reader = new SiteReader(url.Trim());
            SiteInfo info = reader.SiteInfo;
            Uri address = info.Address;
            string hostUrl = address.IsDefaultPort
                ? String.Format("{0}://{1}",address.Scheme, address.Host)
                : String.Format("{0}://{1}:{2}", address.Scheme, address.Host, address.Port);
            string parsedUrl = hostUrl + address.PathAndQuery;

            List<string> imageUrls = new List<string>();
            int count = 0;
            foreach (string imgUrl in info.ImageUrls)
            {
                string parsedImgUrl = null;
                if (!Regex.IsMatch(imgUrl, @".jpg$", RegexOptions.IgnoreCase))
                    continue;
                if (!Regex.IsMatch(imgUrl, @"^(http:|https:|ftp:)//", RegexOptions.IgnoreCase))
                    parsedImgUrl = !Regex.IsMatch(imgUrl, @"^/")
                        ? String.Format("{0}/{1}", hostUrl + address.LocalPath, imgUrl)
                        : String.Format("{0}{1}", hostUrl, imgUrl);
                imageUrls.Add((parsedImgUrl != null) ? parsedImgUrl : imgUrl);
                if (count < size - 1)
                    count++;
                else
                    break;
            }

            return new LinkInfo()
            {
                Url = parsedUrl,
                Title = info.MetaTitle,
                Description = info.MetaDescription,
                ImageUrls = imageUrls.ToArray()
            };
        }

        public static bool IsBrokenLink(string url)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;
            try
            {
                Uri uri = new Uri(url, UriKind.Absolute);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "HEAD";
                request.KeepAlive = false;
                request.Timeout = 5 * 1000;
                response = (HttpWebResponse)request.GetResponse();
                return !(response.StatusCode == HttpStatusCode.OK);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        public static ImageInfo SaveImageInPlace(HttpPostedFileBase file, string intoPath)
        {
            if (file.ContentLength == 0)
            {
                throw new Exception("Content-Length equal zero.");
            }
            string extRegex = @"^(\.jpe?g|\.png|\.gif)$";
            string mimeRegex = @"^image\/(jpeg|png|gif)$";
            if (!Regex.IsMatch(file.ContentType, mimeRegex, RegexOptions.IgnoreCase)
                || !Regex.IsMatch(Path.GetExtension(file.FileName), extRegex, RegexOptions.IgnoreCase))
            {
                throw new Exception("Not image in format. (only for .jpg, .png, .gif)");
            }
            int sizeInKB = file.ContentLength / 1024;
            if (sizeInKB > 1024)
            {
                throw new Exception("Size is limited 2MB.");
            }
            string fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
            string folderUpload = Globals.ApplicationMapPath + intoPath.Replace("/", "\\");
            string linkAccess = Path.Combine(folderUpload, fileName);
            int uploadTimestamp = DateTimeUtils.UnixTimestamp;
            file.SaveAs(linkAccess);

            return new ImageInfo()
            {
                FileName = fileName,
                FileSize = file.ContentLength,
                MimeType = file.ContentType,
                LinkAccess = linkAccess,
                UploadTimestamp = uploadTimestamp,
                ImageStream = file.InputStream
            };
        }

        public static void RemoveImageInPlace(string imageUrl)
        {
            System.IO.File.Delete(imageUrl.Replace("/", "\\"));
        }

        public static string HtmlEncode(string text)
        {
            return Microsoft.Security.Application.Encoder.HtmlEncode(text);
        }

        public static string UrlDecode(string text)
        {
            string decodedString = HttpUtility.UrlDecode(text, Encoding.GetEncoding("ISO-8859-1"));
            decodedString = Regex.Replace(decodedString, @"^\n+", "");
            decodedString = Regex.Replace(decodedString, @"\r+", "");
            decodedString = Regex.Replace(decodedString, @" +", " ");
            return decodedString;
        }

        public static string ApplyHtmlFrom(string text)
        {
            return text.Replace("&#10;", "<br />").Replace("&#13;", "");
        }

        public static string TruncateString(string strIn, int maxSize)
        {
            if (strIn.Length > maxSize)
            {
                StringBuilder sb = new StringBuilder(strIn.Substring(0, maxSize - 3));
                int indexOfLastSpace = sb.ToString().LastIndexOf(" ");
                int lengthFromLastIndex = sb.Length - indexOfLastSpace;
                sb.Remove(indexOfLastSpace, lengthFromLastIndex).Append("...");
                return sb.ToString();
            }
            return strIn;
        }

        public static string PhotoUrl(string filename)
        {
            return RESOURCE_PHOTOS + filename;
        }

        public static string AvatarUrl(string filename)
        {
            return RESOURCE_AVATARS + filename;
        }

        public static string SmileUrl(string filename, string provider = "pidgin", string type = "default")
        {
            return RESOURCE_SMILES + provider + "/" + type + "/" + filename;
        }

        /// <summary>
        /// Converts the datareader to dataset.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>the dataset instance</returns>
        public static DataSet ConvertDataReaderToDataSet(IDataReader reader)
        {
            // add datatable to dataset
            var objDataSet = new DataSet();
            objDataSet.Tables.Add(ConvertDataReaderToDataTable(reader));
            return objDataSet;
        }

        /// <summary>
        /// Converts the datareader to datatable.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>the datatable instance</returns>
        public static DataTable ConvertDataReaderToDataTable(IDataReader reader)
        {
            // create datatable from datareader
            var objDataTable = new DataTable();
            int intFieldCount = reader.FieldCount;
            int intCounter;
            for (intCounter = 0; intCounter <= intFieldCount - 1; intCounter++)
            {
                objDataTable.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
            }
            // populate datatable
            objDataTable.BeginLoadData();
            var objValues = new object[intFieldCount];
            while (reader.Read())
            {
                reader.GetValues(objValues);
                objDataTable.LoadDataRow(objValues, true);
            }
            reader.Close();
            objDataTable.EndLoadData();
            return objDataTable;
        }
    }
}