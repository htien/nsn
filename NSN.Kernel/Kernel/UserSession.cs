using System;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using NewSocialNetwork.Domain;
using NSN.Common;
using SaberLily.Text;
using SaberLily.Utils;

namespace NSN.Kernel
{
    public class UserSession
    {
        #region Properties

        private User _User;
        private string _SessionId;
        private int _CreationTime;
        private int _LastAccessedTime;
        private int _LastVisit;

        public User User
        {
            get { return _User; }
            set
            {
                _User = value;
                if (value == null)
                {
                    try
                    {
                        throw new Exception("UserSession.SetUser with null value." +
                                            " See the stack trace for more information about the call stack." +
                                            " Session ID: " + SessionId);
                    }
                    catch
                    {
                        // LOG
                    }
                }
            }
        }

        public string SessionId
        {
            get { return _SessionId; }
            set { _SessionId = value; }
        }

        public int CreationTime
        {
            get { return _CreationTime; }
            set
            {
                _CreationTime = value;
                _LastAccessedTime = value;
                _LastVisit = value;
            }
        }

        public int LastAccessedTime
        {
            get { return _LastAccessedTime; }
            private set { _LastAccessedTime = value; }
        }

        public DateTime LastAccessedDate
        {
            get { return new DateTime(LastAccessedTime); }
        }

        public int LastVisit
        {
            get { return _LastVisit; }
            set { _LastVisit = value; }
        }

        #endregion

        #region Extra properties

        public string Ip
        {
            get
            {
                if (false /*Block IP và trả về null tại đây*/)
                {
                    //return null;
                }

                HttpRequest request = this.Request;

                string ip = request.Headers["X-Pounded-For"];
                if (ip != null)
                    return ip;

                ip = request.Headers["X-Forwarded-For"];
                if (ip == null)
                {
                    return request.UserHostAddress;
                }
                else
                {
                    // Process the IP to keep the last IP (real ip of the computer on the net)
                    SimpleStringTokenizer tokenizer = new SimpleStringTokenizer(ip, new char[] { ',' });

                    // Ignore all tokens, except the last one
                    IEnumerator e = tokenizer.GetEnumerator();
                    for (int i = 0; i < tokenizer.CountTokens() - 1; i++)
                    {
                        e.MoveNext();
                    }

                    ip = ((string)e.Current).Trim();
                    if (ip.Length == 0)
                    {
                        ip = null;
                    }
                }

                // If the ip is still null, we put 0.0.0.0 to void null values
                if (ip == null)
                {
                    ip = "0.0.0.0";
                }
                return ip;
            }
        }

        public bool IsBot()
        {
            return false;
        }

        public void ping()
        {
            this.LastAccessedTime = Convert.ToInt32(DateTimeUtils.UnixTimestamp);
        }

        #endregion

        #region Private properties

        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        private HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        #endregion

        public UserSession() { }

        #region Methods

        public void BecomesAnonymous(int anonymousUserId)
        {
            this.ClearAllAttributes();

            User user = new User();
            user.UserId = anonymousUserId;
            this.User = user;
        }

        public void BecomesLogged()
        {
            this.SetAttribute(Globals.SSO_LOGGED, "1");
        }

        public bool IsLogged()
        {
            return "1".Equals(this.GetAttribute(Globals.SSO_LOGGED));
        }

        public Session AsSession()
        {
            return new Session()
            {
                UserId = this.User.UserId,
                Ip = this.Ip,
                Start = this.CreationTime,
                LastAccess = this.LastAccessedTime,
                LastVisit = this.LastVisit
            };
        }

        public object GetAttribute(string name)
        {
            return HttpContext.Current.Session[name];
        }

        public void SetAttribute(string name, object value)
        {
            HttpContext.Current.Session[name] = value;
        }

        public void ClearAllAttributes()
        {
            this.Session.RemoveAll();
        }

        #endregion
    }
}