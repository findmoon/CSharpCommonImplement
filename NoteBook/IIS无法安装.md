

网上找到的完全卸载IIS的方法也不靠谱，比如 `%windir%\System32\inetsrv` 即使安全模式下、修改TrustedInstaller权限也无法删除。最最重要的是，**正是由于删除了该文件夹，尤其是该文件夹下config的一些内容（无法全部删除），才导致了IIS无法重新安装。解决办法就是恢复删除的文件，即可重新安装IIS成功。**

从 启用和关闭 Windows 功能 中，勾选安装IIS，无法重新安装，报错如下：

![](img/20230209003901.png)  

目前，似乎除了重装系统，没有解决办法。

Powershell中启用：

```shell
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole, IIS-WebServer, IIS-CommonHttpFeatures, IIS-ManagementConsole, IIS-HttpErrors, IIS-HttpRedirect, IIS-WindowsAuthentication, IIS-StaticContent, IIS-DefaultDocument, IIS-HttpCompressionStatic, IIS-DirectoryBrowsing
```

![](img/20230209005612.png)  

Powershell中启用成功：

![](img/20230209084046.png)  

# 可能 参考

IIS will not install on Windows 10：https://answers.microsoft.com/en-us/windows/forum/all/iis-will-not-install-on-windows-10/251699ca-627a-49e8-800e-a8d038f4b6c3


