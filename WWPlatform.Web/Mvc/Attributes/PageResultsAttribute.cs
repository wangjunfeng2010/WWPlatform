using System;
using System.Collections.Generic;
using System.Web.Mvc;

using WWPlatform.Core.Paging;
using WWPlatform.Core.Utilities;

namespace WWPlatform.Web.Mvc.Attributes
{
    /// <summary>
    /// 标记PageResults特性的方法对结果集进行分页
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PageResultsAttribute : ActionFilterAttribute
    {
        private int pageSize = 15;
        public int PageSize
        { 
            get { return pageSize; } 
            set { pageSize = value; } 
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Contract.ArgumentNotNull(filterContext, "filterContext");

            ViewResult viewResult = filterContext.Result as ViewResult;

            if (viewResult != null && viewResult.ViewData != null && viewResult.ViewData.Model != null)
            {
                //int pageSize;

                //if (!int.TryParse(filterContext.HttpContext.Request["PageSize"], out pageSize)
                //    || (pageSize < 1))
                //{
                //    pageSize = 15;
                //}

                int pageNo;

                if (!int.TryParse(filterContext.HttpContext.Request["p"], out pageNo))
                {
                    pageNo = 1;
                }

                if (pageNo < 1)
                {
                    return;
                }

                ApplyPagingToResult(viewResult.ViewData, pageNo, pageSize);
            }
        }

        private static void ApplyPagingToResult(ViewDataDictionary viewData, int pageNo, int pageSize)
        {
            object model = viewData.Model;
            IPageable pageable = model as IPageable;

            if (pageable != null)
            {
                pageable.SetPage(pageNo, pageSize);
            }
            else
            {
                TryPageEnumerableModel(viewData, pageNo, pageSize);
            }
        }

        private static void TryPageEnumerableModel(ViewDataDictionary viewData, int pageNo, int pageSize)
        {
            object model = viewData.Model;
            Type modelType = model.GetType();

            if (modelType.IsGenericType)
            {
                Type genericArgument = modelType.GetGenericArguments()[0];
                Type genericEnumerableType = typeof(IEnumerable<>).MakeGenericType(genericArgument);

                if (genericEnumerableType.IsAssignableFrom(modelType))
                {
                    Type pagingDecoratorType = typeof(PagingDecorator<>).MakeGenericType(genericArgument);
                    IPageable pageable = (IPageable)Activator.CreateInstance(pagingDecoratorType, model);

                    pageable.SetPage(pageNo, pageSize);

                    viewData.Model = pageable;
                }
            }
        }
    }
}