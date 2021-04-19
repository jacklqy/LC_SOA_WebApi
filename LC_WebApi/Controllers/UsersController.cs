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
using System.Web.Http.Cors;
using System.Web.Security;
using Unity;

/// <summary>
/// 4 WebApi前台调用详析
/// 6 Basic授权认证&权限Filter
/// </summary>
namespace LC_WebApi.Controllers
{
    //[BasicAuthorizeAttribute]//控制器里面的全部方法都进行权限验证
    //[ExceptionFilterAttribute]//控制器里面的全部方法都进行异常驳回
    public class UsersController : ApiController
    {
        private readonly ITb_LogService _iTb_LogService = null;
        public UsersController(ITb_LogService tb_LogService)
        {
            _iTb_LogService = tb_LogService;
        }

        /// <summary>
        /// User Data List
        /// </summary>
        private List<Users> _userList = new List<Users>
        {
            new Users {UserID = 1, UserName = "Superman", UserEmail = "Superman@cnblogs.com"},
            new Users {UserID = 2, UserName = "Spiderman", UserEmail = "Spiderman@cnblogs.com"},
            new Users {UserID = 3, UserName = "Batman", UserEmail = "Batman@cnblogs.com"}
        };

        #region 用户登陆
        //[AllowAnonymous] //允许匿名访问，无效权限认证--框架提供
        [Utility.Filters.AllowAnonymousAttribute]//自定义的
        [HttpGet]
        public string Login(string account, string password)
        {
            if ("Admin".Equals(account) && "123456".Equals(password))//应该数据库校验
            {
                //.net框架提供-》支持设置失效时间等...
                FormsAuthenticationTicket ticketObject = new FormsAuthenticationTicket(0, account, DateTime.Now, DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", account, password), FormsAuthentication.FormsCookiePath);
                var result = new
                {
                    Result = true,
                    Ticket = FormsAuthentication.Encrypt(ticketObject)
                };
                return JsonConvert.SerializeObject(result);
            }
            else
            {
                var result = new { Result = false };
                return JsonConvert.SerializeObject(result);
            }
        }
        #endregion

        #region HttpGet
        // GET api/Users
        [HttpGet]
        //[ExceptionFilterAttribute]//异常捕获特性
        [ActionFilterAttribute]
        public IEnumerable<Users> Get()
        {
            throw new Exception();//异常捕获测试
            return _userList;
        }

        // GET api/Users/5
        [HttpGet]
        //[AllowAnonymous]
        [EnableCors(origins: "http://localhost:9008/", headers: "*", methods: "GET,POST,PUT,DELETE")]//此方法支持跨域请求
        //[BasicAuthorizeAttribute]//方法调用前权限认证
        public Users GetUserByID(int id)
        {
            string idParam = HttpContext.Current.Request.QueryString["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);//异常处理
            }
            return user;
        }

        //GET api/Users/?username=xx
        [HttpGet]
        //[BasicAuthorizeAttribute]//方法调用前权限认证
        public IEnumerable<Users> GetUserByName(string userName)
        {
            string userNameParam = HttpContext.Current.Request.QueryString["userName"];

            return _userList.Where(p => string.Equals(p.UserName, userName, StringComparison.OrdinalIgnoreCase));
        }

        //GET api/Users/?username=xx&id=1
        [HttpGet]
        public IEnumerable<Users> GetUserByNameId(string userName, int id)
        {
            string idParam = HttpContext.Current.Request.QueryString["id"];
            string userNameParam = HttpContext.Current.Request.QueryString["userName"];

            return _userList.Where(p => string.Equals(p.UserName, userName, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// user实体null，因为没有加[FromUri]
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Users> GetUserByModel(Users user)
        {
            string idParam = HttpContext.Current.Request.QueryString["id"];
            string userNameParam = HttpContext.Current.Request.QueryString["userName"];
            string emai = HttpContext.Current.Request.QueryString["email"];

            return _userList;
        }
        /// <summary>
        /// 加了[FromUri]表示从地址获取，这样就能将值赋值给实体对象（通用做法）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Users> GetUserByModelUri([FromUri] Users user)
        {
            string idParam = HttpContext.Current.Request.QueryString["id"];
            string userNameParam = HttpContext.Current.Request.QueryString["userName"];
            string emai = HttpContext.Current.Request.QueryString["email"];

            return _userList;
        }
        /// <summary>
        /// 获取前端传递的序列化的字符串，然后后端在反序列化为实体对象（不是通用做法）
        /// </summary>
        /// <param name="userString"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Users> GetUserByModelSerialize(string userString)
        {
            Users user = JsonConvert.DeserializeObject<Users>(userString);
            return _userList;
        }

        //[HttpGet]
        public IEnumerable<Users> GetUserByModelSerializeWithoutGet(string userString)
        {
            Users user = JsonConvert.DeserializeObject<Users>(userString);
            return _userList;
        }
        /// <summary>
        /// 方法名以Get开头，WebApi会自动默认这个请求就是get请求，而如果你以其他名称开头而又不标注方法的请求方式，那么这个时候服务器虽然找到了这个方法，但是由于请求方式不确定，所以直接返回给你405——方法不被允许的错误。
        /// 最后结论：所有的WebApi方法最好是加上请求的方式（[HttpGet]/[HttpPost]/[HttpPut]/[HttpDelete]），不要偷懒，这样既能防止类似的错误，也有利于方法的维护，别人一看就知道这个方法是什么请求。
        /// </summary>
        /// <param name="userString"></param>
        /// <returns></returns>
        public IEnumerable<Users> NoGetUserByModelSerializeWithoutGet(string userString)
        {
            Users user = JsonConvert.DeserializeObject<Users>(userString);
            return _userList;
        }
        #endregion HttpGet

        #region HttpPost
        //POST api/Users/RegisterNone
        [HttpPost]
        public Users RegisterNone()
        {
            return _userList.FirstOrDefault();
        }

        [HttpPost]
        public Users RegisterNoKey([FromBody] int id)
        {
            string idParam = HttpContext.Current.Request.Form["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //POST api/Users/register
        //只接受一个参数的需要不给key才能拿到
        [HttpPost]
        public Users Register([FromBody] int id)//可以来自FromBody   FromUri
                                                //public Users Register(int id)//可以来自url
        {
            string idParam = HttpContext.Current.Request.Form["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //POST api/Users/RegisterUser
        [HttpPost]
        public Users RegisterUser(Users user)//可以来自FromBody   FromUri，但是默认优先FromBody
        {
            string idParam = HttpContext.Current.Request.Form["UserID"];
            string nameParam = HttpContext.Current.Request.Form["UserName"];
            string emailParam = HttpContext.Current.Request.Form["UserEmail"];

            //var userContent = base.ControllerContext.Request.Content.ReadAsFormDataAsync().Result;
            var stringContent = base.ControllerContext.Request.Content.ReadAsStringAsync().Result;
            return user;
        }


        //POST api/Users/register
        [HttpPost]
        public string RegisterObject(JObject jData)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["User[UserID]"];
            string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
            string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
            string infoParam = HttpContext.Current.Request.Form["info"];
            dynamic json = jData;
            JObject jUser = json.User;
            string info = json.Info;
            var user = jUser.ToObject<Users>();

            return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        }

        [HttpPost]
        public string RegisterObjectDynamic(dynamic dynamicData)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["User[UserID]"];
            string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
            string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
            string infoParam = HttpContext.Current.Request.Form["info"];
            dynamic json = dynamicData;
            JObject jUser = json.User;
            string info = json.Info;
            var user = jUser.ToObject<Users>();

            return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        }
        #endregion HttpPost

        #region HttpPut
        //POST api/Users/RegisterNonePut
        [HttpPut]
        public Users RegisterNonePut()
        {
            return _userList.FirstOrDefault();
        }

        [HttpPut]
        public Users RegisterNoKeyPut([FromBody] int id)
        {
            string idParam = HttpContext.Current.Request.Form["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //POST api/Users/registerPut
        //只接受一个参数的需要不给key才能拿到
        [HttpPut]
        public Users RegisterPut([FromBody] int id)//可以来自FromBody   FromUri
                                                   //public Users Register(int id)//可以来自url
        {
            string idParam = HttpContext.Current.Request.Form["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //POST api/Users/RegisterUserPut
        [HttpPut]
        public Users RegisterUserPut(Users user)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["UserID"];
            string nameParam = HttpContext.Current.Request.Form["UserName"];
            string emailParam = HttpContext.Current.Request.Form["UserEmail"];

            //var userContent = base.ControllerContext.Request.Content.ReadAsFormDataAsync().Result;
            var stringContent = base.ControllerContext.Request.Content.ReadAsStringAsync().Result;
            return user;
        }


        //POST api/Users/registerPut
        [HttpPut]
        public string RegisterObjectPut(JObject jData)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["User[UserID]"];
            string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
            string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
            string infoParam = HttpContext.Current.Request.Form["info"];
            dynamic json = jData;
            JObject jUser = json.User;
            string info = json.Info;
            var user = jUser.ToObject<Users>();

            return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        }

        [HttpPut]
        public string RegisterObjectDynamicPut(dynamic dynamicData)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["User[UserID]"];
            string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
            string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
            string infoParam = HttpContext.Current.Request.Form["info"];
            dynamic json = dynamicData;
            JObject jUser = json.User;
            string info = json.Info;
            var user = jUser.ToObject<Users>();

            return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        }
        #endregion HttpPut

        #region HttpDelete
        //POST api/Users/RegisterNoneDelete
        [HttpDelete]
        public Users RegisterNoneDelete()
        {
            return _userList.FirstOrDefault();
        }

        [HttpDelete]
        public Users RegisterNoKeyDelete([FromBody] int id)
        {
            string idParam = HttpContext.Current.Request.Form["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //POST api/Users/registerDelete
        //只接受一个参数的需要不给key才能拿到
        [HttpDelete]
        public Users RegisterDelete([FromBody] int id)//可以来自FromBody   FromUri
                                                      //public Users Register(int id)//可以来自url
        {
            string idParam = HttpContext.Current.Request.Form["id"];

            var user = _userList.FirstOrDefault(users => users.UserID == id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return user;
        }

        //POST api/Users/RegisterUserDelete
        [HttpDelete]
        public Users RegisterUserDelete(Users user)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["UserID"];
            string nameParam = HttpContext.Current.Request.Form["UserName"];
            string emailParam = HttpContext.Current.Request.Form["UserEmail"];

            //var userContent = base.ControllerContext.Request.Content.ReadAsFormDataAsync().Result;
            var stringContent = base.ControllerContext.Request.Content.ReadAsStringAsync().Result;
            return user;
        }


        //POST api/Users/registerDelete
        [HttpDelete]
        public string RegisterObjectDelete(JObject jData)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["User[UserID]"];
            string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
            string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
            string infoParam = HttpContext.Current.Request.Form["info"];
            dynamic json = jData;
            JObject jUser = json.User;
            string info = json.Info;
            var user = jUser.ToObject<Users>();

            return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        }

        [HttpDelete]
        public string RegisterObjectDynamicDelete(dynamic dynamicData)//可以来自FromBody   FromUri
        {
            string idParam = HttpContext.Current.Request.Form["User[UserID]"];
            string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
            string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
            string infoParam = HttpContext.Current.Request.Form["info"];
            dynamic json = dynamicData;
            JObject jUser = json.User;
            string info = json.Info;
            var user = jUser.ToObject<Users>();

            return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        }
        #endregion HttpDelete
    }
}