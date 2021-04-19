using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace LC_WebApi.Utility.Filters
{
    /// <summary>
    /// 特性异常捕获：Filter的范围仅仅局限在方法里面，如果在方法前和方法后都捕获不到的
    /// </summary>
    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常发生后(没有被catch)，会进到这里
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //actionExecutedContext.Response. 可以获取很多信息,日志一下
            Console.WriteLine(actionExecutedContext.Exception.Message);//日志一下
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
                System.Net.HttpStatusCode.OK, new
                {
                    Result = false,
                    Msg = "出现异常，请联系管理员",
                    //Value=
                });//创造一个返回，具体返回json可以根据自己业务来决定返回的结果

            //base.OnException(actionExecutedContext);
            //ExceptionHandler
        }
    }
}