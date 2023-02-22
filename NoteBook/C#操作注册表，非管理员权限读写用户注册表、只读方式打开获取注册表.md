C#操作注册表，非管理员权限读写用户注册表、只读方式打开获取注册表


# 判断IIS是否安装


```C#
public static bool IisInstalled()
{
    try
    {
        using (RegistryKey iisKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp"))
        {
            return (int)iisKey.GetValue("MajorVersion") >= 6;
        }
    }
    catch
    {
        return false;
    }
}
```