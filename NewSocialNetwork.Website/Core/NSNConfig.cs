using System;
using System.Collections.Specialized;
using System.Web.Configuration;
using NewSocialNetwork.Website.Exceptions;
using NewSocialNetwork.Website.Main;
using SaberLily.Utils;
using System.Web;

namespace NewSocialNetwork.Website.Core
{
    public class NSNConfig : PropertiesConfiguration
    {
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
    }
}