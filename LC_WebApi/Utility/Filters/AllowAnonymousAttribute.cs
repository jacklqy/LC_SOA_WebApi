using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LC_WebApi.Utility.Filters
{
    /// <summary>
    /// 标记此特性的就不会做权限验证
    /// </summary>
    public class AllowAnonymousAttribute : Attribute
    {
    }
}