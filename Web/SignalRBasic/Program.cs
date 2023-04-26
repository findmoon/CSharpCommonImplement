using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using SignalRBasic.HostedServices;
using SignalRBasic.Middlewares;
using System.Net;
using System.Text.RegularExpressions;

namespace SignalRBasic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region 配置 Kestrel 服务器 
            builder.WebHost.UseKestrel(options =>
            {
                //// 设置超时时间，默认130秒
                //options.Limits.KeepAliveTimeout=TimeSpan.FromSeconds(200);
                //// 设置服务器可以处理接收请求头的超时时间，默认30秒
                ////options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(50);

                //var configuration = (IConfiguration)options.ApplicationServices.GetService(typeof(IConfiguration))!;

                //var httpPort = configuration.GetValue("WebHost:HttpPort", 5061);

                //options.Listen(IPAddress.Any, httpPort);

                //// https
                //options.Listen(IPAddress.Any, 5001,
                // listenOptions =>
                // {
                //     listenOptions.UseHttps("certificate.pfx", "topsecret");
                // });
            });
            #endregion

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 添加webSocket后台服务 如何注入MapGet等端点注册？
            //builder.Services.AddHostedService<WebSocketBackService>();

            var app = builder.Build();

            #region [重定向 和 重写 的 自定义(中间件)]对存在的文件，添加 .html 扩展名
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value ?? "";
                // !url.EndsWith('/') &&  不需要
                if (url.Trim().Trim('/').Length > 0)
                {
                    // 重定向 .html 为无扩展名的路径，实现伪静态
                    var pseudoStaticExtenName = ".html";
                    if (url.EndsWith(pseudoStaticExtenName))
                    {
                        context.Response.Redirect(url.Substring(0, url.Length - pseudoStaticExtenName.Length));
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

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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

            #region WebSocket 相关
            //app.UseWebSockets(new WebSocketOptions()
            //{
            //    // 默认2分钟
            //    KeepAliveInterval=TimeSpan.FromSeconds(120),                
            //    //ReceiveBufferSize=4*1024 // 默认4kb,已过时
            //});
            app.UseWebSockets();

            // 处理WebSocket的中间件
            //app.UseWebSocketHandle(); 
            //// 相当于直接 Use
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/ws")
            //    {
            //        if (context.WebSockets.IsWebSocketRequest)
            //        {
            //            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //            await Echo(webSocket);
            //        }
            //        else
            //        {
            //            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //        }
            //    }
            //    else
            //    {
            //        await next(context);
            //    }
            //});
            #endregion

            app.UseAuthorization();

            #region 接口 和 路由
            // 通常不需要调用。显式调用，以调整 UseRouting 和 UseEndpoints 顺序
            app.UseRouting();

            // 添加endpoint -- 自定义接口
            app.MapGet("/", () => "Hello World!");
            // 
            app.Map("/map", async context => {
                await context.Response.WriteAsync("OK");
            });
            // 匹配指定方法
            //app.MapMethods();
            //app.MapPost
            //app.MapPut
            //app.MapDelete

            //app.UseEndpoints(endpoints => {
            //    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            // 默认路由 / 控制器路由配置
            //app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            #region // 正则路由
            //app.MapGet("people_0/{message:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}",
            //                () => "Inline Regex Constraint Matched");
            //app.MapControllerRoute(
            //        name: "people",
            //        pattern: "people/{ssn}",
            //        constraints: new { ssn = "^\\d{3}-\\d{2}-\\d{4}$", },
            //        defaults: new { controller = "People", action = "List" }
            //); 
            #endregion

            #region // 参数转换
            // 定义
            //public class SlugifyParameterTransformer : IOutboundParameterTransformer
            //{
            //    public string? TransformOutbound(object? value)
            //    {
            //        if (value is null)
            //        {
            //            return null;
            //        }

            //        return Regex.Replace(
            //            value.ToString()!,
            //                "([a-z])([A-Z])",
            //            "$1-$2",
            //            RegexOptions.CultureInvariant,
            //            TimeSpan.FromMilliseconds(100))
            //            .ToLowerInvariant();
            //    }
            //}
            // 配置
            // builder.Services.AddRouting(options =>
            //          options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer));
            // 匹配路由
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}"
            //); 
            #endregion


            // 控制器的endpoint
            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}