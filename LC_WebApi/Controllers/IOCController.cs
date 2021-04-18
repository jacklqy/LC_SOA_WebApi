using LC.SOA.Interface;
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

namespace LC_WebApi.Controllers
{
    /// <summary>
    /// NuGet引用Unity/Unity.Interception/Unity.Abstractions/Unity.Container/Unity.Configuration/Unity.Interception.Configuration
    /// </summary>
    public class IOCController : ApiController
    {
        private readonly ITbLogService _iTbLogService = null;
        public IOCController(ITbLogService tbLogService)
        {
            _iTbLogService = tbLogService;
        }

        public string Get(int id)
        {
            return JsonConvert.SerializeObject(_iTbLogService.Query(id));
        }

    }
}
