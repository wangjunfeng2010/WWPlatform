using System;
using System.Web.Mvc;
using System.Web;

namespace WWPlatform.Web.Mvc.Attributes
{
    /// <summary>
    /// 类/方法特性，标记此特性的类/方法抛出的异常会被记录至日志文件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ErrorLogAttribute : FilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!filterContext.ExceptionHandled)
            {
                //并不handle exception，记录日志后将异常bubble给HandleErrorAttribute
                System.Web.Routing.RouteValueDictionary dictionary = filterContext.RouteData.Values;
                //Logger.ErrorException(string.Format("controller:{0} action:{1}", dictionary["controller"], dictionary["action"]), filterContext.Exception);

                //处理404错误
                if (!filterContext.IsChildAction && !filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    Exception innerException = filterContext.Exception;
                    if (new HttpException(null, innerException).GetHttpCode() == 404)
                    {
                        ViewResult result = new ViewResult();
                        result.ViewName = "NotFound";
                        result.TempData = filterContext.Controller.TempData;
                        filterContext.Result = result;
                        filterContext.ExceptionHandled = true;
                        filterContext.HttpContext.Response.Clear();
                        filterContext.HttpContext.Response.StatusCode = 404;
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    }
                }
            }
        }
    }
}