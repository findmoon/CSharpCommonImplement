asp.net mvc 中使用async/await异步编程

> [asp.net mvc 中使用async/await异步编程](https://blog.csdn.net/hurrycxd/article/details/79813822)


其中介绍的 ConfigureAwait(false)

**使用Task.ConfigureAwait(false)实现异步**

我们前面说的asp.net mvc中使用异步编程，必须要将返回类型ActionResult改为了Task<ActionResult>。有没有不改返回值Task<ActionResult>的方法呢，设置异步方法的Task为ConfigureAwait(false)，就可以实现，我们在DownLoadTest类中添加以下的异步方法

```csharp
public async Task<string> DownLoadConfigureAwaitAsync(string url){    Debug.WriteLine(string.Format("异步程序获取{0}开始运行:{1,4:N0}ms", url, watch.Elapsed.TotalMilliseconds));    WebClient wc = new WebClient();    var str= await Task.Run(() =>    {        return wc.DownloadString(url);    }).ConfigureAwait(false);    Debug.WriteLine(string.Format("异步程序获取{0}运行结束:{1,4:N0}ms", url, watch.Elapsed.TotalMilliseconds));    return str;}
```

Home控制器中修改DownLoad

```csharp
public ActionResult DownLoad(){    DownLoadTest dwtest = new DownLoadTest();    var task1 = dwtest.DownLoadConfigureAwaitAsync("https://stackoverflow.com/");    var task2 = dwtest.DownLoadConfigureAwaitAsync("https://github.com/");    Debug.WriteLine("task.Result等待结果打印");    Debug.WriteLine("task1.Result.Length=" + task1.Result.Length);    Debug.WriteLine("task2.Result.Length=" + task2.Result.Length);    return Json(new { task1Status = task1.Status, task2Status = task2.Status }, JsonRequestBehavior.AllowGet);}
```

执行程序，Output输出窗口以下信息：

```cobol
异步程序获取https://stackoverflow.com/开始运行:   2ms
异步程序获取https://github.com/开始运行:  12ms
task.Result等待结果打印
异步程序获取https://github.com/运行结束:1,831ms
异步程序获取https://stackoverflow.com/运行结束:2,075ms
task1.Result.Length=255319
task2.Result.Length=52687
```

页面的信息如下：  

```cobol
{"task1Status":5,"task2Status":5}
```