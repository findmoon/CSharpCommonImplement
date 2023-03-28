**Understanding SynchronizationContext (Part I)**

[toc]

> [Understanding SynchronizationContext (Part I)](https://www.codeproject.com/articles/31971/understanding-synchronizationcontext-part-i)

## SynchronizationContext - MSDN Lets You Down

I don't know why, but there is really not much information about this new class within the .NET Framework. The MSDN documentation contains very little information on how to use `SynchronizationContext`. Initially, I must say that I had a hard time understanding the reason for this new class and how to use it. After reading a lot on the subject, I finally understood the purpose of this class and how it should be used. I decided to write this article to help other developers understand how to use this class, and what it can and cannot do for you. ([MSDN](http://msdn.microsoft.com/en-us/library/system.threading.synchronizationcontext.aspx))

## Using SynchronizationContext to Marshal Code from One Thread to Another

Let's get some technical points out of the way so we can show how to use this class. A `SynchronizationContext` allows a thread to communicate with another thread. Suppose you have two threads, `Thread1` and `Thread2`. Say, `Thread1` is doing some work, and then `Thread1` wishes to execute code on `Thread2`. One possible way to do it is to ask `Thread2` for its `SynchronizationContext` object, give it to `Thread1`, and then `Thread1` can call `SynchronizationContext.Send` to execute the code on `Thread2`. Sounds like magic... Well, there is a something you should know. Not every thread has a `SynchronizationContext` attached to it. One thread that always has a `SynchronizationContext` is the UI thread.

Who puts the `SynchronizationContext` into the UI thread? Any guesses? Give up? OK, here it is, the first control that is created on the thread places the `SynchronizationContext` into that thread. Normally, it is the first form that gets created. How do I know? Well, I tried it out.

Because my code uses `SynchronizationContext.Current`, let me explain what this `static` property gives us. `SynchronizationContext.Current` allows us to get a `SynchronizationContext` that is attached to the current thread. Let's be clear here, `SynchronizationContext.Current` is not a singleton per AppDomain, but per thread. This means that two threads can have different instances of `SynchronizationContext` when calling `SynchronizationContext.Current`. If you wonder where the actual context is stored, it is stored within the Thread data store (and as I said before, not in the global memory space of the appdomain).

OK, let's look at the code that places a `SynchronizationContext` within our UI thread:

C#

\[STAThread\]
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    // let's check the context here
    var context = SynchronizationContext.Current;
    if (context == null)
        MessageBox.Show("No context for this thread");
    else
        MessageBox.Show("We got a context");

    // create a form
    Form1 form = new Form1();

    // let's check it again after creating a form
    context = SynchronizationContext.Current;

    if (context == null)
        MessageBox.Show("No context for this thread");
    else
        MessageBox.Show("We got a context");

    if (context == null)
        MessageBox.Show("No context for this thread");

    Application.Run(new Form1());
}

As you can see, there are a few points to note:

- The first message box will indicate that there is no context attached to the thread. That's because .NET doesn't even know what is going to happen on this thread, and there is no runtime class that initializes the Sync Context for this thread.
- Right after creating the form, notice that the context is set. The `Form` class is responsible for this. It checks if a Sync Context is present, and if it is not, it places it there. Remember that the context is always the same on the same thread, so any UI control can access it. This is because all UI operations must be running on the UI thread. To be more specific, the thread that creates the window is the thread that can communicate with the window. In our case, it is the main thread of the application.

## How Do I Use It?

Now that the UI thread is nice enough to give us a Sync Context so we can "run code" under the UI thread, how do we use it?

