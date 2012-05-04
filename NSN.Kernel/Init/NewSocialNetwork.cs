using System;
using System.Web;
using NSN.Kernel;

namespace NSN.Init
{
    public class NewSocialNetwork : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrap.Instance.Init();
        }

        protected void Session_Start(object sender, EventArgs e) { }

        protected void Application_BeginRequest(object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Application_EndRequest(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e)
        {
            Bootstrap.Instance.Dispose();
        }
    }
}
