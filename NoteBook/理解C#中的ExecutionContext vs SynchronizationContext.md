**理解C#中的ExecutionContext vs SynchronizationContext **

[toc]

> 原文：[https://devblogs.microsoft.com/pfxteam/executioncontext-vs-synchronizationcontext/](https://devblogs.microsoft.com/pfxteam/executioncontext-vs-synchronizationcontext/)  
> 作者：Stephen  
> 翻译：xiaoxiaotank  
> 不来深入了解一下？

为了更好的理解本文内容，强烈建议先看一下[理解C#中的ConfigureAwait](https://www.cnblogs.com/xiaoxiaotank/p/13529413.html)。

虽然原文发布于2012年，但是内容放到今日仍不过时。好，开始吧！

最近，有人问了我几个关于`ExecutionContext`和`SynchronizationContext`的问题，例如：它们俩有什么区别？“流动”它们有什么意义？它们与C＃和VB中新的`async/await`语法糖有什么关系？我想通过本文来解决其中一些问题。

_注意：本文深入到了.NET的高级领域，大多数开发人员都无需关注。_

#### 什么是ExecutionContext，流动它有什么意义？

对于绝大多数开发者来说，不需要关注`ExecutionContext`。它的存在就像空气一样：虽然它很重要，但我们一般是不会关注它的，除非有必要（例如出现问题时）。`ExecutionContext`本质上只是一个用于盛放其他上下文的容器。这些被盛放的上下文中有一些仅仅是辅助性的，而另一些则对于.NET的执行模型至关重要，不过它们都和`ExecutionContext`一样：除非你不得不知道他们存在，或你正在做某些特别高级的事情，或者出了什么问题（，否则你没必要关注它）。

`ExecutionContext`是与“环境”信息相关的，也就是说它会存储与你当前正在运行的环境或“上下文”相关的数据。在许多系统中，这类环境信息使用线程本地存储（TLS）来维护，例如`ThreadStatic`标记的字段或`ThreadLocal<T>`。在同步的世界里，这种线程本地信息就足够了：所有的一切都运行在该线程上，因此，无论你在该线程上使用什么栈帧、正在执行什么功能，等等，在该线程上运行的所有代码都可以查看并受该线程特定数据的影响。例如，`ExecutionContext`盛放的一个上下文叫做`SecurityContext`，它维护了诸如当前“principal”之类的信息以及有关代码访问安全性（CAS）拒绝和允许的信息。这类信息可以与当前线程相关联，这样的话，如果一个栈帧的访问被某个权限拒绝了然后调用另一个方法，那么该调用的方法仍会被拒绝：当尝试执行需要该权限的操作时，CLR会检查当前线程是否允许该操作，并且它也会找到调用者放入的数据。

当从同步世界过渡到异步世界时，事情就变得复杂了起来。突然之间，TLS变得无关紧要。在同步的世界里，如果我先执行操作A，然后再执行操作B，最后执行操作C，那么这三个操作都会在同一线程上执行，所以这三个操作都会受该线程上存储的环境数据的影响。但是在异步的世界里，我可能在一个线程上启动A，然后在另一个线程上完成它，这样操作B就可以在不同于A的线程上启动或运行，并且类似地C也可以在不同于B的线程上启动或运行。 这意味着我们用来控制执行细节的环境不再可行，因为TLS不会在这些异步点上“流动”。线程本地存储特定于某个线程，而这些异步操作并不与特定线程绑定。不过，我们希望有一个逻辑控制流，且环境数据可以与该控制流一起流动，以便环境数据可以从一个线程移动到另一个线程。这就是`ExecutionContext`发挥的作用。

`ExecutionContext`实际上只是一个状态包，可用于从一个线程捕获所有当前状态，然后在控制逻辑继续流动的同时将其还原到另一个线程。通过静态`Capture`方法来捕获`ExecutionContext`：

highlighter- awk

// 把环境状态捕捉到ec中
ExecutionContext ec = ExecutionContext.Capture(); 

在调用委托时，通过静态`Run`方法将环境状态还原回来：

highlighter- axapta

ExecutionContext.Run(ec, delegate
{
    … // 此处的代码会将ec的状态视为环境
}, null); 

.NET Framework中所有异步工作的方法都是以这种方式捕获和还原`ExecutionContext`的（除了那些以“Unsafe”为前缀的方法，这些方法都是不安全的，因为它们显式的不流动`ExecutionContext`）。例如，当你使用`Task.Run`时，对`Run`的调用会导致捕获调用线程的`ExecutionContext`，并将该`ExecutionContext`实例存储到`Task`对象中。稍后，当传递给`Task.Run`的委托作为该`Task`执行的一部分被调用时，会通过调用`ExecutionContext.Run`方法，使委托在刚才存储的上下文中执行。`Task.Run`、`ThreadPool.QueueUserWorkItem`、`Delegate.BeginInvoke`、`Stream.BeginRead`、`DispatcherSynchronizationContext.Post`，以及你可以想到的任何其他异步API，都是这样的。它们全都会捕获`ExecutionContext`，存储起来，然后在调用某些代码时使用它。

当我们讨论“流动`ExecutionContext`”时，指的就是这个过程，即获取一个线程上的环境状态，然后在执行传递的委托时，将状态还原到执行线程上。

#### 什么是SynchronizationContext，捕获和使用它有什么意义？

在软件开发中，我们喜欢抽象。我们几乎不会愿意对特定的实现进行硬编码，相反，在编写大型系统时，我们更原意将特定实现的细节抽象化，以便以后可以插入其他实现，而不必更改我们的大型系统。这就是我们有接口、抽象类，虚方法等的原因。

`SynchronizationContext`只是一种抽象，代表你要执行某些操作的特定环境。举个例子，WinForm拥有UI线程（虽然可能有多个，但出于讨论目的，这并不重要），需要使用UI控件的任何操作都需要在上面执行。为了处理需要先在线程池线程上执行然后再封送回UI线程，以便该操作可以与UI控件一起处理的情形，WinForm提供了`Control.BeginInvoke`方法。你可以向控件的`BeginInvoke`方法传递一个委托，该委托将在与该控件关联的线程上被调用。

因此，如果我正在编写一个需要在线程池线程执行一部分工作，然后在UI线程上再进行一部分工作的组件，那我可以使用`Control.BeginInvoke`。但是，如果我现在要在WPF应用程序中使用我的组件该怎么办？WPF具有与WinForm相同的UI线程约束，但封送回UI线程的机制不同：不是通过`Control.BeginInvoke`，而是在`Dispatcher`实例上调用`Dispatcher.BeginInvoke`（或`InvokeAsync`）。

现在，我们有两个不同的API用于实现相同的基本操作，那么如何编写与UI框架无关的组件呢？当然是通过使用`SynchronizationContext`。`SynchronizationContext`提供了一个虚`Post`方法，该方法只接收一个委托，并在任何地点，任何时间运行它，当然`SynchronizationContext`的实现要认为是合适的。WinForm提供了`WindowsFormSynchronizationContext`类，该类重写了`Post`方法来调用`Control.BeginInvoke`。WPF提供了`DispatcherSynchronizationContext`类，该类重写`Post`方法来调用`Dispatcher.BeginInvoke`，等等。这样，我现在可以在组件中使用`SynchronizationContext`，而不需要将其绑定到特定框架。

如果我要专门针对WinForm编写组件，则可以像这样来实现先进入线程池，然后返回到UI线程的逻辑：

highlighter- csharp

public static void DoWork(Control c)
{
    ThreadPool.QueueUserWorkItem(delegate
    {
        … // 在线程池中执行
        
        c.BeginInvoke(delegate
        {
            … // 在UI线程中执行
        });
    });
} 

如果我把组件改成使用`SynchronizationContext`，就可以这样写：

highlighter- csharp

public static void DoWork(SynchronizationContext sc)
{
    ThreadPool.QueueUserWorkItem(delegate
    {
        … // 在线程池中执行
        
        sc.Post(delegate
        {
            … // 在UI线程中执行
        }, null);
    });
} 

当然，需要传递目标上下文（即sc）来返回显得很烦人（对于某些所需的编程模型而言，这是无法容忍的），因此，`SynchronizationContext`提供了`Current`属性，该属性使你可以从当前线程中寻找上下文，如果存在的话，它会把你返回到该环境。你可以这样“捕获”它（即从`SynchronizationContext.Current`中读取引用，并存储该引用以供以后使用）：

highlighter- csharp

public static void DoWork()
{
    var sc = SynchronizationContext.Current;
    ThreadPool.QueueUserWorkItem(delegate
    {
        … // 在线程池中执行
        
        sc.Post(delegate
        {
            … // 在原始上下文中执行
        }, null);
   });
} 

#### 流动ExecutionContext vs 使用SynchronizationContext

现在，我们有一个非常重要的发现：流动`ExecutionContext`在语义上与捕获`SynchronizationContext`并Post完全不同。

当流动`ExecutionContext`时，你是从一个线程中捕获状态，然后在提供的委托执行期间将该状态恢复回来。而你捕获并使用`SynchronizationContext`时，不会出现这种情况。捕获部分是相同的，因为你要从当前线程中获取数据，但是后续使用状态的方式不同。`SynchronizationContext`是通过`SynchronizationContext.Post`来使用捕获的状态调用委托，而不是在委托调用期间将状态恢复为当前状态。该委托在何时何地以及如何运行完全取决于`Post`方法的实现。

#### 这是如何运用于async/await的？

`async`和`await`关键字背后的框架支持自动与`ExecutionContext`和`SynchronizationContext`交互。  
每当代码等待一个awaiter，awaiter说它尚未完成（例如`awaiter.IsCompleted`返回`false`）时，该方法需要暂停，然后通过awaiter的延续（Continuation）来恢复，这是我之前提到的异步点之一。因此，`ExecutionContext`需要从发出等待的代码一直流动到延续委托的执行，这会由框架自动处理。当异步方法即将挂起时，基础架构会捕获`ExecutionContext`。传递给awaiter的委托会拥有该`ExecutionContext`实例的引用，并在恢复该方法时使用它。这就是使`ExecutionContext`表示的重要“环境”信息跨等待流动的原因。

该框架还支持`SynchronizationContext`。前面对`ExecutionContext`的支持内置于表示异步方法的“构建器”中（例如`System.Runtime.CompilerServices.AsyncTaskMethodBuilder`），并且这些构建器可确保`ExecutionContext`跨等待点流动，而不管使用哪种等待方式。相反，对`SynchronizationContext`的支持已内置在等待`Task`和`Task <TResult>`的支持中。自定义awaiter可以自己添加类似的逻辑，但不会自动获取。这是设计使然，因为自定义何时以及后续如何调用是自定义awaiter使用的原因之一。

默认情况下，当你等待Task时，awaiter将捕获当前的`SynchronizationContext`，当Task完成时，会将提供的延续（Continuation）委托封送到该上下文去执行，而不是在任务完成的线程上，或在`ThreadPool`上执行该委托。如果开发人员不希望这种封送行为，则可以通过更改使用的awaiter来进行控制。虽然在等待`Task`或`Task <TResult>`时始终会采用这种行为，但你可以改为等待`task.ConfigureAwait(…)`。`ConfigureAwait`方法返回一个awaitable，它可以阻止默认的封送处理行为。是否阻止由传递给`ConfigureAwait`方法的布尔值控制。如果continueOnCapturedContext为`true`，就是默认行为；否则，如果为`false`，那么awaiter不会检查`SynchronizationContext`，假装好像没有一样。（注意，待完成的Task完成后，无论`ConfigureAwait`如何，运行时（runtime）可能会检查正在恢复的线程上的当前上下文，以确定是否可以在此处同步运行延续，或必须从那时开始异步安排延续。）

注意，尽管`ConfigureAwait`为更改与`SynchronizationContext`相关的行为提供了显式的与等待相关的编程模型支持，但没有用于阻止`ExecutionContext`流动的与等待相关的编程模型支持，就是故意这样设计的。开发人员在编写异步代码时无需关注`ExecutionContext`。它在基础架构级别的支持，可帮助你在异步环境中模拟同步语义（即TLS）。大多数人可以并且应该完全忽略它的存在（除非他们真的知道自己在做什么，否则应避免使用`ExecutionContext.SuppressFlow`方法）。相反，开发人员应该意识到代码在哪里运行，因此`SynchronizationContext`上升到了值得显式编程模型支持的水平。（实际上，正如我在其他文章中所述，大多数类库开发者都应考虑在每次Task等待时使用`ConfigureAwait(false)`。）

#### SynchronizationContext不是ExecutionContext的一部分吗？

到目前为止，我掩盖了一些细节，但是我还是没法避免它们。

我掩盖的主要内容是`ExecutionContext`能够流动的所有上下文（例如`SecurityContext`，`HostExecutionContext`，`CallContext`等），`SynchronizationContext`实际上就是其中之一。我个人认为，这是API设计中的一个错误，这是自许多版本的.NET首次提出以来引起的一些问题。不过，这是我们已经使用了很长时间的设计，如果现在进行更改那将是一项中断性更改。

当你调用公共的`ExecutionContext.Capture()`方法时，该方法将检查当前的`SynchronizationContext`，如果有，则将其存储到返回的`ExecutionContext`实例中。然后，当你使用公共的`ExecutionContext.Run`方法时，在执行提供的委托期间，捕获的`SynchronizationContext`会被恢复为`Current`。

这有什么问题？作为`ExecutionContext`的一部分流动的`SynchronizationContext`更改了`SynchronizationContext.Current`的含义。`SynchronizationContext.Current`应该可以使你返回到访问`Current`时所处的环境，因此，如果`SynchronizationContext`流到了另一个线程上成为`Current`，那么你就无法信任`SynchronizationContext.Current`的含义。在这种情况下，它可能用于返回到当前环境，也可能用于回到流中先前某个时刻所处的环境。（译注：一定要看到文章末尾，否则你可能会产生误解）

举一个可能出现这种问题的例子，请参考以下代码：

highlighter- csharp

private void button1\_Click(object sender, EventArgs e)
{
    button1.Text = await Task.Run(async delegate
    {
        string data = await DownloadAsync();
        return Compute(data);
    });
} 

我的思维模式告诉我，这段代码会发生这种情况：用户单击button1，导致UI框架在UI线程上调用button1\_Click。然后，代码启动一个在`ThreadPool`上运行的操作（通过Task.Run）。该操作将开始一些下载工作，并异步等待其完成。然后，`ThreadPool`上的延续操作会对下载的结果进行一些计算密集型操作，并返回结果，最终使正在UI线程上等待的Task完成。接着，UI线程会处理该button1\_Click方法的其余部分，并将计算结果存储到button1的Text属性中。

如果`SynchronizationContext`不会作为`ExecutionContext`的一部分流动，那么这是我所期望的。但是，如果流动了，我会感到非常失望。`Task.Run`会在调用时捕获`ExecutionContext`，并使用它来执行传递给它的委托。这意味着调用`Task.Run`时所处的UI线程的`SynchronizationContext`将流入Task，并且在`await DownloadAsync`时再次作为`Current`流入。这意味着await将会找到UI的`SynchronizationContext.Current`，并`Post`该方法的剩余部分作为在UI线程上运行的延续。也就表示我的`Compute`方法很可能会在UI线程上运行，而不是在`ThreadPool`上运行，从而导致我的应用程序出现响应问题。

现在，这个故事有点混乱了：`ExecutionContext`实际上有两个`Capture`方法，但是只公开了一个。mscorlib公开的大多数异步功能所使用的是内部的（mscorlib内部的）`Capture`方法，并且它可选地允许调用方阻止捕获`SynchronizationContext`作为`ExecutionContext`的一部分；对应于`Run`方法的内部重载也支持忽略存储在`ExecutionContext`中的`SynchronizationContext`，实际上是假装没有被捕获（同样，这是mscorlib中大多数功能使用的重载）。这意味着几乎所有在mscorlib中的异步操作的核心实现都不会将`SynchronizationContext`作为`ExecutionContext`的一部分进行流动，但是在其他任何地方的任何异步操作的核心实现都将捕获`SynchronizationContext`作为`ExecutionContext`的一部分。我上面提到了，异步方法的“构建器”是负责在异步方法中流动`ExecutionContext`的类型，这些构建器是存在于mscorlib中的，并且使用的确实是内部重载……（当然，这与task awaiter捕获`SynchronizationContext`并将其Post回去是互不影响的）。为了处理`ExecutionContext`确实流动了`SynchronizationContext`的情况，异步方法基础结构会尝试忽略由于流动而设置为`Current`的`SynchronizationContexts`。

简而言之，`SynchronizationContext.Current`不会在等待点之间“流动”，你放心好了。