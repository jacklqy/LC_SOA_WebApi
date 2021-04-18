using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LC_WebApi.Controllers
{
    /// <summary>
    /// 1 RESTful架构风格和WebApi
    /// 2 WebApi路由&特性路由
    /// 3 WebApi依赖注入&面向切面 (NuGet引用Unity/Unity.Interception/Unity.Abstractions/Unity.Container/Unity.Configuration/Unity.Interception.Configuration)
    /// 4 WebApi前台调用详析
    /// 5 WebApi后台调用详析
    /// 6 Basic授权认证&权限Filter
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
        // GET api/values (http://localhost:50439/api/Values/)
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5  (http://localhost:50439/api/Values/1)
        [Route("api/Values/{id:int}")] //约束路由{id:int}
        public string Get(int id)//ID查询
        {
            return $"{id}_value";
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
