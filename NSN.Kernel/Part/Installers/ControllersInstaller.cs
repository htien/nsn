using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace NSN.Kernel.Part.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyNamed(CfgKeys.ASSEMBLY_NSN_WEBSITE)
                        .BasedOn<IController>()
                        .LifestyleTransient());
        }

        #endregion
    }
}