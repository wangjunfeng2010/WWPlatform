using System.Web.Mvc;

namespace WWPlatform.Web.Areas.CNTV
{
    public class CNTVAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CNTV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //webcast
            context.MapRoute(
                "webcast",
                "w/{id}.html",
                new { controller = "webcast", action = "play" },
                new { id = @"[a-z,A-Z,0-9]+" },
                new[] { "WWPlatform.Web.Controllers.CNTV" }
                );

            //feature
            context.MapRoute(
                "feature",
                "f/{id}.html",
                new { controller = "feature", action = "preview" },
                new { id = @"[a-z,A-Z,0-9]+" },
                new[] { "WWPlatform.Web.Controllers.CNTV" }
                );

            //script
            context.MapRoute(
                "script",
                "s/{id}.html",
                new { controller = "webcast", action = "browse" },
                new { id = @"[a-z,A-Z,0-9]+" },
                new[] { "WWPlatform.Web.Controllers.CNTV" }
                );

            //lib
            context.MapRoute(
                "lib",
                "lib/{condition}.html",
                new { controller = "feature", action = "lib" },
                new { condition = @"\d_[+-]?[\d]+_\d_1" },
                new[] { "WWPlatform.Web.Controllers.CNTV" }
                );
            context.MapRoute(
                "forecast",
                "forecast/{id}.html",
                new { controller = "home", action = "forecast" },
                new { id = @"[a-z,A-Z,0-9]+" },
                new[] { "WWPlatform.Web.Controllers.CNTV" }
                );

            context.MapRoute(
                "cntv",
                "cntv/{action}/{id}",
                new { controller = "home", action = "index", id = UrlParameter.Optional },
                new[] { "WWPlatform.Web.Controllers.CNTV" }
            );
        }
    }
}
