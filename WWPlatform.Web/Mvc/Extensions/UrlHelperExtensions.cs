using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WWPlatform.Model.CNTV;
//using WWPlatform.Model.Fiction;
using WWPlatform.Model.ATK;

namespace WWPlatform.Web.Mvc.Extensions
{
    /// <summary>
    /// 扩充MVC library的LinkExtensions对UrlHelper的扩展
    /// </summary>
    public static class UrlHelperExtensions
    {
        public static string Javascript(this UrlHelper helper, string js)
        {
            return helper.Content("~/scripts/" + js);
        }

        public static string StyleSheet(this UrlHelper helper, string css)
        {
            return helper.Content("~/styles/" + css);
        }

        public static string Image(this UrlHelper helper, string img)
        {
            return helper.Content("~/content/images/" + img);
        }

        public static string Swf(this UrlHelper helper, string swf)
        {
            return helper.Content("~/content/swf/" + swf);
        }


        public static string Action(this UrlHelper helper, Webcast w)
        {
            if (w == null)
                return "";

            HttpRequestBase request = helper.RequestContext.HttpContext.Request;
            return helper.Action("play", "webcast", new RouteValueDictionary(new
            {
                id = w.Base64Key,
                area = "CNTV"
            }));
        }

        public static string Action(this UrlHelper helper, Feature f)
        {
            if (f == null)
                return "";

            HttpRequestBase request = helper.RequestContext.HttpContext.Request;
            return helper.Action("preview", "feature", new RouteValueDictionary(new
            {
                id = f.Base64Key,
                area = "CNTV"
            }));
        }

        public static string Action(this UrlHelper helper, Script s)
        {
            if (s == null)
                return "";

            HttpRequestBase request = helper.RequestContext.HttpContext.Request;
            return helper.Action("browse", "webcast", new RouteValueDictionary(new
            {
                id = s.Webcast.Base64Key,
                area = "CNTV"
            }));
        }

        /*
        public static string Action(this UrlHelper helper, Book book)
        {
            if (book == null)
                return "";

            HttpRequestBase request = helper.RequestContext.HttpContext.Request;
            return helper.Action("preview", "home", new RouteValueDictionary(new
            {
                id = book.Base64Key,
                area = "fiction"
            }));
        }

        public static string Action(this UrlHelper helper, Chapter chapter)
        {
            if (chapter == null)
                return "";

            HttpRequestBase request = helper.RequestContext.HttpContext.Request;
            return helper.Action("browse", "home", new RouteValueDictionary(new
            {
                id = chapter.Book.Base64Key,
                seq = chapter.Seq,
                area = "fiction"
            }));
        }
        */

        public static string Action(this UrlHelper helper, Article article)
        {
            if (article == null)
                return "";

            HttpRequestBase request = helper.RequestContext.HttpContext.Request;
            return helper.Action("browse", "home", new RouteValueDictionary(new
            {
                id = article.Base64Key,
                area = "atk"
            }));
        }
    }
}