using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using System.Reflection;

namespace LC_WebApi.Utility.Filters
{
    /// <summary>
    /// http请求Basic权限认证：在请求方法上标记就会在调用方法前执行权限认证
    /// </summary>
    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// action前会先来这里完成权限校验
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //actionContext.Request.Headers["Authorization"]
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().FirstOrDefault() != null)//方法是否有标记此特性
            {
                return;//继续
            }
            else if (actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().FirstOrDefault() != null)//控制器是否有标记此特性
            {
                return;//继续
            }
            else
            {
                var authorization = actionContext.Request.Headers.Authorization;
                if (authorization == null)
                {
                    this.HandlerUnAuthorization(actionContext);
                }
                else if (this.ValidateTicket(authorization.Parameter))
                {
                    return;//继续
                }
                else
                {
                    this.HandlerUnAuthorization(actionContext);
                }
            }
        }

        private void HandlerUnAuthorization(HttpActionContext actionContext)
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
            //actionContext.Response = new System.Net.Http.HttpResponseMessage()
            //{
            //    StatusCode = System.Net.HttpStatusCode.Unauthorized,
            //};
        }
        private bool ValidateTicket(string encryptTicket)
        {
            //解密Ticket
            if (string.IsNullOrWhiteSpace(encryptTicket))
                return false;
            try
            {
                var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;
                //if (FormsAuthentication.Decrypt(encryptTicket).Expired) return false;//验证票据是否过期
                return string.Equals(strTicket, string.Format("{0}&{1}", "Admin", "123456"));//应该分拆后去数据库验证
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}