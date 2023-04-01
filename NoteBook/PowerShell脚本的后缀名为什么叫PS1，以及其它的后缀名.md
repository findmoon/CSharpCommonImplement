**PowerShell脚本的后缀名为什么叫PS1，以及其它的后缀名、运行ps1文件的不同方式**

[toc]

# 后缀为什么为ps1?

PowerShell 的脚本文件后缀为 ps1，应该最重要的原因是 PS 表示 Photoshop 已经深入人心。为避免误解，通常不应该使用 ps 作为后缀。

".PS1" 中 1 代表 V1.0。同样的 `.ps2` 代表 v2.0 版本的脚本。为了向后兼容，通常会仍旧沿用`.ps1`。

可以通常 `#requires -version 2.0` 命令行，强调 `ps1` 或 `ps2` 后缀的脚本需要 v2.0 版本。

另外， .ps2、.ps2xml 和 .psc2 似乎也都是有效的 powershell 文件后缀。https://stackoverflow.com/questions/62604621/what-are-the-different-powershell-file-types


> `.pss` - Power Shell Script 也已经被使用。

> [PowerShell 脚本的后缀名为什么叫PS1](https://www.pstips.net/why-the-extension-of-powershell-scripts-called-ps1.html) 一文中猜测是 No.1 的意思，挺有意思的。

# 运行 ps1 文件的几种方式

## 调用操作符 - `&`

```sh
> & "D:\Program Files (x86)\Tencent\WeChat\fiveWeChat.ps1"
```

如果没有调用操作符，直接输入路径，返回的也是路径：

```sh
> "D:\Program Files (x86)\Tencent\WeChat\fiveWeChat.ps1"
D:\Program Files (x86)\Tencent\WeChat\fiveWeChat.ps1
> 
```

这种方式，允许ps1文件路径中有空格！

```sh
> New-Item -name "my test.ps1" -Force

    Directory: D:\Program Files (x86)\Tencent\WeChat

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---          2023-04-01    16:11              0 my test.ps1

> Add-Content "my test.ps1" -Value "echo test"
> & ".\my test.ps1"
test
```

## 绝对路径 或 相对路径 直接执行 ps1 文件

比如，我们直接输入 ps1 文件的绝对路径，**保证路径没有空格**，实现直接执行：

```sh
> C:\Test\fiveWeChat.ps1
> 
```

或者，使用相对路径，**保证路径没有空格**，执行当前路径下的 ps1 文件：

```sh
> .\fiveWeChat.ps1
> 
```

## 直接调用`powershell.exe`执行

```sh
> powershell.exe -ExecutionPolicy RemoteSigned -file ".\my test.ps1"
test
```


# 参考

- [powershell不同方式运行ps1文件](https://blog.csdn.net/sdyu_peter/article/details/80570882)
- [Why ".PS1" file extension for PS scripts?](https://microsoft.public.windows.powershell.narkive.com/IiYzIHrN/why-ps1-file-extension-for-ps-scripts)

