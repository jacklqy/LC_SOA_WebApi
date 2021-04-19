using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

/// <summary>
/// 9 自宿主WebApi实现--控制台实现WebApi自宿主
/// 
/// NuGet添加引用：Microsoft.AspNet.WebApi/System.Web.Http.SelfHost
/// 程序集里面引用：System.Web
/// </summary>
namespace WebApiSelf
{
    /// <summary>
    /// 启动时必须以管理员身份打开VS，或者在bin目录下以管理员身份运行程序，否则会报权限异常
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new HttpSelfHostConfiguration("http://localhost:7077");
                config.Routes.MapHttpRoute(name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
                using (var sever = new HttpSelfHostServer(config))
                {
                    sever.OpenAsync().Wait();
                    Console.WriteLine("服务已经启动了。。。。");
                    Console.WriteLine("输入任意字符关闭");
                    Console.Read();
                    sever.CloseAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
