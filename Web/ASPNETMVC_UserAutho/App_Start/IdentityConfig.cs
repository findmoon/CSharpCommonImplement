using ASPNETMVC_UserAutho.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace ASPNETMVC_UserAutho
{
    public class EmailService : IIdentityMessageService
    {
        // 没有 async 仅 Task 关键字，可以return。
        //public Task TestSendAsync(IdentityMessage message)
        //{
        //    // 在此处插入电子邮件服务可发送电子邮件。
        //    return Task.FromResult(0);
        //}
        public async Task SendAsync(IdentityMessage message)
        {
            // 在此处插入电子邮件服务可发送电子邮件。
            await ConfigSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task ConfigSendGridasync(IdentityMessage message)
        {
            var apiKey= WebConfigurationManager.AppSettings["SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var msg = MailHelper.CreateSingleEmail(from: new EmailAddress("MyEmail@abc.com", "MyName"),
                to: new EmailAddress(message.Destination),
                subject: message.Subject,
                plainTextContent: message.Body,
                htmlContent: message.Body);
            #region SendGridMessage
            // new SendGridMessage()
            //{
            //    From = new EmailAddress("MyEmail@abc.com", "MyName"),
            //    Subject = message.Subject,
            //    PlainTextContent = message.Body,
            //    HtmlContent = message.Body
            //};
            //msg.AddTo(new EmailAddress(message.Destination)); 
            #endregion

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            var result = response.Body.ToString();
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                //using (var txtSM=new )
                //{

                //}
                //response.Body.CopyToAsync()
                throw new Exception($"邮件发送失败{response.StatusCode}");
            }
            // 记录返回信息？
        }
    }
    

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // 在此处插入 SMS 服务可发送短信。
            return Task.FromResult(0);
        }
    }

    // 配置此应用程序中使用的应用程序用户管理器。UserManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // 配置要在此应用程序中使用的应用程序登录管理器。
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
