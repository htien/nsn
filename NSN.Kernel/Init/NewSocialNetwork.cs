using System;
using System.Web;
using Castle.Windsor;
using NSN.Kernel;
using NSN.Kernel.Manager;

namespace NSN.Init
{
    public class NewSocialNetwork : HttpApplication
    {
        private static IWindsorContainer container;
        private static ISessionManager sessionManager;

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrap.Instance.Init(this);
            container = (IWindsorContainer)this.Application[CfgKeys.CTX_CONTAINER];
            sessionManager = container.Resolve<ISessionManager>();
        }

        protected void Session_Start(object sender, EventArgs e) { }

        protected void Application_BeginRequest(object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_AuthorizeRequest(object sender, EventArgs e) { }

        protected void Application_PostAuthorizeRequest(object sender, EventArgs e) { }

        protected void Application_ResolveRequestCache(object sender, EventArgs e) { }

        protected void Application_PostResolveRequestCache(object sender, EventArgs e) { }

        protected void Application_PostMapRequestHandler(object sender, EventArgs e) { }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session.SessionID != null)
            {
                UserSession userSession = sessionManager.RefreshSession(HttpContext.Current);
            }
        }

        protected void Application_PostAcquireRequestState(object sender, EventArgs e) { }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e) { }

        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Application_EndRequest(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e)
        {
            Bootstrap.Instance.Dispose();
        }
    }
}
