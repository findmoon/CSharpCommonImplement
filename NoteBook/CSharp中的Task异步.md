C#中的Task异步


[Task.Result跟 Task.GetAwaiter.GetResult()相同吗？怎么选？](https://cloud.tencent.com/developer/article/1649197)


[What is the difference between .Wait() vs .GetAwaiter().GetResult()?](https://stackoverflow.com/questions/36426937/what-is-the-difference-between-wait-vs-getawaiter-getresult)


好文好文好文：[Understanding Control Flow with Async and Await in C#](https://www.pluralsight.com/guides/understand-control-flow-async-await)


[C# multi -thread understanding](https://www.programmerall.com/article/56802485818/)


ConfigureAwait(false) 不在调用方上下文中异步执行方法

ConfigureAwait(true) 在调用方上下文中异步执行方法（避免切换上下文和线程，去执行异步方法）

[Why I no longer use ConfigureAwait(false)](https://dev.to/noseratio/why-i-no-longer-use-configureawait-false-3pne) 

[C#: Why you should use ConfigureAwait(false) in your library code](https://medium.com/bynder-tech/c-why-you-should-use-configureawait-false-in-your-library-code-d7837dce3d7f)