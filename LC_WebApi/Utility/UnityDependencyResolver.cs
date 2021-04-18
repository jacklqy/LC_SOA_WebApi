using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Unity;

namespace LC_WebApi.Utility
{
    /// <summary>
    /// 控制器也要注入--完成容器和WebApi框架融合--实现IDependencyResolver，将容器放进去--初始化讲 config.DependencyResolver 换成自定义的Resolver
    /// </summary>
    public class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer _UnityContainer = null;
        public UnityDependencyResolver(IUnityContainer container)
        {
            _UnityContainer = container;
        }

        public IDependencyScope BeginScope()//Scope作用域
        {
            return new UnityDependencyResolver(this._UnityContainer.CreateChildContainer());
        }

        public void Dispose()
        {
            this._UnityContainer.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _UnityContainer.Resolve(serviceType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _UnityContainer.ResolveAll(serviceType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}