using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Castle.Windsor;

using SaberLily.Web.Factory;

using NewSocialNetwork.Website.Installers;

namespace NewSocialNetwork.Website.Main
{
    /// <summary>
    /// Lớp khởi tạo WindsorContainer, dùng làm nền tảng để kết hợp những module khác.
    /// </summary>
    public sealed class Bootstrap
    {
        private static IWindsorContainer container;
        public static bool isInitialized = false;

        public static Bootstrap Instance
        {
            get { return BootstrapHolder.Instance; }
        }

        /// <summary>
        /// Hàm dựng private ngăn cản việc khởi tạo từ class khác.
        /// </summary>
        private Bootstrap() { }

        /// <summary>
        /// Khởi tạo các thành phần cơ bản cho web.
        /// </summary>
        public void Init()
        {
            if (isInitialized)
            {
                //return;
            }

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitContainer();
            InitControllerFactory();
            InitActiveRecord();

            isInitialized = true;
        }

        /// <summary>
        /// Hủy và thu hồi tài nguyên web.
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }

        private void InitContainer()
        {
            container = new WindsorContainer().Install(
                new LoggerInstaller(),
                new ControllersInstaller()
            );
        }

        private void InitControllerFactory()
        {
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        private void InitActiveRecord()
        {
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;
            ConnectionStringSettingsCollection cfgSettings = WebConfigurationManager.ConnectionStrings;
            NameValueCollection db = WebConfigurationManager.GetSection("databaseSettings", "/" + ConfigKeys.CONFIG_FOLDER_PATH + "Database.config") as NameValueCollection;
            NameValueCollection ar = WebConfigurationManager.GetSection("activeRecordSettings", "/" + ConfigKeys.CONFIG_FOLDER_PATH + "Database.config") as NameValueCollection;

            IDictionary<string, string> settings = new Dictionary<string, string>();
            foreach (string key in ar.AllKeys)
            {
                settings[key] = ar[key];
            }

            if ((!settings.ContainsKey("connection.connection_string_name") || settings["connection.connection_string_name"].Length == 0) &&
                (!settings.ContainsKey("connection.connection_string") || settings["connection.connection_string"].Length == 0))
            {
                bool isRemote = Convert.ToBoolean(appSettings["isRemote"]);
                string @connectionString = (isRemote ? cfgSettings["remote"] : cfgSettings["local"]).ConnectionString;
                connectionString = isRemote
                        ? string.Format(connectionString,
                                db["db.datasource"], db["db.port"], db["db.name"],
                                db["db.user"], db["db.passwd"])
                        : string.Format(connectionString,
                                db["db.datasource"], db["db.port"], db["db.name"]);
                settings["connection.connection_string"] = connectionString;
            }

            InPlaceConfigurationSource configSource = new InPlaceConfigurationSource();
            configSource.Add(typeof(ActiveRecordBase), settings);
            configSource.IsRunningInWebApp = Convert.ToBoolean(appSettings["active-record:isWebapp"]);
            configSource.SetDebugFlag(Convert.ToBoolean(appSettings["active-record:debug"]));

            Assembly asmEntities = Assembly.Load("Lien.NewSocialNetwork.Entities");
            ActiveRecordStarter.Initialize(asmEntities, configSource);
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                } // Parameter defaults
            );
        }

        #region ----- BootstrapHolder Class

        /// <summary>
        /// Lớp BootstrapHolder ở dạng private là một giải pháp thread-safe mà không cần
        /// một cấu trúc đặc biệt khác tương tự để thay thế.
        /// </summary>
        private static class BootstrapHolder
        {
            public static readonly Bootstrap Instance = new Bootstrap();
        }

        #endregion
    }
}
