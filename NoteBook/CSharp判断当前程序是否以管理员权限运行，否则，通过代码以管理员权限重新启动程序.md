**C#判断当前程序是否以管理员权限运行，否则，通过代码以管理员权限重新启动程序【以注册文件类型默认打开程序为例】**

[toc]

在注册文件扩展名默认打开的程序时，需要操作设置注册表，此时就需要管理员权限。但其他时候并不需要管理员权限运行软件。

针对此种情况，有两种解决办法。

- 一是，专门写一个用于`注册扩展名的小程序`（控制台程序即可）。主程序启动时检测是否注册了文件类型，未注册则以管理员权限运行`注册扩展名的程序`。**【推荐】**

- 二是，在主程序启动时检测是否注册了文件类型，未注册，则判断当前程序是否以管理员权限运行，如果以管理员权限运行，则执行文件类型注册；否则以管理员权限重启程序。

**第一种方法是推荐的做法。** 第二种方法，如果管理员权限，不影响软件的后续运行操作，也非常不错。

本篇以第二种情况为例。实现在程序运行时，判断是否管理员权限运行，否则通过Process进程类，以管理员权限重启自己。

# 判断当前程序是否以管理员权限运行

- 判断的代码如下

```C#
//获得当前登录的Windows用户标示 
System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
//创建Windows用户主题
System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
// 判断当前主体是否为管理员权限
if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
{
    // 管理员权限
}
else
{
    // 非管理员权限。以管理员权限重启...
    return;
}
```

- 判断是否为管理员权限运行的方法

```C#
/// <summary>
/// 确定当前主体是否属于具有指定 Administrator 的 Windows 用户组
/// </summary>
/// <returns>如果当前主体是指定的 Administrator 用户组的成员，则为 true；否则为 false。</returns>
public static bool IsAdministrator()
{
    bool result;
    try
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        result = principal.IsInRole(WindowsBuiltInRole.Administrator);

        // 通过线程的CurrentPrincipal获取权限
        //AppDomain domain = Thread.GetDomain();
        //domain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
        //WindowsPrincipal windowsPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
        //result = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    }
    catch
    {
        result = false;
    }
    return result;
}
```

# Process以管理员权限启动一个程序（运行当前程序）

主要通过`ProcessStartInfo`对象的`Verb = "runas"`属性设置，实现以管理员身份运行。

```C#
//创建启动对象
ProcessStartInfo startInfo = new ProcessStartInfo();
// .netfx 默认为true；.net core 默认为false
//startInfo.UseShellExecute = true;
//设置运行文件 
startInfo.FileName = Application.ExecutablePath;
//设置启动参数 
startInfo.Arguments = String.Join(" ", args.Select(a => "\"" + args + "\""));
startInfo.WorkingDirectory = Environment.CurrentDirectory;
//设置启动动作,确保以管理员身份运行 
startInfo.Verb = "runas";
//如果不是管理员，则启动UAC 
Process.Start(startInfo);
```

> 这也是以管理员权限运行的一种实现方式，除了添加应用程序清单文件。

# 程序启动时进行文件类型注册的入口方法处的实现代码

上面介绍的第二种方法的具体代码：

```C#
static class Program
{
    /// <summary>
    /// 应用程序的主入口点。
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        #region 检查文件类型注册，并以管理员权限重新运行 【不太好提取为单一方法】
        var extendName = ".mytxt";

        // 判断注册表数据
        if (!FileTypeRegister.FileTypeRegistered(extendName,true))
        {
            //获得当前登录的Windows用户标示 
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            //创建Windows用户主题 
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            // 判断当前主体是否为管理员权限
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                // 注册文件
                #region 关联文件
                var filetypeRegInfo = new FileTypeRegInfo(extendName, Application.ExecutablePath);
                filetypeRegInfo.Description = Application.ExecutablePath + "打开程序";
                FileTypeRegister.RegisterFileType(filetypeRegInfo);
                #endregion
            }
            else
            {
                //创建启动对象 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                // .netfx 默认为true；.net core 默认为false
                //startInfo.UseShellExecute = true;
                //设置运行文件 
                startInfo.FileName = Application.ExecutablePath;
                //设置启动参数 
                startInfo.Arguments = String.Join(" ", args.Select(a => "\"" + args + "\""));
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                //设置启动动作,确保以管理员身份运行 
                startInfo.Verb = "runas";
                //如果不是管理员，则启动UAC 
                Process.Start(startInfo);
                //退出  // Application.Exit方法不会阻止后面代码的执行，也不会退出，后面代码执行并打开窗体，正常运行。
                Application.Exit();
                return;
            }
        }
        #endregion

        // 处理打开文件时
        string filePath = "";
        if (args.Length==1 && File.Exists(args[0]))
        {
            filePath = args[0];
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1(filePath));
    }

}
```

# 参考

[C#程序以管理员权限运行](https://www.cnblogs.com/Interkey/p/RunAsAdmin.html)