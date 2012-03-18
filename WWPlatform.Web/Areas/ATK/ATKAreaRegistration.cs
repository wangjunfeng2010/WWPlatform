using System.Web.Mvc;

namespace WWPlatform.Web.Areas.ATK
{
    public class ATKAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "atk";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "atk_list",
                "atk/list.html",
                new { controller = "home", action = "list" },
                new string[] { "WWPlatform.Web.Controllers.ATK" }
            );

            context.MapRoute(
                "atk_browse",
                "atk/{id}.html",
                new { controller = "home", action = "browse" },
                new { id = @"[a-z,A-Z,0-9]+" },
                new string[] { "WWPlatform.Web.Controllers.ATK" }
            );

            context.MapRoute(
                "atk",
                "atk/{action}/{id}",
                new { controller = "home", action = "index", id = UrlParameter.Optional },
                new string[] { "WWPlatform.Web.Controllers.ATK" }
            );
        }
    }
}
