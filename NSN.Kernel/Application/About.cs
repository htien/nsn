using System.Reflection;
using System;

namespace NSN.Application
{
    /// -----------------------------------------------------------------------------
    /// Namespace:  NSN.Application
    /// Project:    NewSocialNetwork
    /// Module:     Application
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The About class contains properties that describe the NSN Application.
    /// </summary>
    public class About
    {
        private static ReleaseMode _status = ReleaseMode.None;

        protected internal About() { }

        public string CodeName
        {
            get { return "NewWorld"; }
        }

        public string Name
        {
            get { return "NSN"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Company
        {
            get { return "NSN"; }
        }

        public string Author
        {
            get { return "Phuong Lien"; }
        }

        public string Email
        {
            get { return "insaber2309@yahoo.com"; }
        }

        public string Title
        {
            get { return "NewSocialNetwork"; }
        }

        public string TradeMark
        {
            get { return "NewSocialNetwork,NSN"; }
        }

        public string Type
        {
            get { return "Kernel"; }
        }

        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public ReleaseMode Status
        {
            get
            {
                if (_status == ReleaseMode.None)
                {
                    Assembly ass = Assembly.GetExecutingAssembly();
                    if (Attribute.IsDefined(ass, typeof(AssemblyStatusAttribute)))
                    {
                        Attribute attr = Attribute.GetCustomAttribute(ass, typeof(AssemblyStatusAttribute));
                        if (attr != null)
                        {
                            _status = ((AssemblyStatusAttribute)attr).Status;
                        }
                    }
                }
                return _status;
            }
        }
    }
}
