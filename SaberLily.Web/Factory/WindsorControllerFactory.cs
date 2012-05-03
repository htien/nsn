using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace SaberLily.Web.Factory
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public override void ReleaseController(IController controller)
        {
            container.Release(controller);
            ReleaseController(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }
            return (IController)container.Resolve(controllerType);
        }

        
    }
}