First, do we really have to marshal code into the UI thread? Yes. If you are running on a thread other than the UI thread, you cannot update the UI. Want to be a hero and try it? You will get an exception (in version 1.0, they didn't enforce the exception, it just crashed the application, but in version 2.0, there is a fat ugly exception that pops in your face).

To be fair, I will say that you don't have to use this class to sync into the UI thread. You can use the `InvokeRequired` property (which is on every UI control class) and see if you need to marshal your code. If you get a "`true`" out of `InvokeRequired`, then you have to use `Control.Invoke` to marshal the code to the UI thread. Great! Why keep reading? Well, there is an issue with this technique. You must have a `Control` in order to call `Invoke` on it. It doesn't matter which UI control, but you need at least one control reference available to you within your non-UI thread in order to do this type of thread marshalling. From a design prospective, you never want to have a UI reference within your BI layer. So, you can leave all sync operations on the UI class, and make sure the UI is responsible to marshal its own work (see my article on the MVP pattern). However, this puts more responsibility on the UI, and makes the UI smarter than we want it to be, I must say. It would be nice for the BI to have the ability to marshal code to the UI thread without having a reference to a control or a form.

So, how is it done?

Simple. Create a thread, send it the sync context, and have this thread use the sync object to marshal code into the UI thread. Let's see an example.

In the following example, I have a list box that is populated from a worker thread. The thread simulates a computation and then writes to the UI list box. The thread used to update the UI is launched from the `mToolStripButtonThreads_Click` event handler.

First, let's see what's on the form:

C#

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources =
          new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        this.mListBox = new System.Windows.Forms.ListBox();
        this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        this.mToolStripButtonThreads = new System.Windows.Forms.ToolStripButton();
        this.toolStrip1.SuspendLayout();
        this.SuspendLayout();
        //
        // mListBox
        //
        this.mListBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mListBox.FormattingEnabled = true;
        this.mListBox.Location = new System.Drawing.Point(0, 0);
        this.mListBox.Name = "mListBox";
        this.mListBox.Size = new System.Drawing.Size(284, 264);
        this.mListBox.TabIndex = 0;
        //
        // toolStrip1
        //
        this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem\[\] {
        this.mToolStripButtonThreads});
        this.toolStrip1.Location = new System.Drawing.Point(0, 0);
        this.toolStrip1.Name = "toolStrip1";
        this.toolStrip1.Size = new System.Drawing.Size(284, 25);
        this.toolStrip1.TabIndex = 1;
        this.toolStrip1.Text = "toolStrip1";
        //
        // mToolStripButtonThreads
        //
        this.mToolStripButtonThreads.DisplayStyle =
          System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.mToolStripButtonThreads.Image = ((System.Drawing.Image)
            (resources.GetObject("mToolStripButtonThreads.Image")));
        this.mToolStripButtonThreads.ImageTransparentColor =
             System.Drawing.Color.Magenta;
        this.mToolStripButtonThreads.Name = "mToolStripButtonThreads";
        this.mToolStripButtonThreads.Size = new System.Drawing.Size(148, 22);
        this.mToolStripButtonThreads.Text = "Press Here to start threads";
        this.mToolStripButtonThreads.Click +=
          new System.EventHandler(this.mToolStripButtonThreads\_Click);
        //
        // Form1
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(284, 264);
        this.Controls.Add(this.toolStrip1);
        this.Controls.Add(this.mListBox);
        this.Name = "Form1";
        this.Text = "Form1";
        this.toolStrip1.ResumeLayout(false);
        this.toolStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion 
    private System.Windows.Forms.ListBox mListBox;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton mToolStripButtonThreads;
}

Now, let's see the example:

