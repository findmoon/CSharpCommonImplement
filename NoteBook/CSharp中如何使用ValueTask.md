**C#中如何使用ValueTask？**

[toc]

> 原文 [How to use ValueTask in C#](https://www.infoworld.com/article/3565433/how-to-use-valuetask-in-csharp.html)

使用 C# 中的 ValueTask 避免从异步方法中返回任务对象(`task objects`)时的内存分配。

> 异步编程已经应用了相当长的一段时间。近年来，随着 async 和 await 关键字的引入，它变得更加强大。你可以利用异步编程来提高应用程序的响应能力(`responsiveness`)和吞吐量(`throughput`)。
> 
> C# 中异步方法的建议返回类型是 `Task`，如果要编写返回值的异步方法，则应返回 `Task<T>`。如果要编写事件处理程序，可以改为返回 `void`。
> 
> 在 C# 7.0 之前，异步方法可以返回 `Task`, `Task<T>` 或 `void`。
> 
> 从 C# 7.0 开始，异步方法还可以返回 `ValueTask`（作为 `System.Threading.Tasks.Extensions` 包的一部分提供）或 `ValueTask<T>`。本文讨论了如何在 C# 中使用 `ValueTask`。

若要使用本文中提供的代码示例，应在系统中安装 Visual Studio 2019。

# 创建 .NET 控制台应用程序

Create a .NET Core console application project in Visual Studio （可以创建 .NET6 或 .NET7 控制台应用）

1. Launch the Visual Studio IDE.
2. Click on “Create new project.”
3. In the “Create new project” window, select “Console App (.NET Core)” from the list of templates displayed.
4. Click Next.
5. In the “Configure your new project” window shown next, specify the name and location for the new project.
6. Click Create.

> 个人在 `MiscellaneousTestForNet` 项目下的 `ValueTaskUse` 简单测试。

# 为什么应该使用 ValueTask？

**Task表示某些操作的状态，即操作是否已完成、是否已取消等。异步方法可以返回 Task 或 ValueTask。**

现在，由于 Task 是引用类型，因此从异步方法返回 Task 对象意味着每次调用该方法时，都要在托管堆上分配对象。因此，使用 Task 时需要注意的一个问题是，**每次从方法返回 Task 对象，都需要在托管堆中分配内存**。如果你的方法执行的操作的结果是立即可用或同步完成的，则堆上的分配是不必要的，并因此成本很高。

这正是 ValueTask 来拯救的地方，`ValueTask<T>`提供了两个主要好处：

首先，`ValueTask<T>` 提高了性能，因为它不需要堆分配(`heap allocation`)；
其次，它的实现既容易又灵活。

当结果立即可用时，通过从异步方法返回 `ValueTask<T>` 而不是 `Task<T>`，可以避免不必要的分配开销，因为此处的 “T” 表示结构，而 C# 中的结构(`struct`)是值类型（与 `Task<T>` 中的“T”相反，后者表示类）。

`Task` 和 `ValueTask` 表示 C# 中的两种主要“可等待”类型(`"awaitable" type`)。请注意，你不能阻塞 ValueTask。**如果需要阻塞，则应使用 `AsTask` 方法将 `ValueTask` 转换为 `Task`，然后阻塞该引用 Task object。**

另请注意，**每个 `ValueTask` 只能消费(consume)一次**。在这里，“消费(consume)”一词意味着 `ValueTask` 可以异步等待（`await`）操作完成，或者利用 `AsTask` 将 `ValueTask` 转换为 `Task`。但是，一个 `ValueTask` 只应使用一次，之后 `ValueTask<T>` 应该被忽略。


# C# 中的 ValueTask 示例

假设你有一个返回 Task 的异步方法。可以利用 `Task.FromResult` 来创建 `Task` 对象，如下面给出的代码片段所示。

```C#
public Task<int> GetCustomerIdAsync()
{
    return Task.FromResult(1);
}
```

**上面的代码片段不会创建整个异步状态机的“魔法”（`async state machine`），但它会在托管堆中分配一个 Task 对象。**

为了避免这种分配，你可以利用 ValueTask，如下所示的代码片段。

```C#
public ValueTask<int> GetCustomerIdAsync()
{
    return new ValueTask<int>(1);
}
```

以下代码片段演示了 ValueTask 的一个同步实现。

```C#
 public interface IRepository<T>
 {
     ValueTask<T> GetData();
 }
```

`Repository` 类扩展了 `IRepository` 接口并实现其方法，如下所示：

```C#
 public class Repository<T> : IRepository<T>
 {
     public ValueTask<T> GetData()
     {
         var value = default(T);
         return new ValueTask<T>(value!); // default(T) 可能为null
     }

     // 或者 改为 可空类型T?
     //public ValueTask<T?> GetData()
     //{
     //    var value = default(T);
     //    return new ValueTask<T?>(value);
     //}
 }
```

从 Main 方法调用 GetData 方法：

```C#
static void Main(string[] args)
{
   IRepository<int> repository = new Repository<int>();
   var result = repository.GetData();
   if(result.IsCompleted)
        Console.WriteLine("Operation complete...");
   else
       Console.WriteLine("Operation incomplete...");
   Console.ReadKey();
}
```

现在让我们将另一个方法添加到 repository 中，这次是一个名为 GetDataAsync 的异步方法。以下是修改后的 `IRepository` 接口的外观。

```C#
public interface IRepository<T>
{
  ValueTask<T> GetData();
  ValueTask<T> GetDataAsync();
}
```

`GetDataAsync` 方法在 `Repository` 类中的实现如下面代码片段所示：

```C#
 public class Repository<T> : IRepository<T>
 {
     public ValueTask<T> GetData()
     {
         var value = default(T);
         return new ValueTask<T>(value);
     }
     public async ValueTask<T> GetDataAsync()
     {
         var value = default(T);
         await Task.Delay(100);
         return value!;
     }
 }
```

# 什么时候应该使用 ValueTask

尽管 ValueTask 提供了好处，但使用 ValueTask 代替 Task 需要一些权衡。

ValueTask 是具有两个字段的值类型，而 Task 是具有单个字段的引用类型。因此，使用 ValueTask 意味着处理更多数据，因为方法调用将返回两个数据字段而不是一个。此外，**如果你等待返回 ValueTask 的方法，则该异步方法的状态机也会更大 — 因为它必须容纳包含两个字段的结构，而不是在 Task 的情况下的单个引用**。

此外，如果异步方法的使用者使用 `Task.WhenAll` 或 `Task.WhenAny`，则在异步方法中使用 `ValueTask<T>` 作为返回类型可能会变得代价高昂。这是因为你需要使用 AsTask 方法将 `ValueTask<T>` 转换为 `Task<T>`，这将会产生(托管堆)分配 —— 可以通过第一时间使用一个缓存的 `Task<T>` 轻松避免。

> Further, if the consumer of an asynchronous method uses Task.WhenAll or Task.WhenAny, using ValueTask<T> as a return type in an asynchronous method might become costly. This is because you would need to convert the ValueTask<T> to Task<T> using the AsTask method, which would incur an allocation that could be easily avoided if a cached Task<T> had been used in the first place.

这是经验法则。**当你有一段始终异步的代码，即当操作不会立即完成时，请使用 `Task`；当异步操作的结果已可用或已有一个缓存结果时，请使用 ValueTask**。无论哪种方式，你都应该在考虑 ValueTask 之前，进行必要的性能分析。
