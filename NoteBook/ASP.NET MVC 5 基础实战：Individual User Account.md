**ASP.NET MVC 5 基础实战：使用 Individual User Accounts 创建具有登陆注册、邮箱验证、密码重置、字符图形验证码功能的网站**

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

点击“创建”

运行网站，可以看到需要登陆或注册：


