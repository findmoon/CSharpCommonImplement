**C#打开外部exe程序或显示前置已打开的窗体【查找窗体、显示窗体、前置窗体】**


C#打开外部exe程序，并判断是否程序已开启（此处判断的是窗体是否存在，也可以改为该程序的进程是否存在），未开启的话打开程序，已经在运行了就显示并前置窗体。

```cs
[DllImport("user32.dll ")]
private static extern bool SetForegroundWindow(IntPtr hWnd);
 
[DllImport("user32.dll")]
private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
 
//根据任务栏应用程序显示的名称找窗口的名称
[DllImport("User32.dll", EntryPoint = "FindWindow")]
private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
private const int SW_RESTORE = 9;
 
// execPath 程序路径
// windowTitle 窗口名称(标题)
private void OpenSDRSharp(string execPath,string windowTitle)
{
 
   //查找状态中的窗口名称来查看目标程序是否在运行运行则前置否则打开
   IntPtr findPtr = FindWindow(null, windowTitle);
   if (findPtr.ToInt32() != 0)
   {
      ShowWindow(findPtr, SW_RESTORE); //将窗口还原，如果不用此方法，缩小的窗口不能激活
      SetForegroundWindow(findPtr);//将指定的窗口选中(激活)
    }
    else
    {
         System.Diagnostics.Process.Start(execPath);
    }
}
```

# 参考

[C#设置点击打开外部exe程序，并判断是否程序已开启，未开启的话打开，已经在运行了就前置](https://blog.csdn.net/u010458948/article/details/101509127)