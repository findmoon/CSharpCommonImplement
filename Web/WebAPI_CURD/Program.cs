using System.Net;

namespace WebAPI_CURD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region 自定义的服务注入处理
            // 初始化添加/注入当前的 Services
            builder.Services.InitServices(); 
            #endregion



            var app = builder.Build();

            #region 自定义配置和一些初始化
            // 启动时需要执行的初始化操作
            //await app.InitAppExecAsync();
            app.InitAppExecAsync().Wait();

            // 添加配置中间件
            app.Configure(app.Environment); 
            #endregion

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

      

            app.Run();
        }
    }
}