C#

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void mToolStripButtonThreads\_Click(object sender, EventArgs e)
    {
        // let's see the thread id
        int id = Thread.CurrentThread.ManagedThreadId;
        Trace.WriteLine("mToolStripButtonThreads\_Click thread: " + id);

        // grab the sync context associated to this
        // thread (the UI thread), and save it in uiContext
        // note that this context is set by the UI thread
        // during Form creation (outside of your control)
        // also note, that not every thread has a sync context attached to it.
        SynchronizationContext uiContext = SynchronizationContext.Current;

        // create a thread and associate it to the run method
        Thread thread = new Thread(Run);

        // start the thread, and pass it the UI context,
        // so this thread will be able to update the UI
        // from within the thread
        thread.Start(uiContext);
    }

    private void Run(object state)
    {
        // lets see the thread id
        int id = Thread.CurrentThread.ManagedThreadId;
        Trace.WriteLine("Run thread: " + id);

        // grab the context from the state
        SynchronizationContext uiContext = state as SynchronizationContext;

        for (int i = 0; i < 1000; i++)
        {
            // normally you would do some code here
            // to grab items from the database. or some long
            // computation
            Thread.Sleep(10);

            // use the ui context to execute the UpdateUI method,
            // this insure that the UpdateUI method will run on the UI thread.

            uiContext.Post(UpdateUI, "line " + i.ToString());
        }
    }

    /// <summary\>    /// This method is executed on the main UI thread.    /// </summary\>    private void UpdateUI(object state)
    {
        int id = Thread.CurrentThread.ManagedThreadId;
        Trace.WriteLine("UpdateUI thread:" + id);
        string text = state as string;
        mListBox.Items.Add(text);
    }
}

Let's go over this code. Notice that I log the thread ID of each method so we can review it later.

For example:

C#

// let's see the thread id
int id = Thread.CurrentThread.ManagedThreadId;
Trace.WriteLine("mToolStripButtonThreads\_Click thread: " + id);

When pressing on the toolstrip button, a thread is launched with its delegate pointing to the `Run` method. However, notice that I am passing state to this thread. I am passing the Sync Context of the UI thread by calling:

C#

SynchronizationContext uiContext = SynchronizationContext.Current;

Because I am running on the event handler thread of the toolstrip button, I know I am currently running on the UI thread, and by calling `SynchronizationContext.Current`, I will get the sync context for the UI thread.

`Run` will first grab the `SynchronizationContext` from its state, so it can have the knowledge of how to marshal code into the UI thread.

C#

// grab the context from the state
SynchronizationContext uiContext = state as SynchronizationContext;

The `Run` thread writes 1000 lines into the list box. How? Well, first it uses the `Send` method on the `SynchronizationContext`:

C#

public virtual void Send(SendOrPostCallback d, object state);

Calling `SynchronizationContext.Send` takes two arguments, a delegate pointing to a method and a `state` object. Within our example...

C#

uiContext.Send(UpdateUI, "line " + i.ToString());

... `UpdateUI` is the value we provide for the delegate, and `state` contains the `string` we want to add to the `listbox`. The code in `UpdateUI` is supposed to run on the UI thread, and not on the calling thread.

C#

private void UpdateUI(object state)
{
    int id = Thread.CurrentThread.ManagedThreadId;
    Trace.WriteLine("UpdateUI thread:" + id);
    string text = state as string;
    mListBox.Items.Add(text);
}

Notice that this code directly runs on the UI thread. There is no checking for `InvokerRequired` because I know it is on the UI thread due to the fact that it was used with the `Send` method of the UI `SynchronizationContext`.

Let's look at the thread IDs and see if it makes sense:

mToolStripButtonThreads\_Click thread: 10
Run thread: 3
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
UpdateUI thread:10
... (x1000 times)

This means that the UI thread is 10, the worker thread (`Run`) is 3, and when we update the UI, notice we are on thread ID 10 again (the UI thread). So, everything is working as advertised.

## Error Handling

Very nice, we are able to marshal code into the UI thread, but what happens when the code we marshal throws an exception? Who is responsible to catch it? The UI thread or the worker thread?

C#

private void Run(object state)
{
    // let's see the thread id
    int id = Thread.CurrentThread.ManagedThreadId;
    Trace.WriteLine("Run thread: " + id);

    // grab the context from the state
    SynchronizationContext uiContext = state as SynchronizationContext;

    for (int i = 0; i < 1000; i++)
    {
        Trace.WriteLine("Loop " + i.ToString());
        // normally you would do some code here
        // to grab items from the database. or some long
        // computation
        Thread.Sleep(10);

        // use the ui context to execute the UpdateUI method, this insure that the
        // UpdateUI method will run on the UI thread.

        try
        {
            uiContext.Send(UpdateUI, "line " + i.ToString());
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.Message);
        }
    }
}

