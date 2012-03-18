using System.Web.Mvc;
using WWPlatform.Web.Mvc.Attributes;

namespace WWPlatform.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.View("NotFound").ExecuteResult(this.ControllerContext);
        }
    }
}