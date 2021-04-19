using LC.SOA.Interface;
using LC.SOA.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

/// <summary>
/// 1 RESTful架构风格和WebApi
/// 2 WebApi路由&特性路由
/// </summary>
namespace LC_WebApi.Controllers
{
    /// <summary>
    /// 1 RESTful架构风格和WebApi  
    /// 2 WebApi路由&特性路由
    /// 3 WebApi依赖注入&面向切面 (NuGet引用Unity/Unity.Interception/Unity.Abstractions/Unity.Container/Unity.Configuration/Unity.Interception.Configuration)
    /// 4 WebApi前台调用详析
    /// 5 WebApi后台调用详析
    /// 6 Basic授权认证&权限Filter ：WebApi默认是不支持session的，因为WebApi是无状态的
    ///    无状态：第二次请求和第一次请求不关联
    ///    a)登陆过程，拿到令牌---token/ticket/许可证
    ///    b)登陆验证成功---账号+密码+其它信息+时间---加密一下得到ticket---返回给客户端
    ///    c)客户端请求时ajax就带上这个ticket
    ///    d)接口调用时，就去验证ticket，解密一下，看看信息(缓存或数据库比较)，看看失效时间
    /// 每个方法都验证令牌？就可以基于filter来实现验证，http请求头文件BasicAuth带上ticket传到服务器验证。
    /// 7 异常处理Filter和ActionFilter
    /// 8 WebApi跨域请求：浏览器请求时，如果A网站(域名+端口)页面里面，通过XHR去请求B域名，这个就是跨域
    ///                   这个请求是可以正常到达B服务器后端，正常的相应(200)
    ///                   但是，浏览器不允许这样操作，除非在响应里面有声明(Access-Control-Allow-Origin)
    ///                   为什么不允许呢？是因为浏览器有一个同源策略，出于安全考虑，浏览器限制脚本去发起跨域请求。
    ///                   但是，页面里面的script-src/img-href/js/css/图片 这些是浏览器自己发起的，是可以跨域的
    ///                   a)JSONP---脚本标签自动请求--请求回来的内容执行个回调方法--解析数据 （前端解决方案，比较流行）
    ///                   b)CORS 跨域资源共享，允许服务器在响应头里面指定Access-Control-Allow-Origin，浏览器就会案子响应来操作。NuGet添加引用Microsoft.AspNet.WebApi.Cors
    /// 9 自宿主WebApi实现：WebApiSelf这个项目就是自宿主测试，就可以不用部署在IIS服务器了。一般是放在IIS服务器，这种自宿主不常用。
    /// 10 自动生成WebApi文档：Areas-》HelpPage-》Views-》Help-》Index.cshtml 就是api接口说明文档，框架已经为我们自动生成了一个区域路由来展示。所以只需在浏览器输入：http://localhost:50439/Help/Index 就可以查看到WebApi文档。
    /// 
    /// ----------------------------------------------------------------------------------
    /// 
    /// WebService--->WCF--->WebApi
    /// RESTful架构风格:表现层的状态转化， 是一个接口的设计风格
    ///    资源：万物看成资源，
    ///    统一接口：CRUD增删改查，跟HTTP Method对应
    ///              Create--Post    Read--Get   Update--Put/Patch  Delete--Delete
    ///    URI:统一资源定位符，资源对应的唯一地址
    ///    无状态：基于Http协议， (登陆系统--查询工资--计算税收，有状态)
    ///            无状态的直接一个地址，就能拿到工资，就能得到税收
    ///            
    /// WebService---http协议，soap协议，只能IIS承载，入门简单，XML跨平台
    /// WCF---集大成者，多种协议，多种宿主，整合了RPC。
    /// RPC模式，都是调用方法的，
    /// 
    /// WebApi:RESTful,http协议 无状态 标准化操作  更轻量级，尤其是json，适合移动端
    /// 
    /// 
    /// 网站启动时执行Application_Start---给Routes增加地址规则---请求进来时--会经过路由匹配找到合适的《控制器》
    /// 那怎么找的Action？
    ///    1 根据HttpMethod找方法---用的方法名字开头，Get就是对应Get请求
    ///    2 如果名字不是Get开头，可以加上[HttpGet]
    ///    3 按照参数找最吻合
    /// 
    /// 其实资源是这样定义的，不是一个学生，而可能是一个学校
    ///  可能是一个订单--多件商品，一次查询，订单-商品，数据之前嵌套关系很复杂
    /// 还有个特性路由！可以单独订制
    /// 1 config.MapHttpAttributeRoutes();
    /// 2 标记特性
    /// 
    /// 版本兼容---约束路由---默认值/可空路由---多数据
    /// 
    /// 
    /// IOC容器+配置文件初始化
    /// 控制器也要注入--完成容器和WebApi框架融合--实现IDependencyResolver，将容器放进去--初始化讲 config.DependencyResolver 换成自定义的Resolver
    /// </summary>
    
    //[RoutePrefix("api/Values/")] //action就可以去掉这一节；如果某个方法又不要了，可以在路由前面加个~，比如[Route("~api/Values/{id:int}")]
    public class ValuesController : ApiController
    {
        private readonly ITb_LogService _iTb_LogService = null;
        public ValuesController(ITb_LogService tb_LogService)
        {
            _iTb_LogService = tb_LogService;
        }

        // GET api/values (http://localhost:50439/api/Values/)
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5  (http://localhost:50439/api/Values/1)
        [Route("api/Values/{id:int}")] //约束路由{id:int}
        public string Get(int id)//ID查询
        {
            return JsonConvert.SerializeObject(_iTb_LogService.Find<tb_log>(id));
        }

        //// GET api/values/5  (http://localhost:50439/api/Values/1)
        ////[Route("api/Values/{id:int=10}")] //默认值
        //[Route("api/Values/{id:int?}")] //可空参数
        //public string GetId(int id=10)
        //{
        //    return $"{id}_value";
        //}

        //特性路由里面最常见配置
        //[Route("api/Values/{id:int}/{typeId:int}")]
        [Route("api/Values/{id:int}/Type/{typeId:int}")] //(http://localhost:50439/api/Values/1/type/2) 标准做法
        public string Get(int id,int typeId)
        {
            return $"value-Type {id} {typeId}";
        }

        // GET api/values/jack  (http://localhost:50439/api/Values/jack)
        [Route("api/Values/{name}")]
        public string Get(string name)//名称查询
        {
            return $"{name}_value";
        }

        //特性路由  (http://localhost:50439/api/Values/1/V2)
        [Route("api/Values/{id}/V2")]
        public string GetV2(int id)
        {
            return $"{id}_valueV2";
        }
        //特性路由  (http://localhost:50439/api/Values/1/V2)
        [Route("api/Values/{id}/V2")]
        [HttpGet]//如果不是Get开头的，就需要加这个
        public string LGetV11111(int id)
        {
            return $"{id}_valueV2";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
