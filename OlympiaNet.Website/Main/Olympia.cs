using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;

namespace OlympiaNet.Website.Main
{
    public class Olympia : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrap.Instance.Init();
        }

        protected void Session_Start(object sender, EventArgs e) { }

        protected void Application_BeginRequest(object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e)
        {
            Bootstrap.Instance.Dispose();
        }
    }
}
