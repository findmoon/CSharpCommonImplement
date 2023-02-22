C#使用WMI操作IIS服务器站点


使用WMI管理配置IIS的官方文档：[Managing Sites with IIS 7.0's WMI Provider](https://learn.microsoft.com/en-us/iis/manage/scripting/managing-sites-with-the-iis-wmi-provider)、[Managing Applications and Application Pools on IIS 7.0 with WMI](https://learn.microsoft.com/en-us/iis/manage/scripting/managing-applications-and-application-pools-on-iis-with-wmi)

[Using WMI to Configure IIS](https://learn.microsoft.com/en-us/previous-versions/iis/6.0-sdk/ms525309(v=vs.90))

更理想的解决方式是用 WMI provider操作IIS 7 ，可参见此篇文章http://msdn.microsoft.com/en-us/library/aa347459.aspx

```C#
//操作增加MIME
IISOle.MimeMapClass NewMime = new IISOle.MimeMapClass();
NewMime.Extension = ".xaml"; NewMime.MimeType = "application/xaml+xml";
IISOle.MimeMapClass TwoMime = new IISOle.MimeMapClass();
TwoMime.Extension = ".xap"; TwoMime.MimeType = "application/x-silverlight-app";
rootEntry.Properties["MimeMap"].Add(NewMime);
rootEntry.Properties["MimeMap"].Add(TwoMime);
rootEntry.CommitChanges();
```


[Get IIS Version using WMI "\\root\\webadministration"](https://social.msdn.microsoft.com/Forums/en-US/cff2848e-eff5-4160-b6fa-2013079eafb3/get-iis-version-using-wmi-quotrootwebadministrationquot?forum=iis56configurationscripting)