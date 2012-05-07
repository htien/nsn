using System;
using System.Web;
using System.Web.SessionState;
using NSN.Kernel;
using NHibernate;

namespace NSN.HttpModule
{
    public class RequestFilter : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += OnAcquireRequestState;
        }

        public void Dispose() { }

        #endregion

        private void OnAcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            HttpSessionState session = context.Session;

            // Quá trình kiểm tra loại bỏ những request đối với những static file
            string[] fileExts = { ".css", ".js", ".png", ".gif", ".jpg", "jpeg", "bmp", "swf", ".aspx", ".html" };
            bool acceptRequest = true;
            foreach (string ext in fileExts)
            {
                if (ext.Equals(request.CurrentExecutionFilePathExtension, StringComparison.OrdinalIgnoreCase))
                {
                    acceptRequest = false;
                    break;
                }
            }
            if (!acceptRequest) return;

            UserSession userSession = NSNContext.Current.SessionManager.RefreshSession(HttpContext.Current);
        }
    }
}