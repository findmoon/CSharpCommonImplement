**C#理解ValueTask是什么、为什么使用及何时使用ValueTask**

[toc]

> 原文 [Understanding the Whys, Whats, and Whens of ValueTask](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/)
>
> 翻译原文 [理解C#中的ValueTask](https://www.cnblogs.com/xiaoxiaotank/p/13206569.html) 

> 原文：[https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/](https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/)  
> 作者：Stephen  
> 翻译：xiaoxiaotank  
> 备注：本文要求读者对`Task`有一定的了解，文章文字描述较多，但内容十分充实，相信你认真阅读后，一定让你受益匪浅。

#### 前言

`Task`类是在.NET Framework 4引入的，位于`System.Threading.Tasks`命名空间下，它与派生的泛型类`Task<TResult>`已然成为.NET编程的主力，也是以`async/await`（C# 5引入的）语法糖为代表的异步编程模型的核心。

随后，我会向大家介绍.NET Core 2.0中的新成员`ValueTask/ValueTask<TResult>`，来帮助你在日常开发用例中降低内存分配开销，提升异步性能。

## Task

虽然`Task`的用法有很多，但其最核心的是“承诺（promise）”，用来表示某个操作最终完成。

当你初始化一个操作后，会获取一个与该操作相关的`Task`，当这个操作完成时，`Task`也同样会完成。这个操作的完成情况可能有以下几种：

- 作为初始化操作的一部分同步完成，例如：访问一些已被缓存的数据
- 恰好在你获取到`Task`实例的时候异步完成，例如：访问虽然没被缓存但是访问速度非常快的数据
- 你已经获取到了`Task`实例，并等待了一段时间后，才异步完成，例如：访问一些网络数据

由于操作可能会异步完成，所以当你想要使用最终结果时，你可以通过阻塞来等待结果返回（不过这违背了异步操作的初衷）；或者，使用回调方法，它会在操作完成时被调用，.NET 4通过`Task.ContinueWith`方法显式实现了这个回调方法，如：

csharp

SomeOperationAsync().ContinueWith(task =>
{
    try
    {
        TResult result = task.Result;
        UseResult(result);
    }
    catch(Exception ex)
    {
        HandleException(ex);
    }
}) 

而在.NET 4.5中，`Task`通过结合`await`，大大简化了对异步操作结果的使用，它能够优化上面说的所有情况，无论操作是同步完成、快速异步完成还是已经（隐式地）提供回调之后异步完成，都不在话下，写法如下：

highlighter- isbl

TResult result = await SomeOperationAsync();
UseResult(result); 

`Task`作为一个类（class），非常灵活，并因此带来了很多好处。例如：

- 它可以被任意数量的调用者并发`await`多次
- 你可以把它存储到字典中，以便任意数量的后续使用者对其进行`await`，进而把这个字典当成异步结果的缓存
- 如果需要的话，你可以通过阻塞等待操作完成
- 另外，你还可以对`Task`使用各种各样的操作（称为“组合器”，combinators），例如使用`Task.WhenAny`异步等待任意一个操作率先完成。

不过，在大多数情况下其实用不到这种灵活性，只需要简单地调用异步操作并`await`获取结果就好了：

highlighter- isbl

TResult result = await SomeOperationAsync();
UseResult(result); 

在这种用法中，我们不需要多次`await task`，不需要处理并发`await`，不需要处理同步阻塞，也不需要编写组合器，我们只是异步等待操作的结果。这就是我们编写同步代码（例如`TResult result = SomeOperation()`）的方式，它很自然地转换为了`async/await`的方式。

此外，`Task`也确实存在潜在缺陷，特别是在需要创建大量`Task`实例且要求高吞吐量和高性能的场景下。`Task` 是一个类（class），作为一个类，这意味着每创建一个操作，都需要分配一个对象，而且分配的对象越多，垃圾回收器（GC）的工作量也会越大，我们花在这个上面的资源也就越多，本来这些资源可以用于做其他事情。庆幸的是，运行时（Runtime）和核心库在许多情况下都可以缓解这种情况。

例如，你写了如下方法：

highlighter- arduino

public async Task WriteAsync(byte value) {
    if (\_bufferedCount == \_buffer.Length)
    {
        await FlushAsync();
    }
    \_buffer\[\_bufferedCount++\] = value;
} 

一般来说，缓冲区中会有可用空间，也就无需`Flush`，这样操作就会同步完成。这时，不需要`Task`返回任何特殊信息，因为没有返回值，返回`Task`与同步方法返回`void`没什么区别。因此，运行时可以简单地缓存单个非泛型Task，并将其反复用作任何同步完成的方法的结果（该单例是通过`Task.CompletedTask`公开的）。

或者，你的方法是这样的：

highlighter- arduino

public async Task<bool\> MoveNextAsync() {
    if (\_bufferedCount == 0)
    {
        // 缓存数据
        await FillBuffer();
    }
    return \_bufferedCount > 0;
} 

一般来说，我们想的是会有一些缓存数据，这样`_bufferedCount`就不会等于0，直接返回`true`就可以了；只有当没有缓存数据（即\_bufferedCount == 0）时，才需要执行可能异步完成的操作。而且，由于只有`true`和`false`这两种可能的结果，所以只需要两个`Task<bool>`对象来分别表示`true`和`false`，因此运行时可以将这两个对象缓存下来，避免内存分配。只有当操作异步完成时，该方法才需要分配新的`Task<bool>`，因为调用方在知道操作结果之前，就要得到`Task<bool>`对象，并且要求该对象是唯一的，这样在操作完成后，就可以将结果存储到该对象中。

运行时也为其他类型型维护了一个类似的小型缓存，但是想要缓存所有内容是不切实际的。例如下面这个方法：

highlighter- arduino

public async Task<int\> ReadNextByteAsync() {
    if (\_bufferedCount == 0)
    {
        await FillBuffer();
    }

    if (\_bufferedCount == 0)
    {
        return \-1;
    }

    \_bufferedCount--;
    return \_buffer\[\_position++\];
} 

通常情况下，上面的案例也会同步完成。但是与上一个返回`Task<bool>`的案例不同，该方法返回的`Int32`的可能值约有40亿个结果，如果将它们都缓存下来，大概会消耗数百GB的内存。虽然运行时保留了一个小型缓存，但也只保留了一小部分结果值，因此，如果该方法同步完成（缓冲区中有数据）的返回值是4，它会返回缓存的`Task<int>`，但是如果它同步完成的返回值是42，那就会分配一个新的`Task<int>`，相当于调用了`Task.FromResult(42)`。

许多框架库的实现也尝试通过维护自己的缓存来进一步缓解这种情况。例如，.NET Framework 4.5中引入的`MemoryStream.ReadAsync`重载方法总是会同步完成，因为它只从内存中读取数据。它返回一个`Task<int>`对象，其中`Int32`结果表示读取的字节数。`ReadAsync`常常用在循环中，并且每次调用时请求的字节数是相同的（仅读取到数据末尾时才有可能不同）。因此，重复调用通常会返回同步结果，其结果与上一次调用相同。这样，可以维护单个`Task`实例的缓存，即缓存最后一次成功返回的`Task`实例。然后在后续调用中，如果新结果与其缓存的结果相匹配，它还是返回缓存的`Task`实例；否则，它会创建一个新的`Task`实例，并把它作为新的缓存`Task`，然后将其返回。

即使这样，在许多操作同步完成的情况下，仍需强制分配`Task<TResult>`实例并返回。

## 同步完成时的ValueTask<TResult>

正因如此，在.NET Core 2.0 中引入了一个新类型——`ValueTask<TResult>`，用来优化性能。之前的.NET版本可以通过引用NuGet包使用：`System.Threading.Tasks.Extensions`

`ValueTask<TResult>`是一个结构体（struct），用来包装`TResult`或`Task<TResult>`，因此它可以从异步方法中返回。并且，如果方法是同步成功完成的，则不需要分配任何东西：我们可以简单地使用`TResult`来初始化`ValueTask<TResult>`并返回它。只有当方法异步完成时，才需要分配一个`Task<TResult>`实例，并使用`ValueTask<TResult>`来包装该实例。另外，为了使`ValueTask<TResult>`更加轻量化，并为成功情形进行优化，所以抛出未处理异常的异步方法也会分配一个`Task<TResult>`实例，以方便`ValueTask<TResult>`包装`Task<TResult>`，而不是增加一个附加字段来存储异常（Exception）。

这样，像`MemoryStream.ReadAsync`这类方法将返回`ValueTask<int>`而不需要关注缓存，现在可以使用以下代码：

highlighter- arduino

public override ValueTask<int\> ReadAsync(byte\[\] buffer, int offset, int count) {
    try
    {
        int bytesRead = Read(buffer, offset, count);
        return new ValueTask<int\>(bytesRead);
    }
    catch (Exception e)
    {
        return new ValueTask<int\>(Task.FromException<int\>(e));
    }
} 

## 异步完成时的ValueTask<TResult>

能够编写出在同步完成时无需为结果类型产生额外内存分配的异步方法是一项很大的突破，.NET Core 2.0引入`ValueTask<TResult>`的目的，就是将频繁使用的新方法定义为返回`ValueTask<TResult>`而不是`Task<TResult>`。

例如，我们在.NET Core 2.1中的`Stream`类中添加了新的`ReadAsync`重载方法，以传递`Memory<byte>`来替代`byte[]`，该方法的返回类型就是`ValueTask<int>`。这样，Streams（一般都有一种同步完成的`ReadAsync`方法，如前面的`MemoryStream`示例中所示）现在可以在使用过程中更少的分配内存。

但是，在处理高吞吐量服务时，我们依旧需要考虑如何尽可能地避免额外内存分配，这就要想办法减少或消除异步完成时的内存分配。

使用`await`异步编程模型时，对于任何异步完成的操作，我们都需要返回代表该操作最终完成的对象：调用者需要能够传递在操作完成时调用的回调方法，这就要求在堆上有一个唯一的对象，用作这种特定操作的管道，但是，这并不意味着有关操作完成后能否重用该对象的任何信息。如果对象可以重复使用，则API可以维护一个或多个此类对象的缓存，并将其复用于序列化操作，也就是说，它不能将同一对象用于多个同时进行中的异步操作，但可以复用于非并行访问下的对象。

在.NET Core 2.1中，为了支持这种池化和复用，`ValueTask<TResult>`进行了增强，不仅可以包装`TResult`和`Task<TResult>`，还可以包装新引入的接口`IValueTaskSource<TResult>`。类似于`Task<TResult>`，`IValueTaskSource<TResult>`提供表示异步操作所需的核心支持；

highlighter- reasonml

public interface IValueTaskSource<out TResult>
{
    ValueTaskSourceStatus GetStatus(short token);
    void OnCompleted(Action<object\> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags);
    TResult GetResult(short token);
} 

- `GetStatus`用于实现诸如`ValueTask<TResult>.IsCompleted`之类的属性，返回指示异步操作是否仍在挂起或是否已完成以及完成情况（成功或失败）的指示。
- `OnCompleted`用于`ValueTask<TResult>`的等待者（awaiter），它与调用者提供的回调方法挂钩，当异步操作完成时，等待者继续执行回调方法。
- `GetResult`用于检索操作的结果，以便在操作完成后，等待者可以获取`TResult`或传播可能发生的任何异常。

大多数开发人员永远都不需要用到此接口（指`IValueTaskSource<TResult>`）：方法只是简单地将包装该接口实例的`ValueTask<TResult>`实例返回给调用者，而调用者并不需要知道内部细节。该接口的主要作用是为了让开发人员在编写性能敏感的API时可以尽可能地避免额外内存分配。

.NET Core 2.1中有几个类似的API。最值得关注的是`Socket.ReceiveAsync`和`Socket.SendAsync`，添加了新的重载，例如：

highlighter- reasonml

public ValueTask<int\> ReceiveAsync(Memory<byte\> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default); 

此重载返回`ValueTask<int>`。

如果操作同步完成，则可以简单地构造具有正确结果的`ValueTask<int>`，例如：

highlighter- sql

int result \= …;
return new ValueTask<int\>(result); 

如果它异步完成，则可以使用实现此接口的池对象：

highlighter- arduino

IValueTaskSource<int\> vts = …;
return new ValueTask<int\>(vts); 

该`Socket`实现维护了两个这样的池对象，一个用于Receive，一个用于Send，这样，每次未完成的对象只要不超过一个，即使这些重载是异步完成的，它们最终也不会额外分配内存。`NetworkStream`也因此受益。

例如，在.NET Core 2.1中，`Stream`公开了一个方法：

highlighter- arduino

public virtual ValueTask<int\> ReadAsync(Memory<byte\> buffer, CancellationToken cancellationToken = default); 

`NetworkStream`的重载方法`NetworkStream.ReadAsync`，内部实际逻辑只是交给了`Socket.ReceiveAsync`去处理，所以将优势从`Socket`带到了`NetworkStream`中，使得`NetworkStream.ReadAsync`也有效地不进行额外内存分配了。

## 非泛型的ValueTask

当在.NET Core 2.0中引入`ValueTask<TResult>`时，它纯粹是为了优化异步方法同步完成的情况——避免必须分配一个`Task<TResult>`实例用于存储`TResult`。这也意味着非泛型的`ValueTask`是不必要的（因为没有`TResult`）：对于同步完成的情况，返回值为`Task`的方法可以返回`Task.CompletedTask`单例，此单例由`async Task`方法的运行时隐式返回。

然而，随着即使异步完成也要避免额外内存分配需求的出现，非泛型的`ValueTask`又变得必不可少。因此，在.NET Core 2.1中，我们还引入了非泛型的`ValueTask`和`IValueTaskSource`。它们提供泛型版本对应的非泛型版本，使用方式类似，只是`GetResult`返回`void`。

## 实现IValueTaskSource / IValueTaskSource<TResult>

大多数开发人员都不需要实现这两个接口，它们也不是特别容易实现。如果您需要的话，.NET Core 2.1的内部有几种实现可以用作参考，例如

- [AwaitableSocketAsyncEventArgs](https://github.com/dotnet/corefx/blob/61f51e6b2b26271de205eb8a14236afef482971b/src/System.Net.Sockets/src/System/Net/Sockets/Socket.Tasks.cs#L808)
- [AsyncOperation](https://github.com/dotnet/corefx/blob/89ab1e83a7e00d869e1580151e24f01226acaf3f/src/System.Threading.Channels/src/System/Threading/Channels/AsyncOperation.cs#L37)
- [DefaultPipeReader](https://github.com/dotnet/corefx/blob/a10890f4ffe0fadf090c922578ba0e606ebdd16c/src/System.IO.Pipelines/src/System/IO/Pipelines/Pipe.DefaultPipeReader.cs#L16)

为了使想要这样做的开发人员更轻松地进行开发，将在.NET Core 3.0中计划引入`ManualResetValueTaskSourceCore<TResult>`结构体（译注：目前已引入），用于实现接口的所有逻辑，并可以被包装到其他实现了`IValueTaskSource`和`IValueTaskSource<TResult>`的包装器对象中，这个包装器对象只需要单纯地将大部分实现交给该结构体就可以了。

## ValueTask的有效消费模式

从表面上看，`ValueTask`和`ValueTask<TResult>`的使用限制要比`Task`和`Task<TResult>`大得多 。不过没关系，这甚至就是我们想要的，因为主要的消费方式就是简单地`await`它们。

但是，由于`ValueTask`和`ValueTask<TResult>`可能包装可复用的对象，因此，与`Task`和`Task<TResult>`相比，如果调用者偏离了仅`await`它们的设计目的，则它们在使用上实际回受到很大的限制。通常，以下操作绝对不能用在`ValueTask/ValueTask<TResult>`上：

- `await ValueTask/ValueTask<TResult>`多次。
    
    因为底层对象可能已经被回收了，并已由其他操作使用。而`Task/Task<TResult>`永远不会从完成状态转换为未完成状态，因此您可以根据需要等待多次，并且每次都会得到相同的结果。
    
- 并发`await ValueTask/ValueTask<TResult>`。
    
    底层对象期望一次只有单个调用者的单个回调来使用，并且尝试同时等待它可能很容易引入竞争条件和细微的程序错误。这也是第一个错误操作的一个更具体的情况——`await ValueTask/ValueTask<TResult>`多次。相反，`Task/Task<TResult>`支持任意数量的并发等待
    
- 使用`.GetAwaiter().GetResult()`时操作尚未完成。
    
    `IValueTaskSource / IValueTaskSource<TResult>`接口的实现中，在操作完成前是没有强制要求支持阻塞的，并且很可能不会支持，所以这种操作本质上是一种竞争状态，也不可能按照调用方的意愿去执行。相反，`Task/Task<TResult>`支持此功能，可以阻塞调用者，直到任务完成。
    

如果您使用`ValueTask/ValueTask<TResult>`，并且您确实需要执行上述任一操作，则应使用`.AsTask()`获取`Task/Task<TResult>`实例，然后对该实例进行操作。并且，在之后的代码中您再也不应该与该`ValueTask/ValueTask<TResult>`进行交互。  
简单说就是使用`ValueTask/ValueTask<TResult>`时，您应该直接`await`它（可以有选择地加上`.ConfigureAwait(false)`），或直接调用`AsTask()`且再也不要使用它，例如：

highlighter- reasonml

// 以这个方法为例
public ValueTask<int\> SomeValueTaskReturningMethodAsync();
…
// GOOD
int result = await SomeValueTaskReturningMethodAsync();

// GOOD
int result = await SomeValueTaskReturningMethodAsync().ConfigureAwait(false);

// GOOD
Task<int\> t = SomeValueTaskReturningMethodAsync().AsTask();

// WARNING
ValueTask<int\> vt = SomeValueTaskReturningMethodAsync(); ... // 将实例存储到本地会使它被滥用的可能性更大,
    // 不过这还好，适当使用没啥问题

// BAD: await 多次
ValueTask<int\> vt = SomeValueTaskReturningMethodAsync();
int result = await vt;
int result2 = await vt;

// BAD: 并发 await (and, by definition then, multiple times)
ValueTask<int\> vt = SomeValueTaskReturningMethodAsync();
Task.Run(async () => await vt);
Task.Run(async () => await vt);

// BAD: 在不清楚操作是否完成的情况下使用 GetAwaiter().GetResult()
ValueTask<int\> vt = SomeValueTaskReturningMethodAsync();
int result = vt.GetAwaiter().GetResult(); 

另外，开发人员可以选择使用另一种高级模式，最好你在衡量后确定它可以带来好处之后再使用。具体来说，`ValueTask/ValueTask<TResult>`确实公开了一些与操作的当前状态有关的属性，例如：

- `IsCompleted`，如果操作尚未完成，则返回`false`；如果操作已完成，则返回`true`（这意味着该操作不再运行，并且可能已经成功完成或以其他方式完成）
- `IsCompletedSuccessfully`，当且仅当它已完成并成功完成才返回`true`（意味着尝试等待它或访问其结果不会导致引发异常）

举个例子，对于一些执行非常频繁的代码，想要避免在异步执行时进行额外的性能损耗，并在某个本质上会使`ValueTask/ValueTask<TResult>`不再使用的操作（如`await`、`.AsTask()`）时，可以先检查这些属性。例如，在 .NET Core 2.1的`SocketsHttpHandler`实现中，代码在连接上发出读操作，并返回一个`ValueTask<int>`实例。如果该操作同步完成，那么我们不用关注能否取消该操作。但是，如果它异步完成，在运行时就要发出取消请求，这样取消请求会将连接断开。由于这是一个非常常用的代码，并且通过分析表明这样做的确有细微差别，因此代码的结构基本上如下：

highlighter- reasonml

int bytesRead;
{
    ValueTask<int\> readTask = \_connection.ReadAsync(buffer);
    if (readTask.IsCompletedSuccessfully)
    {
        bytesRead = readTask.Result;
    }
    else
    {
        using (\_connection.RegisterCancellation())
        {
            bytesRead = await readTask;
        }
    }
} 

这种模式是可以接受的，因为在`ValueTask<int>`的`Result`被访问或自身被`await`之后，不会再被使用了。

## 新异步API都应返回ValueTask / ValueTask<TResult>吗？

**当然不是，`Task/Task<TResult>`仍然是默认选择**

正如上文所强调的那样，`Task/Task<TResult>`比`ValueTask/ValueTask<TResult>`更加容易正确使用，所以除非对性能的影响大于可用性的影响，否则`Task/Task<TResult>`仍然是最优的。

此外，返回`ValueTask<TResult>`会比返回`Task<TResult>`多一些小开销，例如，`await Task<TResult>`比`await ValueTask<TResult>`会更快一些，所以如果你可以使用缓存的Task实例（例如，你的API返回`Task`或`Task<bool>`），你或许应该为了更好地性能而仍使用`Task`和`Task<bool>`。而且，`ValueTask/ValueTask<TResult>`相比`Task/Task<TResult>`有更多的字段，所以当它们被`await`、并将它们的字段存储在调用异步方法的状态机中时，它们会在该状态机对象中占用更多的空间。

但是，如果是以下情况，那你应该使用`ValueTask/ValueTask<TResult>`：

1. 你希望API的调用者只能直接`await`它
2. 避免额外的内存分配的开销对API很重要
3. 你预期该API常常是同步完成的，或者在异步完成时你可以有效地池化对象。

在添加抽象、虚拟或接口方法时，您还需要考虑这些方法的重载/实现是否存在这些情况。

## ValueTask和ValueTask<TResult>的下一步是什么？

对于.NET Core库，我们将依然会看到新的API被添加进来，其返回值是`Task/Task<TResult>`，但在适当的地方，我们也将看到添加了新的以`ValueTask/ValueTask<TResult>`为返回值的API。

`ValueTask/ValueTask<TResult>`的一个关键例子就是在.NET Core 3.0添加新的`IAsyncEnumerator<T>`支持。`IEnumerator<T>`公开了一个返回`bool`的`MoveNext`方法，异步`IAsyncEnumerator<T>`则会公开一个`MoveNextAsync`方法。刚开始设计此功能时，我们认为`MoveNextAsync` 应返回`Task<bool>`，一般情况下，通过缓存的`Task<bool>`在同步完成时可以非常高效地执行此操作。但是，考虑到我们期望的异步枚举的广泛性，并且考虑到它们基于是基于接口的，其可能有许多不同的实现方式（其中一些可能会非常关注性能和内存分配），并且鉴于绝大多数的消费者将通过`await foreach`来使用，我们决定`MoveNextAsync`返回`ValueTask<bool>`。这样既可以使同步完成案例变得很快，又可以使用可重用的对象来使异步完成案例的内存分配也减少。实际上，在实现异步迭代器时，C＃编译器会利用此优势，以使异步迭代器尽可能免于额外内存分配。