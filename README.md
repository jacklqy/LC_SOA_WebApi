# LC_SOA_WebApi
    /// 1 RESTful架构风格和WebApi  
    /// 2 WebApi路由&特性路由
    /// 3 WebApi依赖注入&面向切面 (NuGet引用Unity/Unity.Interception/Unity.Abstractions/Unity.Container/Unity.Configuration/Unity.Interception.Configuration)
    /// 4 WebApi前台调用详析
    /// 5 WebApi后台调用详析
    /// 6 Basic授权认证&权限Filter ：WebApi默认是不支持session的，因为WebApi是无状态的
    ///    无状态：第二次请求和第一次请求不关联
    ///    a)登陆过程，拿到令牌---token/ticket/许可证
    ///    b)登陆验证成功---账号+密码+其它信息+时间---加密一下得到ticket---返回给客户端
    ///    c)客户端请求时ajax就带上这个ticket
    ///    d)接口调用时，就去验证ticket，解密一下，看看信息(缓存或数据库比较)，看看失效时间
    /// 每个方法都验证令牌？就可以基于filter来实现验证，http请求头文件BasicAuth带上ticket传到服务器验证。
    /// 7 异常处理Filter和ActionFilter
    /// 8 WebApi跨域请求：浏览器请求时，如果A网站(域名+端口)页面里面，通过XHR去请求B域名，这个就是跨域
    ///                   这个请求是可以正常到达B服务器后端，正常的相应(200)
    ///                   但是，浏览器不允许这样操作，除非在响应里面有声明(Access-Control-Allow-Origin)
    ///                   为什么不允许呢？是因为浏览器有一个同源策略，出于安全考虑，浏览器限制脚本去发起跨域请求。
    ///                   但是，页面里面的script-src/img-href/js/css/图片 这些是浏览器自己发起的，是可以跨域的
    ///                   a)JSONP---脚本标签自动请求--请求回来的内容执行个回调方法--解析数据 （前端解决方案，比较流行）
    ///                   b)CORS 跨域资源共享，允许服务器在响应头里面指定Access-Control-Allow-Origin，浏览器就会案子响应来操作。NuGet添加引用Microsoft.AspNet.WebApi.Cors
    /// 9 自宿主WebApi实现：WebApiSelf这个项目就是自宿主测试，就可以不用部署在IIS服务器了。一般是放在IIS服务器，这种自宿主不常用。
    /// 10 自动生成WebApi文档：Areas-》HelpPage-》Views-》Help-》Index.cshtml 就是api接口说明文档，框架已经为我们自动生成了一个区域路由来展示。所以只需在浏览器输入：http://localhost:50439/Help/Index 就可以查看到WebApi文档。
    /// 

![image](https://user-images.githubusercontent.com/26539681/115218867-93371300-a139-11eb-9039-03cc5ff3e8c5.png)
