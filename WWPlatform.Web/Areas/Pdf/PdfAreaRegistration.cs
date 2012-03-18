using System.Web.Mvc;

namespace WWPlatform.Web.Areas.Pdf
{
    public class PdfAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "pdf";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "pdf",
                "pdf/{token}.html",
                new { controller = "home", action = "create" },
                new { token = @"[w|f][a-z,A-Z,0-9]+" },
                new string[] { "WWPlatform.Web.Controllers.Pdf" }
            );
        }
    }
}