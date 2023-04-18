using System.Net;

namespace WebAPI_CURD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region �Զ���ķ���ע�봦��
            // ��ʼ�����/ע�뵱ǰ�� Services
            builder.Services.InitServices(); 
            #endregion



            var app = builder.Build();

            #region �Զ������ú�һЩ��ʼ��
            // ����ʱ��Ҫִ�еĳ�ʼ������
            //await app.InitAppExecAsync();
            app.InitAppExecAsync().Wait();

            // ��������м��
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