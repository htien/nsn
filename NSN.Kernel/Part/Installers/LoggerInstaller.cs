using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace NSN.Kernel.Part.Installers
{
    public class LoggerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(f => f.UseLog4Net(CfgKeys.CONFIG_FOLDER_PATH + "Log4Net.xml"));
        }

        #endregion
    }
}