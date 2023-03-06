**C#使用SystemEvents.SessionEnding监听Windows系统关闭或注销**



```C#
SystemEvents.SessionEnding += SystemEvents_SessionEnding;

// .........

// 监听Windows系统关闭还是注销
private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
{
    switch (e.Reason)
    {
        case Microsoft.Win32.SessionEndReasons.Logoff:  // 当前用户正在注销
            break;
        case Microsoft.Win32.SessionEndReasons.SystemShutdown:  // 系统正在关闭
            break;
        default:
            break;
    }
}
```