using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

/// <summary>
/// 5 WebApi后台调用详析
/// </summary>
namespace UnitTestWebApi
{
    /// <summary>
    /// HttpClient内部有个连接池，各个请求是隔开的，可以复用链接的 实际应该单例一下
    /// 
    /// 要求HttpClient的实例化都从这里来  全局只要一个实例，不要using
    /// 各个请求是隔开的，可以复用链接的 
    /// </summary>
    public class HttpClientFactory
    {
        private static HttpClient _HttpClient = null;
        static HttpClientFactory()
        {
            _HttpClient = new HttpClient(new HttpClientHandler());
        }
        public static HttpClient GetHttpClient()
        {
            return _HttpClient;
        }
    }



    /// <summary>
    /// WebApi全部遵循的Http协议，HttpMethod
    /// 其实就等同于网页，可以去爬虫
    /// 就时后端模拟Http请求
    /// 
    /// HttpWebRequest-》原始请求方式，很多东西需要自己传入
    /// HttpClient-》.net fromwork4.0出来的，底层做了封装
    /// 
    /// HttpClient-->天生就是为WebApi而生的
    /// using (var http = new HttpClient(handler)) 不太好 tcp其实并不能立即释放掉
    /// 如果高并发式的这样操作，会出现资源不够  积极拒绝
    /// HttpClient内部有个连接池，各个请求是隔开的，可以复用链接的 实际应该单例一下
    /// </summary>
    [TestClass]
    public class UnitTestWebApi
    {
        [TestMethod]
        public void TestMethod()
        {
            this.AuthorizationDemo();

            var result1 = this.GetClient();
            var result2 = this.GetWebQuest();
            var result3 = this.PostClient();
            var result4 = this.PostWebQuest();
            var result5 = this.PutClient();
            var result6 = this.PutWebQuest();
            var result7 = this.DeleteClient();//传值只能是url
            var result8 = this.DeleteWebQuest();
            //Console.WriteLine();
        }

