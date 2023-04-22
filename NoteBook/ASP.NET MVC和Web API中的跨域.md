**ASP.NET MVC和Web API中的跨域**

[toc]

跨域处理的过滤器：

```C#
/// <summary>
/// 允许CORS跨域访问的过滤器 【默认允许所有跨域】
/// 构造函数指定允许的域["http://www.bing.com","http://www.bing.cn"]
/// </summary>
public class AllowCORSFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// 可以预先添加要允许的源
    /// </summary>
    private readonly List<string> _allowOrigins=new List<string>();
    /// <summary>
    /// 请求来自的域 控制
    /// </summary>
    private readonly List<string> _from_domains = new List<string> { "domain2.com", "domain1.com" };
    
    public AllowCORSFilterAttribute(params string[] allowOrigins) {
        if (allowOrigins!=null && allowOrigins.Length>0)
        {
            _allowOrigins.AddRange(allowOrigins);
        };
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (_allowOrigins.Count>0)
        {
            var origin = filterContext.RequestContext.HttpContext.Request.Headers["Origin"];
            if (origin!=null && _allowOrigins.Contains(origin))
            {
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", origin);
            }
        }
        else
        {
            // 生产环境不推荐设置*
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }
        // 只允许来自指定域的跨域
        //if (_from_domains.Contains(filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host))
        //{
        //    filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //}
        //else{
        //}
        base.OnActionExecuting(filterContext);
    }
}
```

可以添加到全局过滤器中、或者添加奥指定控制器 或 Action 上。



https://stackoverflow.com/questions/6290053/setting-access-control-allow-origin-in-asp-net-mvc-simplest-possible-method

https://w3toppers.com/cors-in-asp-net-mvc5/

https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin

https://www.cnblogs.com/OpenCoder/p/6890703.html

https://www.itbaoku.cn/post/554692.html?view=all

https://blog.csdn.net/baijiafan/article/details/126501682








https://www.jianshu.com/p/89a377c52b48

https://blog.csdn.net/sxbei/article/details/121664583