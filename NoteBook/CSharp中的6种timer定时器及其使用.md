**.NET/C# 中的6种timer定时器及其使用**

[toc]

# 六种定时器总览和使用场景

到 .NET 6 为止，C# 中一个提供了六种定时器`timer`相关的类，它们都用于定时（定期）执行一项任务，除了`System.Windows.Forms.Timer`，其它都是用于多线程环境下。

相对来说，各自`timer`有着不同的目的或使用场景，分别是：

- `System.Threading.Timer`：用于使用线程池的线程定时执行单个回调方法(`PeriodicTimer`)。

    `System.Threading.Timer`是一个简单、轻量级的使用回调方法的定时器，由线程池提供线程。不推荐在 Windows Form中使用，因为它的回调不在用户界面线程（UI线程）执行。回调方法在定时器实例化时定义，并且不能改变。该类用于基于服务器或服务组件的多线程环境。没有UI并且在运行时不可见。

- `System.Timers.Timer`：由线程池中的线程定时触发一个事件(`event`)。

    `System.Timers.Timer`用于基于服务器或服务组件的多线程环境（`server-based or service component`）。没有UI并且在运行时不可见。

- `System.Threading.PeriodicTimer`【.NET 6 引入】：允许调用者等待单独的时间之后执行工作。

以上三者均可以在 .NET (Core) 环境中使用。

- `System.Windows.Forms.Timer`：是一个无UI的`WinForm`组件，用于单线程环境中定期执行一个事件(`event`)。【也就是，在UI线程上定时执行一个或多个事件方法，没有用到多线程】。

    在 Windows Form 中推荐使用 `System.Windows.Forms.Timer`，对于基于服务器的定时器功能，可以考虑使用 `System.Timers.Timer` 触发事件和使用额外功能。

- `System.Web.UI.Timer`【仅在 .NET Framework下可用】：一个 `ASP.NET` 组件，用于定期执行异步或同步的 `web page Postback`。

- `System.Windows.Threading.DispatcherTimer`：一个集成在`Dispatcher`队列中的定时器，以指定的时间间隔和权限去处理。

> 注：只要使用一个定时器 Timer，必须要做到始终引用它。与任何托管对象一样，**当没有对 Timer 的引用时，它会受到垃圾回收(`garbage collection`)的影响，定时器处于活动状态并不妨碍其被回收**。

# 新的现代的定时器 PeriodicTimer

`PeriodicTimer`是 .NET 6 引入的新的定时器类型，其目的是避免使用回调。

避免回调，可以使我们在长期操作中免于处理可能发生的内存泄漏，并且，可以编写`async`代码而不是在回调中使用异步的同步方式(`sync over async approach`)。使用之前的定时器类型可能要处理的另一个问题是`回调重叠`(`callback overlapping`)，如果没有为 **回调重叠** 的场景编写代码，则可能会在应用程序中看到异常的行为。



# 参考

- [Timer Class](https://learn.microsoft.com/en-us/dotnet/api/system.threading.timer?view=net-7.0)
- [Timers](https://learn.microsoft.com/en-us/dotnet/standard/threading/timers)
- [PeriodicTimer Class](https://learn.microsoft.com/en-us/dotnet/api/system.threading.periodictimer?view=net-7.0)