        #region HttpClient Get
        /// <summary>
        /// HttpClient实现Get请求
        /// </summary>
        private string GetClient()
        {
            //string url = "http://localhost:50439/api/users/GetUserByName?username=superman";
            //string url = "http://localhost:50439/api/users/GetUserByID?id=1";
            //string url = "http://localhost:50439/api/users/GetUserByNameId?userName=Superman&id=1";
            //string url = "http://localhost:50439/api/users/Get";
            //string url = "http://localhost:50439/api/users/GetUserByModel?UserID=11&UserName=Eleven&UserEmail=57265177%40qq.com";
            string url = "http://localhost:50439/api/users/GetUserByModelUri?UserID=11&UserName=Eleven&UserEmail=57265177%40qq.com";
            //string url = "http://localhost:50439/api/users/GetUserByModelSerialize?userString=%7B%22UserID%22%3A%2211%22%2C%22UserName%22%3A%22Eleven%22%2C%22UserEmail%22%3A%2257265177%40qq.com%22%7D";
            //string url = "http://localhost:50439/api/users/GetUserByModelSerializeWithoutGet?userString=%7B%22UserID%22%3A%2211%22%2C%22UserName%22%3A%22Eleven%22%2C%22UserEmail%22%3A%2257265177%40qq.com%22%7D";
            //string url = "http://localhost:50439/api/users/NoGetUserByModelSerializeWithoutGet?userString=%7B%22UserID%22%3A%2211%22%2C%22UserName%22%3A%22Eleven%22%2C%22UserEmail%22%3A%2257265177%40qq.com%22%7D";
            var handler = new HttpClientHandler();//{ AutomaticDecompression = DecompressionMethods.GZip };

            using (var http = new HttpClient(handler))
            {
                var response = http.GetAsync(url).Result;//拿到异步结果
                Console.WriteLine(response.StatusCode); //确保HTTP成功状态值
                //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        #endregion

        #region HttpWebRequest实现Get请求
        /// <summary>
        /// HttpWebRequest实现Get请求
        /// </summary>
        private string GetWebQuest()
        {
            //string url = "http://localhost:50439/api/users/GetUserByName?username=superman";
            //string url = "http://localhost:50439/api/users/GetUserByID?id=1";
            //string url = "http://localhost:50439/api/users/GetUserByNameId?userName=Superman&id=1";
            //string url = "http://localhost:50439/api/users/Get";
            //string url = "http://localhost:50439/api/users/GetUserByModel?UserID=11&UserName=Eleven&UserEmail=57265177%40qq.com";
            //string url = "http://localhost:50439/api/users/GetUserByModelUri?UserID=11&UserName=Eleven&UserEmail=57265177%40qq.com";
            string url = "http://localhost:50439/api/users/GetUserByModelSerialize?userString=%7B%22UserID%22%3A%2211%22%2C%22UserName%22%3A%22Eleven%22%2C%22UserEmail%22%3A%2257265177%40qq.com%22%7D";
            //string url = "http://localhost:50439/api/users/GetUserByModelSerializeWithoutGet?userString=%7B%22UserID%22%3A%2211%22%2C%22UserName%22%3A%22Eleven%22%2C%22UserEmail%22%3A%2257265177%40qq.com%22%7D";
            //string url = "http://localhost:50439/api/users/NoGetUserByModelSerializeWithoutGet?userString=%7B%22UserID%22%3A%2211%22%2C%22UserName%22%3A%22Eleven%22%2C%22UserEmail%22%3A%2257265177%40qq.com%22%7D";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 30 * 1000;

            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            //request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            string result = "";
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        #endregion

        #region HttpClient实现Post请求
        /// <summary>
        /// HttpClient实现Post请求
        /// </summary>
        private string PostClient()
        {
            //string url = "http://localhost:50439/api/users/register";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"","1" }
            //};

            //string url = "http://localhost:50439/api/users/RegisterNoKey";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"","1" }
            //};

            //string url = "http://localhost:50439/api/users/RegisterUser";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"UserID","11" },
            //    {"UserName","Eleven" },
            //    {"UserEmail","57265177@qq.com" },
            //};

            string url = "http://localhost:50439/api/users/RegisterObject";
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"User[UserID]","11" },
                {"User[UserName]","Eleven" },
                {"User[UserEmail]","57265177@qq.com" },
                {"Info","this is muti model" }
            };

            HttpClientHandler handler = new HttpClientHandler();
            using (var http = new HttpClient(handler))
            {
                //使用FormUrlEncodedContent做HttpContent
                var content = new FormUrlEncodedContent(dic);
                var response = http.PostAsync(url, content).Result;
                Console.WriteLine(response.StatusCode); //确保HTTP成功状态值
                return response.Content.ReadAsStringAsync().Result;
            }

        }
        #endregion

        #region  HttpWebRequest实现post请求
        /// <summary>
        /// HttpWebRequest实现post请求
        /// </summary>
        private string PostWebQuest()
        {
            //string url = "http://localhost:50439/api/users/register";
            //var postData = "1";

            //string url = "http://localhost:50439/api/users/RegisterNoKey";
            //var postData = "1";

            var user = new
            {
                UserID = "11",
                UserName = "Eleven",
                UserEmail = "57265177@qq.com"
            };
            //string url = "http://localhost:50439/api/users/RegisterUser";
            //var postData = JsonHelper.ObjectToString(user);

            var userOther = new
            {
                User = user,
                Info = "this is muti model"
            };
            string url = "http://localhost:50439/api/users/RegisterObject";
            var postData = Newtonsoft.Json.JsonConvert.SerializeObject(userOther);

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 30 * 1000;//设置30s的超时
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "POST";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            postStream.Write(data, 0, data.Length);
            postStream.Close();

            string result = "";
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion

        #region HttpClient实现Put请求
        /// <summary>
        /// HttpClient实现Put请求
        /// </summary>
        private string PutClient()
        {
            string url = "http://localhost:50439/api/users/RegisterPut";
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"","1" }
            };

            //string url = "http://localhost:50439/api/users/RegisterNoKeyPut";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"","1" }
            //};

            //string url = "http://localhost:50439/api/users/RegisterUserPut";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"UserID","11" },
            //    {"UserName","Eleven" },
            //    {"UserEmail","57265177@qq.com" },
            //};

            //string url = "http://localhost:50439/api/users/RegisterObjectPut";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"User[UserID]","11" },
            //    {"User[UserName]","Eleven" },
            //    {"User[UserEmail]","57265177@qq.com" },
            //    {"Info","this is muti model" }
            //};

            HttpClientHandler handler = new HttpClientHandler();
            using (var http = new HttpClient(handler))
            {
                //使用FormUrlEncodedContent做HttpContent
                var content = new FormUrlEncodedContent(dic);
                var response = http.PutAsync(url, content).Result;
                Console.WriteLine(response.StatusCode); //确保HTTP成功状态值
                return response.Content.ReadAsStringAsync().Result;
            }

        }
        #endregion

