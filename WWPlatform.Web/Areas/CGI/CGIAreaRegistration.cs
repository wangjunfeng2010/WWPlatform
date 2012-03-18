using System.Web.Mvc;

namespace WWPlatform.Web.Areas.CGI
{
    public class CGIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "CGI"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "fcgi-bin",
                "fcgi-bin/{action}",
                new { controller = "home", id = UrlParameter.Optional },
                new string[] { "WWPlatform.Web.Controllers.CGI" }
            );
        }
    }
}
