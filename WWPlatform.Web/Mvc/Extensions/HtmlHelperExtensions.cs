using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WWPlatform.Core.Paging;
using WWPlatform.Model.CNTV;

namespace WWPlatform.Web.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString PagingFor(this HtmlHelper helper, IPageable pageable)
        {
            return new MvcHtmlString(new PaginationBuilder(helper, pageable).Build());
        }

        public static MvcHtmlString ActionLink(this HtmlHelper helper, Webcast w)
        {
            string id = w == null ? "" : w.Base64Key;
            HttpRequestBase request = helper.ViewContext.RequestContext.HttpContext.Request;
            return helper.ActionLink(w.Title, "play", "webcast", request.Url.Scheme, request.Url.Host, null,
                new { id = id, area = "CNTV" }, new { target = "_video" });
        }
    }
}