        #region  HttpWebRequest实现put请求
        /// <summary>
        /// HttpWebRequest实现put请求
        /// </summary>
        private string PutWebQuest()
        {
            //string url = "http://localhost:50439/api/users/registerPut";
            //var postData = "1";

            //string url = "http://localhost:50439/api/users/RegisterNoKeyPut";
            //var postData = "1";

            var user = new
            {
                UserID = "11",
                UserName = "Eleven",
                UserEmail = "57265177@qq.com"
            };
            //string url = "http://localhost:50439/api/users/RegisterUserPut";
            //var postData = JsonHelper.ObjectToString(user);

            var userOther = new
            {
                User = user,
                Info = "this is muti model"
            };
            string url = "http://localhost:50439/api/users/RegisterObjectPut";
            var postData = Newtonsoft.Json.JsonConvert.SerializeObject(userOther);

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 30 * 1000;//设置30s的超时
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "PUT";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            postStream.Write(data, 0, data.Length);
            postStream.Close();

            string result = "";
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion

        #region HttpClient实现Delete请求
        /// <summary>
        /// HttpClient实现Put请求
        /// 没放入数据
        /// </summary>
        private string DeleteClient()
        {
            string url = "http://localhost:50439/api/users/RegisterDelete/1";
            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"","1" }
            };

            //string url = "http://localhost:50439/api/users/RegisterNoKeyDelete";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"","1" }
            //};

            //string url = "http://localhost:50439/api/users/RegisterUserDelete";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"UserID","11" },
            //    {"UserName","Eleven" },
            //    {"UserEmail","57265177@qq.com" },
            //};

            //string url = "http://localhost:50439/api/users/RegisterObjectDelete";
            //Dictionary<string, string> dic = new Dictionary<string, string>()
            //{
            //    {"User[UserID]","11" },
            //    {"User[UserName]","Eleven" },
            //    {"User[UserEmail]","57265177@qq.com" },
            //    {"Info","this is muti model" }
            //};

            HttpClientHandler handler = new HttpClientHandler();
            using (var http = new HttpClient(handler))
            {
                //使用FormUrlEncodedContent做HttpContent
                var content = new FormUrlEncodedContent(dic);
                var response = http.DeleteAsync(url).Result;
                Console.WriteLine(response.StatusCode); //确保HTTP成功状态值
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        #endregion

        #region  HttpWebRequest实现put请求
        /// <summary>
        /// HttpWebRequest实现put请求
        /// </summary>
        private string DeleteWebQuest()
        {
            //string url = "http://localhost:50439/api/users/registerDelete";
            //var postData = "1";

            //string url = "http://localhost:50439/api/users/RegisterNoKeyDelete";
            //var postData = "1";

            var user = new
            {
                UserID = "11",
                UserName = "Eleven",
                UserEmail = "57265177@qq.com"
            };
            //string url = "http://localhost:50439/api/users/RegisterUserDelete";
            //var postData = JsonHelper.ObjectToString(user);

            var userOther = new
            {
                User = user,
                Info = "this is muti model"
            };
            string url = "http://localhost:50439/api/users/RegisterObjectDelete";
            var postData = Newtonsoft.Json.JsonConvert.SerializeObject(userOther);

            var request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 30 * 1000;//设置30s的超时
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            request.Method = "Delete";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            Stream postStream = request.GetRequestStream();
            postStream.Write(data, 0, data.Length);
            postStream.Close();

            string result = "";
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        #endregion


        #region 用户登陆 获取ticket后使用
        private void AuthorizationDemo()
        {
            string ticket = "";
            {
                string loginUrl = "http://localhost:50439/api/users/Login?Account=Admin&Password=123456";
                var handler = new HttpClientHandler();//{ AutomaticDecompression = DecompressionMethods.GZip };

                using (var http = new HttpClient(handler))
                {
                    var response = http.GetAsync(loginUrl).Result;//拿到异步结果
                    Console.WriteLine(response.StatusCode); //确保HTTP成功状态值
                                                            //await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                    ticket = response.Content.ReadAsStringAsync().Result.Replace("\"{\\\"Result\\\":true,\\\"Ticket\\\":\\\"", "").Replace("\\\"}\"", "");
                    //ticket = JsonHelper.StringToObject<TicketModel>(response.Content.ReadAsStringAsync().Result).Ticket;
                }
            }
            //带权限认证的请求
            {
                string url = "http://localhost:50439/api/users/GetUserByName?username=superman";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 30 * 1000;
                request.Headers.Add(HttpRequestHeader.Authorization, "BasicAuth " + ticket);//头文件增加Authorization
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
                //request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                string result = "";
                using (var res = request.GetResponse() as HttpWebResponse)
                {
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                        result = reader.ReadToEnd();
                    }
                }
            }
            {
                string url = "http://localhost:50439/api/users/GetUserByName?username=superman";
                var handler = new HttpClientHandler();

                using (var http = new HttpClient(handler))
                {
                    http.DefaultRequestHeaders.Add("Authorization", "BasicAuth " + ticket);//头文件增加Authorization
                    var response = http.GetAsync(url).Result;
                    Console.WriteLine(response.StatusCode);
                    string result = response.Content.ReadAsStringAsync().Result;
                }
            }
        }
        #endregion
    }
}
