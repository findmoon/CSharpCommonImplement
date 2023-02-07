**c# 获取当前程序运行根目录汇总记录**

很多需要明确含义和不同点，有些混乱。

当前程序文件：`Process.GetCurrentProcess().MainModule.FileName`



//获取绝对路径，调用如

string fileName = string.Format("~/RuleConfigFiles/Campaign_{0}.JSON", CampaignID);
var localFile = Utilities.MapPath(fileName);

 

static public string MapPath(string url)   

{    

if (HttpContext.Current != null)     

　　return HttpContext.Current.Server.MapPath(url);

return System.Web.Hosting.HostingEnvironment.MapPath(url);   

}

 

 

 

// 获取程序的基目录。  

1.System.AppDomain.CurrentDomain.BaseDirectory

// 获取模块的完整路径。

2.System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName


// 获取和设置当前目录(该进程从中启动的目录)的完全限定目录。

3.System.Environment.CurrentDirectory


// 获取应用程序的当前工作目录。

4.System.IO.Directory.GetCurrentDirectory()

// 获取和设置包括该应用程序的目录的名称。

5.System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase

// 获取启动了应用程序的可执行文件的路径。

6.System.Windows.Forms.Application.StartupPath


// 获取启动了应用程序的可执行文件的路径及文件名

7.System.Windows.Forms.Application.ExecutablePath

 

 

 

1、取得控制台应用程序的根目录方法

方法1、Environment.CurrentDirectory 取得或设置当前工作目录的完整限定路径
方法2、AppDomain.CurrentDomain.BaseDirectory 获取基目录，它由程序集冲突解决程序用来探测程序集

2、取得Web应用程序的根目录方法

方法1、HttpRuntime.AppDomainAppPath.ToString();//获取承载在当前应用程序域中的应用程序的应用程序目录的物理驱动器路径。

如string xmlPath = System.Web.HttpRuntime.AppDomainAppPath + "\\ConfigFiles\\BaseConfig.xml";


方法2、Server.MapPath("") 或者 Server.MapPath("~/");//返回与Web服务器上的指定的虚拟路径相对的物理文件路径
方法3、Request.ApplicationPath;//获取服务器上ASP.NET应用程序的虚拟应用程序根目录

3、取得WinForm应用程序的根目录方法

1、Environment.CurrentDirectory.ToString();//获取或设置当前工作目录的完全限定路径
2、Application.StartupPath.ToString();//获取启动了应用程序的可执行文件的路径，不包括可执行文件的名称
3、Directory.GetCurrentDirectory();//获取应用程序的当前工作目录
4、AppDomain.CurrentDomain.BaseDirectory;//获取基目录，它由程序集冲突解决程序用来探测程序集
5、AppDomain.CurrentDomain.SetupInformation.ApplicationBase;//获取或设置包含该应用程序的目录的名称

补充：

以下两个方法可以获取执行文件名称

1、Process.GetCurrentProcess().MainModule.FileName;//可获得当前执行的exe的文件名。
2、Application.ExecutablePath;//获取启动了应用程序的可执行文件的路径，包括可执行文件的名称

# 参考【如下需要进入查看确认和整理】

[C# 获取程序路径的几种方法及其区别](https://blog.csdn.net/u011353510/article/details/122240674) 比较好的文章

[C#获取当前程序所在路径的各种方法](https://www.cnblogs.com/bosins/p/15187947.html)

[c# 获取当前程序运行根目录](https://www.cnblogs.com/itjeff/p/6362368.html)

[C#获取当前程序所在路径的各种方法](https://blog.csdn.net/BeanGo/article/details/128711649)

[C# 获取当前程序运行路径](https://blog.csdn.net/june905206961/article/details/78428551)

[C#获取当前程序运行路径的几种方法](https://www.cnblogs.com/Fooo/p/15711725.html)

[【C#文件操作篇】C#获取当前应用程序所在路径](https://blog.csdn.net/zgscwxd/article/details/86644464)

[C# 获取当前路径7种方法](https://www.cnblogs.com/shiyh/p/10573405.html)

[C# WinForm获取 当前执行程序路径的几种方法](https://blog.csdn.net/bruce135lee/article/details/78732376)