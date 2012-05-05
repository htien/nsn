using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using Castle.Windsor;
using NSN.Framework;
using NSN.Kernel.Part.Installers;
using NSN.Sub.Castle;
using SaberLily.Web.Extension.LowercaseRouteMVC;

namespace NSN.Kernel
{
    /// <summary>
    /// Lớp khởi tạo WindsorContainer, dùng làm nền tảng để kết hợp những module khác.
    /// </summary>
    public sealed class Bootstrap
    {
        private HttpApplication nsn;
        private IWindsorContainer container;

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
        public void Init(HttpApplication nsn)
        {
            this.nsn = nsn;

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitContainer();
            InitControllerFactory();
            InitActiveRecord();
        }

        /// <summary>
        /// Hủy và thu hồi tài nguyên web.
        /// </summary>
        public void Dispose()
        {
            this.container.Dispose();
        }

        private void InitContainer()
        {
            this.container = new WindsorContainer(CfgKeys.CONFIG_FOLDER_PATH + "CastleWindsor.xml");
            this.nsn.Application.Add(CfgKeys.CTX_NSNCONTAINER, this.container);
            this.container.Install(new LoggerInstaller(),
                         new StandardInstaller(),
                         new RepositoriesInstaller(),
                         new ModulesInstaller(),
                         new ServicesInstaller(),
                         new ControllersInstaller());
        }

        private void InitControllerFactory()
        {
            var controllerFactory = new WindsorControllerFactory(this.container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        private void InitActiveRecord()
        {
            INSNConfig config = this.container.Resolve<INSNConfig>();
            ConnectionStringSettingsCollection cfgSettings = WebConfigurationManager.ConnectionStrings;
            NameValueCollection db = WebConfigurationManager.GetSection("databaseSettings", "/" + CfgKeys.CONFIG_FOLDER_PATH) as NameValueCollection;
            NameValueCollection ar = WebConfigurationManager.GetSection("activeRecordSettings", "/" + CfgKeys.CONFIG_FOLDER_PATH) as NameValueCollection;

            IDictionary<string, string> settings = new Dictionary<string, string>();
            foreach (string key in ar.AllKeys)
            {
                settings[key] = ar[key];
            }

            string connStrKey = "connection.connection_string";
            string connStrNameKey = "connection.connection_string_name";
            bool hasConnectionStringName = settings.ContainsKey(connStrNameKey) && settings[connStrNameKey].Length > 0;
            bool hasConnectionString = settings.ContainsKey(connStrKey) && settings[connStrKey].Length > 0;

            if (!hasConnectionStringName && !hasConnectionString ||
                hasConnectionStringName && settings[connStrNameKey].Equals("tien.somee.com"))
            {
                bool isRemote = Convert.ToBoolean(config[CfgKeys.ISREMOTE]);
                string @connectionString = (isRemote
                            ? cfgSettings[CfgKeys.CONNECTION_REMOTE_NAME]
                            : cfgSettings[CfgKeys.CONNECTION_LOCAL_NAME]
                        ).ConnectionString;
                connectionString = isRemote
                        ? string.Format(connectionString,
                                db[CfgKeys.DB_DATASOURCE], db[CfgKeys.DB_PORT], db[CfgKeys.DB_NAME],
                                db[CfgKeys.DB_USER], db[CfgKeys.DB_PASSWORD])
                        : string.Format(connectionString,
                                db[CfgKeys.DB_DATASOURCE], db[CfgKeys.DB_PORT], db[CfgKeys.DB_NAME]);

                if (hasConnectionStringName)
                {
                    settings.Remove(connStrNameKey);
                }
                settings[connStrKey] = connectionString;
            }

            InPlaceConfigurationSource configSource = new InPlaceConfigurationSource();
            configSource.Add(typeof(ActiveRecordBase), settings);
            configSource.IsRunningInWebApp = Convert.ToBoolean(config[CfgKeys.GLOBAL_ACTIVERECORD_ISWEBAPP]);
            configSource.SetDebugFlag(Convert.ToBoolean(config[CfgKeys.GLOBAL_ACTIVERECORD_DEBUG]));

            Assembly asmEntities = Assembly.Load(CfgKeys.ASSEMBLY_NSN_ENTITIES);
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

            routes.Clear();

            // Root route
            routes.MapRouteLowercase(
                "Root",
                "",
                new { controller = "Home", action = "Stream", userid = (string)null }
            );

            // Single Sign-On routes

            routes.MapRouteLowercase(
                "SSO",
                "Auth/{action}",
                new { controller = "Auth", action = "Register" }
            );

            // Back-end routes

            routes.MapRouteLowercase(
                "AdminHome",
                "NSN-Admin",
                new { controller = "Admin", action = "Index" }
            );
            routes.MapRouteLowercase(
                "Admin",
                "NSN-Admin/{action}/{id}",
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional },
                new { id = @"^\d+$" }
            );

            // Front-end routes

            routes.MapRouteLowercase(
                "ProfileAction",
                "{userid}/{action}",
                new { controller = "Profile", action = "Posts" }
            );
            routes.MapRouteLowercase(
                "LinkAction",
                "Links/{action}",
                new { controller = "Link", action = "List" }
            );
            routes.MapRouteLowercase(
                "MessageAction",
                "Messages/{action}",
                new { controller = "Message", action = "List" }
            );
            routes.MapRouteLowercase(
                "PhotoAlbumAction",
                "{userid}/Photos/{albumid}/{action}",
                new { controller = "PhotoAlbum", action = "Index" },
                new { albumid = @"^\d+$" }
            );
            routes.MapRouteLowercase(
                "PhotoAction",
                "Photo/{photoid}/{action}",
                new { controller = "Photo", action = "Index" },
                new { photoid = @"^\d+$" }
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
