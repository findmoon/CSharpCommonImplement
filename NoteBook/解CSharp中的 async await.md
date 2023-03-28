理解C#中的 async await

[toc]

> [理解C#中的 async await](https://www.cnblogs.com/xiaoxiaotank/p/14303803.html)

#### 前言

一个老掉牙的话题，园子里的相关优秀文章已经有很多了，我写这篇文章完全是想以自己的思维方式来谈一谈自己的理解。（PS：文中涉及到了大量反编译源码，需要静下心来细细品味）

## 从简单开始

为了更容易理解这个问题，我们举一个简单的例子：用异步的方式在控制台上分两步输出“Hello World!”，我这边使用的是Framework 4.5.2

highlighter- arduino

class Program
{
    static async Task Main(string\[\] args) {
        Console.WriteLine("Let's Go!");
    
        await TestAsync();
        
        Console.Write(" World!");
    }

    static Task TestAsync() {
        return Task.Run(() =>
        {
            Console.Write("Hello");
        });
    }
}
C# 复制 全屏

## 探究反编译后的源码

接下来我们使用 .NET reflector （也可使用 dnSpy 等） 反编译一下程序集，然后一步一步来探究 async await 内部的奥秘。

### Main方法

highlighter- pf

\[DebuggerStepThrough\]
private static void <Main>(string\[\] args)
{
    Main(args).GetAwaiter().GetResult();
}

\[AsyncStateMachine(typeof(<Main>d\_\_0)), DebuggerStepThrough\]
private static Task Main(string\[\] args)
{
    <Main>d\_\_0 stateMachine = new <Main>d\_\_0 
    {
        <>t\_\_builder = AsyncTaskMethodBuilder.Create(),
        args = args,
        <>1\_\_state = -1
    };
    stateMachine.<>t\_\_builder.Start<<Main>d\_\_0>(ref stateMachine);
    return stateMachine.<>t\_\_builder.Task;
}

// 实现了 IAsyncStateMachine 接口
\[CompilerGenerated\]
private sealed class <Main>d\_\_0 : IAsyncStateMachine
{
    // Fields
    public int <>1\_\_state;
    public AsyncTaskMethodBuilder <>t\_\_builder;
    public string\[\] args;
    private TaskAwaiter <>u\_\_1;

    // Methods
    private void MoveNext() { }
    \[DebuggerHidden\]
    private void SetStateMachine(IAsyncStateMachine stateMachine) { }
} 

卧槽！竟然有两个 Main 方法：一个同步、一个异步。原来，虽然我们写代码时为了在 Main 方法中方便异步等待，将 void Main 改写成了async Task Main，但是实际上程序入口仍是我们熟悉的那个 void Main。

另外，我们可以看到异步 Main 方法被标注了`AsyncStateMachine`特性，这是因为在我们的源代码中，该方法带有修饰符`async`，表示该方法是一个异步方法。

好，我们先看一下异步Main方法内部实现，它主要做了三件事：

1. 首先，创建了一个类型为`<Main>d__0`的状态机 stateMachine，并初始化了公共变量 **<>t\_\_builder**、args、**<>1\_\_state = -1**
    - <>t\_\_builder：负责异步相关的操作，是实现异步 Main 方法异步的核心
    - <>1\_\_state：状态机的当前状态
2. 然后，调用`Start`方法，借助 stateMachine, 来执行我们在异步 Main 方法中写的代码
3. 最后，将指示异步 Main 方法运行状态的`Task`对象返回出去

### Start

首先，我们先来看一下`Start`的内部实现

highlighter- csharp

// 所属结构体：AsyncTaskMethodBuilder

\[SecuritySafeCritical, DebuggerStepThrough, \_\_DynamicallyInvokable\]
public void Start<TStateMachine\>(ref TStateMachine stateMachine) where TStateMachine: IAsyncStateMachine
{
    if (((TStateMachine) stateMachine) == null)
    {
        throw new ArgumentNullException("stateMachine");
    }
    ExecutionContextSwitcher ecsw = new ExecutionContextSwitcher();
    RuntimeHelpers.PrepareConstrainedRegions();
    try
    {
        ExecutionContext.EstablishCopyOnWriteScope(ref ecsw);
        // 状态机状态流转
        stateMachine.MoveNext();
    }
    finally
    {
        ecsw.Undo();
    }
} 

我猜，你只能看懂`stateMachine.MoveNext()`，对不对？好，那我们就来看看这个状态机类`<Main>d__0`，并且着重看它的方法`MoveNext`。

### MoveNext

highlighter- kotlin

\[CompilerGenerated\]
private sealed class <Main\>d\_\_0 : IAsyncStateMachine
{
    // Fields
    public int <>1\_\_state;
    public AsyncTaskMethodBuilder <>t\_\_builder;
    public string\[\] args;
    private TaskAwaiter <>u\_\_1;

    // Methods
    private void MoveNext()
    {
        // 在 Main 方法中，我们初始化 <>1\_\_state = -1，所以此时 num = -1
        int num = this.<>1\_\_state;
        try
        {
            TaskAwaiter awaiter;
            if (num != 0)
            {
                Console.WriteLine("Let's Go!");
                // 调用 TestAsync()，获取 awaiter，用于后续监控 TestAsync() 运行状态
                awaiter = Program.TestAsync().GetAwaiter();
                
                // 一般来说，异步任务不会很快就完成，所以大多数情况下都会进入该分支
                if (!awaiter.IsCompleted)
                {
                    // 状态机状态从 -1 流转为 0
                    this.<>1\_\_state = num = 0;
                    this.<>u\_\_1 = awaiter;
                    Program.<Main>d\_\_0 stateMachine = this;
                    // 配置  TestAsync() 完成后的延续
                    this.<>t\_\_builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<Main>d\_\_0>(ref awaiter, ref stateMachine);
                    return;
                }
            }
            else
            {
                awaiter = this.<>u\_\_1;
                this.<>u\_\_1 = new TaskAwaiter();
                this.<>1\_\_state = num = -1;
            }
            awaiter.GetResult();
            Console.Write(" World!");
        }
        catch (Exception exception)
        {
            this.<>1\_\_state = -2;
            this.<>t\_\_builder.SetException(exception);
            return;
        }
        this.<>1\_\_state = -2;
        this.<>t\_\_builder.SetResult();
    }

    \[DebuggerHidden\]
    private void SetStateMachine(IAsyncStateMachine stateMachine)
    {
    }
} 

[![](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165435649-2140245165.png)](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165435649-2140245165.png)

先简单理一下内部逻辑：

1. 设置变量 num = -1，此时 num != 0，则会进入第一个if语句，
2. 首先，执行`Console.WriteLine("Let's Go!")`
3. 然后，调用异步方法`TestAsync`，`TestAsync`方法会在另一个线程池线程中执行，并获取指示该方法运行状态的 awaiter
4. 如果此时`TestAsync`方法已执行完毕，则像没有异步一般：
    1. 继续执行接下来的`Console.Write(" World!")`
    2. 最后设置 <>1\_\_state = -2，并设置异步 Main 方法的返回结果
5. 如果此时`TestAsync`方法未执行完毕，则：
    1. 设置 <>1\_\_state = num = 0
    2. 调用`AwaitUnsafeOnCompleted`方法，用于配置当`TestAsync`方法完成时的延续，即`Console.Write(" World!")`
    3. 返回指示异步 Main 方法执行状态的 Task 对象，由于同步 Main 方法中通过使用`GetResult()`同步阻塞主线程等待任务结束，所以不会释放主线程（废话，如果释放了程序就退出了）。不过对于其他子线程，一般会释放该线程

大部分逻辑我们都可以很容易的理解，唯一需要深入研究的就是`AwaitUnsafeOnCompleted`，那我们接下来就看看它的内部实现

### AwaitUnsafeOnCompleted

highlighter- csharp

// 所属结构体：AsyncTaskMethodBuilder

\[\_\_DynamicallyInvokable\]
public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine\>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter: ICriticalNotifyCompletion where TStateMachine: IAsyncStateMachine
{
    this.m\_builder.AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref awaiter, ref stateMachine);
}

