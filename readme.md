**常见/通用C#功能的实现**

# 已实现

1. ExecCMD：执行cmd命令并获取返回值
2. FileLogging：自定义简单的文件日志写入程序
3. FilesWatcher：C#的文件监控模块
4. GUIDExample：GUID使用示例
5. MinimizeSystemTray：最小化到系统托盘
7. RigthTopButton：右上角控制按钮的使用和显示隐藏
7. StartWitchPCLibAdmin、StartWithPCLibNoAdmin：开机启动库
8. StartWithPC：开机启动测试
9. TimeStampsCalc：时间戳计算
10. MACNetworkAddressExample->WMINetworkConfiguration.cs：C#通过WMI管理网络的示例
11. EncodingCase：`Encoding.Default` 等 .NET 字符编码的简要介绍

# 未实现

0. [真正]随机数生成的实现  机制、处理实现
6. RandomString：生成或获取随机字符串。随机中文、英文大小写数字、string扩展方法获取其内的随机内容 并没有真正随机，重复获取多次，每次都会相同
1. EventLogUse：EventLog的使用（Windows下EventLog）
2. GetIPFromNetWork：从网络中获取所有的IP，应该通过当前ip和子网掩码，循环请求局域网内所有ip获取是否可用
3. MACNetworkAddressExample：MAC地址的获取，本机MAC，获取局域网内所有可用机器的MAC（应该通过当前ip和子网掩码，循环arp请求局域网内所有ip获取MAC地址）
4. MaskedTextBoxExample：MaskedTextBox的使用
5. Notifications：实现Windows下的通知
6. SimplePainting：简单的绘图程序
7. TextInputPlaceholder：带占位符的文本框
8. TopMost：winform在屏幕最前方显示
9. ValidCode：C#生成验证码
10. 与非托管代码的互操作等

# 自定义控件

## 实现

1. 圆形按钮

## 未实现或未具体了解代码

1. 根据圆形按钮的实现代码，是否可以通过 Region 实现任意形状按钮的实现？
2. 