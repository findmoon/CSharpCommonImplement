How to Write a Custom Logging Provider in ASP.NET Core

> [How to Write a Custom Logging Provider in ASP.NET Core](https://www.codeproject.com/Articles/1556475/How-to-write-a-custom-logging-provider-in-Asp-Net)

[How to Use LoggerFactory and Microsoft.Extensions.Logging for .NET Core Logging With C#](https://stackify.com/net-core-loggerfactory-use-correctly/)

[The difference between GetService() and GetRequiredService() in ASP.NET Core](https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core/)

[How To Start Logging With NLog](https://betterstack.com/community/guides/logging/how-to-start-logging-with-nlog/)

GetService() 返回null

GetRequiredService() 抛出异常

# 如何从 serviceProvider 获取注入的 Nlog（目前似乎不知道如何获取）

如下 获取的是 `Microsoft.Extensions.Logging.ILogger`：

```C#
var logger = serviceProvider.GetService<ILogger<AnyClass>>();
```

或者，通过 LogManager 获取

```C#
var logger = LogManager.GetCurrentClassLogger();

// 或
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
```

# 参考

[Unable to resolve ILogger from Microsoft.Extensions.Logging](https://stackoverflow.com/questions/52921966/unable-to-resolve-ilogger-from-microsoft-extensions-logging)

[How to inject NLog using Autofac](https://stackoverflow.com/questions/75958881/how-to-inject-nlog-using-autofac)