// 所属结构体：AsyncTaskMethodBuilder<TResult>

\[SecuritySafeCritical, \_\_DynamicallyInvokable\]
public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine\>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter: ICriticalNotifyCompletion where TStateMachine: IAsyncStateMachine
{
    try
    {
        // 用于流转状态机状态的 runner
        AsyncMethodBuilderCore.MoveNextRunner runnerToInitialize = null;
        Action completionAction = this.m\_coreState.GetCompletionAction(AsyncCausalityTracer.LoggingOn ? this.Task : null, ref runnerToInitialize);
        if (this.m\_coreState.m\_stateMachine == null)
        {
            // 此处构建指示异步 Main 方法执行状态的 Task 对象
            Task<TResult> builtTask = this.Task;
            this.m\_coreState.PostBoxInitialization((TStateMachine) stateMachine, runnerToInitialize, builtTask);
        }
        awaiter.UnsafeOnCompleted(completionAction);
    }
    catch (Exception exception)
    {
        AsyncMethodBuilderCore.ThrowAsync(exception, null);
    }
} 

咱们一步一步来，先看一下`GetCompletionAction`的实现：

highlighter- reasonml

// 所属结构体：AsyncMethodBuilderCore

\[SecuritySafeCritical\]
internal Action GetCompletionAction(Task taskForTracing, ref MoveNextRunner runnerToInitialize)
{
    Action defaultContextAction;
    MoveNextRunner runner;
    Debugger.NotifyOfCrossThreadDependency();
    // 
    ExecutionContext context = ExecutionContext.FastCapture();
    if ((context != null) && context.IsPreAllocatedDefault)
    {
        defaultContextAction = this.m\_defaultContextAction;
        if (defaultContextAction != null)
        {
            return defaultContextAction;
        }
        
        // 构建 runner
        runner = new MoveNextRunner(context, this.m\_stateMachine);
        // 返回值
        defaultContextAction = new Action(runner.Run);
        if (taskForTracing != null)
        {
            this.m\_defaultContextAction = defaultContextAction = this.OutputAsyncCausalityEvents(taskForTracing, defaultContextAction);
        }
        else
        {
            this.m\_defaultContextAction = defaultContextAction;
        }
    }
    else
    {
        runner = new MoveNextRunner(context, this.m\_stateMachine);
        defaultContextAction = new Action(runner.Run);
        if (taskForTracing != null)
        {
            defaultContextAction = this.OutputAsyncCausalityEvents(taskForTracing, defaultContextAction);
        }
    }
    if (this.m\_stateMachine == null)
    {
        runnerToInitialize = runner;
    }
    return defaultContextAction;
} 

