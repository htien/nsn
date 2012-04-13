using Castle.Facilities.AutoTx;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace NewSocialNetwork.Website.Installers
{
    public class AutoTxInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.AddFacility<AutoTxFacility>();
        }

        #endregion
    }
}