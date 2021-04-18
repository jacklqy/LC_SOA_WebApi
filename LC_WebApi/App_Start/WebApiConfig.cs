using LC_WebApi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LC_WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            //config.DependencyResolver 换成自定义的Resolver
            config.DependencyResolver = new UnityDependencyResolver(ContainerFactory.BuildContainer());

            // Web API 路由---特性路由
            config.MapHttpAttributeRoutes();

            //这里增加了一个类似mvc的路由，其实是违背了RESTful规则，不建议，这里只是为了方便测试
            config.Routes.MapHttpRoute(
            name: "CustomApi",//默认的api路由
            routeTemplate: "api/{controller}/{action}/{id}",//正则规则，以api开头，第二个是控制器  第三个是参数
            defaults: new { id = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",//默认的api路由
                routeTemplate: "api/{controller}/{id}",//正则规则，以api开头，第二天是控制器，第三个是参数
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
