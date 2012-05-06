using Castle.Windsor;
using NSN.Application;
using NSN.Framework;
using NSN.Manager;

namespace NSN.Kernel
{
    /// <summary>
    /// Defines the context for the environment of the NSN Application.
    /// </summary>
    public class NSNContext
    {
        private static NSNContext _context;
        private readonly About _about;
        public IWindsorContainer Container { get; set; }
        public INSNConfig Config { get; set; }
        public ISessionManager SessionManager { get; set; }

        protected NSNContext() : this(new About())
        { }

        protected NSNContext(About about)
        {
            this._about = about;
        }

        public About About
        {
            get { return _about; }
        }

        public static NSNContext Current
        {
            get
            {
                if (_context == null)
                    _context = new NSNContext();
                return _context;
            }
            set
            {
                _context = value;
            }
        }
    }
}
