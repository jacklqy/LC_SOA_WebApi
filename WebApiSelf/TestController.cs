using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiSelf
{
    /// <summary>
    /// 服务启动后浏览器输入测试：http://localhost:7077/api/test 和 http://localhost:7077/api/test/1
    /// </summary>
    public class TestController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value Get";
        }


    }
}
