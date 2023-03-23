**C#中的Thread.SpinWait和Thread.Sleep方法**

`Thread.SpinWait`对于实现锁非常有用，.NET 中的 Monitor 和 ReaderWriterLock 内部就是使用的它。

SpinWait 将处理器推进到一个很小(轻量)的循环中，循环次数由参数 `iterations` 指定，等待的时间取决于处理器的速度！

与 Sleep 方法对比，调用 Sleep 的线程会 放弃（yields） 其当前处理器时间切片的剩余部分，即使指定的间隔为零也是如此。为 Sleep 指定非零间隔会从线程计划程序中删除考虑的该线程，直到时间间隔过去。

SpinWait 通常对普通应用程序没有用。在大多数情况下，应使用 .NET Framework 提供的同步类，例如，调用 Monitor.Enter 或 Monitor.Enter的包装语句。

# 参考

[Thread.SpinWait(Int32) Method](https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread.spinwait?view=netframework-4.6.2&f1url=%3FappId%3DDev16IDEF1%26l%3DZH-CN%26k%3Dk(System.Threading.Thread.SpinWait)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.6.2)%3Bk(DevLang-csharp)%26rd%3Dtrue)