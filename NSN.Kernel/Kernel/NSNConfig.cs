using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Configuration;
using SaberLily.Utils;

namespace NSN.Kernel
{
    public class NSNConfig : PropertiesConfiguration
    {
        public static NSNConfig Instance { get; private set; }

        public NSNConfig()
        {
            try
            {
                LoadAppSettings();
                LoadGlobalProps();
            }
            catch (Exception e)
            {
                throw new NSNException(e.Message, e);
            }
        }

        public string ApplicationPath
        {
            get { return HttpContext.Current.Server.MapPath("~/"); }
        }

        private void LoadAppSettings()
        {
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;
            foreach (string key in appSettings.AllKeys)
            {
                this[key] = appSettings[key];
            }
        }

        private void LoadGlobalProps()
        {
            this.LoadFromFile(ApplicationPath + "/Config/SystemGlobals.properties", this[CfgKeys.GLOBAL_CHARSET]);
        }

        private new void Reload() { /* Không cho phép xài chức năng này */ }

        public static void SetInstance(NSNConfig config)
        {
            NSNConfig.Instance = config;
        }
    }
}