using System.Web;
using System.Web.Configuration;

namespace NewSocialNetwork.Website.Main
{
    public static class CfgKeys
    {
        public static string CONFIG_FOLDER_NAME = "Config";
        public static string CONFIG_FOLDER_PATH = CONFIG_FOLDER_NAME + "/";
        public static string PHYSICAL_CONFIG_PATH = HttpContext.Current.Server.MapPath(CONFIG_FOLDER_PATH);

        public static string DEBUG = "debug";
        public static string ISREMOTE = "isRemote";

        public static string GLOBAL_VERSION = "webpages:Version";
        public static string GLOBAL_CHARSET = "webpages:Charset";
        public static string GLOBAL_ACTIVERECORD_ISWEBAPP = "active-record:isWebapp";
        public static string GLOBAL_ACTIVERECORD_DEBUG = "active-record:debug";
        public static string GLOBAL_MELEZEMINIFIER_AGGRESSIVE = "meleze-minifier:Aggressive";
        public static string GLOBAL_MELEZEMINIFIER_COMMENTS = "meleze-minifier:Comments";
        public static string GLOBAL_MELEZEMINIFIER_JAVASCRIPT = "meleze-minifier:Javascript";

        public static string CONNECTION_REMOTE_NAME = "remote";
        public static string CONNECTION_LOCAL_NAME = "local";

        public static string DB_DATASOURCE = "db.datasource";
        public static string DB_PORT = "db.port";
        public static string DB_NAME = "db.name";
        public static string DB_USER = "db.user";
        public static string DB_PASSWORD = "db.passwd";

        public static string ASSEMBLY_NSN_ENTITIES = "Lien.NewSocialNetwork.Entities";
    }
}