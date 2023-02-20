C#实现启动、停止、重启IIS应用服务
using System.Diagnostics;
using System.ServiceProcess;

ServiceController sc = new ServiceController("iisadmin");
if (sc.Status == ServiceControllerStatus.Running)
{
sc.Stop();//停止
}
// ServiceController sc = new ServiceController("iisadmin");
// sc.Start();//启动

Process.Start("iisreset");//重启

主要是Diagnostics、ServiceProcess两个命名空间
————————————————
版权声明：本文为CSDN博主「yxtyxt3311」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
原文链接：https://blog.csdn.net/yxtyxt3311/article/details/4015691