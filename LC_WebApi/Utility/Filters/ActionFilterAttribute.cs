using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LC_WebApi.Utility.Filters
{
    /// <summary>
    /// Action方法执行前和方法执行后
    /// </summary>
    public class ActionFilterAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Console.WriteLine("1234567");
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Console.WriteLine("2345678");
            ////自定义允许跨域，就可以不用NuGet引用Microsoft.AspNet.WebApi.Cors了。只有相应的头里面包含Access-Control-Allow-Origin属性就可以。
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}