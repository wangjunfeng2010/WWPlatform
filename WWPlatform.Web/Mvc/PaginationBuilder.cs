using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using WWPlatform.Core.Paging;
using WWPlatform.Core.Utilities;

namespace WWPlatform.Web.Mvc
{
    public sealed class PaginationBuilder
    {
        public const string NextClass = "next";
        public const string NextLabel = "Next";
        public const string PreviousClass = "prev";
        public const string PreviousLabel = "Prev";
        private readonly string _action;

        private readonly HtmlHelper _htmlHelper;
        private readonly IPageable _pageable;

        private readonly RouteValueDictionary _routeValues = new RouteValueDictionary();

        public PaginationBuilder(HtmlHelper htmlHelper, IPageable pageable)
        {
            Contract.ArgumentNotNull(htmlHelper, "htmlHelper");
            Contract.ArgumentNotNull(pageable, "pageable");

            _htmlHelper = htmlHelper;
            _pageable = pageable;

            _action = _htmlHelper.ViewContext.RouteData.Values["action"].ToString();
        }

        public string Build()
        {
            if (_pageable.PageCount <= 1)
            {
                return string.Empty;
            }

            CopyRouteValues();

            TagBuilder outerDiv = new TagBuilder("div");
            outerDiv.Attributes["id"] = "pager";
            outerDiv.AddCssClass("mod_pagenav");

            TagBuilder main = new TagBuilder("p");
            main.AddCssClass("mod_pagenav_main");
            main.InnerHtml = BuildNav();

            outerDiv.InnerHtml = main.ToString();// BuildLinks();

            return outerDiv.ToString();
        }

        private string BuildNav()
        {
            //prev
            StringBuilder innerhtml = new StringBuilder();

            if (_pageable.PageNo == 1)
            {
                innerhtml.Append(BuildLabel("上一页", new string[] { "prev_disable", "prev" }));
            }
            else
            {
                innerhtml.Append(BuildLink("上一页", _pageable.PageNo, "prev"));
            }

            innerhtml.Append("<span class=\"mod_pagenav_count\">");
            int offset = 3;
            int barsize = offset * 2 + 1;
            if (_pageable.PageCount <= barsize)
            {
                for (int i = 1; i <= _pageable.PageCount; i++)
                {
                    if (_pageable.PageNo == i)
                    {
                        innerhtml.Append(BuildLabel(i.ToString(CultureInfo.CurrentCulture), "current"));
                    }
                    else
                    {
                        innerhtml.Append(BuildLink(i.ToString(CultureInfo.CurrentCulture), i, "c_txt6"));
                    }
                }
            }
            else
            {
                if (_pageable.PageNo < offset + 1)
                {

                    for (int i = 1; i <= barsize; i++)
                    {
                        if (_pageable.PageNo == i)
                        {
                            innerhtml.Append(BuildLabel(i.ToString(CultureInfo.CurrentCulture), "current"));
                        }
                        else
                        {
                            innerhtml.Append(BuildLink(i.ToString(CultureInfo.CurrentCulture), i, "c_txt6"));
                        }
                    }
                    innerhtml.Append(BuildLabel("...", new string[] { "mod_pagenav_more" }));
                    innerhtml.Append(BuildLink(_pageable.PageCount.ToString(CultureInfo.CurrentCulture), _pageable.PageCount, "c_txt6"));
                }
                else
                {
                    innerhtml.Append(BuildLink("1", 1, "c_txt6"));
                    innerhtml.Append(BuildLabel("...", new string[] { "mod_pagenav_more" }));
                    if (_pageable.PageNo < _pageable.PageCount - offset)
                    {
                        for (int i = _pageable.PageNo - offset; i < _pageable.PageNo; i++)
                        {
                            if (i == 1)
                            {
                                continue;
                            }
                            innerhtml.Append(BuildLink(i.ToString(CultureInfo.CurrentCulture), i, "c_txt6"));
                        }

                        for (int i = _pageable.PageNo; i <= _pageable.PageNo + offset; i++)
                        {
                            if (_pageable.PageNo == i)
                            {
                                innerhtml.Append(BuildLabel(i.ToString(CultureInfo.CurrentCulture), "current"));
                            }
                            else
                            {
                                innerhtml.Append(BuildLink(i.ToString(CultureInfo.CurrentCulture), i, "c_txt6"));
                            }
                        }
                        innerhtml.Append(BuildLabel("...", new string[] {"mod_pagenav_more" }));
                        innerhtml.Append(BuildLink(_pageable.PageCount.ToString(CultureInfo.CurrentCulture), _pageable.PageCount, "c_txt6"));
                    }
                    else
                    {
                        for (int i = _pageable.PageCount - offset * 2; i <= _pageable.PageCount; i++)
                        {
                            if (_pageable.PageNo == i)
                            {
                                innerhtml.Append(BuildLabel(i.ToString(CultureInfo.CurrentCulture), "current"));
                            }
                            else
                            {
                                innerhtml.Append(BuildLink(i.ToString(CultureInfo.CurrentCulture), i, "c_txt6"));
                            }
                        }
                    }
                }
            }

            innerhtml.Append("</span>");
            //next
            if (_pageable.PageCount == _pageable.PageNo)
            {
                innerhtml.Append(BuildLabel("下一页", new string[] { "next_disable", "next" }));
            }
            else
            {
                innerhtml.Append(BuildLink("下一页", _pageable.PageNo + 1, "next"));
            }
            return innerhtml.ToString();
        }

        private static string BuildLabel(string label, params string[] @class)
        {
            TagBuilder span = new TagBuilder("span");
            foreach (var cls in @class)
            {
                span.AddCssClass(cls);
            }

            TagBuilder inner = new TagBuilder("span");
            inner.SetInnerText(label);
            span.InnerHtml = inner.ToString();

            return span.ToString();
        }

        private string BuildLink(string linkText, int PageNo, string @class)
        {
            TagBuilder a = new TagBuilder("a");
            a.AddCssClass(@class);
            a.Attributes["href"] = "?p=" + PageNo.ToString();
            a.Attributes["title"] = linkText;

            TagBuilder inner = new TagBuilder("span");
            inner.InnerHtml = linkText;

            a.InnerHtml = inner.ToString();
            return a.ToString();
        }

        private void CopyRouteValues()
        {
            foreach (var parameter in _htmlHelper.ViewContext.HttpContext.Request.QueryString.AllKeys)
            {
                _routeValues.Add(parameter, _htmlHelper.ViewContext.HttpContext.Request[parameter]);
            }
        }
    }
}