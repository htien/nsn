using System;
using System.Web;
using NHibernate;
using NSN.Common;
using NSN.Kernel;

namespace NewSocialNetwork.DataAccess
{
    public sealed class DAOUtils
    {
        private static ISession _currentSession;

        public static ISession CurrentSession
        {
            get
            {
                HttpContext currentContext = HttpContext.Current;
                if (currentContext == null)
                {
                    if (_currentSession != null)
                    {
                        return _currentSession;
                    }
                    else
                    {
                        _currentSession = OpenSession(currentContext);
                        return _currentSession;
                    }
                }
                else
                {
                    ISession session = (ISession)currentContext.Items[Globals.NHIBERNATE_SESSION_KEY];
                    if (session == null)
                    {
                        session = OpenSession(currentContext);
                        currentContext.Items[Globals.NHIBERNATE_SESSION_KEY] = session;
                    }
                    return session;
                }
            }
        }

        private static ISessionFactory GetSessionFactory(HttpContext context)
        {
            return NSNContext.Current.Container.Resolve<ISessionFactory>();
        }

        public static ISession OpenSession(HttpContext context)
        {
            ISession session = GetSessionFactory(context).OpenSession();
            if (session == null)
            {
                throw new InvalidOperationException("Call to SessionFactory.OpenSession() returned null.");
            }
            return session;
        }

        public static void CloseSession(HttpContext context)
        {
            ISession session = (ISession)context.Items[Globals.NHIBERNATE_SESSION_KEY];
            if (session != null)
            {
                session.Flush();
                session.Close();
            }
            context.Items[Globals.NHIBERNATE_SESSION_KEY] = null;
        }
    }
}
