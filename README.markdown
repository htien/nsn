New Social Network - ASP.NET Web Application
============================================

Sử dụng các công nghệ
---------------------

* __RDBMS__: Microsoft SQL Server 2008 R2 Express (hoặc cao hơn)

* __Phía Server__: ASP.NET MVC 3 (với Razor View Engine),
  Castle Windsor 3, Castle ActiveRecord 3, NHibernate 3.2.

* __Phía Client__: jQuery(, Closure Library)

* __Môi trường lập trình__: Visual Studio 2010 Web Developer Express (hoặc cao)

    Plugins:

    * [NuGet] \(giúp cài đặt và cập nhật các library\)

* __Web Server__: IIS 7.5+

* __Trình quản lý source__: [msysgit] \(Git for Windows\)


Sử dụng Github
--------------

### Code

#### Branches

#### Commits

#### Tags

#### Downloads

### Network


Hướng dẫn mở đề án
------------------

### Tổng quát

Đề án được viết bằng ngôn ngữ ASP.NET C# nên chỉ có thể mở bằng Visual Studio (bản 2010).

__*Lưu ý*__: Visual Studio phải được cài đặt Nuget plugin.

Đề án được lưu trữ lại:

  * [https://github.com/lilylnx/hellworld-dotnet/][1]
  * [http://code.google.com/p/helloworld-dotnet/][2]

Sử dụng công cụ [msysgit] để tải đề án về.

Sau khi tải thành công. Theo các bước bên dưới trước khi mở bằng Visual Studio:

1. Đổi tên tập tin `Web.new.config` thành `Web.config` trong thư mục
   `NewSocialNetwork.Website\Config`.
2. ...


Hiệu chỉnh IDE
--------------

Yêu cầu hiệu chỉnh lại tính năng `Format Document` trong Visual Studio
để code trình bày được đẹp và chuẩn :)

    using System.Web.Mvc;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    namespace NewSocialNetwork.Website.Installers
    {
        public class ControllersInstaller : IWindsorInstaller
        {
            #region IWindsorInstaller Members

            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Classes.FromThisAssembly()
                                    .BasedOn<IController>()
                                    .LifestyleTransient());
            }
            #endregion
        }
    }

Đang cập nhật...


[1]: https://github.com/lilylnx/hellworld-dotnet/ "'helloworld-dotnet' trên Github"
[2]: http://code.google.com/p/helloworld-dotnet/ "'helloworld-dotnet' trên Google Code"
[msysgit]: http://code.google.com/p/msysgit/
[NuGet]: http://nuget.org


--
Tien Nguyen ([Blog](http://lilylnx.wordpress.com/))