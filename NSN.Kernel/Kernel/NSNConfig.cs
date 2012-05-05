using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Configuration;
using NSN.Framework;
using SaberLily.Utils;

namespace NSN.Kernel
{
    public class NSNConfig : PropertiesConfiguration, INSNConfig
    {
        public static INSNConfig Instance { get; set; }

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

        #region INSNConfig Members

        public string ApplicationPath
        {
            get { return HttpContext.Current.Server.MapPath("~/"); }
        }

        #endregion

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
    }
}