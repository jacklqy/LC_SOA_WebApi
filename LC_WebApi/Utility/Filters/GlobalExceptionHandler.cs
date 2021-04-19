using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace LC_WebApi.Utility.Filters
{
    /// <summary>
    /// WebApi的全局异常处理
    /// 继承ExceptionHandler，覆写Handler---初始化项目时，服务替换上
    /// </summary>
    public class GlobalExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// 判断是否要进行异常处理，规则自己定
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            string url = context.Request.RequestUri.AbsoluteUri;
            return url.Contains("/api/");//判断请求url包含/api/就需要进行异常处理

            //return base.ShouldHandle(context);
        }
        /// <summary>
        /// 完成异常处理
        /// </summary>
        /// <param name="context"></param>
        public override void Handle(ExceptionHandlerContext context)
        {
            //Console.WriteLine(context);//日志一下
            context.Result = new ResponseMessageResult(context.Request.CreateResponse(
                System.Net.HttpStatusCode.OK, new
                {
                    Result = false,
                    Msg = "出现异常，请联系管理员",
                    Debug = context.Exception.Message
                }));

            //if(context.Exception is HttpException)//根据不同的异常可以返回不同的信息
        }
    }
}