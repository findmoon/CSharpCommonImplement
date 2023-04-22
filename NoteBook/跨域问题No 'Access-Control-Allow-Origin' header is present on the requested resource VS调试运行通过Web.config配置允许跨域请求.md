跨域问题No 'Access-Control-Allow-Origin' header is present on the requested resource VS调试运行通过Web.config配置允许跨域请求

Access to XMLHttpRequest at 'https://192.168.103.12:44330/Init' from origin 'http://localhost:44331' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' header is present on the requested resource.

在 ASP.NET MVC 5 中，通过代码过滤器设置 `Access-Control-Allow-Origin:*` 跨域之后，Get 简单请求没有任何问题。

后面使用 POST 方法跨域请求时，CORS preflight 即 OPTION 请求时，就发生了上面的报错信息。

问题的原因还是服务器跨域设置，不允许跨域导致的。

因此，后面尝试在跨域处理的过滤器中，添加另外允许跨域的代码：

```C#
filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");

//filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "*");

// Access-Control-Allow-Headers: <header-name>[, <header-name>]*  ||| Access-Control-Allow-Headers:x-requested-with,content-type
filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
```

仍然直接报错。

后面才意识到，OPTIONS 的这个跨域报错请求，根本就没进入到设置允许跨域的代码，打断点根本就没执行请求，浏览器就报错了。

也就是，ASP.NET MVC根本就没处理(这个跨域)请求。

而能够对请求进行处理的地方，在 ASP.NET MVC 程序的代码之前，就只有 VS调试启动的本地IIS Express了。

于是，想到需要修改 Web.config 站点配置允许跨域。

Web.config 设置允许跨域，在 `configuration` 下 `<system.webServer>` 节点配置。如下示例：

```xml
  <system.webServer>
    <httpProtocol>
      <customHeaders>
        <add name="Access-Control-Allow-Origin" value="*" />
        <add name="Access-Control-Allow-Headers" value="*" />
        <add name="Access-Control-Allow-Methods" value="GET, POST, PUT, DELETE, OPTIONS" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
```

> 自定义的处理跨域的过滤器，要取消设置：
> 
> ```C#
> // Web.config等设置了跨域，则代码中不需要设置。否则可能会重复
> // The 'Access-Control-Allow-Origin' header contains multiple values '*, *', but only one is allowed.
> //filters.Add(new AllowCORSFilterAttribute());
> ```
> 
> 如果使用的 `Microsoft.AspNet.Cors` Nuget 包，其配置 `config.EnableCors` 也需要取消。

设置后重新运行VS网站，跨域请求成功！不再报错。

> 这个请求处理的时机的问题！！！

# 关于跨域的参考推荐


- [No 'Access-Control-Allow-Origin' header is present on the requested resource—when trying to get data from a REST API](https://stackoverflow.com/questions/43871637/no-access-control-allow-origin-header-is-present-on-the-requested-resource-whe) 绝对值得一看

- [Fixing "No 'Access-Control-Allow-Origin' Header Present"](https://www.stackhawk.com/blog/fixing-no-access-control-allow-origin-header-present/) 好文

- [IIS CORS module Configuration Reference](https://learn.microsoft.com/en-us/iis/extensions/cors-module/cors-module-configuration-reference)

- [CORS issue - No 'Access-Control-Allow-Origin' header is present on the requested resource](https://stackoverflow.com/questions/42016126/cors-issue-no-access-control-allow-origin-header-is-present-on-the-requested)

- [Why does my JavaScript code receive a "No 'Access-Control-Allow-Origin' header is present on the requested resource" error, while Postman does not?](https://stackoverflow.com/questions/20035101/why-does-my-javascript-code-receive-a-no-access-control-allow-origin-header-i)

- [Response to preflight request doesn't pass access control check](https://stackoverflow.com/questions/35588699/response-to-preflight-request-doesnt-pass-access-control-check)

- [asp.net webapi 跨域访问 在vs调试里面和部署到IIS里面的配置问题](https://blog.csdn.net/caikeyter/article/details/82865082)

- [Spring CORS No 'Access-Control-Allow-Origin' header is present](https://stackoverflow.com/questions/35091524/spring-cors-no-access-control-allow-origin-header-is-present)

- [解决 js ajax跨域访问报“No ‘Access-Control-Allow-Origin‘ header is present on the requested resource.”错误](https://blog.csdn.net/cnyygj/article/details/68489200)