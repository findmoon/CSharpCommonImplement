**C#在WPF、Winform和控制台等应用中获取可执行文件路径、运行程序目录的方法【总结】**

[toc]

# 通用获取路径的方法

## Process.GetCurrentProcess().MainModule

```C#
Console.WriteLine($"System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName：{System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName}");
```

该方法是获取当前进程对应的可执行程序exe文件路径最正确的方法。虽然有可能MainModule为空，因此需要做条件判断。

`System.Reflection.Assembly.GetExecutingAssembly().Location`、`System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase` 获取的是程序集的完整路径，如果是位于主程序中，则为可执行程序exe文件路径；否则有可能为生成的dll路径，这点要注意。

> .Net framework 的控制台应用，不能使用。

## Assembly.GetExecutingAssembly()

```C#
// URL格式的程序集路径
Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase：{System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase}");

Console.WriteLine();

// 完整路径
Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().Location：{System.Reflection.Assembly.GetExecutingAssembly().Location}");
```

输出，主要看下 URL 格式文件：

```C#
System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase：file:///D:/SoftWareDevelope/CSharp/csharp-common-implement/Test/HelloWorld/bin/Debug/net6.0/HelloWorld.dll

System.Reflection.Assembly.GetExecutingAssembly().Location：D:\SoftWareDevelope\CSharp\csharp-common-implement\Test\HelloWorld\bin\Debug\net6.0\HelloWorld.dll
```

## System.AppDomain.CurrentDomain 获取目录

```C#
 // 路径
Console.WriteLine($"System.AppDomain.CurrentDomain.BaseDirectory：{System.AppDomain.CurrentDomain.BaseDirectory}");

Console.WriteLine();

// 路径
Console.WriteLine($"System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase：{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}");
```

输出：

```C#
System.AppDomain.CurrentDomain.BaseDirectory：D:\SoftWareDevelope\CSharp\csharp-common-implement\Test\HelloWorld\bin\Debug\net6.0\

System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase：D:\SoftWareDevelope\CSharp\csharp-common-implement\Test\HelloWorld\bin\Debug\net6.0\
```

## Directory.GetCurrentDirectory

```C#
Console.WriteLine($"System.IO.Directory.GetCurrentDirectory()：{System.IO.Directory.GetCurrentDirectory()}");
```

输出：

```C#
System.IO.Directory.GetCurrentDirectory()：D:\SoftWareDevelope\CSharp\csharp-common-implement\Test\HelloWorld\bin\Debug\net6.0
```

## System.Environment.CurrentDirectory 当前上下文目录

```C#
Console.WriteLine($"System.Environment.CurrentDirectory：{System.Environment.CurrentDirectory}");
```

输出：

```C#
System.Environment.CurrentDirectory：D:\SoftWareDevelope\CSharp\csharp-common-implement\Test\HelloWorld\bin\Debug\net6.0
```

## 对应获取程序集名称的几个方法

```C#
System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;//exe name with extensionName

System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;  // dll name without ExtensionName

System.AppDomain.CurrentDomain.FriendlyName;  // without ExtensionName

System.Diagnostics.Process.GetCurrentProcess().ProcessName; // with extensionName

```

## 关于 FullName

```C#
System.Reflection.Assembly.GetExecutingAssembly().FullName

// HelloWorld, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName

// HelloWorld, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
```

# Winform 的 System.Windows.Forms.Application.ExecutablePath

Winform下获取可执行程序文件路径或目录，除了上面介绍的几种方法，还可以使用 `System.Windows.Forms.Application.ExecutablePath`

```C#
// 完整路径
Debug.WriteLine($"System.Windows.Forms.Application.ExecutablePath：{System.Windows.Forms.Application.ExecutablePath}");

Debug.WriteLine(Environment.NewLine);

// 启动路径
Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase：{System.Windows.Forms.Application.StartupPath}");

Debug.WriteLine(Environment.NewLine);

// 产品名 
Debug.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().Location：{System.Windows.Forms.Application.ProductName}");

Debug.WriteLine(Environment.NewLine);
```

# 参考

[WPF下获取文件运行路径、运行文件名等](https://www.cnblogs.com/luguangguang/p/15176237.html)
