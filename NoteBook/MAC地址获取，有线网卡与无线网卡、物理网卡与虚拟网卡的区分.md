MAC地址获取，有线网卡与无线网卡、物理网卡与虚拟网卡的区分


[MAC地址获取，有线网卡与无线网卡、物理网卡与虚拟网卡的区分](https://blog.csdn.net/Jack_Law/article/details/103781619)

cmd命令行执行：

1. Wmic Path Win32_NetworkAdapter get GUID,MACAddress,NetEnabled,PhysicalAdapter,Index

备注：

GUID:连接唯一标识;

MACAddress:网卡地址;

NetEnabled: 是否启用了适配器，True为启用，False为禁用;

PhysicalAdapter: 适配器是否物理或逻辑适配器,True为物理，False为逻辑;

Index: 网络适配器的索引号,存储在系统注册表中。注册表路径Win32Registry|System\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318}.



2. Wmic Path Win32_NetworkAdapterConfiguration get IPEnabled,MACAddress,SettingID, IPAddress, IPSubnet,Index

备注：

IPEnabled: 是否启用了适配器，True为启用，False为禁用;

MACAddress:网卡地址;

SettingID: 连接唯一标识;

IPAddress:IP地址;

IPSubnet:子网掩码;

Index: Windows网络适配器配置的索引号，在有多个配置时使用。注册表路径Win32Registry|System\\CurrentControlSet\\Control\\Class\\{4D36E972-E325-11CE-BFC1-08002BE10318};


-----------------------------------
----------------------------------------

[WMI 类 Win32_NetworkAdapterConfiguration管理网卡的一个参数介绍](https://blog.csdn.net/qq_36154886/article/details/114537676)

[WMI＿网卡相关对象（一）](https://www.cnblogs.com/aocshallo/articles/Win32_NetworkAdapterConfiguration.html)

[Using PowerShell to Get or Set NetworkAdapterConfiguration-View and Change Network Settings Including DHCP, DNS, IP Address and More (Dynamic AND Static) Step-By-Step](https://itproguru.com/expert/2012/01/using-powershell-to-get-or-set-networkadapterconfiguration-view-and-change-network-settings-including-dhcp-dns-ip-address-and-more-dynamic-and-static-step-by-step/)

[Find only physical network adapters with WMI Win32_NetworkAdapter class](https://weblogs.sqlteam.com/mladenp/2010/11/04/find-only-physical-network-adapters-with-wmi-win32_networkadapter-class/)

[PowerShell Win32_NetworkAdapterConfiguration](https://www.computerperformance.co.uk/powershell/win32-networkadapterconfiguration/)

[Win32_NetworkAdapter 网卡 参数说明](https://blog.csdn.net/yeyingss/article/details/49385435)


[Getting configurations for physical network adapters](https://www.itprotoday.com/devops-and-software-development/getting-configurations-physical-network-adapters)：

```sh
get-wmiobject -class win32_networkadapterconfiguration | where-object { (get-wmiobject -class win32_networkadapter -filter "physicaladapter=true" | select -expand name) -contains $_.description }
```

# 参考

- [Win32_NetworkAdapter 类](https://learn.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-networkadapter?redirectedfrom=MSDN)

- [Win32_NetworkAdapterConfiguration 类](https://learn.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-networkadapterconfiguration)
