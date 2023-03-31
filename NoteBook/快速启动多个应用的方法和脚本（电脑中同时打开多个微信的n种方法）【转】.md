**快速启动多个应用的方法和脚本（电脑中同时打开多个微信的n种方法）【转】**

[toc]

总结如下：



- 一：极其快速的多次双击打开。比如，进入`C:\Program Files (x86)\Tencent\WeChat`，快速双击`WeChat.exe`程序。

- 二：按住回车键的同时双击打开。容易不小心多个n多个。

- 三：cmd命令行下 运行 `start WeChat.exe&Wechat.exe&Wechat.exe`，`&` 用于连接要打开的多个应用程序

- 四：bat批处理执行 `start "" "C:\Program Files (x86)\Tencent\WeChat\WeChat.exe"`，直接放入多条命令即可。

- 