**C#操作注册表，非管理员权限读写用户注册表、只读方式打开获取注册表**

[toc]

# 非管理员权限读写用户注册表



# 非管理员权限 只读获取注册表信息

如果仅仅是 **读取注册表数据**，则本身就不需要管理员权限。

## OpenSubKey 读取注册表

`OpenSubKey`直接读取。第二个参数可写入不设置为true，就不需要管理员权限。

```C#
// 文件扩展名的文件类型
var extendName=".myExt";
using (RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName))
{
    if (softwareKey != null)
    {
        return true;
    }
    var value = (string)softwareKey.GetValue("");
    // if (strictDetect)
    // {
    //     return value == FileTypeRegInfo.GetRelationName(extendName);
    // }
    return !string.IsNullOrWhiteSpace(value);
}
```

## RegistryPermissionAttribute 实现只读方式获取注册表

如下示例所示，之前从网上看到指定`RegistryPermissionAttribute`特性不需要管理员权限读取注册表。实际测试不需要此特性【也许和不同Windows版本有关】。

- 判断IIS的版本、是否安装

```C#
/// <summary>  
/// 获取本地IIS版本  
/// </summary>  
/// <returns></returns>  
[RegistryPermissionAttribute(SecurityAction.PermitOnly, Read = @"HKEY_LOCAL_MACHINE\Software\Microsoft\InetStp")]
public Version GetIISVersion()
{
    using (RegistryKey componentsKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
    {
        // 或者查看 VersionString
        if (componentsKey != null)
        {
            int majorVersion = (int)componentsKey.GetValue("MajorVersion", -1);
            int minorVersion = (int)componentsKey.GetValue("MinorVersion", -1);

            if (majorVersion != -1 && minorVersion != -1)
            {
                return new Version(majorVersion, minorVersion);
            }
        }

        return null;// new Version(0, 0);
    }
}
/// <summary>
/// 判断iis是否安装
/// </summary>
/// <returns></returns>
[RegistryPermissionAttribute(SecurityAction.PermitOnly, Read = @"HKEY_LOCAL_MACHINE\Software\Microsoft\InetStp")]
public bool IISInstalled()
{
    try
    {
        using (RegistryKey iisKey = Registry.
            LocalMachine.
            OpenSubKey(@"Software\Microsoft\InetStp"))
        {
            return (int)iisKey.GetValue("MajorVersion") >= 6;
            // 判断 Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp", "VersionString", null) != null;
        }
    }
    catch (Exception)
    {
        return false;
    }
}
```