/// <summary\> /// This method is executed on the main UI thread. /// </summary\> private void UpdateUI(object state)
{
    throw new Exception("Boom");
}

I modified the code so that the `UpdateUI` method throws an exception:

C#

throw new Exception("Boom");

Also, I have modified the `Run` method to place a `try/catch` around the `Send` method.

C#

try
{
    uiContext.Send(UpdateUI, "line " + i.ToString());
}
catch (Exception e)
{
    Trace.WriteLine(e.Message);
}

When running this code, I noticed that the exception is caught in the `Run` thread and not on the UI thread. This is interesting because you might expect the exception to bring down the UI thread, considering no class is catching the exception on the UI thread.

Therefore, the `Send` method is doing a little magic; it is executing our code in a blocking fashion, and it reports back any exception during its execution.

## Send vs. Post

Using `Send` is only one of two possible methods you can use to marshal code on the UI thread. There is another method called `Post`. What's the difference? A lot!

Maybe it is time to see this class in more detail, so let's review the interface of `SynchronizationContext`:

C#

// Summary:
// Provides the basic functionality for propagating a synchronization context
// in various synchronization models.
public class SynchronizationContext
{
    // Summary:
    // Creates a new instance of the System.Threading.SynchronizationContext class.
    public SynchronizationContext();

    // Summary:
    // Gets the synchronization context for the current thread.
    //
    // Returns:
    // A System.Threading.SynchronizationContext object representing the current
    // synchronization context.
    public static SynchronizationContext Current { get; }

    // Summary:
    // When overridden in a derived class, creates a copy of the synchronization
    // context.
    //
    // Returns:
    // A new System.Threading.SynchronizationContext object.
    public virtual SynchronizationContext CreateCopy();
    //
    // Summary:
    // Determines if wait notification is required.
    //
    // Returns:
    // true if wait notification is required; otherwise, false.
    public bool IsWaitNotificationRequired();
    //
    // Summary:
    // When overridden in a derived class, responds to the notification that an
    // operation has completed.
    public virtual void OperationCompleted();
    //
    // Summary:
    // When overridden in a derived class, responds to the notification that an
    // operation has started.
    public virtual void OperationStarted();
    //
    // Summary:
    // When overridden in a derived class, dispatches an asynchronous message to
    // a synchronization context.
    //
    // Parameters:
    // d:
    // The System.Threading.SendOrPostCallback delegate to call.
    //
    // state:
    // The object passed to the delegate.
    public virtual void Post(SendOrPostCallback d, object state);
    //
    // Summary:
    // When overridden in a derived class, dispatches a synchronous message to a
    // synchronization context.
    //
    // Parameters:
    // d:
    // The System.Threading.SendOrPostCallback delegate to call.
    //
    // state:
    // The object passed to the delegate.
    public virtual void Send(SendOrPostCallback d, object state);
    //
    // Summary:
    // Sets the current synchronization context.
    //
    // Parameters:
    // syncContext:
    // The System.Threading.SynchronizationContext object to be set.
    public static void SetSynchronizationContext(SynchronizationContext syncContext);
    //
    // Summary:
    // Sets notification that wait notification is required and prepares the callback
    // method so it can be called more reliably when a wait occurs.
    protected void SetWaitNotificationRequired();
    //
    // Summary:
    // Waits for any or all the elements in the specified array to receive a signal.
    //
    // Parameters:
    // waitHandles:
    // An array of type System.IntPtr that contains the native operating system
    // handles.
    //
    // waitAll:
    // true to wait for all handles; false to wait for any handle.
    //
    // millisecondsTimeout:
    // The number of milliseconds to wait, or System.Threading.Timeout.Infinite
    // (-1) to wait indefinitely.
    //
    // Returns:
    // The array index of the object that satisfied the wait.
    \[PrePrepareMethod\]
    \[CLSCompliant(false)\]
    public virtual int Wait(IntPtr\[\] waitHandles, bool waitAll, int millisecondsTimeout);
    //
    // Summary:
    // Helper function that waits for any or all the elements in the specified array
    // to receive a signal.
    //
    // Parameters:
    // waitHandles:
    // An array of type System.IntPtr that contains the native operating system
    // handles.
    //
    // waitAll:
    // true to wait for all handles; false to wait for any handle.
    //
    // millisecondsTimeout:
    // The number of milliseconds to wait, or System.Threading.Timeout.Infinite
    // (-1) to wait indefinitely.
    //
    // Returns:
    // The array index of the object that satisfied the wait.
    \[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)\]
    \[PrePrepareMethod\]
    \[CLSCompliant(false)\]
    protected static int WaitHelper(IntPtr\[\] waitHandles,
                     bool waitAll, int millisecondsTimeout);
}

