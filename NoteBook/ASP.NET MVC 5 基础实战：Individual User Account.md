**ASP.NET MVC 5 基础实战：使用 Individual User Accounts 创建具有登陆注册、[SendGrid发送]邮箱验证、密码重置、字符图形验证码功能的网站**

[toc]

> 主要翻译参考自官网的 [Create a secure ASP.NET MVC 5 web app with log in, email confirmation and password reset (C#)](https://learn.microsoft.com/en-us/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset)

通过在创建ASP.NET MVC网站项目时指定 `Individual User Accounts` Authentication，即使用 **ASP.NET Identity membership system** 实现具有最基础的资源保护（即授权认证）功能的网站。

因此，需要实现的功能有：

- 用户的登陆
- 新用户注册
- 邮箱验证
- 密码重置
- 登录注册时输入图形验证码中的字符串或数学运算结果。

# 创建 个人账户 认证的`ASP.NET MVC`项目

创建`ASP.NET MVC`项目，名称为`ASPNETMVC_UserAutho`，authentication 选择 `Individual User Accounts`，使用Https：

![](img/20230216163708.png)  

之前的版本：

![](img/20230216163844.png)  

点击“创建”。

使用个人账户认证创建的web项目，默认已经给出了最基础的相对比较完备的用户认证和授权功能。

可以查看默认生成的`AccountController`控制器中的各种处理逻辑。

![](img/20230217101421.png)  

通过`[Authorize]`过滤器特性实现用户身份的认证，可以用于控制器或Action方法上，也可以注册为全局过滤器，为整个网站添加认证保护。

`[AllowAnonymous]`过滤器特性允许匿名访问（不需要登陆），比如 Login、Register等页面，需要任何人都可以访问。

运行网站，可以看到登陆或注册按钮：

![](img/20230217101945.png)  

点击注册，完成第一个用户的注册。并登陆。

![](img/20230217111124.png)  

# Individual User Accounts 创建的数据库

在 Visual Studio 的服务器资源管理器(`Server Explorer`)中，导航到`Data Connections\DefaultConnection\Table`可以看到创建的表。

默认站点没有任何用户数据。当第一次注册时，会在连接字符串中指定的`|DataDirectory|`（项目的`App_Data`）路径下创建数据库、表和第一个用户数据。

> 只有注册用户后，`App_Data/`路径下才会创建默认的Identity用到的用户数据库文件。默认附加到`MSSQLLocalDB`中。

![](img/20230217111446.png)  

选择`AspNetUsers`，可以查看表定义和刚才注册的邮箱用户。【右键-`Open table definition`；右键-`Show Table Data`】

![](img/20230217113314.png)  

![](img/20230217113149.png)  

可以看到刚注册用户的邮箱验证为`False`。

> 表定义中的 UserName 为 `nvarchar(256)`，这个256感觉有些没必要这么大。

选择注册的用户这一行，并删除。后续会重新创建一个需要邮箱验证的用户。

# 使用 SendGrid 发送邮件

此处使用 SendGrid邮件提供程序 实现邮件的发送。

> 此外，还可以使用SMTP，或其它机制发送邮件。

>  email service相对SMTP更安全和不易出错，且能提供更多的管理、邮件分析等。

## 注册 SendGrid 并添加 Sender

参考[官网](https://docs.sendgrid.com/ui/sending-email/senders)注册SendGrid，并添加个一个Sender（注册或创建Sender需要邮箱验证）

![](img/20230217135151.png)  

注册后默认提示创建 Identity 就会添加一个 Sender：

然后在集成指导中按照要求创建API Key：

![](img/20230217150710.png)  

复制创建的 API key 用于在`ASP.NET MVC`中使用。（推荐使用机密用户管理的方式存储）

## 配置使用SendGrid发送邮件

在Nuget包管理器控制台中，安装SendGrid：

```sh
Install-Package SendGrid
```

在`App_Start/IdentityConfig.cs`下找到`EmailService`，添加类似如下的代码：

```C#
public class EmailService : IIdentityMessageService
{
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
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("MyEmail@abc.com", "MyName"),
            Subject = message.Subject,
            PlainTextContent = message.Body,
            HtmlContent = message.Body
        };
        msg.AddTo(new EmailAddress(message.Destination));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);

        // 记录返回信息？
    }
}
```

`Web.config`文件中`SendGridApiKey`配置项如下（使用了机密数据存储）：

```xml
  <appSettings configBuilders="Secrets">

    <add key="SendGridApiKey" value="SendGrid ApiKey in here" />
  </appSettings>
```

## Account 控制器中启用并完善邮箱验证的功能

`Register` Action 方法中已经给出了邮箱验证的代码，将此部分取消注释即可。

修改`Register`方法，在登陆之前必须进行邮箱验证。默认的注册方法，会使用户直接登陆，但大多数情况下，我们希望验证邮箱后才能登陆。

注释掉`SignInAsync`方法阻止登陆，然后添加需要验证邮箱的提示信息，返回登陆页面。

```C#
//
// POST: /Account/Register
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl = null)
{
    ViewBag.ReturnUrl = returnUrl;

    if (ModelState.IsValid)
    {
        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(user.Id, "用户注册确认", "请通过单击<a href=\"" + callbackUrl + "\">此处</a>来确认你的帐户");

            ViewBag.ConfirmEmailMsg = $"检查你的邮箱 {model.Email}  并确认账户, 必须在邮箱确认后才能登陆！";

            // 提示信息：验证邮箱后登陆
            return View("Login", new LoginViewModel());
        }
        AddErrors(result);
    }

    return View(model);
}
```

> `Views\Account\ConfirmEmail.cshtml`视图为邮箱确认成功后的界面。

更新 HttpPost Login Action 方法，实现没验证邮箱不允许登陆。

```C#
//
// POST: /Account/Login
[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    // Require the user to have a confirmed email before they can log on.
    var user = await UserManager.FindByNameAsync(model.EmailOrUserName) ?? await UserManager.FindByEmailAsync(model.EmailOrUserName);
    if (user != null)
    {
        if (!await UserManager.IsEmailConfirmedAsync(user.Id))
        {
            ViewBag.LoginErrorMsg = "必须确认邮箱后才能登陆";
            return View(model);
        }
    }

    // 这不会计入到为执行帐户锁定而统计的登录失败次数中
    // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
    var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
    switch (result)
    {
        case SignInStatus.Success:
            return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
            return View("Lockout");
        case SignInStatus.RequiresVerification:
            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        case SignInStatus.Failure:
        default:
            ModelState.AddModelError("", "无效的登录尝试。");
            return View(model);
    }
}
```

## 测试邮箱验证登陆功能

在 Home 控制器的 Contact 方法上添加 `[Authorize]` 特性。测试访问“联系方式”页面，会返回到登陆页面。

![](img/20230217172311.png)  

在未验证邮箱前，尝试登陆，提示“必须确认邮箱”：

![](img/20230217173017.png)  

