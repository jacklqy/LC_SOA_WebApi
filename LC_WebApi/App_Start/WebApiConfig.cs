using LC_WebApi.Utility;
using LC_WebApi.Utility.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace LC_WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            #region Unity IOC容器初始化
            //IOC：config.DependencyResolver 换成自定义的Resolver
            config.DependencyResolver = new UnityDependencyResolver(ContainerFactory.BuildContainer()); 
            #endregion

            #region Filters过滤器注册
            //全局注册--所以WebApi请求都要做权限验证
            //config.Filters.Add(new BasicAuthorizeAttribute());
            //全局注册--所以的控制器和action方法都注册增加异常捕获
            config.Filters.Add(new ExceptionFilterAttribute());
            #endregion

            #region 全局异常驳回
            //替换系统默认的全局异常捕获，使用自定义全局异常捕获
            //config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler()); 
            #endregion

            #region 全局跨域设置(不建议)
            ////[EnableCors(origins: "http://localhost:9008/", headers: "*", methods: "GET,POST,PUT,DELETE")]
            ////第一个参数：允许的域名和端口；...具体EnableCorsAttribute参数配置看文档
            ////[EnableCors(origins: "http://localhost:9008/", headers: "*", methods: "GET,POST,PUT,DELETE")]
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*")); //全部都允许，太粗暴了，不安全，建议针对单个action或控制器，比如：在action方法或者控制器上面添加-》[EnableCors(origins: "http://localhost:9008/", headers: "*", methods: "GET,POST,PUT,DELETE")]
            #endregion

            // Web API 路由---特性路由
            config.MapHttpAttributeRoutes();

            //需放在默认路由上面，不然会被默认路由先匹配到。
            //这里增加了一个类似mvc的路由，其实是违背了RESTful规则，不建议，这里只是为了方便测试
            config.Routes.MapHttpRoute(
            name: "CustomApi",//默认的api路由
            routeTemplate: "api/{controller}/{action}/{id}",//正则规则，以api开头，第二个是控制器  第三个是参数
            defaults: new { id = RouteParameter.Optional }
        );

            //默认路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",//默认的api路由
                routeTemplate: "api/{controller}/{id}",//正则规则，以api开头，第二天是控制器，第三个是参数
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
