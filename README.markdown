New Social Network - ASP.NET Web Application
============================================

Sử dụng các công nghệ
---------------------

* __RDBMS__: Microsoft SQL Server 2008 R2 Express (hoặc cao hơn)

* __Phía Server__: ASP.NET MVC 3 (với Razor View Engine),
  Castle Windsor 3, Castle ActiveRecord 3, NHibernate 3.2.

* __Phía Client__: jQuery(, Closure Library)

* __Môi trường lập trình__: Visual Studio 2010 Web Developer Express (hoặc cao hơn)

    Plugins:

    * [NuGet] \(giúp cài đặt và cập nhật các library\)

* __Web Server__: IIS 7.5+

* __Trình quản lý source__: [msysgit] \(Git for Windows\)


Sử dụng Github
--------------

### # Code

##### Branches

##### Commits

##### Tags

Đây là một commit đặc biệt, một sự đánh dấu cột mốc quan trong (milestone).

Chẳng hạn, có thể đánh dấu source tại thời điểm A là v1.0 (version 1.0)
và thời điểm B là v2.0 (version 2.0). Qua đó source có thể được quản lý
theo từng version cụ thể hơn.

##### Downloads

Nơi này chứa các tập tin nằm bên ngoài thư mục đề án, chẳng hạn như tập tin
text, hình ảnh, tập tin nén v.v..
Hiện tại, cụ thể trong đề án `New Social Network` chỉ dành để chứa tập tin
.7z, .zip, .rar.

Thành viên có thể tải database dạng detach tại đây. Sau khi download về máy,
có thể dùng SQL Studio Management để attach nó vào.

### # Network

Trang này chứa biểu đồ mô tả quá trình làm việc của các thành viên trên các nhánh.
Cụ thể là các commit, quá trình phân nhánh và ghép nhánh.
Theo biểu đồ mô tả có thể thấy, nhánh `master` (màu đen, trên cùng) là luôn luôn
không đổi, và các nhánh khác sau khi đã hoàn thành nhiệm vụ đều thể hiện
1 đường nối ghép vào nhánh `master` này.


Hướng dẫn mở đề án
------------------

### Tổng quát

Đề án được viết bằng ngôn ngữ ASP.NET C# nên chỉ có thể mở bằng Visual Studio (bản 2010).

__*Lưu ý: Visual Studio phải được cài đặt Nuget plugin.*__

Đề án được lưu trữ lại:

  * [https://github.com/lilylnx/hellworld-dotnet/][1]
  * [http://code.google.com/p/helloworld-dotnet/][2]

Sử dụng công cụ [msysgit] để tải đề án về.

Sau khi tải thành công. Theo các bước bên dưới trước khi mở bằng Visual Studio:

1. Đổi tên tập tin `Web.new.config` thành `Web.config` trong thư mục
   `NewSocialNetwork.Website\Config`.
2. ...

Sau khi hoàn thành các bước trên, hãy mở đề án bằng Visual Studio và tiếp tục
thực hiện các bước bên dưới:

1. Cập nhật thư viện cho đề án:

    Vào menu `Tools > Library Package Manager > Package Manager Console`.
    Gõ lệnh `Get-Help NuGet`, nếu không thành công nghĩa là NuGet chưa
    được cài đặt.
    
    Cài đặt NuGet: chọn menu `Tools > Extension Manager...`, hãy tìm kiếm
    và cài đặt `NuGet Package Manager`. Khi thành công, hãy tiếp theo bên dưới.
    
    Tại cửa sổ Console, chọn _Default project_ là: `NewSocialNetwork.Website`.
    Tiếp theo, nhập lệnh theo cú pháp sau để tự động tải và cài đặt thư viện
    cho project: `Install-Package PackageName`
    
    Ví dụ: `Install-Package Castle.ActiveRecord`.
    Tương tự cho các thư viện khác, bạn hãy xem trong mục __Refenrence__
    của đề án, nếu thấy thiếu thư viện nào thì hãy cài đặt tương tự như
    câu lệnh bên trên.
    
    __*Sau khi hoàn thành cài đặt tất cả thư viện, hãy nhập lệnh:
    `Update-Package` để cập nhật version mới nhất cho các thư viện.*__

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


--
Tien Nguyen ([Blog][tiennguyen-blog])


[1]: https://github.com/lilylnx/hellworld-dotnet/ "'helloworld-dotnet' trên Github"
[2]: http://code.google.com/p/helloworld-dotnet/ "'helloworld-dotnet' trên Google Code"
[msysgit]: http://code.google.com/p/msysgit/
[NuGet]: http://nuget.org
[tiennguyen-blog]: http://lilylnx.wordpress.com/ "Tien Nguyen - The Long Road"