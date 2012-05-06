using System;
using System.Web;
using NSN.Kernel;

namespace NSN.Init
{
    /// <summary>
    /// Đây là đối tượng HttpHandler chính của ứng dụng, nắm quyền sinh sát toàn bộ
    /// quá trình khởi tạo mọi đối tượng và thành phần cho ứng dụng :).
    /// 
    /// P.S: chú thích document cho class và method chủ yếu dùng English, đôi lúc có dùng cả Vietnamese.
    /// </summary>
    public class NewSocialNetwork : HttpApplication
    {
        /// <summary>
        /// Raised only once when the very first resource from application is requested.
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            // Chuyển Bootstrap Init() tại sự kiện _BeginRequest
            // Để dành khởi tạo thứ khác quan trọng hơn :)), sau này sẽ nâng cấp ^^~
        }

        /// <summary>
        /// Called each time when instance of the HttpApplication is created.
        /// </summary>
        public override void Init() { }

        protected void Session_Start(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs as the first event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Bootstrap.Instance.Init(this);
        }

        /// <summary>
        /// Occurs when a security module has established the identity of the user.
        /// </summary>
        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when a security module has established the identity of the user.
        /// </summary>
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when a security module has verified user authorization.
        /// </summary>
        protected void Application_AuthorizeRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when the user for the current request has been authorized.
        /// </summary>
        protected void Application_PostAuthorizeRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET finishes an authorization event to let the caching modules serve requests
        /// from the cache, bypassing execution of the event handler (for example, a page or an XML Web service).
        /// </summary>
        protected void Application_ResolveRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET bypasses execution of the current event handler and
        /// allows a caching module to serve a request from the cache.
        /// </summary>
        protected void Application_PostResolveRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET has mapped the current request to the appropriate event handler.
        /// </summary>
        protected void Application_PostMapRequestHandler(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET acquires the current state
        /// (for example, session state) that is associated with the current request.
        /// </summary>
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            UserSession userSession = NSNContext.Current.SessionManager.RefreshSession(HttpContext.Current);
        }

        /// <summary>
        /// Occurs when the request state (for example, session state) that is associated with
        /// the current request has been obtained.
        /// </summary>
        protected void Application_PostAcquireRequestState(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET starts executing an event handler (for example, a page or an XML Web service).
        /// </summary>
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e) { }

        /*
         * Call the ProcessRequest method (or the asynchronous version IHttpAsyncHandler.BeginProcessRequest)
         * of the appropriate IHttpHandler class for the request.
         * 
         * For example, if the request is for a page, the current page instance handles the request.
         */

        /// <summary>
        /// Occurs when the ASP.NET event handler (for example, a page or an XML Web service) finishes execution.
        /// </summary>
        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs after ASP.NET finishes executing all request event handlers.
        /// This event causes state modules to save the current state data.
        /// </summary>
        protected void Application_ReleaseRequestState(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET has completed executing all request event handlers and
        /// the request state data has been stored.
        /// </summary>
        protected void Application_PostReleaseRequestState(object sender, EventArgs e) { }

        /*
         * Perform response filtering if the Filter property is defined.
         */

        /// <summary>
        /// Occurs when ASP.NET finishes executing an event handler in order to let caching modules store responses
        /// that will be used to serve subsequent requests from the cache.
        /// </summary>
        protected void Application_UpdateRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET finishes updating caching modules and storing responses
        /// that are used to serve subsequent requests from the cache.
        /// </summary>
        protected void Application_PostUpdateRequestCache(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET performs any logging for the current request.
        /// 
        /// Supports: IIS 7.0 and with the .NET Framework 3.0 or later.
        /// </summary>
        protected void Application_LogRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs when ASP.NET has completed processing all the event handlers for the LogRequest event.
        /// 
        /// Supports: IIS 7.0 and with the .NET Framework 3.0 or later.
        /// </summary>
        protected void Application_PostLogRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs as the last event in the HTTP pipeline chain of execution when ASP.NET responds to a request.
        /// </summary>
        protected void Application_EndRequest(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET sends HTTP headers to the client.
        /// </summary>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e) { }

        /// <summary>
        /// Occurs just before ASP.NET sends content to the client.
        /// </summary>
        protected void Application_PreSendRequestContent(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        /// <summary>
        /// Called each time when instance of the HttpApplication is destroyed.
        /// </summary>
        public override void Dispose() { }

        /// <summary>
        /// Raised only once when the application terminates.
        /// </summary>
        protected void Application_End(object sender, EventArgs e)
        {
            Bootstrap.Instance.Dispose();
        }
    }
}
