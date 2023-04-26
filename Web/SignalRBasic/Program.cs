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

            #region ���� Kestrel ������ 
            builder.WebHost.UseKestrel(options =>
            {
                //// ���ó�ʱʱ�䣬Ĭ��130��
                //options.Limits.KeepAliveTimeout=TimeSpan.FromSeconds(200);
                //// ���÷��������Դ����������ͷ�ĳ�ʱʱ�䣬Ĭ��30��
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

            // ���webSocket��̨���� ���ע��MapGet�ȶ˵�ע�᣿
            //builder.Services.AddHostedService<WebSocketBackService>();

            var app = builder.Build();

            #region [�ض��� �� ��д �� �Զ���(�м��)]�Դ��ڵ��ļ������ .html ��չ��
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value ?? "";
                // !url.EndsWith('/') &&  ����Ҫ
                if (url.Trim().Trim('/').Length > 0)
                {
                    // �ض��� .html Ϊ����չ����·����ʵ��α��̬
                    var pseudoStaticExtenName = ".html";
                    if (url.EndsWith(pseudoStaticExtenName))
                    {
                        context.Response.Redirect(url.Substring(0, url.Length - pseudoStaticExtenName.Length));
                        // Ĭ��Ϊ 302 redirect ��ʱ�ض���: 302 ������ʱ��ת��(Temporarily Moved )��
                        // 301 redirect �����ض���: 301 ����������ת��(Permanently Moved)
                        return; // �˴�Ϊ�ض���,�ض��������Ҫִ��֮����м��

                        //var option = new RewriteOptions();
                        //option.AddRedirect()
                    }
                    else
                    {
                        if (url.EndsWith('/'))
                        {
                            url = url.TrimEnd('/');
                        }
                        // �ж� .html ��̬ҳ���Ƿ���ڡ����ڣ���Ϊα��̬��ҳ�棬�����չ�� ��Ҳ������չ��.html֮����������͡�
                        // �˴�����ΪURL��д
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

            #region ��̬�ļ� html ����
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //������ҳ���û���`localhost`���ʵ�����`wwwroot`�µ�Index.html�ļ�
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            //defaultFilesOptions.RedirectToAppendTrailingSlash=true;

            app.UseDefaultFiles(defaultFilesOptions);

            //���þ�̬�ļ�
            app.UseStaticFiles();

            #endregion

            #region WebSocket ���
            //app.UseWebSockets(new WebSocketOptions()
            //{
            //    // Ĭ��2����
            //    KeepAliveInterval=TimeSpan.FromSeconds(120),                
            //    //ReceiveBufferSize=4*1024 // Ĭ��4kb,�ѹ�ʱ
            //});
            app.UseWebSockets();

            // ����WebSocket���м��
            //app.UseWebSocketHandle(); 
            //// �൱��ֱ�� Use
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

            #region �ӿ� �� ·��
            // ͨ������Ҫ���á���ʽ���ã��Ե��� UseRouting �� UseEndpoints ˳��
            app.UseRouting();

            // ���endpoint -- �Զ���ӿ�
            app.MapGet("/", () => "Hello World!");
            // 
            app.Map("/map", async context => {
                await context.Response.WriteAsync("OK");
            });
            // ƥ��ָ������
            //app.MapMethods();
            //app.MapPost
            //app.MapPut
            //app.MapDelete

            //app.UseEndpoints(endpoints => {
            //    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            // Ĭ��·�� / ������·������
            //app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            #region // ����·��
            //app.MapGet("people_0/{message:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}",
            //                () => "Inline Regex Constraint Matched");
            //app.MapControllerRoute(
            //        name: "people",
            //        pattern: "people/{ssn}",
            //        constraints: new { ssn = "^\\d{3}-\\d{2}-\\d{4}$", },
            //        defaults: new { controller = "People", action = "List" }
            //); 
            #endregion

            #region // ����ת��
            // ����
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
            // ����
            // builder.Services.AddRouting(options =>
            //          options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer));
            // ƥ��·��
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller:slugify=Home}/{action:slugify=Index}/{id?}"
            //); 
            #endregion


            // ��������endpoint
            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}