Notice the comment for the `Post` method:

C#

//
// Summary:
// When overridden in a derived class, dispatches an asynchronous message to
// a synchronization context.
//
// Parameters:
// d:
// The System.Threading.SendOrPostCallback delegate to call.
//
// state:
// The object passed to the delegate.
public virtual void Post(SendOrPostCallback d, object state);

The key word here is **asynchronous**. This means that `Post` will not wait for the execution of the delegate to complete. `Post` will "Fire and Forget" about the execution code within the delegate. It also means that you cannot catch exceptions as we did with the `Send` method. Suppose an exception is thrown, it will be the UI thread that will get it; unhanding the exception will terminate the UI thread.

However, `Post` or `Send`, the execution of the delegate always runs on the correct thread. Just replace the code with `Post` instead of `Send`, and you will still get the right thread ID when executing on the UI thread.

## So Now, I Can Use SynchronizationContext to Sync Any Thread I Want, Right? Nope!

At this point, you might try to use `SynchronizationContext` with any thread. However, you will soon find that your thread does not have a `SynchronizationContext` when using `SynchronizationContext.Current`, and it always returns `null`. No big deal you say, and you simply create a `SynchronizationContext` if there isn't one. Simple. But, it does not really work.

Let's look at a program similar to the one we used for the UI thread:

C#

class Program
{
    private static SynchronizationContext mT1 = null;

    static void Main(string\[\] args)
    {
        // log the thread id
        int id = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine("Main thread is " + id);

        // create a sync context for this thread
        var context = new SynchronizationContext();
        // set this context for this thread.
        SynchronizationContext.SetSynchronizationContext(context);

        // create a thread, and pass it the main sync context.
        Thread t1 = new Thread(new ParameterizedThreadStart(Run1));
        t1.Start(SynchronizationContext.Current);
        Console.ReadLine();
    }

    static private void Run1(object state)
    {
        int id = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine("Run1 Thread ID: " + id);

        // grab  the sync context that main has set
        var context = state as SynchronizationContext;

        // call the sync context of main, expecting
        // the following code to run on the main thread
        // but it will not.
        context.Send(DoWork, null);

        while (true)
            Thread.Sleep(10000000);
    }

    static void DoWork(object state)
    {
        int id = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine("DoWork Thread ID:" + id);
    }
}

This simple console application is something you should not do at home. This program does not work, and it is done simply to prove a point. Notice I am setting a Sync Context on the main console thread. I am simply creating a new instance of the object. Then, I am setting it to my current thread. This is similar to what the UI thread does when a form is created (not really, but I will explain later). Then, I create a thread `Run1`, and pass it the context of the main thread. When I try to call `Send`, based on my trace, I notice `Send` was called on the `Run1` thread and not on the main thread as we may expect. Here is the output:

Main thread is 10
Run1 Thread ID: 11
DoWork Thread ID:11

Notice that `DoWork` is executed on thread 11, the same thread as `Run1`. Not much of a `SynchronizationContext` into the main thread. Why? What's going on? Well... This is the part when you realize that nothing is for free in life. Threads can't just switch contexts between them, they must have an infrastructure built-in into them in order to do so. The UI thread, for example, uses a message pump, and within its `SynchronizationContext`, it leverages the message pump to sync into the UI thread.

