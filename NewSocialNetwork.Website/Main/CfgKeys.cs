using System.Web;

namespace NewSocialNetwork.Website.Main
{
    public static class CfgKeys
    {
        public static string CONFIG_FOLDER_NAME = "Config";
        public static string CONFIG_FOLDER_PATH = CONFIG_FOLDER_NAME + "/";
        public static string CONFIG_PHYSICAL_PATH = HttpContext.Current.Server.MapPath(CONFIG_FOLDER_PATH);

        public static string CTX_NSNCONFIG = "__NSNConfig";

        public static string ASSEMBLY_NSN_ENTITIES = "Lien.NewSocialNetwork.Entities";

        public static string CONNECTION_REMOTE_NAME = "remote";
        public static string CONNECTION_LOCAL_NAME = "local";
        public static string DB_DATASOURCE = "db.datasource";
        public static string DB_PORT = "db.port";
        public static string DB_NAME = "db.name";
        public static string DB_USER = "db.user";
        public static string DB_PASSWORD = "db.passwd";

        public static string SSO_LOGGED = "sso.logged";
        public static string TYPE_SSO = "sso";

        /*** SystemGlobals.properties ***/

        public static string DEBUG = "debug";
        public static string ISREMOTE = "isRemote";

        public static string GLOBAL_VERSION = "webpages:Version";
        public static string GLOBAL_CHARSET = "webpages:Charset";
        public static string GLOBAL_ACTIVERECORD_ISWEBAPP = "active-record:isWebapp";
        public static string GLOBAL_ACTIVERECORD_DEBUG = "active-record:debug";
        public static string GLOBAL_MELEZEMINIFIER_AGGRESSIVE = "meleze-minifier:Aggressive";
        public static string GLOBAL_MELEZEMINIFIER_COMMENTS = "meleze-minifier:Comments";
        public static string GLOBAL_MELEZEMINIFIER_JAVASCRIPT = "meleze-minifier:Javascript";

        public static string NSN_AUTHOR_NAME = "author.name";
        public static string NSN_AUTHOR_EMAIL = "author.email";

        public static string NSN_CODENAME = "nsn.codename";
        public static string NSN_VERSION = "nsn.version";
        public static string NSN_NAME = "nsn.name";
        public static string NSN_DESCRIPTION = "nsn.description";

        public static string PAGE_NAME = "page.name";
        public static string PAGE_TITLE = "page.title";
        public static string PAGE_METATAG_KEYWORDS = "page.metatag.keywords";
        public static string PAGE_METATAG_DESCRIPTION = "page.metatag.description";
        public static string PAGE_METATAG_GENERATOR = "page.metatag.generator";

        public static string DATETIME_FORMAT = "datetime.format";
        public static string DATETIME_DATE_FORMAT = "datetime.date.format";
        public static string DATETIME_TIME_FORMAT = "datetime.time.format";

        public static string I18N_INTERNAL = "i18n.internal";
        public static string I18N_DEFAULT = "i18n.default";

        public static string AUTHENTICATION_TYPE = "authentication.type";
        public static string LOGIN_AUTHENTICATOR = "login.authenticator";
        public static string SSO_IMPLEMENTATION = "sso.implementation";
        public static string SSO_LOGOUT_URL = "sso.logOutUrl";

        public static string ANONYMOUS_USER_ID = "anonymous.userId";
        public static string AUTO_LOGIN_ENABLED = "auto.login.enabled";
    }
}