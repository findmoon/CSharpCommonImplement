**CSharp记录**


- [VisualStudio2019新建C#类自动添加作者版权等信息](https://www.cnblogs.com/minuy/p/14058721.html)

- [最新的Microsoft Visual Studio新建文件自动添加注释教程](https://blog.csdn.net/weixin_44451672/article/details/127508974)

- 异步方法同步调用还会新开线程吗？  Async异步方法如果使用 GetAwaiter().GetResult() 同步调用，还会新开线程吗？ 还是说需要看异步方法具体的实现？

- 在一个异步线程中还有必要使用异步方法吗 

- 程序关闭时，等待 Async 异步线程的结束？

- c# new action begininvoke 需要调用 endinvoke 吗？两者必须成对出现吗？

似乎是需要成对出现，或者上原则上应该成对出现，官方[Calling Synchronous Methods Asynchronously](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/calling-synchronous-methods-asynchronously)介绍  `No matter which technique you use, always call EndInvoke to complete your asynchronous call`【无论使用何种方法，都要调用 EndInvoke 来完成异步调用】。

找时间验证 如果不调用 EndInvoke 会发生什么？安全问题、内存泄露？还是无法GC（等同内存泄露）

常见实现中，log记录改为：

```C#
#region BeginInvoke 的处理方式，改为 Task
// 仅 BeginInvoke 
//new Action(() =>
//{
//    DealLogInfo($"[{ DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}", isShowMessageBox, false);
//}).BeginInvoke(null, null);

BeginInvoke 后 成对 调用 EndInvoke
var logAction = new Action(() =>
{
   DealLogInfo($"[{DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}", isShowMessageBox, false);
});

logAction.BeginInvoke(ar =>
{
   logAction.EndInvoke(ar);
}, null); 
#endregion
```

[delegate.BeginInvoke的使用是否必须调用EndInvoke](https://bbs.csdn.net/topics/392177091?page=1)

[C#多线程教程(1)：BeginInvoke和EndInvoke方法，解决主线程延时Thread.sleep柱塞问题（转）](https://developer.aliyun.com/article/349104)

正常线程执行完，就会自动结束，EndInvoke 肯定会 影响 BeginInvoke 的线程的结束或者其他处理，因为会等待 EndInvoke 来获取结束的结果。具体待研究

# 弃元 _

https://learn.microsoft.com/zh-cn/dotnet/csharp/fundamentals/functional/discards

[C# 7.0 使用下划线忽略使用的变量](https://blog.csdn.net/lindexi_gd/article/details/83583121)

[C#7下划线作用：作为数字千分位的分割](https://blog.csdn.net/zang141588761/article/details/100152325)

[为什么我不建议在 C# 中用下划线 _ 开头来表示私有字段？](https://blog.csdn.net/qq_37925422/article/details/104547879)


# Task<TResult>.ConfigureAwait(true)

[Task<TResult>.ConfigureAwait(Boolean)](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1.configureawait?view=netframework-4.6.1&f1url=%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(System.Threading.Tasks.Task%25601.ConfigureAwait)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.6.1)%3Bk(DevLang-csharp)%26rd%3Dtrue)

# ExecInitFujidb().Result 这么调用直接卡死了，不继续执行

var inintDB= ExecInitFujidb().GetAwaiter().GetResult();

异步调用链，异步方法想要同步调用，需要将整个异步调用链的所有异步方法都改为同步调用，才能同步执行，否则可能调用链中的其他异步方法已经执行完了，但是由于没有将其同步化，就会导致系统等在那里，处于卡死状态.

异步方法同步化调用 需要有一定的前提，并不是所有的直接同步调用就可以。

# [Use ASP.NET Core APIs in a class library](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/target-aspnetcore?view=aspnetcore-6.0&tabs=visual-studio)

# [The difference between GetService() and GetRequiredService() in ASP.NET Core](https://andrewlock.net/the-difference-between-getservice-and-getrquiredservice-in-asp-net-core/)

# [最佳实践系列：ASP.NET Core 3.0中的验证机制——给错误信息加上默认值](https://www.jianshu.com/p/0b03fadb1641)


# [怎么查询oracle有哪些用户](https://www.php.cn/oracle/489128.html)

# Oracle

在 会话 级别下 不能修改 NLS_CHARACTERSET：

```sql
ALTER SESSION SET NLS_CHARACTERSET='UTF8'

错误报告 -
ORA-00922: ？？缺失或无效
00922. 00000 -  "missing or invalid option"
```

其他的 NLS 参数，比如 `NLS_TIME_FORMAT` 就可以修改成功

```sql
ALTER SESSION SET NLS_TIME_FORMAT='HH24.MI.SSXFF'

Session已变更。
```

# [Return different types of content from ASP.NET Core MVC Action Result](https://geeksarray.com/blog/return-different-types-of-content-from-asp-net-core-mvc-action-result)

# [ADO.NET 参数化查询](https://www.cnblogs.com/zhangyuanbo12358/p/3959924.html)

# [Oracle数据库之事务](https://www.cnblogs.com/zf29506564/p/5772380.html)

[Oracle 事务详解（transaction）](https://blog.csdn.net/qq_34745941/article/details/107865782)

# 关于SQLLite在并发多线程访问时的情况，是否需要额外处理？


# [UserControl 用户自定义控件](https://www.cnblogs.com/coolkiss/archive/2010/09/07/1820467.html)

# [Cycle Sort](https://www.cnblogs.com/coolkiss/archive/2010/12/15/1906562.html)

# COPY COMPLETE CONSOLE OUTPUT IN A WPF TEXTBOX?-WPF

You can use [Console.OpenStandardOutput](https://msdn.microsoft.com/en-us/library/16f09842.aspx) to intercept `Console.Writeline()`\- This [answer](https://stackoverflow.com/a/6024185/7292772) and this [link](http://nmarkou.blogspot.com/2011/12/redirect-console-output-to-textbox.html) explain how to implement a `TextBoxStreamWriter` and inject it in standard output stream.

As for `ConsoleTraceListener` - you should be able to write your own textbox based `TraceListener` and assign it to appropriate `TraceSources` (as they support multiple trace listeners). This [answer](https://stackoverflow.com/a/1389289/7292772) highlights how to implement the same.

More details regarding trace sources and listeners can be found [here](https://blogs.msdn.microsoft.com/mikehillberg/2006/09/14/trace-sources-in-wpf/).

# [《你不常用的c#之三》:Action 之怪状](https://www.cnblogs.com/zhaox583132460/p/3406233.html)

# [《你不常用的c#之XX》](https://www.cnblogs.com/javalzy/p/6155589.html)


# [3分钟带你了解PowerShell发展历程——PowerShell各版本资料整理](https://www.cnblogs.com/lavender000/p/6931405.html)