**IIS web服务器中HttpClient导致的高CPU负载[译]**

[toc]

> 翻译自 [High CPU load in IIS web server caused by HttpClient](https://port135.com/high-cpu-load-iis-web-server/)

高CPU负载是web服务器一直在抗争的最常见的问题。可能有几个根本原因，例如死锁、硬件不足、高流量、编码不佳等。

在这篇文章中，我将解释由非线程安全对象引起的高 CPU 负载的原因和解决方案。

此问题可能以多种方式出现。其中最显然的一种是，高资源使用率。你可以在 任务管理器、资源监视器 或 性能监视器 中 监视资源使用情况。

> Task Manager, Resource Monitor or Performance Monitor

在深入探究之前，先记住什么是 线程安全：

**线程安全是多线程程序上下文中的计算机编程概念。一段线程安全的代码，要保证多个线程可以同时以安全的方式执行操作共享的数据结构。**

> Thread safety is a computer programming concept applicable in the context of multi-threaded programs. A piece of code is thread-safe if it only manipulates shared data structures in a manner that guarantees safe execution by multiple threads at the same time.
> 
> [Thread safety](https://en.wikipedia.org/wiki/Thread_safety)

# 性能监视器和事件查看器中的高CPU负载

下面的性能监视器图表显示 CPU 负载约为 80%（红色实线）。它还演示了错误计数（棕色虚线）与请求总数（绿色虚线）如何并行增加。

> CPU load 、error count、request total

![](img/High%20CPU%20load%20displayed%20in%20Performance%20Monitor.png)  

此问题可能会在事件查看器中显示为错误或警告消息，乍一看似乎与 CPU 负载无关。以下消息记录在事件查看器的系统容器(`System container`)中，其中提到无响应的应用程序池(`unresponsive application pool`)，指出了高 CPU 负载的问题。

> Event ID: 5010 (Warning)
>
> A process serving application pool “X” failed to respond to a ping. The process id was “1234”
>

# 高CPU负载的根本原因

正如文章开头提到的，有多种原因会导致web服务器遇到高CPU负载。在当前场景中，原因是多次使用非线程安全对象的实例。更具体地说，**从不同线程使用相同的 HttpClient 实例会增加 CPU 使用率**。

在 `HttpClient` 的官方文档中，建议只使用此对象的一个实例：

**HttpClient 旨在实例化一次，并在应用程序的整个生命周期中重复使用。为每个请求实例化 HttpClient 类将耗尽重负载下可用的套接字数。这将导致 SocketException 错误。**

> HttpClient is intended to be instantiated once and re-used throughout the life of an application. Instantiating an HttpClient class for every request will exhaust the number of sockets available under heavy loads. This will result in SocketException errors.
> 
> [HttpClient Class](https://docs.microsoft.com/en-gb/dotnet/api/system.net.http.httpclient?view=netframework-4.7.1)

但是，在实践中，这不是一个好主意，特别是对于请求相互踩踏的情况（`in practice, it is not a good idea especially for the scenarios where requests step on each other`）：

**如果你有相关的请求（或不会相互踩踏），那么使用相同的 HttpClient 很有意义。**

> If you have requests that are related (or **won’t step on eachother**) then using the same HttpClient makes a lot of sense.
> 
> [Best practice usage of HttpClient](https://social.msdn.microsoft.com/Forums/en-US/4e12d8e2-e0bf-4654-ac85-3d49b07b50af/best-practice-usage-of-httpclient-for-rest-calls-maximum-throughput?forum=netfxnetcom)

如果目标是错误的 CPU 体系结构（32 位或 64 位），即与 DLL 库不匹配，也可能导致问题。查看这篇文章以获取解决方案：[如何找出dll和exe文件的处理器体系结构（x86，x64）？](https://port135.com/2018/07/07/find-processor-architecture-dll-assembly/)

# 高CPU负载的解决方案

类似该实例的许多情况下，应用程序不应仅使用 HttpClient 的一个实例。它会导致请求相互重叠并增加 CPU 负载。

与其调用相同的 HttpClient 实例（或创建 HttpClient 对象的函数），使用新实例应该可以解决高 CPU 使用率问题。

如果你没有 CPU 使用率问题，但你的工作进程 （w3wp.exe） 定期崩溃，这篇文章可以帮助你修复它：[w3wp.exe crashes every 5 minutes with error code 0xc0000374](https://port135.com/2019/01/11/w3wp-exe-crashes-0xc0000374/)

