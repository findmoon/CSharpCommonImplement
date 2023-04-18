using System.Text.Json.Serialization;
using WebAPI_CURD.Helpers;
using WebAPI_CURD.Repositories;
using WebAPI_CURD.Services;
using Microsoft.AspNetCore.Builder;

namespace WebAPI_CURD
{
    /// <summary>
    /// 初始化配置WebApplication，主要为添加 Services 和 Middleware
    /// </summary>
    public static class InitCfgWebApp
    {
        /// <summary>
        /// 初始化添加 当前Web的所有服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection InitServices(this IServiceCollection services)
        {
            // var env = builder.Environment;

            services.AddSingleton<DataContext>();
            services.AddCors();
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            // package AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure DI for application services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
        /// <summary>
        /// 执行初始化的一些必须操作
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task<WebApplication> InitAppExecAsync(this WebApplication app)
        {
            // ensure database and tables exist
            // 创建数据库和表，仅启动时执行一次

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            await context.Init();


            return app;
        }

        /// <summary>
        /// 配置 ApplicationBuilder 中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder Configure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())

            app.UseCors(x => x.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();


        
            return app;
        }

    }
}
