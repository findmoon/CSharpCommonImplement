**C#中实现只运行一个应用程序（只启动一个进程）的方法：Mutex互斥体的进程间互斥实现，及其最大优点**

[toc]

> 只运行一个程序，只能打开一个程序

# 互斥体的进程间互斥实现

Mutex 对象用于多线程与多进程之间对数据或资源的同步访问，每个调用线程在获取互斥体(mutex)所有权之前都会被阻塞，并且所有获取互斥体所有权的线程在使用完后都，**必须调用`ReleaseMutex`释放**。

> A synchronization primitive that can also be used for interprocess synchronization.

> 同样的原理，系统全局的Semaphore信号灯（命名的），也可以实现只运行一个程序。

如下是 Mutex 进程间互斥实现的单体应用程序。同样，和之前的介绍一样，通过`#define GlobalSingleton`自定义编译符号，实现 全局单体应用 或 文件夹内的单体应用 两种形式。

默认全局整个系统只能运行一个程序。

```C#
#define GlobalSingleton
using System;
using System.Windows.Forms;

namespace SingletonApp_Mutex_Winform
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool ret;

            // 注意 自定义编译符号 GlobalSingleton 实现 全局单体应用 或 文件夹内单体应用，可根据  注释#define GlobalSingleton
#if GlobalSingleton
            // 全局单个应用，当前系统内单个应用
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
#else
            // 当前文件夹内单个应用。不同文件夹，可以打开，相同文件夹只打开一个
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ExecutablePath.Replace("\\",""), out ret);
#endif
            if (ret)
            {
                #region 原有的默认Winform项目生成的代码
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //Application.DoEvents();

                Application.Run(new Form1());
                #endregion

                // 释放全局互斥体 
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
            else
            {
                // 提示信息。
                // 可根据需要修改为 复制 SingletonApp_Winform\Program.cs 文件内的进程获取代码 myProcess = GetRunningInstance();
                // - 并设置窗体前置激活 CallBack myCallBack = new CallBack(FindAppWindow);EnumWindows(myCallBack, 0); 或 HandleRunningInstanceWhnd(myProcess);
                MessageBox.Show(null, "有一个和本程序相同的应用程序已经在运行，请不要同时运行多个程序。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   
                Application.Exit();//退出程序
                return;
            }

        }
    }
}
```

# 互斥体只运行一个应用的最大优点

因为使用的是系统级别的全局互斥体，因此可以做到“真正的只运行一个应用”！

之前介绍的通过遍历进程的方式，由于遍历时需要时间，只要足够快速的瞬间打开多个程序，就可以实现运行多个，而不是只运行一个应用。

因此，**如果想要实现“真正的只运行一个应用”，推荐这种全局互斥体的方式**。

# Mutex 互斥体介绍

> A synchronization primitive that can also be used for interprocess synchronization.

Mutex 表示可以用于线程间或进程间同步的同步基元。

当两个或多个线程需要同时访问共享资源时，系统需要同步机制来确保一次只有一个线程使用资源。Mutex 是一个同步基元，仅向一个线程授予对共享资源的独占访问权限。如果一个线程获取了互斥体，则要获取互斥体的第二个线程将暂停，一直到第一个线程释放互斥体。

> **Mutex实现了 IDisposable 接口，应该显式调用 Dispose 方法，或使用 using**

## 本地 Mutex 对象

如下，是官网给出的使用本地 Mutex 对象实现对受保护的资源同步访问的方法，在获取互斥体的所有权之前，调用线程会被阻塞，而拥有互斥体所有权的线程必须调用 ReleaseMutex 释放。

```C#
using System;
using System.Threading;

class Example
{
    // Create a new Mutex. The creating thread does not own the mutex.
    private static Mutex mut = new Mutex();
    private const int numIterations = 1;
    private const int numThreads = 3;

    static void Main()
    {
        // Create the threads that will use the protected resource.
        for(int i = 0; i < numThreads; i++)
        {
            Thread newThread = new Thread(new ThreadStart(ThreadProc));
            newThread.Name = String.Format("Thread{0}", i + 1);
            newThread.Start();
        }

        // The main thread exits, but the application continues to
        // run until all foreground threads have exited.
    }

    private static void ThreadProc()
    {
        for(int i = 0; i < numIterations; i++)
        {
            UseResource();
        }
    }

    // This method represents a resource that must be synchronized
    // so that only one thread at a time can enter.
    private static void UseResource()
    {
        // Wait until it is safe to enter.
        Console.WriteLine("{0} is requesting the mutex", 
                          Thread.CurrentThread.Name);
        mut.WaitOne();

        Console.WriteLine("{0} has entered the protected area", 
                          Thread.CurrentThread.Name);

        // Place code to access non-reentrant resources here.

        // Simulate some work.
        Thread.Sleep(500);

        Console.WriteLine("{0} is leaving the protected area", 
            Thread.CurrentThread.Name);

        // Release the Mutex.
        mut.ReleaseMutex();
        Console.WriteLine("{0} has released the mutex", 
            Thread.CurrentThread.Name);
    }
}
// The example displays output like the following:
//       Thread1 is requesting the mutex
//       Thread2 is requesting the mutex
//       Thread1 has entered the protected area
//       Thread3 is requesting the mutex
//       Thread1 is leaving the protected area
//       Thread1 has released the mutex
//       Thread3 has entered the protected area
//       Thread3 is leaving the protected area
//       Thread3 has released the mutex
//       Thread2 has entered the protected area
//       Thread2 is leaving the protected area
//       Thread2 has released the mutex
```

`ut.WaitOne(1000)` 可以传入参数，指定获取互斥体的超时时间。

WaitOne 用于获取互斥体，一个线程内可以多次调用，但必须多次释放 ReleaseMutex 。

## 本地互斥体 和 系统全局互斥体

互斥体分为两种类型：本地互斥体（未命名）和命名的系统互斥体。

本地 mutex 仅存在于进程中，它可由进程中能够引用互斥体对象(Mutex)的任何线程使用。每个未命名的 Mutex 对象都表示单独的本地互斥体。

如下，表示两个独立的互斥体：

```C#
Mutex mut = new Mutex();
Mutex mut1 = new Mutex();
```

命名的系统互斥体在整个操作系统中可见，可用于同步进程的活动。可以使用接受名称的构造函数创建一个 Mutex 对象，该对象代表命名的系统互斥体。可以创建多个表示同一个名称的系统 Mutex 对象，它们都表示同一个 mutex，也可以使用 OpenExisting 方法打开已存在的命名的系统 mutex。

比如，最开始实现只运行一个程序的代码，就是创建了一个全局的系统互斥体，通过进程间同步，确保只运行一个进程(窗体)。名称为`Application.ProductName`或`Application.ExecutablePath.Replace("\\","")`：

```C#
System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
```

# 参考

[Mutex Class](https://learn.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=netframework-4.6.2)