using System;
using System.Web;

namespace NSN.Kernel.HttpModule
{
    public class RequestFilter : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        public void Dispose() { }

        #endregion

        private void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            HttpRequest request = context.Request;
            string[] fileExts = { ".css", ".js", ".jpg", ".gif", ".png", ".html" };
            bool accept = true;
            foreach (string ext in fileExts)
            {
                if (ext.Equals(request.CurrentExecutionFilePathExtension, StringComparison.OrdinalIgnoreCase))
                {
                    accept = false;
                    break;
                }
            }
            if (!accept) return;
        }
    }
}
