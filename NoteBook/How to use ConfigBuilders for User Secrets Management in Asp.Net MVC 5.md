**How to use ConfigBuilders for User Secrets Management in Asp.Net MVC 5**

[toc]

> 绝对的好文，关于 Asp.Net MVC 5 中用户机密管理的介绍，Asp.Net Core提供了开箱即用的用户机密管理机制，但是却很难找到有关Asp.Net程序对于机密管理的介绍。实际上，对于WPF、Winform等程序的用户机密存储，也应该受到同样的重视。
> 
> 本文介绍使用 `Microsoft.Configuration.ConfigurationBuilders.UserSecrets` 包来管理开发阶段用户机密，并且可以做到和Asp.Net Core的默认行为一样，在存储库之外，毫不费力地配置、使用用户机密，就和直接使用Web.config、App.config中的机密数据一样。
> 
> `Microsoft.Configuration.ConfigurationBuilders.UserSecrets` 基于 `Microsoft.Configuration.ConfigurationBuilders.Base`，而它要求 .NET Framework 4.7.1 

> 原文 [How to use ConfigBuilders for User Secrets Management in Asp.Net MVC 5](https://hamidmosalla.com/2022/12/30/how-to-use-configbuilders-for-user-secrets-management-in-asp-net-mvc-5/)


用户机密存储的默认路径为：`%AppData%\Microsoft\UserSecrets\xxxxxx`，`xxxxxx`为一个随机的文件夹名称，也是`userSecretsId`。

比如，我们将网站项目使用的.NET版本改为Framework 4.7.12后，直接在项目的右键菜单中，就可以看到“管理用户机密”的选项。

![](img/20230217145111.png)  


Web.config或App.config中使用

```xml
  <appSettings configBuilders="Secrets">
    <add key="password" value="This is a placeholder info" />
  </appSettings>
```

`Microsoft.Configuration.ConfigurationBuilders.UserSecrets`安装时生成的密码文件`secrets.xml`：

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <secrets ver="1.0" >
    <secret name="SendGridApiKey" value="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" />
  </secrets>
</root>
```

获取：

```C#
var password = WebConfigurationManager.AppSettings["password"];
```

[How to safely manage passwords in .Net applications](https://medium.com/@hammadarif/how-to-safely-manage-passwords-in-net-applications-a546a481c41c)

[Application Secret Management: How to Implement a Good Secret Management Strategy](https://hamidmosalla.com/2022/12/29/application-secret-management-how-to-implement-a-good-secret-management-strategy/)


[best practices for storing API access keys in asp.NET MVC](https://stackoverflow.com/questions/61411810/best-practices-for-storing-api-access-keys-in-asp-net-mvc)

[Where do I add dotnet user-secrets](https://stackoverflow.com/questions/73390937/where-do-i-add-dotnet-user-secrets)

[How to securely store passwords in Visual Studio 2019 with Manage User Secrets](https://www.ryadel.com/en/visual-studio-2019-securely-store-db-passwords-manage-user-secrets-asp-net-core/)

[Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)


[.NET中如何安全地存储认证信息（C#）](https://blog.csdn.net/Toshiya14/article/details/53965550)



[Store application secrets safely during development](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/secure-net-microservices-web-applications/developer-app-secrets-storage)