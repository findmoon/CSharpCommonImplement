**ASP.NET Core 6自定义重定向和重写，实现.html扩展名伪静态，提供静态文件，返回纯HTML文件或内容**

[toc]

> 位于 ASP.NET Core 6 的 Web API `SignalRBasic` 项目下。

html、js、css等前端静态文件结构目录如下，wwwroot位于项目根目录，其中给出几个用于测试的html文件。

```sh
SignalRBasic
    --- wwwroot
        --- css
        --- js
        --- lib
        --- Examples
            --- 其他示例html文件
        --- index.html
        --- test.html
        --- Tic-Tac-Toe.html
```

# 简要说明

静态文件存放在项目的 web root 目录，默认路径为`{content root}/wwwroot`，可以使用 `UseWebRoot` 方法修改。

[Content root](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-7.0#content-root) 和 [Web root](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-7.0#web-root).

`CreateBuilder` 方法设置当前目录为 content root：

```C#
var builder = WebApplication.CreateBuilder(args);
```

静态文件的访问通过相对于 web root 的 **相对路径** 实现。

比如 `https://<hostname>/images/<image_file_name>` 访问静态图片文件，其位置为 `wwwroot/images/` 下，如 `wwwroot/images/MyImage.jpg`。

> ```sh
> <img src="~/images/MyImage.jpg" class="img" alt="My image" />
> ```
>
> tilde character(波浪号) `~` 指向 web root。

# 静态文件服务，html文件

`Program.cs` 文件，添加 **`app.UseStaticFiles()` 方法启用静态文件服务**，`app.UseDefaultFiles` 添加默认文件，可以根据需要添加其他的默认文件，如 index.htm、main.html 等。

```cs
    // .....
    app.UseHttpsRedirection();

    #region 静态文件 html 服务
    DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
    //defaultFilesOptions.DefaultFileNames.Clear();
    //设置首页，用户打开`localhost`访问到的是`wwwroot`下的Index.html文件
    defaultFilesOptions.DefaultFileNames.Add("index.html");
    //defaultFilesOptions.RedirectToAppendTrailingSlash=true;
    
    app.UseDefaultFiles(defaultFilesOptions);

    //启用静态文件
    app.UseStaticFiles();

    #endregion


    app.UseAuthorization();
```

这样可以实现访问 `http://localhost:5000/` 返回 wwwroot 下的 index.html 文件。

同时 `http://localhost:5000/index.html` `http://localhost:5000/Tic-Tac-Toe.html` 可以访问到对应的 html 静态文件，及其文件内链接的css、js等。

> `app.UseStaticFiles()` 的位置可以根据需要位于 `UseAuthorization` 授权之前还是之后。

# 自定义中间件 实现重定向和重写

> 注：**重定向后，中间件应该直接返回，没有必要执行其他的处理**，否则可能达不到重定向的效果。

中间件中 `context.Response.Redirect()` 方法可以实现重定向。将`.html`后缀重定向为无后缀的url地址。

`context.Request.Path` 重新赋值，实现重写rewrite效果。通过判断 web root 下的html文件是否存在，存在则为路径添加`.html`后缀，使后续的 静态文件服务(中间件) 可以提供返回wwwroot对应路径的html纯前端页面。

```C#
        var app = builder.Build();
        
        #region [重定向 和 重写 的 自定义(中间件)]对存在的文件，添加 .html 扩展名
        app.Use(async (context, next) =>
        {
            var url = context.Request.Path.Value??"";
            // !url.EndsWith('/') &&  不需要
            if (url.Trim().Trim('/').Length>0)
            {
                // 重定向 .html 为无扩展名的路径，实现伪静态
                var pseudoStaticExtenName = ".html";
                if (url.EndsWith(pseudoStaticExtenName))
                {
                    context.Response.Redirect(url.Substring(0, url.Length- pseudoStaticExtenName.Length));
                    // 默认为 302 redirect 临时重定向: 302 代表暂时性转移(Temporarily Moved )。
                    // 301 redirect 永久重定向: 301 代表永久性转移(Permanently Moved)
                    return; // 此处为重定向,重定向后则不需要执行之后的中间件

                    //var option = new RewriteOptions();
                    //option.AddRedirect()
                }
                else
                {
                    if (url.EndsWith('/'))
                    {
                        url = url.TrimEnd('/');
                    }
                    // 判断 .html 静态页面是否存在。存在，则为伪静态的页面，添加扩展名 【也可以扩展除.html之外的其他类型】
                    // 此处本质为URL重写
                    // https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/url-rewriting?view=aspnetcore-6.0
                    if (File.Exists(Path.Combine("wwwroot", Path.Combine($"{url}{pseudoStaticExtenName}".Split('/')))))
                    {
                        context.Request.Path = $"{url}{pseudoStaticExtenName}";
                    }
                }
            }
            await next();
        });
        #endregion
```

此时访问 `http://localhost:5000/index.html` `http://localhost:5000/Tic-Tac-Toe.html` 将会被重定向到无后缀的url路径。

`http://localhost:5000` `http://localhost:5000/index` `http://localhost:5000/Tic-Tac-Toe` 均可以访问到正确的html静态文件。

# 提供 其他静态文件路径

如果不想使用默认的 `/wwwroot` 路径作为静态文件路径，则可以通过 `UseFileServer` 或 `UseWebRoot` 方法指定新位置。

> `UseFileServer` 结合了 `UseStaticFiles`、`UseDefaultFiles` 和可选的 `UseDirectoryBrowser` 的功能。

比如，在 `/StaticFiles` 路径内 提供前端静态文件内容(服务)：

```C#
        // ....
        app.UseFileServer(new FileServerOptions  
        {  
            FileProvider = new PhysicalFileProvider(  
                Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),  
            RequestPath = "/StaticFiles",  
            EnableDefaultFiles = true  
        }) ;  

        
        app.UseHttpsRedirection();  

        app.UseRouting();  

        app.UseAuthorization();  

        app.UseEndpoints(endpoints =>  
        {  
            endpoints.MapControllers();  
        });  
```

# 通过文本内容 或 读取指定文件，在Action方法中返回html

## ASP.Net core and .Net 5+

```C#
[ApiController]
[Route("[controller]")]
public class IndexController
{
    public ContentResult Get()
    {
        var fileContents = File.ReadAllText("./Content/HelloWorld.html");
        return new ContentResult
        {
            Content = fileContents,
            ContentType = "text/html"
        };
    }
}
```

或 

`ContentResult` 对象：

```C#
[HttpGet]
public ContentResult Index()
{
    var html = "<p>Welcome to Code Maze</p>";

    return new ContentResult
    {
        Content = html,
        ContentType = "text/html"
    };
}
```

- `ControllerBase.Content` 方法

```C#
// public class UserController : ControllerBase { }

[HttpGet("verify")]
public ContentResult Verify()
{
    var html = "<div>Your account has been verified.</div>";

    return base.Content(html, "text/html");
}
```

- `Response.Body.WriteAsync` 响应写入

```C#
/// <summary>
///     返回html的组合
/// </summary>
/// <returns></returns>
[HttpGet]
[Route("api/Message/ReturnHtml")]
public async Task ReturnHtml()
{
    StringBuilder htmlStringBuilder = new StringBuilder();
    htmlStringBuilder.Append("<html>");
    htmlStringBuilder.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> </head>");//支持中文
    htmlStringBuilder.Append("<body>");
    htmlStringBuilder.Append("<spen style=\"font-size: 300%\">");//让字体变大
    string a = $"返回的内容";
    htmlStringBuilder.Append(a);
    htmlStringBuilder.Append("</spen>");
    htmlStringBuilder.Append("</body>");
    htmlStringBuilder.Append("</html>");
    result = htmlStringBuilder.ToString();
    var data = Encoding.UTF8.GetBytes(result);
    if (accept.Any(x => x.MediaType == "text/html"))
    {
        Response.ContentType = "text/html";
    }
    else
    {
        Response.ContentType = "text/plain";
    }
    await Response.Body.WriteAsync(data, 0, data.Length);
}
```

## ASP.NET Web 

使用 `HttpContext.Current.Server.MapPath` ：

```C#
[HttpGet]
public HttpResponseMessage HelloWorld()
{
   var fileContents = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/HelloWorld.html"));
   var response = new HttpResponseMessage();
   response.Content = new StringContent(fileContents);
   response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
   return response;
}
```

读取文件也可以修改为：

```C#
var fileContents = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Content/HelloWorld.html"));
```

或

```C#
public ActionResult Index()
{

    var staticPageToRender = new FilePathResult("~/img/test.html", "text/html");
    return staticPageToRender;
}
```

# 附：URL Rewrite 中间件实现 HTTP 重定向到 HTTPS

```C#
var options = new RewriteOptions()
    .AddRedirectToHttpsPermanent();

app.UseRewriter(options);
```

> `Microsoft.AspNetCore.Rewrite` 以 nuget 包的形式提供。

# 参考

- [如何在 ASP.NET Core 中重写 URL](https://blog.csdn.net/gangzhucoll/article/details/121718446) 好文

- [Static files in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files)

- [Using Static Files (HTML, JavaScript) In Web API](https://www.c-sharpcorner.com/article/using-static-files-html-javascript-in-web-api/)

- [Returning HTML or any file using Asp.Net controller](https://peterdaugaardrasmussen.com/2017/03/11/returning-html-or-any-file-using-web-api/)

- [ASP.NET CORE WebAPI项目 设置HTML静态页面为首页](https://www.cnblogs.com/QiFen9/p/13213970.html)

# 其他

- [ASP.NET MVC重新定向并在路径中使用HTML扩展名实现伪静态-配置Web.config和代码`MapRoute`](https://www.cnblogs.com/djd66/p/15238368.html)

```xml
<system.webServer>
      <validation validateIntegratedModeConfiguration="false"/>
      <modules runAllManagedModulesForAllRequests="true" />
</system.webServer>
```

```C#
 routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}.html",
                defaults: new { controller = "Home", action = "Info", id = UrlParameter.Optional }
            );
```

- [ASP.NET Core 中的 URL 重写中间件](https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/url-rewriting?view=aspnetcore-6.0)
- [How to server or return html file from action method](https://social.msdn.microsoft.com/Forums/en-US/d9ff31a5-d5f1-44ab-9f51-058ee3a0f5ac/how-to-server-or-return-html-file-from-action-method?forum=aspmvc)
- [ASP.NET Core Web API Files and Folders](https://dotnettutorials.net/lesson/asp-net-core-web-api-files-and-folders/)
- [How to Return HTML From ASP.NET Core Web API](https://code-maze.com/aspnetcore-webapi-return-html/)

- [Asp.Net中通常不修改IIS实现URL重写,支持任意扩展名及无扩展名(伪静态)](https://blog.csdn.net/ysq5202121/article/details/7004896)
