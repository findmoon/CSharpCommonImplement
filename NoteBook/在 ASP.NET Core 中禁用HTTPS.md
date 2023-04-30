在 ASP.NET Core 调试时禁用或关闭HTTPS，不强制跳转https，默认打开http


好文[5 ways to set the URLs for an ASP.NET Core app](https://andrewlock.net/5-ways-to-set-the-urls-for-an-aspnetcore-app/)

- 不强制或不自动跳转https - 注释掉 https 的重定向：

```C#
app.UseHttpsRedirection();
```


- 默认打开http - `Properties/launchSettings.json` 中修改`applicationUrl`

调试时默认打开的页面url为 `Properties/launchSettings.json` 文件中 `applicationUrl` 指定的 url地址。通过修改它，可以设置默认打开的url地址。 

```json
...
"applicationUrl": "https://localhost:5001;http://localhost:5000",
...
```

比如吧 https 删除，只留 http。

> 通过 Visual Studio Code 或者 dotnet new 命令创建的项目，可能不会存在launchSettings.json文件，并且在发布项目的时候也不会将launchSettings.json文件包含进去


- cli中运行时指定`--urls`参数

`dotnet run --urls=http://0.0.0.0:5000,https://0.0.0.0:5001` 指定运行时启动的url。

- `UseUrls()` 方法中指定


- 修改Kestrel配置禁用https

