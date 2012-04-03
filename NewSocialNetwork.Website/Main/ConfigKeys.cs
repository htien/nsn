using System.Web;

namespace NewSocialNetwork.Website.Main
{
    public static class ConfigKeys
    {
        public static string CONFIG_FOLDER_NAME = "Config";
        public static string CONFIG_FOLDER_PATH = CONFIG_FOLDER_NAME + "/";
        public static string PHYSICAL_CONFIG_PATH = HttpContext.Current.Server.MapPath(CONFIG_FOLDER_PATH);

        public static System.Text.Encoding ENCODING = System.Text.Encoding.UTF8;
        public static string TEXT_ENCODING = "UTF-8";
    }
}