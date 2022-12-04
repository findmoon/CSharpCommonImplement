**ASP.NET Core 6使用内置 HTTP Logging 中间件记录请求返回信息到日志中【及RequestBody记录的大坑】**

[toc]

# RequestBody 请求体无法记录的坑

先说一下这个坑。

如果要记录 RequestBody请求体 的内容，必须必须**要有模型绑定。** 也就是在对应的 Action 中有相关的model类对象，它用于模型绑定请求体。只有有参数、有模型绑定，哪怕是一个不匹配的模型类，才能记录下来，否则 `RequestBody: ` 后面就为空，无法记录。

这个问题折腾了好半天，原本还以为是请求的内容有问题，反复测试才发现这个原因。

由此可以推断，请求体的 `RequestBody` 的内容记录发生在尝试模型绑定的时候，而不管这个模型是否可以正确绑定。如果没有模型类，不去尝试绑定，则不会去读取记录 `RequestBody` 请求体。

`RequestBody` 请求体无法记录时 NLog 日志文件内容如下：

```ini
2022-11-11 14:37:55.1633|INFO|Executed endpoint 'MyServer.Controllers.MyTestController.Test (MyServer)'
2022-11-11 14:37:55.1633|INFO|RequestBody: 
2022-11-11 14:37:55.1633|INFO|ResponseBody: {"status":500,
    "msg": null
}
```

存在模型类绑定时，`RequestBody` 请求体记录是在 `Route matched` 之后，执行 `Executing JsonResult` 之前，比上面没请求体日志时记录时间要早。正好符合模型绑定的时期。

# 内置 HTTP Logging 的使用

内置的 HTTP Logging 使用非常简单，在 `Startup.cs` 的文件的 `Configure` 应用配置中，尽可能早的调用 `app.UseHttpLogging();` 原则上只位于静态文件处理中间件（`UseStaticFiles`）的后面。

```C#
app.UseHttpLogging();
```

**HTTP Logging日志记录潜在中可能会记录个人身份信息，因此需要考虑该情况，并避免记录敏感信息。**

```C#
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");

app.Run();
```

# HTTP Logging 日志级别的设置

`appsettings[.Development].json` 文件的 `"LogLevel": {` 内指定HTTP日志记录的级别：

```json
{
  "Logging": {
    "LogLevel": {

      "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware": "Information"

    }
  }
}
```

# HTTP Logging 的可选配置项

在 服务配置 中，可以添加对`AddHttpLogging`的可选配置，比如 记录哪些字段、添加额外的请求或返回头、添加要记录的额外的`MediaType`、修改请求体或返回体的日志限制（默认大小为32KB）等

`ConfigureServices(IServiceCollection services)` 服务配置中：

```C#
services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;
    //logging.LoggingFields = HttpLoggingFields.All;
    // | HttpLoggingFields.RequestMethod| HttpLoggingFields.RequestQuery| HttpLoggingFields.RequestPath;
    logging.RequestBodyLogLimit = 4096; // 4096B，默认32KB

    // logging.RequestHeaders.Add("sec-ch-ua");
    // logging.ResponseHeaders.Add("MyResponseHeader");
    // logging.MediaTypeOptions.AddText("application/javascript");
});
```

> 如果 `logging.RequestHeaders` 等对应的请求头字段没有设置，在实际log记录中，该请求头字段的值将会显示为 `[Redacted]` 。
> 
> Redacted —— 已编辑。

# 附：关于 NLog 过滤日志记录，忽略部分日志不记录

实际使用可以发现，即使设置记录的字段为  `RequestBody | ResponseBody` 还是会记录一些多余信息，其内容如下：

```ini
022-11-02 10:53:53.8055|INFO|Request starting HTTP/1.1 POST http://localhost:5000/Test application/json 73
2022-11-02 10:53:53.8055|INFO|Request:

2022-11-02 10:53:53.8055|INFO|Executing endpoint 'MyServer.Controllers.MyTestController.Test (MyServer)'
2022-11-02 10:53:53.8493|INFO|Route matched with {action = "Test", controller = "NXT"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] Test(MyServer.APIModel.InitDBParamModel) on controller MyServer.Controllers.MyTestController (MyServer).
2022-11-02 10:53:53.8923|INFO|RequestBody: {
    "abc":"edf",
    "use":true
 }
2022-11-02 10:53:55.8512|INFO|Executing JsonResult, writing value of type 'MyServer.APIModel.ResultDataModel`1[[System.Boolean, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]'.
2022-11-02 10:53:55.8512|INFO|Response:

2022-11-02 10:53:55.8512|INFO|Executed action MyServer.Controllers.MyTestController.Test (MyServer) in 2009.2568ms
2022-11-02 10:53:55.8512|INFO|Executed endpoint 'MyServer.Controllers.MyTestController.Test (MyServer)'
2022-11-02 10:53:55.8512|INFO|ResponseBody: {"status":200,"message":null}
2022-11-02 10:53:55.8512|INFO|Request finished HTTP/1.1 POST http://localhost:5000/Test application/json 73 - 200 - application/json;+charset=utf-8 2053.7283ms
```

可以看到，我们只是想要记录请求体和返回体，诸如 执行端点、路由匹配、执行`JsonResult`结果返回 等都没必要记录。

同时，`AddHttpLogging` 设置中只记录字段 `RequestBody`、`ResponseBody`，有关请求和返回的Headers没有要记录的内容，因此出现 `Request:` 和 `Response:` 后面回车后的内容为空（本来是Headers相关内容）。

这些都没必要显示出来。因此，设置 `nlog.config`，配置 NLog 过滤掉这些信息。

```xml
  <rules>
    <!--All logs, including from Microsoft-->
    <logger name="*" minlevel="Info" writeTo="allfile" >
      <!-- 必须指定 defaultAction 否则整个都过滤掉，不记录Log -->
      <filters defaultAction='Log'>
        <when condition="starts-with('${message}','Request:') || starts-with('${message}','Executing ') || starts-with('${message}','Route matched ') || starts-with('${message}','Executed ') || starts-with('${message}','Response:')" action="Ignore" />
      </filters>
    </logger>
  </rules>
```

`<when condition` 表示当满足条件时，`action="Ignore"` 表示忽略掉满足条件的日志。

注意：**在 `<filters defaultAction='Log'` 上必须指定 `defaultAction`，否则会将整个 `logger`记录器 的内容都过滤掉不记录Log。**

# 附：关于性能Performance

Bombardier 是一个用Go编写的快速跨平台HTTP基准测试工具。此篇 [ASP.NET Core 6 - HttpLogging - log requests/responses](https://josef.codes/asp-net-core-6-http-logging-log-requests-responses/) 有使用它对 HttpLogging 的简单性能测试。

# 附：关于不记录响应头的问题

搜索相关问题是，看到一个 [HttpLoggingMiddleware does not log all response headers when log response body](https://github.com/dotnet/aspnetcore/issues/36920) 的问题。

具体未详细了解，应该是一些响应头没有记录下来。但该问题的解决或讨论，官方似乎放到 .NET 8 的计划中，有需要可以了解下。

# 参考

- [HTTP Logging in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-6.0)