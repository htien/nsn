using System;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

using SaberLily.Web.Factory;

using NewSocialNetwork.Website.Installers;
using System.Reflection;

namespace NewSocialNetwork.Website.Main
{
    /// <summary>
    /// Lớp khởi tạo WindsorContainer, dùng làm nền tảng để kết hợp những module khác.
    /// </summary>
    public sealed class Bootstrap
    {
        private static IWindsorContainer container;

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
            IConfigurationSource config = System.Configuration.ConfigurationManager.GetSection("activeRecord") as Castle.ActiveRecord.Framework.IConfigurationSource;
            Assembly asmEntities = Assembly.Load("Lien.NewSocialNetwork.Entities");
            ActiveRecordStarter.Initialize(asmEntities, config);
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
