using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using WWPlatform.Core.Utilities;

namespace WWPlatform.Web.Components
{
	public class UnityControllerFactory : DefaultControllerFactory
	{
		private readonly IUnityContainer _unityContainer;

		public UnityControllerFactory(IUnityContainer unityContainer)
		{
			Contract.ArgumentNotNull(unityContainer, "unityContainer");

			_unityContainer = unityContainer;
		}

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }
            else if (!typeof(IController).IsAssignableFrom(controllerType))
            {
                return base.GetControllerInstance(requestContext,controllerType);
            }

            return (IController)_unityContainer.Resolve(controllerType);
        }
	}
}