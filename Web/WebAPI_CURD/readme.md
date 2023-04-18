WebAPI_CURD ������ CURD ���ܵ� ASP .Net Core WebAPI[.NET 6] ��Ŀ��ʹ�� Dapper + SQLite���Լ� AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection��BCrypt.Net-Next

�Լ�ʹ���� WebAPI ��ĿĬ�ϵ� app.UseSwagger(); app.UseSwaggerUI(); ��Swashbuckle��  ��ͨ�� /Swagger ���ʡ�

> �����ο��� [.NET 7.0 + Dapper + SQLite - CRUD API Tutorial in ASP.NET Core](https://jasonwatmore.com/net-7-dapper-sqlite-crud-api-tutorial-in-aspnet-core)

�ļ��ṹ���ܣ�

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


Program.cs ��һЩ�Զ��崦���ʵ�ֺͽ��ܣ�

```C#
 #region ���á�����Զ��������ļ���JsonFile��
 // ����Զ��������ļ�
 //builder.Configuration.AddJsonFile("custom_settings.json", optional: false, reloadOnChange: true);
 #endregion

 #region ���� Kestrel ������
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
#region �Զ���ķ���ע�봦��
// ��ʼ�����/ע�뵱ǰ�� Services
builder.Services.InitServices(); 
#endregion
```

```C#
#region �Զ������ú�һЩ��ʼ��
// ����ʱ��Ҫִ�еĳ�ʼ������
//await app.InitAppExecAsync();
app.InitAppExecAsync().Wait();
// ��������м��
app.Configure(app.Environment); 
#endregion
```
