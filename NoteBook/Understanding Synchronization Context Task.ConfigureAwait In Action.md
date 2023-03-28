**Understanding Synchronization Context Task.ConfigureAwait In Action**

[toc]

> [Understanding Synchronization Context Task.ConfigureAwait In Action](https://www.c-sharpcorner.com/article/understanding-synchronization-context-task-configureawait-in-action/)


## Overview

When dealing with asynchronous code, one of the most important concepts that you must have a solid understanding of is **synchronization context**. Synchronization context is one of the most ignored concepts in the asynchronous programming realm as it is often hard for developers to understand. Today, we will try to simplify things as much as we can. We will have a look at _SynchronizationContext_ class and see how it affects code behavior in action. We will also have a look at one of the most important methods in the TPL library, _Task.ConfigureAwait()_.

## Introduction

Synchronization Context is the environment that a thread runs in. It’s the set of characteristics the define how the thread responds to messages. Think of it as the scope that defines thread boundaries. No thread can access another thread’s data or pass any unit of work to the other thread without communicating through the target thread’s synchronization context. Worth mentioning that different threads may share the same synchronization context, and it is optional for a thread to have a synchronization context.

## A Bit of History

If you are coming from a Windows development background, you will absolutely know that in WinForms, for example, you cannot access UI controls from any other thread. You must offload your task, or in other words, delegate your code (unit of work) to the UI thread through the _Control.BeginInvoke()_ function, or more precisely, using the _ISynchronizeInvoke_ pattern. The _ISynchronizeInvoke_ allowed you to queue a unit of work to the UI thread for processing. If you do not follow this pattern, your code may crash with a **cross-thread access** error.

Then, the synchronization context was introduced. And in fact, WinForms uses the _ISynchronizeInvoke_ pattern internally. The introduction of synchronization context allowed a common understanding of thread environment and boundaries. You do not have to think of each framework separately, you must understand what a synchronization context is, and how to delegate work to other threads for processing.

## Implementation

Although synchronization context is implemented differently in various .NET platforms, the idea is the same, and all implementations derive from the default .NET synchronization context class, _SynchronizationContext_ (_mscorlib.dll: System_) which introduces one static property, _Current_, which returns the current thread’s _SynchronizationContext_, and two main methods, _Send()_ and _Post()_.

Simply put, the difference between _Send()_ and _Post()_ is that _Send()_ will run this unit of work synchronously. In other words, when a source thread delegates some work to another thread, it will be blocked until the target thread completes. On the other hand, the caller thread will not be blocked if _Post()_ is used. See the following graph.

When we have a look at the internal implementation of the default SynchronizationContext, we can see the following,

```csharp
public virtual void Send(SendOrPostCallback d, object state) => d(state);
public virtual void Post(SendOrPostCallback d, object state) => ThreadPool.QueueUserWorkItem(new WaitCallback(d.Invoke), state);
```

C#

Copy

_Send()_ just executes the delegate, whereas _Post()_ uses _ThreadPool_ to execute the delegate asynchronously.

Please note that although _Send()_ and _Post()_ behave differently by default, they may not when implemented by a platform. Now let us take a quick look at how various platforms implement synchronization context.

### WinForms

In Windows Forms apps, the synchronization context is implemented through _WindowsFormsSynchronizationContext_ (Syst_em.Windows.Forms.dll: System.Windows.Forms_), which itself derives from the default _SynchronizationContext_ (_mscorlib.dll: System_). This implementation simply calls _Control.Invoke()_ and _Control.BeginInvoke()_ for _Send()_ and _Post()_, respectively. They ensure that the unit of work is sent to the UI thread.

When _WindowsFormsSynchronizationContext_ is applied? It is applied (i.e., installed) when you instantiate your form for the first time. Note that those asserts will pass,

```csharp
[STAThread]
static void Main() {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    // SynchronizationContext is still null here
    Debug.Assert(SynchronizationContext.Current == null);
    var frm = new TasksForm();
    // Now SynchronizationContext is installed
    Debug.Assert(SynchronizationContext.Current != null && SynchronizationContext.Current is WindowsFormsSynchronizationContext);
    Application.Run(frm);
}
```

C#

Copy

### WPF and Silverlight

In WPF and Silverlight, the synchronization context is implemented through _DispatcherSynchronizationContext_ (_WindowsBase.dll: System.Windows.Threading_) and it acts the same as WinForms’s counterpart, it passes the delegated unit of work to the UI thread for execution and it uses internally _Dispatcher.Invoke()_ and _Dispatcher.BeginInvoke()_ for delegation.

### Classic ASP.NET

In Classic ASP.NET, the synchronization context is implemented through _AspNetSynchronizationContext_ (_System.Web.dll: System.Web_), however, it is implemented differently than its counterparts in other platforms. As there is no UI-thread concept in ASP.NET, and as each request needs a separate thread for processing, the _AspNetSynchronizationContext_ just maintains a queue of outstanding operations that when all are finished, the request then can be marked as completed. _Post()_ still passes delegated work to _ThreadPool_.

Is this the only usage for _AspNetSynchronizationContext_? No. The most important task of _AspNetSynchronizationContext_ is that it ensures that the request threads can access things like _HttpContext.Current_ and other relevant identity and culture data. You know this. There were times when you cached a reference to _HttpContext.Current_ before running your threads, that is when _AspNetSynchronizationContext_ comes in handy.

### ASP.NET Core

There is no _AspNetSynchronizationContext_ equivalent in ASP.NET Core, it has been removed. Why? It is part of the various performance improvements that have been applied to the new platform. It frees the app from the overhead incurred when entering and leaving synchronization contexts (which we will talk about in a moment.) Please read more about ASP.NET Core’s SynchronizationContext [here](https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html).

## Synchronization Context in Action

Now, let us see synchronization context in action. In this example, we are going to see how to pass a unit of work from a worker thread to the UI thread. Fire a new WinForms project and update the designer code to match the following,

```csharp
private void InitializeComponent() {
    this.ResultsListBox = new System.Windows.Forms.ListBox();
    this.RegularThreadsButton = new System.Windows.Forms.Button();
    this.UIThreadTest = new System.Windows.Forms.Button();
    this.SuspendLayout();
    //
    // ResultsListBox
    //
    this.ResultsListBox.FormattingEnabled = true;
    this.ResultsListBox.Location = new System.Drawing.Point(12, 12);
    this.ResultsListBox.Name = "ResultsListBox";
    this.ResultsListBox.Size = new System.Drawing.Size(516, 212);
    this.ResultsListBox.TabIndex = 1;
    //
    // RegularThreadsButton
    //
    this.RegularThreadsButton.Location = new System.Drawing.Point(12, 232);
    this.RegularThreadsButton.Name = "RegularThreadsButton";
    this.RegularThreadsButton.Size = new System.Drawing.Size(516, 23);
    this.RegularThreadsButton.TabIndex = 2;
    this.RegularThreadsButton.Text = "Regular Thread Test";
    this.RegularThreadsButton.UseVisualStyleBackColor = true;
    this.RegularThreadsButton.Click += new System.EventHandler(this.RegularThreadsButton_Click);
    //
    // UIThreadTest
    //
    this.UIThreadTest.Location = new System.Drawing.Point(12, 261);
    this.UIThreadTest.Name = "UIThreadTest";
    this.UIThreadTest.Size = new System.Drawing.Size(516, 23);
    this.UIThreadTest.TabIndex = 3;
    this.UIThreadTest.Text = "UI-Context Thread Test";
    this.UIThreadTest.UseVisualStyleBackColor = true;
    this.UIThreadTest.Click += new System.EventHandler(this.UIThreadTest_Click);
    //
    // ThreadsForm
    //
    this.AutoScaleDimensions = new System.Drawing.SizeF(6 F, 13 F);
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    this.ClientSize = new System.Drawing.Size(540, 294);
    this.Controls.Add(this.UIThreadTest);
    this.Controls.Add(this.RegularThreadsButton);
    this.Controls.Add(this.ResultsListBox);
    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.Name = "ThreadsForm";
    this.Text = "SynchronizationContext Sample";
    this.ResumeLayout(false);
}
private System.Windows.Forms.ListBox ResultsListBox;
private System.Windows.Forms.Button RegularThreadsButton;
private System.Windows.Forms.Button UIThreadTest;
```

C#

Copy

Now go to the form code, and add the following,

```csharp
private void RegularThreadsButton_Click(object sender, EventArgs e) {
    RunThreads(null);
}
private void UIThreadTest_Click(object sender, EventArgs e) {
    // SynchronizationContext.Current will return a reference to WindowsFormsSynchronizationContext
    RunThreads(SynchronizationContext.Current);
}
private void RunThreads(SynchronizationContext context) {
    this.ResultsListBox.Items.Clear();
    this.ResultsListBox.Items.Add($ "UI Thread {Thread.CurrentThread.ManagedThreadId}");
    this.ResultsListBox.Items.Clear();
    int maxThreads = 3;
    for (int i = 0; i < maxThreads; i++) {
        Thread t = new Thread(UpdateListBox);
        t.IsBackground = true;
        t.Start(context); // passing context to thread proc
    }
}
private void UpdateListBox(object state) {
    // fetching passed SynchrnozationContext
    SynchronizationContext syncContext = state as SynchronizationContext;
    // get thread ID
    var threadId = Thread.CurrentThread.ManagedThreadId;
    if (null == syncContext) // no SynchronizationContext provided
        this.ResultsListBox.Items.Add($ "Hello from thread {threadId}, currently executing thread is {Thread.CurrentThread.ManagedThreadId}");
    else syncContext.Send((obj) => this.ResultsListBox.Items.Add($ "Hello from thread {threadId}, currently executing thread is {Thread.CurrentThread.ManagedThreadId}"), null);
}
```

C#

Copy

The above code simply fires 3 threads that just add records to the list box that state current calling and executing threads. Now run the code in **debug mode** and hit the “Regular Threads Test” button. The execution will pause, and you will get the following exception,

The trick is here. By default, we cannot access other thread’s data (in this case, the controls) from any other thread. When you do, you receive a **cross-thread operation error** wrapped in an **InvalidOperationException** exception.

_A little note here, WinForms will ignore this exception when you are working out of debugging mode. To enable this exception at all times, add the following line to the Main() method before Application.Run()._

```csharp
Control.CheckForIllegalCrossThreadCalls = true;
```

C#

Copy

Now, run the application again and hit the “UI-Context Thread Test” button.

As the code passes the execution to the main UI thread through _SynchronizationContext.Send()_, we can see now in the results that caller threads are different, however, the work has been passed to the main thread, thread 1, which has handled the code successfully.

## Tasks and Synchronization Context

Synchronization context is one core part of the async/await pattern. When you await a task, you suspend the execution of the current async method until the execution of the given task completes.

Let us dig into more details of the above illustration. _await_ does not just wait for the worker thread to finish! Roughly what happens is that _await_ captures the current synchronization context before performing the asynchronous task (**leaving** the current synchronization context.) After the asynchronous task returns, it references the original synchronization context again (**re-entering** the synchronization context) and the rest of the method continuous.

Let us see this in action. Fire a new project or add a new form to the existing project. Go to the designer code of the new form and update it to match the following,

```csharp
private void InitializeComponent() {
    this.ResultsListBox = new System.Windows.Forms.ListBox();
    this.NoContextButton = new System.Windows.Forms.Button();
    this.UIContextButton = new System.Windows.Forms.Button();
    this.SuspendLayout();
    //
    // ResultsListBox
    //
    this.ResultsListBox.FormattingEnabled = true;
    this.ResultsListBox.Location = new System.Drawing.Point(13, 13);
    this.ResultsListBox.Name = "ResultsListBox";
    this.ResultsListBox.Size = new System.Drawing.Size(429, 264);
    this.ResultsListBox.TabIndex = 0;
    //
    // NoContextButton
    //
    this.NoContextButton.Location = new System.Drawing.Point(13, 284);
    this.NoContextButton.Name = "NoContextButton";
    this.NoContextButton.Size = new System.Drawing.Size(429, 23);
    this.NoContextButton.TabIndex = 1;
    this.NoContextButton.Text = "Task without Synchronization Context";
    this.NoContextButton.UseVisualStyleBackColor = true;
    this.NoContextButton.Click += new System.EventHandler(this.NoContextButton_Click);
    //
    // UIContextButton
    //
    this.UIContextButton.Location = new System.Drawing.Point(13, 313);
    this.UIContextButton.Name = "UIContextButton";
    this.UIContextButton.Size = new System.Drawing.Size(429, 23);
    this.UIContextButton.TabIndex = 2;
    this.UIContextButton.Text = "Task with UI Synchronization Context";
    this.UIContextButton.UseVisualStyleBackColor = true;
    this.UIContextButton.Click += new System.EventHandler(this.UIContextButton_Click);
    //
    // TasksForm
    //
    this.AutoScaleDimensions = new System.Drawing.SizeF(6 F, 13 F);
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    this.ClientSize = new System.Drawing.Size(454, 345);
    this.Controls.Add(this.UIContextButton);
    this.Controls.Add(this.NoContextButton);
    this.Controls.Add(this.ResultsListBox);
    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.Name = "TasksForm";
    this.Text = "TasksForm";
    this.ResumeLayout(false);
}
private System.Windows.Forms.ListBox ResultsListBox;
private System.Windows.Forms.Button NoContextButton;
private System.Windows.Forms.Button UIContextButton;
```

C#

Copy

Now, go to the form code and add the following,

```csharp
private void NoContextButton_Click(object sender, EventArgs e) {
    RunTask(null);
}
private void UIContextButton_Click(object sender, EventArgs e) {
    RunTask(SynchronizationContext.Current);
}
private void RunTask(SynchronizationContext context) {
    this.ResultsListBox.Items.Clear();
    this.ResultsListBox.Items.Add($ "UI Thread {Thread.CurrentThread.ManagedThreadId}");
    Task.Run(async () => {
        if (null != context) SynchronizationContext.SetSynchronizationContext(context);
        LogMessage($ "Task started");
        if (null == SynchronizationContext.Current) LogMessage($ "Task synchronization context is null");
        else LogMessage($ "Task synchronization context is {SynchronizationContext.Current.GetType().Name}");
        await Task.Delay(1000);
        LogMessage($ "Task thread is {Thread.CurrentThread.ManagedThreadId}");
        LogMessage($ "Control.InvokeRequired = {this.ResultsListBox.InvokeRequired}");
        LogMessage($ "Trying to manipulate UI...");
        try {
            this.ResultsListBox.Items.Add("Successfully accessed UI directly!");
        } catch (InvalidOperationException) {
            LogMessage($ "Failed!");
        }
        LogMessage($ "Task finished");
    });
}
private void LogMessage(string msg) {
    this.ResultsListBox.Invoke((Action)(() => {
        this.ResultsListBox.Items.Add(msg);
    }));
}
```

C#

Copy

The above code simply has two options, one that does not set the synchronization context of the task, leaving it null, and one sets it to the synchronization context of the UI thread. The code awaits a task and tests UI accessibility from the current thread. When we run the application and hit the no-context button, we get the following results,

The code after _await_ is running in a different thread, and it failed to access the controls directly. Now, hit the UI-context button and see the results,

The second option simply sets the synchronization context to the UI-thread synchronization context using a call to _SynchronizationContext.SetSynchronizationContext()_. And that affected our behavior, when we called _await_ it captured the current synchronization context (which is _WinFormsSynchronizationContext_), then **left** the current context to the task given and waited for its completion. After the completion of the task, it **re-entered** the current context again, and you have been able to access the UI controls using the UI thread without any delegates or callbacks.

A little note here, you might ask yourself why we had to use _SetSynchronizationContext()?!_ Isn’t _await_ supposed to capture the synchronization context automatically? Yes, it is. But as we are running in the context of a new task (we used _Task.Run()_), it does not have a synchronization context. By default, worker tasks and threads do not have a synchronization context (you can investigate this by checking _SynchronizationContext.Current_.) That is why we had to reference the UI context first before our call to _Task.Run()_ then we had to set it using _SetSynchronizationContext()_. In promise-style tasks and outside _Task.Run()_ you may use the _ConfigureAwait()_ option, explained in a moment.

## ConfigureAwait in Action

One of the core concepts of synchronization context is **context switching**. It is what happens when you await a task. You **capture** the current context before awaiting the task, **leaving** it to the task context, then **recovering** (**re-entering**) it back when the task completes. This process is highly expensive and in many scenarios, you do not need it! As an example, if you are not handling UI-controls after the task, why switching to the original context again? Why don’t you save time and avoid this round?

The rule of thumb says that if you are developing a library, or you do not need access to the UI controls, or you can reference synchronization data (like _HttpContext.Current_) for later usage, save your time and effort and disable context switching.

Here comes _Task.ConfigureAwait()_ in handy. It has a single parameter, _continueOnCapturedContext_, which enables context recovering if set to true (default behavior if _ConfigureAwait()_ is not used) or disables it when set to false.

Let us see this in action.

### ConfigureAwait in WinForms

Fire a new WinForms project or add a new form to the existing one. Switch to the form designer code and update it to match the following,

```csharp
private void InitializeComponent() {
    this.ResultsListBox = new System.Windows.Forms.ListBox();
    this.ConfigureTrueButton = new System.Windows.Forms.Button();
    this.ConfigureFalseButton = new System.Windows.Forms.Button();
    this.SuspendLayout();
    //
    // ResultsListBox
    //
    this.ResultsListBox.FormattingEnabled = true;
    this.ResultsListBox.Location = new System.Drawing.Point(12, 12);
    this.ResultsListBox.Name = "ResultsListBox";
    this.ResultsListBox.Size = new System.Drawing.Size(517, 342);
    this.ResultsListBox.TabIndex = 0;
    //
    // ConfigureTrueButton
    //
    this.ConfigureTrueButton.Location = new System.Drawing.Point(12, 357);
    this.ConfigureTrueButton.Name = "ConfigureTrueButton";
    this.ConfigureTrueButton.Size = new System.Drawing.Size(516, 23);
    this.ConfigureTrueButton.TabIndex = 1;
    this.ConfigureTrueButton.Text = "Task.ConfigureAwait(true) Test";
    this.ConfigureTrueButton.UseVisualStyleBackColor = true;
    this.ConfigureTrueButton.Click += new System.EventHandler(this.ConfigureTrueButton_Click);
    //
    // ConfigureFalseButton
    //
    this.ConfigureFalseButton.Location = new System.Drawing.Point(12, 386);
    this.ConfigureFalseButton.Name = "ConfigureFalseButton";
    this.ConfigureFalseButton.Size = new System.Drawing.Size(516, 23);
    this.ConfigureFalseButton.TabIndex = 2;
    this.ConfigureFalseButton.Text = "Task.ConfigureAwait(false) Test";
    this.ConfigureFalseButton.UseVisualStyleBackColor = true;
    this.ConfigureFalseButton.Click += new System.EventHandler(this.ConfigureFalseButton_Click);
    //
    // ConfigureAwaitForm
    //
    this.AutoScaleDimensions = new System.Drawing.SizeF(6 F, 13 F);
    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    this.ClientSize = new System.Drawing.Size(541, 421);
    this.Controls.Add(this.ConfigureFalseButton);
    this.Controls.Add(this.ConfigureTrueButton);
    this.Controls.Add(this.ResultsListBox);
    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.Name = "ConfigureAwaitForm";
    this.Text = "Task.ConfigureAwait Sample";
    this.ResumeLayout(false);
}
private System.Windows.Forms.ListBox ResultsListBox;
private System.Windows.Forms.Button ConfigureTrueButton;
private System.Windows.Forms.Button ConfigureFalseButton;
```

C#

Copy

Now switch to the form code and add the following,

```csharp
private void ConfigureTrueButton_Click(object sender, EventArgs e) {
    AsyncTest(true);
}
private void ConfigureFalseButton_Click(object sender, EventArgs e) {
    AsyncTest(false);
}
private async void AsyncTest(bool configureAwait) {
    this.ResultsListBox.Items.Clear();
    try {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-EG");
        this.ResultsListBox.Items.Add("Async test started");
        this.ResultsListBox.Items.Add(string.Format("configureAwait = {0}", configureAwait));
        this.ResultsListBox.Items.Add(string.Format("Current thread ID = {0}", Thread.CurrentThread.ManagedThreadId));
        this.ResultsListBox.Items.Add(string.Format("Current culture = {0}", Thread.CurrentThread.CurrentCulture));
        this.ResultsListBox.Items.Add("Awaiting a task...");
        await Task.Delay(500).ConfigureAwait(configureAwait);
        this.ResultsListBox.Items.Add("Task completed");
        this.ResultsListBox.Items.Add(string.Format("Current thread ID: {0}", Thread.CurrentThread.ManagedThreadId));
        this.ResultsListBox.Items.Add(string.Format("Current culture: {0}", Thread.CurrentThread.CurrentCulture));
    } catch (InvalidOperationException ex) {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        this.ResultsListBox.BeginInvoke((Action)(() => {
            this.ResultsListBox.Items.Add($ "{ex.GetType().Name} caught from thread {threadId}");
        }));
    }
}
```

C#

Copy

The code simply awaits a task and switches _ConfigureAwait()_ based on the button clicked. It also changes the cultural information of the current thread before the switch. Run the form and click the “ConfigureAwait(true)” button.

The behavior is as expected. We recovered the original synchronization context, we preserved thread environment data like culture, and we were able to access UI controls directly with ease.

Now hit the “ConfigureAwait(false)” button and see the results,

When setting _ConfigureAwait.continueOnCapturedContext_ to false, we were not able to return to the original context, we also received an _InvalidOperationException_ error due to cross-thread access.

### ConfigureAwait in ASP.NET MVC

Fire a new MVC project, and update index_.cshtml_ file to match the following,

```markdown
@model IEnumerable<String>

@if (null != Model && Model.Any())
{
  <ul>
    @foreach (var val in Model)
    {
      <li>@val</li>
    }
  </ul>
}
```

Markdown

Copy

Now, go to the _Home_ controller and add the following code,

```csharp
private List < string > results = new List < string > ();
public async Task < ActionResult > Index(bool configureAwait = false) {
    await AsyncTest(configureAwait);
    return View(results);
}
private async Task AsyncTest(bool configureAwait) {
    results.Add($ "Async test started, ConfigureAwait = {configureAwait}");
    if (null == System.Web.HttpContext.Current) results.Add($ "HttpContext.Current is null");
    else results.Add($ "HttpContext.Current is NOT null");
    results.Add($ "Current thread ID = {Thread.CurrentThread.ManagedThreadId}");
    results.Add("Awaiting task...");
    await Task.Delay(1000).ConfigureAwait(configureAwait);
    results.Add("Task completed");
    results.Add($ "Current thread ID = {Thread.CurrentThread.ManagedThreadId}");
    if (null == System.Web.HttpContext.Current) results.Add($ "HttpContext.Current is null");
    else results.Add($ "HttpContext.Current is NOT null");
}
```

C#

Copy

Run the app and examine the difference between the two scenarios.

You can now see that when setting _ConfigureAwait.continueOnCapturedContext_ to _false_, the original synchronization context is not recovered, and we lost access to _HttpContext.Current_. On the other hand, when set to _true,_ we recover the original synchronization context, and we gain access to _HttpContext.Current_. Note that switching to the original thread is non-relevant here, as there is no UI thread in ASP.NET, unlike desktop apps.

## Final Note

We can sum up all the above in two points:

- For better performance, use _ConfigureAwait(false)_ when in the library or when you do not need to access UI elements after _await_.
- Use _SynchronizationContext.Set()_ when you need to recapture the original context in a thread scenario.

Finally, I hope I was able to simplify things and demonstrate various aspects of the synchronization context. Please feel free to share with me your thoughts and your feedback.

Code is available on [GitHub](https://github.com/elsheimy/Samples.SynchronizationContext).