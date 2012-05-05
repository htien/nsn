using Castle.Windsor;
using NSN.Framework;
using NSN.Kernel.Manager;

namespace NSN.Kernel
{
    public class NSNContextAware
    {
        public static IWindsorContainer Container { get; set; }
        public static INSNConfig Config { get; set; }
        public static ISessionManager SessionManager { get; set; }

        private NSNContextAware() { }
    }
}
