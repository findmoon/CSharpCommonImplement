**C#判断当前线程是否为UI主线程，强制在主线程初始化Form**

[toc]

首先要明确，强制在UI主线程初始化Form是否有必要？因为后续操作要考虑到UI线程操作的安全性和限制。

如果Form可以正常使用（除非，非UI线程下使用有问题，这个可能性基本没有），则没必要强制在UI线程下操作。

本篇主要介绍，如何判断当前的线程是否为UI主线程。以及，略有些多余的强制用户在主线程下初始化一次实例。

# 判断当前线程是否为UI主线程

> 主要参考自 [How to tell if a thread is the main thread in C#](https://stackoverflow.com/questions/2374451/how-to-tell-if-a-thread-is-the-main-thread-in-c-sharp)

下面是判断的方法，基本上应该是正确无误的。


```C#
/// <summary>
/// 检查是否是UI主线程
/// </summary>
public static bool CheckForMainThread()
{
    if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA &&
        !Thread.CurrentThread.IsBackground && !Thread.CurrentThread.IsThreadPoolThread && Thread.CurrentThread.IsAlive)
    {
        // 是否还需要其他条件？
        //MethodInfo correctEntryMethod = Assembly.GetEntryAssembly().EntryPoint;
        // 主线程的id总是为1吗？
        // 感觉还应该加上 Thread.CurrentThread.ManagedThreadId == 1

        return true;

        //// 只需要一个InvokeRequired即可？
        //if (Application.OpenForms[0].InvokeRequired && Application.OpenForms[0]!=this)
        //{
        //    //we're on the main thread, since invoking is NOT required
        //}
    }

    // throw exception, the current thread is not the main thread...
    return false;
}
```

- `MethodInfo correctEntryMethod = Assembly.GetEntryAssembly().EntryPoint;`获取执行程序集的入口方法。
- `Application.OpenForms[0].InvokeRequired` 获取当前打开的窗体中的第一个窗体（即主窗体），从而判断是否需要异步Invoke调用。
- 主线程的id总是为1吗？即`Thread.CurrentThread.ManagedThreadId`。是否应该加上 `Thread.CurrentThread.ManagedThreadId == 1` 的判断，以完全确保是主线程？ **前提是，UI主线程的ID总是为1。**

# 判断当前线程是否为主线程的其它方法

## 在主线程记录当前的id，然后在其它线程判断【最准确的方法】

在启动程序时生成静态变量`mainThreadId`，并把主线程Id赋值给它。

在其它地方判断当前线程id是否和`mainThreadId`主线程id相等(直接调用方法)。

```C#
// Do this when you start your application
static int mainThreadId;

// In Main method:
mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

// If called in the non main thread, will return false;
public static bool IsMainThread
{
    get { return System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId; }
}
```

如下是一个更直观的代码。在其他任何地方，只要调用`IsRunningOnStartupThread()`就知道是否是启动线程。

```C#
static class Program
{
    private static Thread _startupThread = null;

    [STAThread]
    static void Main()
    {
        _startupThread = Thread.CurrentThread;

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }

    public static bool IsRunningOnStartupThread()
    {
        return Thread.CurrentThread == _startupThread;
    }
}
```

## Winform 或 WPF 中判断 SynchronizationContext.Current 是否为空【未确认】

**在线程池线程中 `SynchronizationContext.Current` 将返回 null。**

## WPF中 App.Current.Dispatcher.Thread

```C#
if (App.Current.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
{
    //we're on the main thread
}
```

```C#
public static void InvokeIfNecessary (Action action)
{
    if (Thread.CurrentThread == Application.Current.Dispatcher.Thread)
        action ();
    else {
        Application.Current.Dispatcher.Invoke(action);
    }
}
```

## WPF中的Dispatcher.CheckAccess()

```C#
private void UpdateControls()
{
    if (!Dispatcher.CheckAccess())
    {
        // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
        Dispatcher.BeginInvoke(new Action(UpdateControls));
        return;
    }
    else
    {
        // We're in the UI thread, update the controls
        TextTime.Text = DateTime.Now.ToLongTimeString();
    }
    
}
```


```C#
if (!control.Dispatcher.CheckAccess())
{
  // Do non UI Thread stuff
}
```

## Winform 中 Application.OpenForms[0].InvokeRequired

```C#
if (!Application.OpenForms[0].InvokeRequired)
{
    //we're on the main thread, since invoking is NOT required
}
```

```C#
if(control.InvokeRequired) 
{
    // Do non UI thread stuff
}
```

## 提取检查是否是UI线程的方法【适用于Winform和WPF】

```C#
public static bool CurrentlyOnUiThread<T>(T control)
{ 
   if(T is System.Windows.Forms.Control)
   {
      System.Windows.Forms.Control c = control as System.Windows.Forms.Control;
      return !c.InvokeRequired;
   }
   else if(T is System.Windows.Controls.Control)
   {
      System.Windows.Controls.Control c = control as System.Windows.Control.Control;
      return c.Dispatcher.CheckAccess()
   }
}
```

> 参考 [Detecting whether on UI thread in WPF and Winforms](https://stackoverflow.com/questions/5143599/detecting-whether-on-ui-thread-in-wpf-and-winforms)

## Winform Application.MessageLoop

```C#
bool isMessageLoopThread = System.Windows.Forms.Application.MessageLoop;
```

```C#
bool isUIThread1 = SynchronizationContext.Current != null;
bool isUIThread2 = Application.MessageLoop;
bool isUIThread3 = Thread.CurrentThread.GetApartmentState() == ApartmentState.STA;
```

> I would suggest that it's the kind of decision the caller should make. You could always write wrapper methods to make it easier - but it means that you won't have problems with the caller being in an "odd" situation (e.g. a UI framework you don't know about, or something else with an event loop) and you making the wrong decision for them.
> 
> If the method ever needs to provide feedback in the right thread, I'd pass in an ISynchronizeInvoke (implemented by Control) to do that in a UI-agnostic way.
> 
> [how-to-detect-if-were-on-a-ui-thread#answer-1149408](https://stackoverflow.com/questions/1149402/how-to-detect-if-were-on-a-ui-thread#answer-1149408)

## 借助 [ThreadStatic] 记录是否主线程

> 一种很巧妙的方法。不过似乎在 .NET Core 中不可用（未确认，应该是可用的）。

```C#
static class Program
{
  [ThreadStatic]
  public static readonly bool IsMainThread = true;

  //...
}
```

**解释：**

`IsMainThread`字段的初始化被编译到静态构造函数中，并在该类第一次使用时运行完成（技术上，在任何静态成员首次访问之前）。假设该类在主线程中首先使用，静态构造函数将被调用，字段将被设置true。

因为使用了`[ThreadStatic]`特性，它在每个线程中都有独立的值。初始化器仅仅运行一次，即在访问该类型的第一个线程中，该线程中的值为`true`；但该字段在所有其它线程中保持未初始化状态，值为`false`。

## WPF 中不要使用 Dispatcher.CurrentDispatcher

**不要使用`if(Dispatcher.CurrentDispatcher.Thread == Thread.CurrentThread) ...`。**

Dispatcher.CurrentDispatcher will, if the current thread do not have a dispatcher, create and return a new Dispatcher associated with the current thread.

```C#
Dispatcher dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
if (dispatcher != null)
{
   // We know the thread have a dispatcher that we can use.
}
```

> [detecting-whether-on-ui-thread-in-wpf#answer-14280425](https://stackoverflow.com/questions/5143599/detecting-whether-on-ui-thread-in-wpf-and-winforms#answer-14280425)

## UWP 中

> **在 UWP 应用中，UI 线程不是主线程！！！**

`CoreWindow.GetForCurrentThread().Dispatcher`

`private static CoreDispatcher Dispatcher => Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;`

[CoreDispatcher.HasThreadAccess](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.core.coredispatcher.hasthreadaccess) returns a bool indicating if you are on the UI thread or not.

# 判断当前非UI主线程初始化窗体，则报错

```C#
/// <summary>
/// 如果需要在子线程中调用静态方式显示消息，请先在UI主线程中调用此方法初始化
/// </summary>
/// <param name="msg"></param>
/// <exception cref="Exception"></exception>
public static void InitForStatic(string msg)
{
    if (showOneForm == null || showOneForm.IsDisposed || showOneForm.Disposing)
    {
        if (CheckForMainThread())
        {
            showOneForm = new Form_Alert(msg);
        }
        else
        {
            throw new Exception("若直接调用静态方法，请确保先在UI主线程中调用 InitForStatic()方法 初始化一次实例！");
        }
    }
}
```

# 另，winform窗体在显示前就设置 `ShowInTaskbar = false` 将不会创建窗体句柄，可能导致显示问题。

**通常，应该在窗体 `Show()` 显示之后再设置 `ShowInTaskbar = false`，以便窗体可以正确显示出来。**
