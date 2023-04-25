**[IIS]如何检查IIS、IIS某一模块是否安装或启用**

[toc]

# PowerShell 中检查 IIS 或 IIS模块 是否安装或启用

- `Get-WindowsOptionalFeature` 获取Windows功能，通过 State 确认是否安装。
- `Get-WindowsFeature` 获取Windows功能，通过 InstallState 确认是否安装。【需要 Windows Server 版本才可用】



```sh
(Get-WindowsFeature Web-Server).InstallState -eq "Installed"
```

比如查看 WebSockts 协议 模块是否安装启用：

```sh
PS C:\Windows\System32> (Get-WindowsOptionalFeature -Online -FeatureName IIS-WebSockets).State  -eq "Enabled"
True
PS C:\Windows\System32> Get-WindowsOptionalFeature -Online -FeatureName IIS-WebSockets

FeatureName      : IIS-WebSockets
DisplayName      : WebSocket 协议
Description      : IIS 10.0 and ASP.NET 4.8 support writing server applications that communicate over the WebSocket Pro
                   tocol.
RestartRequired  : Possible
State            : Enabled
CustomProperties :

```

检查 IIS 是否安装 `IIS-WebServer`、`IIS-WebServerRole`

```sh
PS C:\Windows\System32> Get-WindowsOptionalFeature -Online -FeatureName IIS-WebServer

FeatureName      : IIS-WebServer
DisplayName      : 万维网服务
Description      : 安装 IIS 10.0 万维网服务。提供对 HTML 网站的支持，以及对 ASP.NET、经典 ASP 和 Web 服务器扩展的可选支
                   持。
RestartRequired  : Possible
State            : Enabled
CustomProperties :


PS C:\Windows\System32> Get-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole

FeatureName      : IIS-WebServerRole
DisplayName      : Internet Information Services
Description      : Internet Information Services 提供对 Web 和 FTP 服务器的支持，以及对 ASP.NET 网站、动态内容(如经典 A
                   SP 和 CGI)和本地及远程管理的支持。
RestartRequired  : Possible
State            : Enabled
CustomProperties :
```

# Dism 检查 IIS 或 IIS模块 是否安装或启用

- `Dism /online /Get-Features`
- `Dism /online /Get-FeatureInfo /FeatureName:IIS-WebServerRole`
- `Dism /online /Get-FeatureInfo /FeatureName:IIS-WebServer`

```sh
PS C:\Windows\System32> Dism /online /Get-FeatureInfo /FeatureName:IIS-WebServerRole

部署映像服务和管理工具
版本: 10.0.19041.844

映像版本: 10.0.19045.2846

功能信息:

功能名称 : IIS-WebServerRole
显示名称 : Internet Information Services
描述 : Internet Information Services 提供对 Web 和 FTP 服务器的支持，以及对 ASP.NET 网站、动态内容(如经典 ASP 和 CGI)和 本地及远程管理的支持。
需要重新启动 : Possible
状态 : 已启用

自定义属性:

(没有找到任何自定义属性)

操作成功完成。
PS C:\Windows\System32> Dism /online /Get-FeatureInfo /FeatureName:IIS-WebServer

部署映像服务和管理工具
版本: 10.0.19041.844

映像版本: 10.0.19045.2846

功能信息:

功能名称 : IIS-WebServer
显示名称 : 万维网服务
描述 : 安装 IIS 10.0 万维网服务。提供对 HTML 网站的支持，以及对 ASP.NET、经典 ASP 和 Web 服务器扩展的可选支持。
需要重新启动 : Possible
状态 : 已启用

自定义属性:

(没有找到任何自定义属性)

操作成功完成。
```

# 检查注册表中

```sh
HKLM\Software\Microsoft\Inetstp -> Folder must exist
HKLM\Software\Microsoft\Inetstp\VersionString -> Value must be valid
```

# 参考

> Get-WindowsFeature
>
> Gets information about Windows Server roles, role services, and features that are available for installation and installed on a specified server.

> `Get-WindowsFeature | Where-Object Installed` 仅获取服务器上已安装的角色和功能（`roles and features`）

- [Get-WindowsOptionalFeature](https://learn.microsoft.com/en-us/powershell/module/dism/get-windowsoptionalfeature?view=windowsserver2022-ps)

- [Get-WindowsFeature](https://learn.microsoft.com/en-us/powershell/module/servermanager/get-windowsfeature?view=windowsserver2022-ps)

- [How PowerShell can find features and roles on Windows servers](https://www.networkworld.com/article/3639038/how-powershell-can-find-features-and-roles-on-windows-servers.html)