发现一个熟悉的家伙——**ExecutionContext**，它是用来给咱们延续方法（即`Console.Write(" World!");`）提供运行环境的，注意这里用的是`FastCapture()`，该内部方法并未捕获`SynchronizationContext`，因为不需要流动它。什么？你说你不认识它？大眼瞪小眼？那你应该好好看看[《理解C#中的ExecutionContext vs SynchronizationContext》](https://www.cnblogs.com/xiaoxiaotank/p/13666913.html)了

接着来到`new MoveNextRunner(context, this.m_stateMachine)`，这里初始化了 runner，我们看看构造函数中做了什么：

highlighter- pf

\[SecurityCritical\]
internal MoveNextRunner(ExecutionContext context, IAsyncStateMachine stateMachine)
{
    // 将 ExecutionContext 保存了下来
    this.m\_context = context;
    
    // 将 stateMachine 保存了下来（不过此时为 null）
    this.m\_stateMachine = stateMachine;
} 

往下来到`defaultContextAction = new Action(runner.Run)`，你可以发现，最终咱们返回的就是这个 defaultContextAction ，所以这个`runner.Run`至关重要，不过别着急，我们等用到它的时候我们再来看其内部实现。

最后，回到`AwaitUnsafeOnCompleted`方法，继续往下走。构建指示异步 Main 方法执行状态的 Task 对象，设置当前的状态机后，来到`awaiter.UnsafeOnCompleted(completionAction);`，要记住，入参 completionAction 就是刚才返回的`runner.Run`：

highlighter- reasonml

// 所属结构体：TaskAwaiter

\[SecurityCritical, \_\_DynamicallyInvokable\]
public void UnsafeOnCompleted(Action continuation)
{
    OnCompletedInternal(this.m\_task, continuation, true, false);
}

\[MethodImpl(MethodImplOptions.NoInlining), SecurityCritical\]
internal static void OnCompletedInternal(Task task, Action continuation, bool continueOnCapturedContext, bool flowExecutionContext)
{
    if (continuation == null)
    {
        throw new ArgumentNullException("continuation");
    }
    StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
    if (TplEtwProvider.Log.IsEnabled() || Task.s\_asyncDebuggingEnabled)
    {
        continuation = OutputWaitEtwEvents(task, continuation);
    }
    
    // 配置延续方法
    task.SetContinuationForAwait(continuation, continueOnCapturedContext, flowExecutionContext, ref lookForMyCaller);
} 

直接来到代码最后一行，看到延续方法的配置

highlighter- reasonml

// 所属类：Task

\[SecurityCritical\]
internal void SetContinuationForAwait(Action continuationAction, bool continueOnCapturedContext, bool flowExecutionContext, ref StackCrawlMark stackMark)
{
    TaskContinuation tc = null;
    if (continueOnCapturedContext)
    {
        // 这里我们用的是不进行流动的 SynchronizationContext
        SynchronizationContext currentNoFlow = SynchronizationContext.CurrentNoFlow;
        // 像 Winform、WPF 这种框架，实现了自定义的 SynchronizationContext，
        // 所以在 Winform、WPF 的 UI线程中进行异步等待时，一般 currentNoFlow 不会为 null
        if ((currentNoFlow != null) && (currentNoFlow.GetType() != typeof(SynchronizationContext)))
        {
            // 如果有 currentNoFlow，那么我就用它来执行延续方法
            tc = new SynchronizationContextAwaitTaskContinuation(currentNoFlow, continuationAction, flowExecutionContext, ref stackMark);
        }
        else
        {
            TaskScheduler internalCurrent = TaskScheduler.InternalCurrent;
            if ((internalCurrent != null) && (internalCurrent != TaskScheduler.Default))
            {
                tc = new TaskSchedulerAwaitTaskContinuation(internalCurrent, continuationAction, flowExecutionContext, ref stackMark);
            }
        }
    }
    if ((tc == null) & flowExecutionContext)
    {
        tc = new AwaitTaskContinuation(continuationAction, true, ref stackMark);
    }
    if (tc != null)
    {
        if (!this.AddTaskContinuation(tc, false))
        {
            tc.Run(this, false);
        }
    }
    // 这里会将 continuationAction 设置为 awaiter 中 task 对象的延续方法，所以当 TestAsync() 完成时，就会执行 runner.Run
    else if (!this.AddTaskContinuation(continuationAction, false))
    {
        AwaitTaskContinuation.UnsafeScheduleAction(continuationAction, this);
    }
} 

对于我们的示例来说，既没有自定义 SynchronizationContext，也没有自定义 TaskScheduler，所以会直接来到最后一个`else if (...)`，重点在于`this.AddTaskContinuation(continuationAction, false)`，这个方法会将我们的延续方法添加到 Task 中，以便于当 TestAsync 方法执行完毕时，执行 runner.Run

### runner.Run

好，是时候让我们看看 runner.Run 的内部实现了：

highlighter- csharp

\[SecuritySafeCritical\]
internal void Run()
{
    if (this.m\_context != null)
    {
        try
        {
            // 我们并未给 s\_invokeMoveNext 赋值，所以 callback == null
            ContextCallback callback = s\_invokeMoveNext;
            if (callback == null)
            {
                // 将回调设置为下方的 InvokeMoveNext 方法
                s\_invokeMoveNext = callback = new
                ContextCallback(AsyncMethodBuilderCore.MoveNextRunner.InvokeMoveNext);
            }
            ExecutionContext.Run(this.m\_context, callback, this.m\_stateMachine, true);
            return;
        }
        finally
        {
            this.m\_context.Dispose();
        }
    }
    this.m\_stateMachine.MoveNext();
}

\[SecurityCritical\]
private static void InvokeMoveNext(object stateMachine)
{
    ((IAsyncStateMachine) stateMachine).MoveNext();
} 

来到`ExecutionContext.Run(this.m_context, callback, this.m_stateMachine, true);`，这里的 callback 是`InvokeMoveNext`方法。所以，当`TestAsync`执行完毕后，就会执行延续方法 runner.Run，也就会执行`stateMachine.MoveNext()`促使状态机继续进行状态流转，这样逻辑就打通了：

highlighter- kotlin

private void MoveNext()
{
    // num = 0
    int num = this.<>1\_\_state;
    try
    {
        TaskAwaiter awaiter;
        if (num != 0)
        {
            Console.WriteLine("Let's Go!");
            awaiter = Program.TestAsync().GetAwaiter();

            if (!awaiter.IsCompleted)
            {
                this.<>1\_\_state = num = 0;
                this.<>u\_\_1 = awaiter;
                Program.<Main>d\_\_0 stateMachine = this;
                this.<>t\_\_builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<Main>d\_\_0>(ref awaiter, ref stateMachine);
                return;
            }
        }
        else
        {
            awaiter = this.<>u\_\_1;
            this.<>u\_\_1 = new TaskAwaiter();
            // 状态机状态从 0 流转到 -1
            this.<>1\_\_state = num = -1;
        }
        
        // 结束对 TestAsync() 的等待
        awaiter.GetResult();
        // 执行延续方法
        Console.Write(" World!");
    }
    catch (Exception exception)
    {
        this.<>1\_\_state = -2;
        this.<>t\_\_builder.SetException(exception);
        return;
    }
    
    // 状态机状态从 -1 流转到 -2
    this.<>1\_\_state = -2;
    // 设置异步 Main 方法最终返回结果
    this.<>t\_\_builder.SetResult();
} 

至此，整个异步方法的执行就结束了，通过一张图总结一下：  
[![](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165259645-250717138.png)](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165259645-250717138.png)

最后，我们看一下各个线程的状态，看看和你的推理是否一致（如果有不清楚的联系我，我会通过文字补充）：  
[![](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165320220-2013763193.gif)](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165320220-2013763193.gif)

## 多个 async await 嵌套

理解了async await的简单使用，那你可曾想过，如果有多个 async await 嵌套，那会出现什么情况呢？接下来就改造一下我们的例子，来研究研究：

highlighter- arduino

static Task TestAsync() {
    return Task.Run(async () =>
    {
        // 增加了这行
        await Task.Run(() =>
        {
            Console.Write("Say: ");
        });

        Console.Write("Hello");
    });
} 

反编译之后的代码，上面已经讲解的我就不再重复贴了，主要看看`TestAsync()`就行了：

highlighter- pf

private static Task TestAsync() => 
    Task.Run(delegate {
        <>c.<<TestAsync>b\_\_1\_0>d stateMachine = new <>c.<<TestAsync>b\_\_1\_0>d {
            <>t\_\_builder = AsyncTaskMethodBuilder.Create(),
            <>4\_\_this = this,
            <>1\_\_state = -1
        };
        stateMachine.<>t\_\_builder.Start<<>c.<<TestAsync>b\_\_1\_0>d>(ref stateMachine);
        return stateMachine.<>t\_\_builder.Task;
    }); 

哦！原来，async await 的嵌套也就是状态机的嵌套，相信你通过上面的状态机状态流转，也能够梳理除真正的执行逻辑，那我们就只看一下线程状态吧：  
[![](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165459091-2100640352.gif)](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165459091-2100640352.gif)

这也印证了我上面所说的：当子线程完成执行任务时，会被释放，或回到线程池供其他线程使用。

## 多个 async await 在同一方法中顺序执行

又可曾想过，如果有多个 async await 在同一方法中顺序执行，又会是何种景象呢？同样，先来个例子：

highlighter- arduino

static async Task Main(string\[\] args) {
    Console.WriteLine("Let's Go!");

    await Test1Async();

    await Test2Async();

    Console.Write(" World!");
}

static Task Test1Async() {
    return Task.Run(() =>
    {
        Console.Write("Say: ");
    });
}

static Task Test2Async() {
    return Task.Run(() =>
    {
        Console.Write("Hello");
    });
} 

直接看状态机：

highlighter- kotlin

\[CompilerGenerated\]
private sealed class <Main\>d\_\_0 : IAsyncStateMachine
{
	// Fields
	public int <>1\_\_state;
	public AsyncTaskMethodBuilder <>t\_\_builder;
	public string\[\] args;
	private TaskAwaiter <>u\_\_1;

    // Methods
	private void MoveNext()
	{
		int num = this.<>1\_\_state;
		try
		{
			TaskAwaiter awaiter;
			TaskAwaiter awaiter2;
			if (num != 0)
			{
				if (num == 1)
				{
					awaiter = this.<>u\_\_1;
					this.<>u\_\_1 = default(TaskAwaiter);
					this.<>1\_\_state = -1;
					goto IL\_D8;
				}
				Console.WriteLine("Let's Go!");
				awaiter2 = Program.Test1Async().GetAwaiter();
				if (!awaiter2.IsCompleted)
				{
					this.<>1\_\_state = 0;
					this.<>u\_\_1 = awaiter2;
					Program.<Main>d\_\_0 <Main>d\_\_ = this;
					this.<>t\_\_builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<Main>d\_\_0>(ref awaiter2, ref <Main>d\_\_);
					return;
				}
			}
			else
			{
				awaiter2 = this.<>u\_\_1;
				this.<>u\_\_1 = default(TaskAwaiter);
				this.<>1\_\_state = -1;
			}
			awaiter2.GetResult();
			
			// 待 Test1Async 完成后，继续执行 Test2Async
			awaiter = Program.Test2Async().GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				this.<>1\_\_state = 1;
				this.<>u\_\_1 = awaiter;
				Program.<Main>d\_\_0 <Main>d\_\_ = this;
				this.<>t\_\_builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<Main>d\_\_0>(ref awaiter, ref <Main>d\_\_);
				return;
			}
			IL\_D8:
			awaiter.GetResult();
			Console.Write(" World!");
		}
		catch (Exception exception)
		{
			this.<>1\_\_state = -2;
			this.<>t\_\_builder.SetException(exception);
			return;
		}
		this.<>1\_\_state = -2;
		this.<>t\_\_builder.SetResult();
	}

	\[DebuggerHidden\]
	private void SetStateMachine(IAsyncStateMachine stateMachine)
	{
	}
} 

