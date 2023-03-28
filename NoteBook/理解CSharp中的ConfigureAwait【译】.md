**理解C#中的ConfigureAwait**

[toc]

> 转自 [理解C#中的ConfigureAwait](https://www.cnblogs.com/xiaoxiaotank/p/13529413.html) 

> 原文：[https://devblogs.microsoft.com/dotnet/configureawait-faq/](https://devblogs.microsoft.com/dotnet/configureawait-faq/)  
> 作者：Stephen  
> 翻译：xiaoxiaotank  
> 静下心来，你一定会有收获。

七年前（原文发布于2019年）.NET的编程语言和框架库添加了`async/await`语法糖。自那以后，它犹如星火燎原一般，不仅遍及整个.NET生态，还被许许多多的其他语言和框架所借鉴。当然，.NET也有很大改进，就拿对使用异步的语言结构上的补充来说，它提供了异步API支持，并对`async/await`的基础架构进行了根本改进（特别是 .NET Core中性能和可分析性的提升）。

然而，大家对`ConfigureAwait`的原理和使用仍然有一些困惑。接下来，我们会从`SynchronizationContext`开始讲起，然后过渡到`ConfigureAwait`，希望这篇文章能够为你解惑。废话少说，进入正文。

#### 什么是SynchronizationContext？

[System.Threading.SynchronizationContext](https://docs.microsoft.com/zh-cn/dotnet/api/system.threading.synchronizationcontext?view=netcore-3.1)的文档是这样说的：“提供在各种同步模型中传播同步上下文的基本功能”，太抽象了。

在99.9%的使用场景中，`SynchronizationContext`仅仅被当作一个提供虚（virtual）`Post`方法的类，该方法可以接收一个委托，然后异步执行它。虽然`SynchronizationContext`还有许多其他的虚成员，但是很少使用它们，而且和我们今天的内容无关，就不说了。`Post`方法的基础实现就仅仅是调用一下`ThreadPool.QueueUserWorkItem`，将接收的委托加入线程池队列去异步执行。

另外，派生类可以选择重写（override）`Post`方法，让委托在更加合适的位置和时间去执行。

例如，WinForm有一个[派生自SynchronizationContext的类](https://github.com/dotnet/winforms/blob/94ce4a2e52bf5d0d07d3d067297d60c8a17dc6b4/src/System.Windows.Forms/src/System/Windows/Forms/WindowsFormsSynchronizationContext.cs)，重写了`Post`方法，内部执行`Control.BeginInvoke`，这样，调用该`Post`方法就会在该控件的UI线程上执行接收的委托。WinForm依赖Win32的消息处理机制，并在UI线程上运行“消息循环”，该线程就是简单的等待新消息到达，然后去处理。这些消息可能是鼠标移动和点击、键盘输入、系统事件、可供调用的委托等。所以，只需要将委托传递给`SynchronizationContext`实例的`Post`方法，就可以在控件的UI线程中执行。

和WinForm一样，WPF也有一个[派生自SynchronizationContext的类](https://github.com/dotnet/wpf/blob/ac9d1b7a6b0ee7c44fd2875a1174b820b3940619/src/Microsoft.DotNet.Wpf/src/WindowsBase/System/Windows/Threading/DispatcherSynchronizationContext.cs)，重写了`Post`方法，通过`Dispatcher.BeginInvoke`将接收的委托封送到UI线程。与WinForm通过控件管理不同的是，WPF是由Dispatcher管理的。

Windows运行时（WinRT）也不例外，它有一个[派生自SynchronizationContext的类](https://github.com/dotnet/runtime/blob/60d1224ddd68d8ac0320f439bb60ac1f0e9cdb27/src/libraries/System.Runtime.WindowsRuntime/src/System/Threading/WindowsRuntimeSynchronizationContext.cs)，重写了`Post`方法，通过`CoreDispatcher`将接收的委托排队送到UI线程。

当然，不仅仅“在UI线程中执行该委托”这一种用法，任何人都可以重写`SynchronizationContext`的`Post`方法做任何事。例如，我可能不会关心委托在哪个线程上执行，但是我想确保任何在我自定义的`SynchronizationContext`实例中执行的任何委托都可以在一定的并发程度下执行。那么，我会实现这样一个自定义类：

csharp

internal sealed class MaxConcurrencySynchronizationContext : SynchronizationContext
{
    private readonly SemaphoreSlim \_semaphore;

    public MaxConcurrencySynchronizationContext(int maxConcurrencyLevel) =>
        \_semaphore = new SemaphoreSlim(maxConcurrencyLevel);

    public override void Post(SendOrPostCallback d, object state) =>
        \_semaphore.WaitAsync().ContinueWith(delegate
        {
            try 
            { 
                d(state); 
            } 
            finally 
            { 
                \_semaphore.Release(); 
            }
        }, default, TaskContinuationOptions.None, TaskScheduler.Default);

    public override void Send(SendOrPostCallback d, object state)
    {
        \_semaphore.Wait();
        try 
        { 
            d(state);
        } 
        finally 
        { 
            \_semaphore.Release();
        }
    }
} 

事实上，单元测试框架xunit就提供了一个[SynchronizationContext的派生类](https://github.com/xunit/xunit/blob/d81613bf752bb4b8774e9d4e77b2b62133b0d333/src/xunit.execution/Sdk/MaxConcurrencySyncContext.cs)，和我写的这个很类似，用于限制可以并发的测试相关的代码量。

与抽象的优点一样：它提供了一个API，可用于将委托排队进行处理，无需了解该实现的细节，这是实现者所期望的。所以，如果我正在编写一个库，想要停下来做一些工作，然后将委托排队送回“原始上下文”继续执行，那么我只需要获取他们的`SynchronizationContext`，存下来。当完成工作后，在该上下文上调用`Post`去传递我想要调用的委托即可。我不需在WinForm中知道要获取一个控件并调用`BeginInvoke`，不需要在WPF中知道要对`Dispatcher`进行`BeginInvoke`，也不需要在xunit中知道要以某种方式获取其上下文并排队，我只需要获取当前的`SynchronizationContext`并在以后使用它就可以了。为此，借助`SynchronizationContext`提供的`Current`属性，我可以编写如下代码来实现上述功能：

highlighter- typescript

public void DoWork(Action worker, Action completion)
{
    SynchronizationContext sc = SynchronizationContext.Current;
    ThreadPool.QueueUserWorkItem(\_ =>
    {
        try 
        { 
            worker();
        }
        finally 
        { 
            sc.Post(\_ => completion(), null); 
        }
    });
} 

如果框架想要通过`Current`公开自定义的上下文，可以使用`SynchronizationContext.SetSynchronizationContext`方法进行设置。

#### 什么是TaskScheduler？

`SynchronizationContext`是对“调度程序（scheduler）”的通用抽象。个别框架会有自己的抽象调度程序，比如`System.Threading.Tasks`。当Tasks通过委托的形式进行排队和执行时，会用到`System.Threading.Tasks.TaskScheduler`。和`SynchronizationContext`提供了一个`virtual Post`方法用于将委托排队调用一样（稍后，我们会通过典型的委托调用机制来调用委托），`TaskScheduler`也提供了一个`abstract QueueTask`方法（稍后，我们会通过`ExecuteTask`方法来调用该Task）。

通过`TaskScheduler.Default`我们可以获取到`Task`默认的调度程序`ThreadPoolTaskScheduler`——线程池（译注：这下知道为什么`Task`默认使用的是线程池线程了吧）。并且可以通过继承`TaskScheduler`来重写相关方法来实现在任意时间任意地点进行Task调用。例如，核心库中有个类，名为`System.Threading.Tasks.ConcurrentExclusiveSchedulerPair`，其实例公开了两个`TaskScheduler`属性，一个叫`ExclusiveScheduler`，另一个叫`ConcurrentScheduler`。调度给`ConcurrentScheduler`的任务可以并发，但是要在构造`ConcurrentExclusiveSchedulerPair`时就要指定最大并发数（类似于前面演示的`MaxConcurrencySynchronizationContext`）；相反，在`ExclusiveScheduler`执行任务时，那么将只允许运行一个排他任务，这个行为很像读写锁。

和`SynchronizationContext`一样，`TaskScheduler`也有一个`Current`属性，会返回当前调度程序。不过，和`SynchronizationContext`不同的是，它没有设置当前调度程序的方法，而是在启动Task时就要提供，因为当前调度程序是与当前运行的Task相关联的。所以，下方的示例程序会输出“`True`”，这是因为和`StartNew`一起使用的lambda表达式是在`ConcurrentExclusiveSchedulerPair`的`ExclusiveScheduler`上执行的（我们手动指定cesp.ExclusiveScheduler），并且`TaskScheduler.Current`也会指向该`ExclusiveScheduler`：

highlighter- arduino

using System;
using System.Threading.Tasks;

class Program
{
    static void Main() {
        var cesp = new ConcurrentExclusiveSchedulerPair();
        Task.Factory.StartNew(() =>
        {
            Console.WriteLine(TaskScheduler.Current == cesp.ExclusiveScheduler);
        }, default, TaskCreationOptions.None, cesp.ExclusiveScheduler)
        .Wait();
    }
} 

有趣的是，`TaskScheduler`提供了一个静态的`FromCurrentSynchronizationContext`方法，该方法会创建一个`SynchronizationContextTaskScheduler`实例并返回，以便在原始的`SynchronizationContext.Current`上的`Post`方法对任务进行排队执行。

#### SynchronizationContext和TaskScheduler是如何与await关联起来的呢？

假设有一个UI App，它有一个按钮。当点击按钮后，会从网上下载一些文本并将其设置为按钮的内容。我们应当只在UI线程中访问该按钮，因此当我们成功下载新的文本后，我们需要从拥有按钮控制权的的线程中将其设置为按钮的内容。如果不这样做的话，会得到一个这样的异常：

highlighter- scala

System.InvalidOperationException: 'The calling thread cannot access this object because a different thread owns it.' 

如果我们自己手动实现，那么可以使用前面所述的`SynchronizationContext`将按钮内容的设置传回原始上下文，例如借助`TaskScheduler`：

highlighter- typescript

private static readonly HttpClient s\_httpClient = new HttpClient();

private void downloadBtn\_Click(object sender, RoutedEventArgs e)
{
    s\_httpClient.GetStringAsync("http://example.com/currenttime").ContinueWith(downloadTask =>
    {
        downloadBtn.Content = downloadTask.Result;
    }, TaskScheduler.FromCurrentSynchronizationContext());
} 

或直接使用`SynchronizationContext`：

highlighter- typescript

private static readonly HttpClient s\_httpClient = new HttpClient();

private void downloadBtn\_Click(object sender, RoutedEventArgs e)
{
    SynchronizationContext sc = SynchronizationContext.Current;
    s\_httpClient.GetStringAsync("http://example.com/currenttime").ContinueWith(downloadTask =>
    {
        sc.Post(delegate
        {
            downloadBtn.Content = downloadTask.Result;
        }, null);
    });
} 

不过，这两种方式都需要显式指定回调，更好的方式是通过`async/await`自然地进行编码：

highlighter- typescript

private static readonly HttpClient s\_httpClient = new HttpClient();

private async void downloadBtn\_Click(object sender, RoutedEventArgs e)
{
    string text = await s\_httpClient.GetStringAsync("http://example.com/currenttime");
    downloadBtn.Content = text;
} 

就这样，成功在UI线程上设置了按钮的内容，与上面手动实现的版本一样，`await Task`默认会关注`SynchronizationContext.Current`和`TaskScheduler.Current`两个参数。当你在C#中使用`await`时，编译器会进行代码转换来向“可等待者”（这里为`Task`）索要（通过调用`GetAwaiter`）“awaiter”（这里为`TaskAwaiter<string>`）。该awaiter负责挂接回调（通常称为“继续（continuation）”），当等待的对象完成时，该回调将被封送到状态机，并使用在注册回调时捕获的上下文或调度程序来执行此回调。尽管与实际代码不完全相同（实际代码还进行了其他优化和调整），但大体上是这样的：

highlighter- pgsql

object scheduler = SynchronizationContext.Current;
if (scheduler is null && TaskScheduler.Current != TaskScheduler.Default)
{
    scheduler = TaskScheduler.Current;
} 

说人话就是，它先检查有没有设置当前`SynchronizationContext`，如果没有，则再判断当前调度程序是否为默认的`TaskScheduler`。如果不是，那么当准备好调用回调时，会使用该调度程序执行回调；否则，通常会作为完成已等待任务的操作的一部分来执行回调（译注：这个“否则”我也没看懂，我的理解是如果有当前上下文，则使用当前上下文执行回调；如果当前上下文为空，且使用的是默认调度程序`ThreadPoolTaskScheduler`，则会启用线程池线程执行回调）。

#### ConfigureAwait(false)做了什么？

`ConfigureAwait`方法并没有什么特别：编译器或运行时均不会以任何特殊方式对其进行标识。它仅仅是一个返回结构体（`ConfiguredTaskAwaitable`）的方法，该结构体包装了调用它的原始任务以及调用者指定的布尔值。注意，`await`可以用于任何正确模式的类型（而不仅仅是Task，在C#中只要类包含`GetAwaiter()` 方法和`bool IsCompleted`属性，并且`GetAwaiter()`的返回值包含 `GetResult()`方法、`bool IsCompleted`属性和实现了 `INotifyCompletion`接口，那么这个类的实例就是可以`await` 的）。当编译器访问实例的`GetAwaiter`方法（模式的一部分）时，它是根据`ConfigureAwait`返回的类型进行操作的，而不是直接使用Task，此外，还提供了一个钩子，用于通过该自定义awaiter更改`await`的行为。

具体来说，如果等待`ConfigureAwait(continueOnCapturedContext：false)`返回的类型`ConfiguredTaskAwaitable`，而非直接等待`Task`，最终会影响上面展示的捕获目标上下文或调度程序的逻辑。它使得上面展示的逻辑变成了这样：

highlighter- pgsql

object scheduler = null;
if (continueOnCapturedContext)
{
    scheduler = SynchronizationContext.Current;
    if (scheduler is null && TaskScheduler.Current != TaskScheduler.Default)
    {
        scheduler = TaskScheduler.Current;
    }
} 

换句话说，通过指定参数为`false`，即使有当前上下文或调度程序用于回调，它也会假装没有。

#### 我为什么要使用ConfigureAwait(false)？

`ConfigureAwait(continueOnCapturedContext: false)`用于避免强制在原始上下文或调度程序中进行回调，有以下好处：

**提升性能**

比起直接调用，排队进行回调会更加耗费性能，一个是因为会有一些额外的工作（一般是额外的内存分配），另一个是因为无法使用我们本来希望在运行时中采用的某些优化（当我们确切知道回调将如何调用时，我们可以进行更多优化，但如果将其移交给抽象的任意实现，则有时会受到限制）。对于大多数情况，即使检查当前的`SynchronizationContext`和`TaskScheduler`也可能会增加一定的开销（两者都会访问线程静态变量）。如果`await`之后的代码并不需要在原始上下文中运行，那么使用`ConfigureAwait(false)`就可以避免上述花销：它不用排队，且可以利用所有可以进行的优化，还可以避免不必要的线程静态访问。

**避免死锁**

假如有一个方法，使用`await`等待网络下载结果，你需要通过同步阻塞的方式调用该方法等待其完成，比如使用`.Wait()`、`.Result`或`.GetAwaiter().GetResult()`。

思考一下，如果限制当前`SynchronizationContext`并发数为1，会发生什么情况？方式不限，无论是显式地通过类似于前面所说的`MaxConcurrencySynchronizationContext`的方式，还是隐式地通过仅具有一个可以使用的线程的上下文来实现，例如UI线程，你都可以在那个线程上调用该方法并阻塞它等待操作完成，该操作将开启网络下载并等待。在默认情况下， 等待`Task`会捕获当前`SynchronizationContext`，所以，当网络下载完成时，它会将回调排队返回到`SynchronizationContext`中执行剩下的操作。但是，当前唯一可以处理排队回调的线程却还被你阻塞着等待操作完成，不幸的是，在回调处理完毕之前，该操作永远不会完成。完蛋，死锁了！

即使不将上下文并发数限制为1，而是通过其他任何方式对资源进行了限制，结果也是如此。比如，我们将`MaxConcurrencySynchronizationContext`限制为4，这时，我们对该上下文进行4次排队调用，每个调用都会进行阻塞等待操作完成。现在，我们在等待异步方法完成时仍阻塞了所有资源，这些异步方法能否完成取决于是否可以在已经完全消耗掉的上下文中处理它们的回调。哦吼，又死锁了！

如果该方法改为使用`ConfigureAwait(false)`，那么它就不会将回调排队送回原始上下文，进而避免了死锁。

#### 我为什么要使用ConfigureAwait(true)？

**绝对没必要使用**，除非你闲的蛋疼使用它来表明你是故意不使用`ConfigureAwait(false)`的（例如消除VS的静态分析警告或类似的警告等），使用`ConfigureAwait(true)`没有任何意义。`await task`和`await task.ConfigureAwait(true)`在功能上没有任何区别，如果你在生产环境的代码中发现了`ConfigureAwait(true)`，那么你可以直接删除它，不会有任何副作用。

`ConfigureAwait`方法接收一个布尔值参数，可能在某些特殊情况下，你需要通过传入变量来控制配置，不过，99%的情况下都是通过硬编码的方式传入的，如`ConfigureAwait(false)`

#### 什么时候应该使用ConfigureAwait(false)？

这取决于：你在实现应用程序级代码还是通用库代码？

当你编写应用程序时，你通常需要使用默认行为（这就是`ConfigureAwait(true)`是默认行为的原因（译注：原作者应该是想要表达编写应用程序比通用库更加频繁，所以该行为会更频繁的使用））。如果应用模型或环境（例如WinForm，WPF，ASP.NET Core等）发布了自定义`SynchronizationContext`，那么基本上可以肯定有一个很好的理由：它为关注同步上下文的代码提供了一种与应用模型或环境适当交互的方式。所以如果你使用WinForm写事件处理器、在xunit中写单元测试或在ASP .NET MVC控制器中编码，无论应用程序模型是否确实发布了`SynchronizationContext`，您都想使用该`SynchronizationContext`（如果存在），那么您可以简单地`await`默认的`ConfigureAwait(true)`，如果存在回调，就可以将其正确地封送到原始上下文中执行。这就形成了以下一般指导：**如果您正在编写应用程序级代码，请不要使用`ConfigureAwait(false)`**。如果您回想一下本文前面的Click事件处理程序代码示例：

highlighter- typescript

private static readonly HttpClient s\_httpClient = new HttpClient();

private async void downloadBtn\_Click(object sender, RoutedEventArgs e)
{
    string text = await s\_httpClient.GetStringAsync("http://example.com/currenttime");
    downloadBtn.Content = text;
} 

代码`downloadBtn.Content = text`需要在原始上下文中执行，但如果代码违反了该准则，在错误的情况下使用了`ConfigureAwait(false)`：

highlighter- typescript

private static readonly HttpClient s\_httpClient = new HttpClient();

private async void downloadBtn\_Click(object sender, RoutedEventArgs e)
{
    string text = await s\_httpClient.GetStringAsync("http://example.com/currenttime")
        .ConfigureAwait(false);     // bug
    downloadBtn.Content = text;
} 

这将导致出现错误的结果。依赖于`HttpContext.Current`的经典ASP.NET应用程序中的代码也是如此，使用`ConfigureAwait(false)`然后尝试使用`HttpContext.Current`也可能会导致问题。

相反，通用库之所以成为“通用库”，原因之一是因为它们不关心使用它们的环境。您可以在Web应用程序、客户端应用程序或测试程序中使用它们，这无关紧要，因为库代码与可能使用的应用程序模型无关。那么，无关就意味着它不会做任何需要以特定方式与应用程序模型进行交互的事情，例如：它不会访问UI控件，因为通用库对UI控件一无所知。由于我们不需要在任何特定环境中运行代码，那么我们可以避免将回调强制送回到原始上下文，这可以通过使用`ConfigureAwait(false)`来实现，并享受到其带来的性能和可靠性优势。这形成了以下一般指导：**如果要编写通用库代码，请使用`ConfigureAwait(false)`**。这就是为什么您会在`.NET Core`运行时库中看到每个（或几乎每个）`await`时都要使用`ConfigureAwait(false)`的原因；如果不是这样的话（除了少数例外），那很可能是一个要修复的BUG。例如，[此Pull request](https://github.com/dotnet/corefx/pull/38610)修复了`HttpClient`中缺少的`ConfigureAwait(false)`调用。

当然，与其他指导一样，在某些特殊的情况下可能不适用。例如，在通用库中，具有可调用委托的API是一个较大的例外（或至少需要考虑的例外）。在这种情况下，库的调用者可能会传递由库调用的应用程序级代码，然后有效地呈现了库那些“通用”假设。例如，以LINQ中`Where`的异步版本（运行时库不存在该方法，仅仅是假设）为例：`public static async IAsyncEnumerable<T> WhereAsync(this IAsyncEnumerable<T> source, Func<T, bool> predicate)`。这里的`predicate`是否需要在调用者的原始`SynchronizationContext`上重新调用？这要取决于`WhereAsync`的实现，因此，它可能选择不使用`ConfigureAwait(false)`。

即使有这些特殊情况，一般指导仍然是一个很好的起点：如果要编写通用库或与应用程序模型无关的代码，请使用`ConfigureAwait(false)`，否则请不要这样做。

#### 以下是一些常见问题

##### ConfigureAwait(false)能保证回调不会在原始上下文中运行吗？

**并不能保证**！它虽能保证它不会被排队回到原始上下文中……但这并不意味着`await task.ConfigureAwait(false)`后的代码仍不会在原始上下文中运行。因为当等待已经完成的可等待对象时（即Task实例返回时该Task已经完成了），后续代码将会保持同步运行，而无需强制排队等待。所以，如果您等待的任务在等待时就已经完成了，那么无论您是否使用了`ConfigureAwait(false)`，紧随其后的代码也会在拥有当前上下文的当前线程上继续执行。

##### 我的方法中仅在第一次`await`时使用`ConfigureAwait(false)`而剩下的代码不使用可以吗？

一般来说，不行，参考前面的FAQ。如果`await task.ConfigureAwait(false)`在等待时就已完成了（实际上很常见），那么`ConfigureAwait(false)`将毫无意义，因为线程在此之后继续在该方法中执行代码，并且仍在与之前相同的上下文中执行。

有一个例外是：如果您知道第一次等待始终会异步完成，并且正在等待的事物会在没有自定义`SynchronizationContext`或`TaskScheduler`的环境中调用其回调。例如，.NET运行时库中的`CryptoStream`希望确保其潜在的计算密集型代码不会被调用者以同步方式进行调用，因此它使用[自定义的`awaiter`](https://github.com/dotnet/runtime/blob/4f9ae42d861fcb4be2fcd5d3d55d5f227d30e723/src/libraries/System.Security.Cryptography.Primitives/src/System/Security/Cryptography/CryptoStream.cs#L205)来确保第一次等待后的所有内容都在线程池线程上运行。但是，即使在这种情况下，您也会注意到下一次等待仍将使用`ConfigureAwait(false)`;从技术上讲，使用`ConfigureAwait(false)`不是必需的，但是它使代码审查变得很容易，这样每次查看该块代码时，就无需分析一番来了解为什么取消`ConfigureAwait(false)`。

##### 我可以使用`Task.Run`来避免使用`ConfigureAwait(false)`吗？

是的，你可以这样写：

highlighter- csharp

Task.Run(async delegate
{
    await SomethingAsync(); // 不会找到原始上下文
}); 

没有必要对`SomethingAsync`调用`ConfigureAwait(false)`，因为传递给`Task.Run`的委托将运行在线程池线程上，堆栈上没有更高级别的用户代码，因此`SynchronizationContext.Current`将返回`null`。此外，`Task.Run`隐式使用`TaskScheduler.Default`，所以`TaskScheduler.Current`也会指向该`Default`。也就是说，无论是否使用`ConfigureAwait(false)`，`await`都会做出相同的行为。它也不能保证此Lambda内的代码可以做什么。如果您写了这样一段代码：

highlighter- csharp

Task.Run(async delegate
{
    SynchronizationContext.SetSynchronizationContext(new SomeCoolSyncCtx());
    await SomethingAsync(); // will target SomeCoolSyncCtx
}); 

那么在`SomethingAsync`内部你会发现`SynchronizationContext.Current`就是`SomeCoolSyncCtx`实例，并且该`await`和`SomethingAsync`内部的所有未配置的`await`都将返回到该上下文。因此，要使用这种方式，您需要了解排队的所有代码可能会做什么或不做什么，以及它的行为是否会阻碍您的行为。

这种方法还需要以创建或排队其他任务对象为代价。这取决于您的性能敏感性，对您的应用程序或库而言可能无关紧要。

另外要注意，这些技巧可能会引起更多的问题，并带来其他意想不到的后果。例如，静态分析工具（例如Roslyn分析仪）提供了标记不使用`ConfigureAwait(false)`的标志等待，正如[CA2007](https://docs.microsoft.com/en-us/visualstudio/code-quality/ca2007?view=vs-2019)。如果启用了这样的分析器，并采用该技巧来避免使用`ConfigureAwait`，那么分析器很有可能会标记它，这其实会给您带来更多工作。那么，也许您可能会因为其烦扰而禁用了分析器，这将会导致您忽略代码库中实际上应该一直使用`ConfigureAwait(false)`的其他代码。

##### 我能用SynchronizationContext.SetSynchronizationContext来避免使用ConfigureAwait(false)吗？

**不行！** 额。。好吧，也许可以。这取决于你写的代码。可能一些开发者这样写：

highlighter- reasonml

Task t;
var old = SynchronizationContext.Current;
SynchronizationContext.SetSynchronizationContext(null);
try
{
    t = CallCodeThatUsesAwaitAsync(); // 在方法内部进行 await 不会感知到原始上下文
}
finally 
{
    SynchronizationContext.SetSynchronizationContext(old); 
}
await t; // 这时则会回到原始上下文 

我们希望`CallCodeThatUsesAwaitAsync`中的代码看到的当前上下文是null，而且确实如此。但是，以上内容不会影响`TaskScheduler`的等待状态，因此，如果此代码在某些自定义`TaskScheduler`上运行，那么在`CallCodeThatUsesAwaitAsync`（不使用`ConfigureAwait(false)`）内部等待后仍将排队返回该自定义`TaskScheduler`。

所有这些注意事项也适用于前面`Task.Run`相关的FAQ：这种解决方法可能会带来一些性能方面的问题，并且try中的代码也可以通过设置其他上下文（或使用非默认TaskScheduler来调用代码）来阻止这种尝试。

使用这种模式，您还需要注意一些细微的变化：

highlighter- csharp

var old = SynchronizationContext.Current;
SynchronizationContext.SetSynchronizationContext(null);
try
{
    await t;
}
finally 
{ 
    SynchronizationContext.SetSynchronizationContext(old);
} 

找到问题没？可能很难发现但是影响很大。这样写没法保证`await`最终会回到原始线程上执行回调并继续执行生下的代码，也就是说将`SynchronizationContext`重置回原始上下文这个操作可能实际上并未在原始线程上进行，这可能导致该线程上的后续工作项看到错误的上下文（为解决这一问题，具有良好编码规范的应用模型在设置了自定义上下文时，通常会在调用任何其他用户代码之前添加代码以手动将其重置）。而且即使它确实在同一线程上运行，也可能要等一会儿，这样一来，上下文仍无法适当恢复。而且，如果它在其他线程上运行，可能最终会在该线程上设置错误的上下文。等等。很不理想。

##### 如果我用了`GetAwaiter().GetResult()`，我还需要使用`ConfigureAwait(false)`吗？

**不需要**，`ConfigureAwait`只影响回调。具体来说，awaiter模式要求awaiters 公开`IsCompleted`属性、`GetResult`方法和`OnCompleted`方法（可选使用`UnsafeOnCompleted`方法）。`ConfigureAwait`只会影响`OnCompleted/UnsafeOnCompleted`的行为，因此，如果您只是直接调用等待者的`GetResult()`方法，那么你无论是在`TaskAwaiter`上还是在`ConfiguredTaskAwaitable.ConfiguredTaskAwaiter`上进行操作，都是没有任何区别的。因此，如果在代码中看到`task.ConfigureAwait(false).GetAwaiter().GetResult()`，则可以将其替换为`task.GetAwaiter().GetResult()`（并考虑是否真的需要这样的阻塞）。

##### 我知道我的运行环境永远不会具有自定义SynchronizationContext或自定义TaskScheduler

我可以跳过使用ConfigureAwait(false)吗？  
**也许可以，这取决于你是如何确定“永远不会”的。** 如之前的FAQ，仅仅因为您正在使用的应用程序模型未设置自定义`SynchronizationContext`且未在自定义`TaskScheduler`上调用您的代码并不意味着其他用户或库代码未设置。因此，您需要确保不存在这种情况，或至少要意识到这种风险。

##### 我听说在 .NET Core中ConfigureAwait(false)已经不再需要了，这是真的吗？

**假的！** 在.NET Core上运行时仍需要使用它，和在.NET Framework上运行时需要使用的原因完全相同，在这方面没有任何改变。

不过，有一些变化的是某些环境是否发布了自己的`SynchronizationContext`。特别是虽然在.NET Framework上的经典ASP.NET具有自己的`SynchronizationContext`，但是ASP.NET Core却没有。这意味着默认情况下，在ASP.NET Core应用程序中运行的代码是看不到自定义`SynchronizationContext`的，从而减少了在这种环境中运行`ConfigureAwait(false)`的需要。

但这并不意味着永远不会存在自定义的`SynchronizationContext`或`TaskScheduler`。如果某些用户代码（或您的应用程序正在使用的其他库代码）设置了自定义上下文并调用了您的代码，或在自定义`TaskScheduler`的预定`Task`中调用您的代码，那么即使在ASP.NET Core中，您的等待对象也可能会看到非默认上下文或调度程序，从而促使您想要使用`ConfigureAwait(false)`。当然，在这种情况下，如果您想要避免同步阻塞（任何情况下，都应避免在Web应用程序中进行同步阻塞），并且不介意在这种有限的情况下有细微的性能开销，那您可能无需使用`ConfigureAwait(false)`就可以实现。

##### 我在await using一个IAsyncDisposable的对象时我可以使用ConfigureAwait吗？

**可以，不过有些小问题。** 与前面的FAQ中所述的`IAsyncEnumerable<T>`一样，.NET运行时公开了一个`IAsyncDisposable`的扩展方法`ConfigureAwait` 的扩展方法，并且`await using`能很好地与此一起工作，因为它实现了适当的模式（即公开了适当的DisposeAsync方法）：

highlighter- csharp

await using (var c = new MyAsyncDisposableClass().ConfigureAwait(false))
{
    ...
} 

这里的问题是，变量c的类型现在不是`MyAsyncDisposableClass`，而是`System.Runtime.CompilerServices.ConfiguredAsyncDisposable`，这是从`IAsyncDisposable`上的`ConfigureAwait`扩展方法返回的类型。

为了解决这个问题，您需要多写一行：

highlighter- csharp

var c = new MyAsyncDisposableClass();
await using (c.ConfigureAwait(false))
{
    ...
} 

现在，变量c的类型又是所需的`MyAsyncDisposableClass`了。这还具有增加c范围的作用；如果有影响，则可以将整个内容括在大括号中。

##### 我使用了ConfigureAwait(false)，但是我的AsyncLocal在等待之后仍然流向代码，那是个BUG吗？

**不，这是预期的。** `AsyncLocal<T>`数据流是`ExecutionContext`的一部分，它与`SynchronizationContext`是相互独立的。除非您使用`ExecutionContext.SuppressFlow()`明确禁用了`ExecutionContext`流，否则`ExecutionContext`（以及`AsyncLocal<T>`数据）将始终在等待状态中流动，无论是否使用`ConfigureAwait`来避免捕获原始的`SynchronizationContext`。有关更多信息，请参见此[博客](https://devblogs.microsoft.com/pfxteam/executioncontext-vs-synchronizationcontext/)。

##### 可以在语言层面帮助我避免在我的库中显式使用ConfigureAwait(false)吗？

类库开发人员有时会对需要使用`ConfigureAwait(false)`而感到沮丧，并想要使用侵入性较小的替代方法。

目前还没有，至少没有内置在语言、编译器或运行时中。不过，对于这种解决方案可能是什么样的，有许多建议，比如：

[https://github.com/dotnet/csharplang/issues/645](https://github.com/dotnet/csharplang/issues/645)

[https://github.com/dotnet/csharplang/issues/2542](https://github.com/dotnet/csharplang/issues/2542)

[https://github.com/dotnet/csharplang/issues/2649](https://github.com/dotnet/csharplang/issues/2649)

[https://github.com/dotnet/csharplang/issues/2746](https://github.com/dotnet/csharplang/issues/2746)

如果这对您很重要，或者您有新的有趣的想法，我鼓励您为这些或新的讨论贡献自己的想法。