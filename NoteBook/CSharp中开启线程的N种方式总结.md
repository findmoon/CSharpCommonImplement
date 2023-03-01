CSharp中开启线程的N种方式总结

[toc]

# BeginInvoke 异步委托开启线程

```C#
new Action<int,int>((a,b)=>{
    Debug.WriteLine("异步信息");
    Debug.WriteLine(a+b);
}).BeginInvoke(3,4,null,null);
Debug.WriteLine("同步信息");
```

可以通过输出的前后顺序，查看异步执行。当然，也可以判断线程id（`Thread.CurrentThread.ManagedThreadId`）。

# Thread类 新建线程

```C#
Thread t=new Thread(MyMethod);      //创建了线程还未开启
t.Start("启动子线程");   //用来给函数传递参数，开启线程
Debug.WriteLine("同步信息");

//thread开启线程要求：该方法参数只能有一个，且是object类型
static void MyMethod(object state){
    Debug.WriteLine("异步信息，线程ID："+Thread.CurrentThread.ManagedThreadId);
    Debug.WriteLine(state?.toString());
}
```

> 后续的示例，均使用`MyMethod`方法。

# 通过线程池开启线程

`ThreadPool.QueueUserWorkItem`添加到线程池队列中使用线程池的线程异步执行方法。未必会新建线程，但是使用某个线程运行方法。

```C#
ThreadPool.QueueUserWorkItem(MyMethod);
Debug.WriteLine("同步信息");

```

# Task 类

> Task 类未必会切换线程执行，可以通过配置使用`ConfigureAwait(false)`。
> 
> 要具体判断线程id，以确定是否切换了线程。

## new Task 新建Task

```C#
Task t=new Task(()=>{MyMethod(null);});
t.Start();
Debug.WriteLine("同步信息");

```

## Task.Run

```C#
Task.Run(()=>{MyMethod(null);});

Debug.WriteLine("同步信息");

```

## TaskFactory

```C#
TaskFactory tf=new TaskFactory();
tf.StartNew(()=>{MyMethod();});
Debug.WriteLine("同步信息");
```

# 参考

[C#开启线程的四种方式](https://blog.csdn.net/hyy_sui_yuan/article/details/81263281)