So, the UI thread has it own `SynchronizationContext` class, but it is a class that derives from `SynchronizationContext`, and it is called `System.Windows.Forms.WindowsFormsSynchronizationContext`. Now, this class has a very different implementation from the simple plain vanilla `SynchronizationContext`. The UI version overrides the `Post` and `Send` methods, and provides a "message pump" version of these methods (I tried to get the source code of this class, but I didn't find it). So, what does the plain vanilla `SynchronizationContext` do?

Somehow, I was able to get the source code of `SynchronizationContext`, and here it is: [found it here](http://www.koders.com/csharp/fid874F22E35DC2F0B76C07FACB4F4CFDE95C28D488.aspx).

(I have removed the attributes, and did some minor formatting just to have the code fit in the page.)

C#

namespace System.Threading
{
    using Microsoft.Win32.SafeHandles;
    using System.Security.Permissions;
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Reflection;

    internal struct SynchronizationContextSwitcher : IDisposable
    {
        internal SynchronizationContext savedSC;
        internal SynchronizationContext currSC;
        internal ExecutionContext \_ec;

        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is SynchronizationContextSwitcher))
                return false;
            SynchronizationContextSwitcher sw = (SynchronizationContextSwitcher)obj;
            return (this.savedSC == sw.savedSC &&
                    this.currSC == sw.currSC && this.\_ec == sw.\_ec);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(SynchronizationContextSwitcher c1,
                                       SynchronizationContextSwitcher c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(SynchronizationContextSwitcher c1,
                                       SynchronizationContextSwitcher c2)
        {
            return !c1.Equals(c2);
        }

        void IDisposable.Dispose()
        {
            Undo();
        }

        internal bool UndoNoThrow()
        {
            if (\_ec  == null)
            {
                return true;
            }

            try
            {
                Undo();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Undo()
        {
            if (\_ec  == null)
            {
                return;
            }

            ExecutionContext  executionContext =
              Thread.CurrentThread.GetExecutionContextNoCreate();
            if (\_ec != executionContext)
            {
                throw new InvalidOperationException(Environment.GetResourceString(
                          "InvalidOperation\_SwitcherCtxMismatch"));
            }
            if (currSC != \_ec.SynchronizationContext)
            {
                throw new InvalidOperationException(Environment.GetResourceString(
                          "InvalidOperation\_SwitcherCtxMismatch"));
            }
            BCLDebug.Assert(executionContext != null, " ExecutionContext can't be null");
            // restore the Saved Sync context as current
            executionContext.SynchronizationContext = savedSC;
            // can't reuse this anymore
            \_ec = null;
        }
    }

    public delegate void SendOrPostCallback(Object state);

    \[Flags\]
    enum SynchronizationContextProperties
    {
        None = 0,
        RequireWaitNotification = 0x1
    };

    public class SynchronizationContext
    {
        SynchronizationContextProperties \_props = SynchronizationContextProperties.None;

        public SynchronizationContext()
        {
        }

        // protected so that only the derived sync
        // context class can enable these flags
        protected void SetWaitNotificationRequired()
        {
            // Prepare the method so that it can be called
            // in a reliable fashion when a wait is needed.
            // This will obviously only make the Wait reliable
            // if the Wait method is itself reliable. The only thing
            // preparing the method here does is to ensure there
            // is no failure point before the method execution begins.

            RuntimeHelpers.PrepareDelegate(new WaitDelegate(this.Wait));
            \_props |= SynchronizationContextProperties.RequireWaitNotification;
        }

        public bool IsWaitNotificationRequired()
        {
            return ((\_props &
              SynchronizationContextProperties.RequireWaitNotification) != 0);
        }

        public virtual void Send(SendOrPostCallback d, Object state)
        {
            d(state);
        }

        public virtual void Post(SendOrPostCallback d, Object state)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(d), state);
        }

        public virtual void OperationStarted()
        {
        }

        public virtual void OperationCompleted()
        {
        }

        // Method called when the CLR does a wait operation
        public virtual int Wait(IntPtr\[\] waitHandles,
                       bool waitAll, int millisecondsTimeout)
        {
            return WaitHelper(waitHandles, waitAll, millisecondsTimeout);
        }

        // Static helper to which the above method
        // can delegate to in order to get the default
        // COM behavior.
        protected static extern int WaitHelper(IntPtr\[\] waitHandles,
                         bool waitAll, int millisecondsTimeout);

        // set SynchronizationContext on the current thread
        public static void SetSynchronizationContext(SynchronizationContext syncContext)
        {
            SetSynchronizationContext(syncContext,
              Thread.CurrentThread.ExecutionContext.SynchronizationContext);
        }

        internal static SynchronizationContextSwitcher
          SetSynchronizationContext(SynchronizationContext syncContext,
          SynchronizationContext prevSyncContext)
        {
            // get current execution context
            ExecutionContext ec = Thread.CurrentThread.ExecutionContext;
            // create a switcher
            SynchronizationContextSwitcher scsw = new SynchronizationContextSwitcher();

            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                // attach the switcher to the exec context
                scsw.\_ec = ec;
                // save the current sync context using the passed in value
                scsw.savedSC = prevSyncContext;
                // save the new sync context also
                scsw.currSC = syncContext;
                // update the current sync context to the new context
                ec.SynchronizationContext = syncContext;
            }
            catch
            {
                // Any exception means we just restore the old SyncCtx
                scsw.UndoNoThrow(); //No exception will be thrown in this Undo()
                throw;
            }
            // return switcher
            return scsw;
        }

        // Get the current SynchronizationContext on the current thread
        public static SynchronizationContext Current
        {
            get
            {
                ExecutionContext ec = Thread.CurrentThread.GetExecutionContextNoCreate();
                if (ec != null)
                    return ec.SynchronizationContext;
                return null;
            }
        }

        // helper to Clone this SynchronizationContext,
        public virtual SynchronizationContext CreateCopy()
        {
            // the CLR dummy has an empty clone function - no member data
            return new SynchronizationContext();
        }

        private static int InvokeWaitMethodHelper(SynchronizationContext syncContext,
            IntPtr\[\] waitHandles,
            bool waitAll,
            int millisecondsTimeout)
        {
            return syncContext.Wait(waitHandles, waitAll, millisecondsTimeout);
        }
    }
}

