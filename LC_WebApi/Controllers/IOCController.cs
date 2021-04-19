using LC.SOA.Interface;
using LC.SOA.Model;
using LC.SOA.Model.ModelExt;
using LC_WebApi.Utility;
using LC_WebApi.Utility.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Unity;

/// <summary>
/// 3 WebApi依赖注入&面向切面
/// </summary>
namespace LC_WebApi.Controllers
{
    /// <summary>
    /// NuGet引用Unity/Unity.Interception/Unity.Abstractions/Unity.Container/Unity.Configuration/Unity.Interception.Configuration
    /// </summary>
    public class IOCController : ApiController
    {
        private readonly ITb_LogService _iTb_LogService = null;
        public IOCController(ITb_LogService tb_LogService)
        {
            _iTb_LogService = tb_LogService;
        }

        [Route("api/IOC/{id:int}")] //约束路由{id:int}
        public string Get(int id)
        {
            return JsonConvert.SerializeObject(_iTb_LogService.Find<tb_log>(id));
        }

    }
}
