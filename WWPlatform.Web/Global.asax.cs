using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using WWPlatform.Web.Components;

namespace WWPlatform
{
    public class MvcApplication : HttpApplication
    {
        //DI容器变量
        static private IUnityContainer unityContainer;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //siteuser
            routes.MapRoute(
                "siteuser",
                "qq/{action}",
                new { controller = "siteuser", action = "login" },
                new[] { "WWPlatform.Web.Controllers" }
                );

            //search
            routes.MapRoute(
                "search",
                "search.html",
                new { controller = "home", action = "search" },
                new[] { "WWPlatform.Web.Controllers" }
                );

            routes.MapRoute(
                "default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults,
                new[] { "WWPlatform.Web.Controllers" }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            unityContainer.Dispose();
        }

        public override void Init()
        {
            unityContainer = new UnityContainerBuilder(
                new HttpContextSessionStore(),
                new HttpContextPerRequestStore(this),
                new HttpContextIdentityProvider()
                ).Build();

            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(unityContainer));

            //ModelBinders.Binders.DefaultBinder = new Reposi
        }
    }
}