Look at the implementation of `Send` and `Post`...

C#

public virtual void Send(SendOrPostCallback d, Object state)
{
    d(state);
}

public virtual void Post(SendOrPostCallback d, Object state)
{
    ThreadPool.QueueUserWorkItem(new WaitCallback(d), state);
}

`Send` simply calls the delegate on the calling thread (no thread switching of any kind), and `Post` does the same thing, but simply uses the `ThreadPool` to do it in an async fashion. In my opinion, this class should be `abstract`. The default implementation of this class is confusing and useless. It is one of two reasons I decided to write this article.

## Conclusion

I hope you know more about this class now, and you understand how to use it. Within .NET, I found two classes that provide a custom synchronization. One for the WinForms thread context and one for WPF thread context. I am sure there are more, but these are the ones I found so far. The default implementation of the class, as I showed you, does nothing to switch code from one thread to another. This is simply because threads, by default, do not have this type of mechanism. UI threads, on the other hand, have a message pump and Windows APIs such as `SendMessage` and `PostMessage` that I am sure are used when marshalling code to the UI thread.

However, this should not be the end of the road for this class. You can make your own `SynchronizationContext`, it is really simple. In fact, I had to write one. At my work, we needed to have all COM based calls executed on an STA thread. However, our application is using the thread pool and WCF, and it was not simple to just marshal code into an STA thread. Therefore, I decided to code my own version of `SynchronizationContext` called `StaSynchronizationContext`. I will show how I did it in part II of this article.

Happy .NETting.

## License

This article, along with any associated source code and files, is licensed under [The Code Project Open License (CPOL)](http://www.codeproject.com/info/cpol10.aspx)