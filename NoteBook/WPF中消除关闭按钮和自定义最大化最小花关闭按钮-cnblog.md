**C#/WPF中彻底消除隐藏关闭按钮，及自定义最大化最小化关闭按钮**

[toc]

> 由于是 win32 API 的操作，因此同样适用于Winform。

# 消除隐藏关闭按钮

对关闭按钮的隐藏消除，是通过win32 API实现的。具体使用如下所示。

## SetWindowLongPtr API

`SetWindowLongPtr` 是Windows的一个API，作用是改变窗口的属性。

```C#
LONG_PTR WINAPI SetWindowLongPtr(
  _In_ HWND     hWnd,
  _In_ int      nIndex,
  _In_ LONG_PTR dwNewLong
);
```

第一个参数是窗口句柄。第二个参数是offset，当它的值是`GWL_STYLE`（-16）时，可以设置窗口风格。第三个参数是要设置的值，针对`GWL_STYLE`的可能取值参见[Window Styles](https://msdn.microsoft.com/en-us/library/windows/desktop/ms632600%28v=vs.85%29.aspx)，有关 最大化（`WS_MAXIMIZEBOX=0x00010000L`） 、最小化按钮（`WS_MINIMIZEBOX=0x00020000L`） 和 窗口标题栏上的菜单（`WS_SYSMENU=0x00080000L`）。可以通过设置这对应的值去掉最大化、最小化和关闭按钮。

如果仅仅设置最大化或最小化按钮，将会使其禁用变灰，而不是隐藏。只有同时设置最大化和最小化按钮时，才会将两者去掉。

在调用`SetWindowLongPtr`之前要调用`GetWindowLongPtr`来获取当前Window的信息。

如果窗体数据被缓存，则在调用`SetWindowLongPtr`之后，需要调用`SetWindowPos`确保设置生效。

> 官网介绍`SetWindowLongPtr`(但，实际 在 64位Windows上的32位C#程序中引入`SetWindowLongPtr`会报错，需要根据位数判断使用`SetWindowLongPtr`还是`SetWindowLong`，`GetWindowLongPtr`同理。)：
> 
> To write code that is compatible with both 32-bit and 64-bit versions of Windows, use GetWindowLongPtr. When compiling for 32-bit Windows, GetWindowLongPtr is defined as a call to the GetWindowLong function.
> 
> `SetWindowLongPtr` 用于替代 `SetWindowLong`，它可以编写 32 位和 64 位Windows兼容的代码。对于 32 位 Windows，SetWindowLongPtr 内部直接调用 SetWindowLong 函数。

## SetWindowLongPtr 设置最大化最小化关闭按钮

如下，在 加载后的事件 或 激活事件 中，调用 `SetWindowLongPtr` API 设置隐藏最大化最小化关闭按钮。

```C#
#region 去除隐藏最大化最小化关闭按钮
private const int GWL_STYLE = -16;

// 窗口标题栏上的菜单【最大最小关闭】
private const int WS_SYSMENU = 0x80000;
// 最大化按钮
private const int WS_MAXIMIZEBOX = 0x10000;
// 最小化按钮
private const int WS_MINIMIZEBOX = 0x20000;

private const int SWP_NOMOVE = 0x0002;
private const int SWP_NOSIZE = 0x0001;
private const int SWP_NOZORDER = 0x0004;
private const int SWP_FRAMECHANGED = 0x0020;

[DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);
[DllImport("user32.dll")]
extern private static int GetWindowLongPtr(IntPtr hWnd, int nindex);

[DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
extern private static int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);
[DllImport("user32.dll")]
private static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);

[DllImport("user32.dll")]
public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

/// <summary>
/// 隐藏最小化最大化按钮，等同 ResizeMode.NoResize
/// </summary>
/// <param name="hwnd"></param>
private static void HideMinMaxButtons(IntPtr hwnd)
{
    if (IntPtr.Size == 4)
    {
        var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
        SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
    }
    else
    {
        var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
        SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
    }
    //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
    SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
}
/// <summary>
/// 隐藏最小化最大化关闭按钮
/// </summary>
/// <param name="hwnd"></param>
private static void HideMinMaxCloseButtons(IntPtr hwnd)
{
    if (IntPtr.Size == 4)
    {
        var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
        SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
    }
    else
    {
        var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
        SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
    }
    //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
    SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
}
/// <summary>
/// [禁用]最小化按钮 变非常灰 
/// </summary>
/// <param name="hwnd"></param>
private static void DisableMinButton(IntPtr hwnd)
{
    if (IntPtr.Size == 4)
    {
        var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
        SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_MINIMIZEBOX);
    }
    else
    {
        var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
        SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_MINIMIZEBOX);
    }
    //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
    SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
}
/// <summary>
/// [禁用]最大化按钮 变非常灰
/// </summary>
/// <param name="hwnd"></param>
private static void DisableMaxButton(IntPtr hwnd)
{
    if (IntPtr.Size == 4)
    {
        var currentStyle = GetWindowLong32(hwnd, GWL_STYLE);
        SetWindowLong32(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX);
    }
    else
    {
        var currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
        SetWindowLongPtr(hwnd, GWL_STYLE, currentStyle & ~WS_MAXIMIZEBOX);
    }
    //call SetWindowPos to make sure the SetWindowLongPtr take effect according to MSDN
    SetWindowPos(hwnd, 0, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
}
#endregion
```

> 隐藏按钮后，仍然可以通过 Alt+F4 关闭窗口，因此要禁用关闭操作，还需要重载`OnClosing`函数，或者，`Closing`事件中处理。

## 去掉最大化最小化关闭按钮的测试

### 隐藏最大化最小化关闭按钮

```C#
Activated += MainWindow_Activated;
//Loaded += MainWindow_Loaded;

/// ......

/// <summary>
/// MainWindow_Loaded MainWindow_Activated 等事件中执行，会在按钮显示后再消失(隐藏)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void MainWindow_Activated(object sender, EventArgs e)
{
    var hwnd = new WindowInteropHelper(this).Handle;
    HideMinMaxCloseButtons(hwnd);
}
```

### 隐藏最大化最小化按钮

```C#
/// <summary>
/// MainWindow_Loaded MainWindow_Activated 等事件中执行，会在按钮显示后再消失(隐藏)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void MainWindow_Activated(object sender, EventArgs e)
{
    var hwnd = new WindowInteropHelper(this).Handle;
    HideMinMaxButtons(hwnd);
}
```

### 禁用最大化按钮

```C#
/// <summary>
/// MainWindow_Loaded MainWindow_Activated 等事件中执行，会在按钮显示后再消失(隐藏)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void MainWindow_Activated(object sender, EventArgs e)
{
    var hwnd = new WindowInteropHelper(this).Handle;;
    DisableMaxButton(hwnd);
}
```

### 禁用最小化按钮

```C#
/// <summary>
/// MainWindow_Loaded MainWindow_Activated 等事件中执行，会在按钮显示后再消失(隐藏)
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void MainWindow_Activated(object sender, EventArgs e)
{
    var hwnd = new WindowInteropHelper(this).Handle;;
    DisableMinButton(hwnd);
}
```

## 禁用关闭按钮（变灰）

首先引入需要的 API：

```C#
[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
private static extern IntPtr GetSystemMenu(IntPtr hWnd, UInt32 bRevert);
[DllImport("USER32.DLL ", CharSet = CharSet.Unicode)]
private static extern UInt32 RemoveMenu(IntPtr hMenu, UInt32 nPosition, UInt32 wFlags);
private const UInt32 SC_CLOSE = 0x0000F060;
private const UInt32 MF_BYCOMMAND = 0x00000000;
```

同样，在 加载后事件 中调用如下方法，禁用关闭按钮

```C#
var hwnd = new WindowInteropHelper(this).Handle;  //获取window的句柄
IntPtr hMenu = GetSystemMenu(hwnd, 0);
RemoveMenu(hMenu, SC_CLOSE, MF_BYCOMMAND);
```

# 参考

- [WPF中如何禁用/去除窗口右上角的关闭按钮](https://www.cnblogs.com/khler/archive/2009/11/26/1611446.html)
- [如何去掉WinForm或者WPF的最大化和最小化按钮](http://fresky.github.io/2015/07/14/how-to-remove-the-maximize-and-minimize-button/)