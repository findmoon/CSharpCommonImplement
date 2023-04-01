**快速启动多个应用的方法和脚本（电脑中同时打开多个微信的n种方法）**

[toc]

总结如下：

- 一：极其快速的多次双击打开。比如，进入`C:\Program Files (x86)\Tencent\WeChat`，快速双击`WeChat.exe`程序。

- 二：多次快速按回车。先选中`WeChat.exe`程序，然后快速多次按回车键。

- 三：按住回车键的同时双击打开。容易不小心多个n多个。

- 四：cmd命令行下 运行 `start WeChat.exe&Wechat.exe&Wechat.exe`，`&` 用于连接要打开的多个应用程序

- 五：bat批处理执行 `start "" "C:\Program Files (x86)\Tencent\WeChat\WeChat.exe"`，直接放入多条命令即可。

- 六：Powershell 的 `ForEach-Object -Parallel` 并行执行。比如 `1..5 | ForEach-Object -Parallel { start WeChat.exe }` 一次启动5个程序。

- 保存为Powershell脚本，如`fiveWeChat.ps1`。在 Powershell中执行`.\fiveWeChat.ps1`

- Powershell 如何同时运行多条命令，目前所知 `|`管道、`;`、`&&` 都是顺序执行的，执行完一个再执行下一个，没法同时直接执行多个（实现运行多个程序）。包括启动进程命令 `1..5 | ForEach-Object  { start-process Wechat.exe; }` 或 `start-process Wechat.exe;start-process Wechat.exe;start-process Wechat.exe;`。

- 另：`Powershell` 的 ForEach-Object 方法，如果运气好的话，可以启动多个程序。比如`1..5 | ForEach-Object  { start Wechat.exe; }`

- `powershell-command`、`Invoke-Command`、`Get-Command`

- `Start-Job -ScriptBlock {start WeChat.exe}; Start-Job -ScriptBlock {start WeChat.exe};`

> `start-process PowerShell -verb runas` 以管理员身份启动 PowerShell
>
> `start-process` 的别名 saps 或 start
> 
> 并非所有命令可以使用`|`连接符。
> 
> PowerShell 是单线程的，一次只能做一件事。
> 
> `||` 前面的命令执行失败才执行后面的；`&&` 前面的命令执行成功才执行后面的。
> 
> `&&` this will run the second command only if the first one succeeds.
> `||` this will run the second command only if the first one fails.
> 
> `&&` 和 `||` 操作符 需要 PowerShell 7 版本。

> Powershell 使用命令在资源管理器中打开当前工作目录：`explorer (pwd).Path`。

# 打开多个微信的方法

## 快速双击打开微信

重点是快速多次的双击。

## 按住回车键鼠标双击

按住Enter键的同时双击打开微信，并快速放开回车键，即可实现多开微信，

缺点是不好掌控，一不留神放开回车键的速度慢了点的话，就会打开太多的微信

## 多次快速按回车

鼠标选择 echat.exe 程序文件，快速多次按回车键，也可以打开多个程序。

## cmd中执行 `start WeChat.exe&Wechat.exe&Wechat.exe`

进入到 微信 软件所在目录，在 cmd 中执行 `start WeChat.exe&Wechat.exe&Wechat.exe`，几个`Wechat.exe`就表示打开几个微信，`&`连接。

也可以通过执行软件目录，比如 `start "" "C:\Program Files (x86)\Tencent\WeChat\WeChat.exe"` 执行。

## bat批处理实现

如下，在 bat 批处理文件中通过start打开三个：

```sh
@echo off

start "" "C:\Program Files (x86)\Tencent\WeChat\WeChat.exe"
start "" "C:\Program Files (x86)\Tencent\WeChat\WeChat.exe"
start "" "C:\Program Files (x86)\Tencent\WeChat\WeChat.exe"

exit
```

## Powershell 中 `ForEach-Object -Parallel`

如下，启动三个：

```sh
1..3 | ForEach-Object -Parallel { start WeChat.exe }
```


# 参考

- [PowerShell ForEach-Object -Parallel Feature](https://devblogs.microsoft.com/powershell/powershell-foreach-object-parallel-feature/)
- [1个电脑怎么打开多个微信，这几种方法，你都学会了吗](https://zhuanlan.zhihu.com/p/610922905)
