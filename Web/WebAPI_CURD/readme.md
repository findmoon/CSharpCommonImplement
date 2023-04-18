WebAPI_CURD 基本的 CURD 功能的 ASP .Net Core WebAPI[.NET 6] 项目，使用 Dapper + SQLite，以及 AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection、BCrypt.Net-Next

以及使用了 WebAPI 项目默认的 app.UseSwagger(); app.UseSwaggerUI(); 【Swashbuckle】  【通过 /Swagger 访问】

> 基本参考自 [.NET 7.0 + Dapper + SQLite - CRUD API Tutorial in ASP.NET Core](https://jasonwatmore.com/net-7-dapper-sqlite-crud-api-tutorial-in-aspnet-core)

文件结构介绍：

- Controllers

Define the end points / routes for the web api, controllers are the entry point into the web api from client applications via http requests.

- Models

Represent request and response models for controller methods, request models define parameters for incoming requests and response models define custom data returned in responses when required. The example only contains request models because it doesn't contain any routes that require custom response models, entities are returned directly by the user GET routes.

- Services

Contain business logic and validation code, services are the interface between controllers and repositories for performing actions or retrieving data.

- Repositories

Contain database access code and SQL queries.

- Entities

Represent the application data that is stored in the database.
Dapper maps relational data from the database to instances of C# entity objects to be used within the application for data management and CRUD operations.

- Helpers

Anything that doesn't fit into the above folders.


Program.cs 中一些自定义处理的实现和介绍：

```C#
 #region 配置、添加自定义配置文件（JsonFile）
 // 添加自定义配置文件
 //builder.Configuration.AddJsonFile("custom_settings.json", optional: false, reloadOnChange: true);
 #endregion

 #region 配置 Kestrel 服务器
 builder.WebHost.UseKestrel(options =>
     {
         var configuration = (IConfiguration)options.ApplicationServices.GetService(typeof(IConfiguration))!;

         var httpPort = configuration.GetValue("WebHost:HttpPort", 5061);

         options.Listen(IPAddress.Any, httpPort);

         //// https
         //options.Listen(IPAddress.Any, 5001,
         // listenOptions =>
         // {
         //     listenOptions.UseHttps("certificate.pfx", "topsecret");
         // });
     }); 
 #endregion
```

```C#
#region 自定义的服务注入处理
// 初始化添加/注入当前的 Services
builder.Services.InitServices(); 
#endregion
```

```C#
#region 自定义配置和一些初始化
// 启动时需要执行的初始化操作
//await app.InitAppExecAsync();
app.InitAppExecAsync().Wait();
// 添加配置中间件
app.Configure(app.Environment); 
#endregion
```
