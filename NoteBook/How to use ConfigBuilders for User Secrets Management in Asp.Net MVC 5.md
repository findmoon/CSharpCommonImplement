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


# net471版本之前的机密/敏感数据、密码的保护实现

具体参见 [Best practices for deploying passwords and other sensitive data to ASP.NET and Azure App Service](https://learn.microsoft.com/en-us/aspnet/identity/overview/features-api/best-practices-for-deploying-passwords-and-other-sensitive-data-to-aspnet-and-azure) 的介绍。

通过`file`指定额外的不包含代码库中的密码配置文件【file中的配置将和此处的配置合并】：

```xml
   <appSettings file="..\..\AppSettingsSecrets.config">      
      <add key="webpages:Version" value="3.0.0.0" />
      <add key="webpages:Enabled" value="false" />
      <add key="ClientValidationEnabled" value="true" />
      <add key="UnobtrusiveJavaScriptEnabled" value="true" />      
   </appSettings>
```

使用 `configSource` 属性替换整个`<connectionStrings>`标签【`configSource`会将当前整个`<connectionStrings>`替换掉，而不是合并】

```xml
<connectionStrings configSource="ConnectionStrings.config">
</connectionStrings>
```

控制台应用程序的`appSettings`的`file`属性不支持相对路径，因此可指定绝对路径。

```xml
<configuration>
  <appSettings file="C:\secrets\AppSettingsSecrets.config">
    <add key="TwitterMaxThreads" value="24" />
    <add key="StackOverflowMaxThreads" value="24" />
    <add key="MaxDaysForPurge" value="30" />
  </appSettings>
</configuration>
```