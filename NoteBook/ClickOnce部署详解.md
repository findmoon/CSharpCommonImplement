**ClickOnce部署详解**

[toc]

# 什么是ClickOnce

微软官方文档介绍如下：

**ClickOnce 是一种部署技术，使用该技术可创建自行更新的基于 Windows 的应用程序，这些应用程序可以通过最低程度的用户交互来安装和运行。**

ClickOnce 技术可以发布 `Windows Presentation Foundation` (.xbap) 、Windows 窗体 (.exe) 、控制台应用程序 (.exe) 或 Office 解决方案 (.dll) 等类型的程序集。 既可以在本地(脱机)运行，也可以需要联机模式下运行（这样不会在电脑上永久安装任何内容）。

ClickOnce 应用程序可以自行更新，程序可以检查是否有较新的版本，如果有会自动替换任何的更新文件。开发人员可以指定更新行为；网络管理员也可以控制更新策略，如将更新标记为强制性更新。此外，还可将更新回滚到早期版本。

# ClickOnce 部署的优势

ClickOnce 部署克服了部署中最常见的三个主要问题：

- **更新应用程序的困难**

使用 `Microsoft Windows Installer` 部署的应用程序，每次更新时，都必须重新安装整个程序；而 ClickOnce 部署，不仅可以自动提供更新，而且可以做到只有更改过的应用程序部分才会被下载，然后从新的并行文件夹重新安装完整的、更新后的应用程序。

- **对用户的计算机的影响**

使用 Windows Installer 部署时，应用程序通常依赖于共享组件，这就会有潜在的版本冲突问题；而使用 ClickOnce 部署时，每个应用程序都是独立的，不会干扰其他应用程序。

- **安全权限**

Windows Installer 部署要求管理员权限，并且只允许受限制的用户安装；而 ClickOnce 部署允许非管理用户安装应用程序，并可以仅授予应用程序所需要的那些代码访问安全权限。

# ClickOnce 概要介绍

这部分对ClickOnce进行整体性的介绍，对于后面 ClickOnce 的使用、创建很有帮助，也需要相互来对照，才能理解本部分介绍的内容。

## ClickOnce的安全

ClickOnce 安全的核心基于证书、代码访问安全策略 和 ClickOnce 信任提示。

### Certificates 证书

认证码证书(`Authenticode certificates`) 用于验证应用程序发布者的真实性。

通过 **使用 认证码 进行应用程序部署**，ClickOnce 可以防止恶意程序伪装成已经证实的、值得信赖的来源的合法程序。此外，还可使用证书对应用程序和部署清单进行签名，以证明文件未被篡改。

证书还可用于配置客户端计算机，使其拥有受信任的发布者的列表。如果某个应用程序来自受信任的发布者，则可在没有任何用户交互的情况下安装它。

### Code access security 代码访问安全性

代码访问安全性可以限制代码对受保护资源的访问权限，可以选择 Internet 或本地 Intranet 区域来限制权限。

`ProjectDesigner` 属性中的“安全性”页面可以设置适合的应用程序安全性区域。

### ClickOnce trust prompt 信任提示

当应用程序请求超过区域允许的权限时，系统会提示用户，并交由用户做是否允许的信任决定。

## ClickOnce的部署

ClickOnce 部署架构的核心基于两个 XML 清单文件(`manifest files`)：应用程序清单和部署清单。这两个文件用于描述 ClickOnce应用 从哪安装、如何更新，以及何时更新。

### 发布ClickOnce应用程序

应用程序清单描述了应用自身，包括程序集、依赖项和组成应用的文件、请求的权限 和 可用更新的位置。应用程序开发人员可以在 Visual Studio 的发布向导（适用于 .NET Core 和 .NET 5+ 的发布工具）或 Windows 软件开发工具包 (SDK) 中的清单生成和编辑工具 (Mage.exe) 来制作应用程序清单。

部署清单描述了应用程序如何部署，包括应用清单的位置、客户端应该运行的应用的版本。

> .NET Core 3.1 和 .NET 5+ 应用部署 ClickOnce 使用的是 dotnetmage.exe。
> 
> .NET Framework 部署 ClickOnce 使用的则是 Mage.exe。











由于 ClickOnce 应用程序是隔离的，因此安装或运行 ClickOnce 应用程序不会中断现有应用程序。 ClickOnce 应用程序是自包含的；每个 ClickOnce 应用程序都安装到每个用户、每个应用程序缓存中，并安全的运行。ClickOnce 应用程序也可以从 Internet 或 Intranet 安全区域中运行。