可见，就是一个状态机状态一直流转就完事了。我们就看看线程状态吧：  
[![](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165515998-311183181.gif)](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165515998-311183181.gif)

## WPF中使用 async await

上面我们都是通过控制台举的例子，这是没有任何`SynchronizationContext`的，但是WPF（Winform同理）就不同了，在UI线程中，它拥有属于自己的`DispatcherSynchronizationContext`。

highlighter- csharp

private async void Button\_Click(object sender, RoutedEventArgs e)
{
    // UI 线程会一直保持 Running 状态，不会导致 UI 假死
    Show(Thread.CurrentThread);

    await TestAsync();

    Show(Thread.CurrentThread);
}

private Task TestAsync()
{
    return Task.Run(() =>
    {
        Show(Thread.CurrentThread);
    });
}

private static void Show(Thread thread)
{
    MessageBox.Show($"{nameof(thread.ManagedThreadId)}: {thread.ManagedThreadId}" +
        $"\\n{nameof(thread.ThreadState)}: {thread.ThreadState}");
} 

通过使用`DispatcherSynchronizationContext`执行延续方法，又回到了 UI 线程中  
[![](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165534935-171511763.gif)](https://img2020.cnblogs.com/blog/1010000/202101/1010000-20210120165534935-171511763.gif)