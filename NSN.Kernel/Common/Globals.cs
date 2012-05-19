using System;
using System.Data;
using System.Web;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NSN.Kernel;

namespace NSN.Common
{
    public sealed class Globals
    {
        /**************************************************************************/

        public const string CONFIG_FOLDER_NAME = "Config";
        public const string CONFIG_FOLDER_PATH = CONFIG_FOLDER_NAME + "/";
        public static string CONFIG_PHYSICAL_PATH = HttpContext.Current.Server.MapPath(CONFIG_FOLDER_PATH);

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
        public const string glbUserNameRegEx = @"";
        public const string glbScriptFormat = "<script type=\"text/javascript\" src=\"{0}\"></script>";

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
            string strServerPath;
            strServerPath = Request.MapPath(Request.ApplicationPath);
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

        public static Photo GetAlbumAvatar(int albumId)
        {
            IPhotoRepository photoRepo = NSNContext.Current.Container.Resolve<IPhotoRepository>();
            return photoRepo.GetFirstPhotoByAlbum(albumId);
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