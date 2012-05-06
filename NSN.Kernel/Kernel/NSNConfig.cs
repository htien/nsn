using System;
using System.Collections.Specialized;
using System.Web.Configuration;
using NSN.Common;
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
            get { return Globals.ApplicationPath; }
        }

        public string ApplicationMapPath
        {
            get { return Globals.ApplicationMapPath; }
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
            this.LoadFromFile(ApplicationMapPath + "/Config/SystemGlobals.properties", this[Globals.GLOBAL_CHARSET]);
        }

        private new void Reload() { /* Không cho phép xài chức năng này */ }
    }
}