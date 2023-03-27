**C# wpf 通过HwndHost渲染视频【好文】**

[toc]

> 原文 [C# wpf 通过HwndHost渲染视频](https://blog.csdn.net/u013113678/article/details/121275982)

# WPF视频渲染系列

[第一章 使用HwndHost渲染视频（本章）](https://blog.csdn.net/u013113678/article/details/121275982)  
[第二章 使用d3d渲染视频](https://blog.csdn.net/u013113678/article/details/121296452)  
[第三章 使用d3d渲染dxva2数据](https://blog.csdn.net/u013113678/article/details/124223161)  
[第四章 使用WriteableBitmap渲染视频](https://blog.csdn.net/u013113678/article/details/121311562)

-

### 文章目录

- [WPF视频渲染系列](https://blog.csdn.net/u013113678/article/details/121275982#WPF_2)
- [前言](https://blog.csdn.net/u013113678/article/details/121275982#_13)
- [一、如何实现](https://blog.csdn.net/u013113678/article/details/121275982#_17)
- [二、使用方式](https://blog.csdn.net/u013113678/article/details/121275982#_91)
- [三、示例](https://blog.csdn.net/u013113678/article/details/121275982#_121)
- [总结](https://blog.csdn.net/u013113678/article/details/121275982#_130)

-

# 前言

日常开发中，特别是音视频开发，需要在界面上渲染视频，比如制作一个播放器、或者视频编辑工具、以及视频会议客户端。通常拿到的是像素格式数据，此时需要渲染到wpf窗口上就需要一定的方法，本文介绍一种通过hwnd渲染的方法，控件既能提供hwnd又能嵌入wpf窗口里。

-

# 一、如何实现

通过继承HwndHost并实现抽象方法即可作为一个带句柄的wpf控件在xaml中使用，代码如下：  
win32Api版本：

```csharp
class NativeHost : HwndHost
{
    new public IntPtr Handle
    {
        get { return (IntPtr)GetValue(HandleProperty); }
        set { SetValue(HandleProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Hwnd.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HandleProperty =
        DependencyProperty.Register("Handle", typeof(IntPtr), typeof(NativeHost), new PropertyMetadata(IntPtr.Zero));
    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        Handle = CreateWindowEx(
            0, "static", "",
            WS_CHILD | WS_VISIBLE | LBS_NOTIFY,
            0, 0,
            (int)Width, (int)Height,
            hwndParent.Handle,
            IntPtr.Zero,
            IntPtr.Zero,
            0);
        return new HandleRef(this, Handle);
    }
    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        DestroyWindow(hwnd.Handle);
    }
    const int WS_CHILD = 0x40000000;
    const int WS_VISIBLE = 0x10000000;
    const int LBS_NOTIFY = 0x001;
    [DllImport("user32.dll")]
    internal static extern IntPtr CreateWindowEx(int exStyle, string className, string windowName, int style, int x, int y, int width, int height, IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, [MarshalAs(UnmanagedType.AsAny)] object pvParam);
    [DllImport("user32.dll")]
    static extern bool DestroyWindow(IntPtr hwnd);
}
```

HwndSource版本：

```csharp
class NativeHost : HwndHost
{
    new public IntPtr Handle
    {
        get { return (IntPtr)GetValue(HandleProperty); }
        set { SetValue(HandleProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Hwnd.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HandleProperty =
        DependencyProperty.Register("Handle", typeof(IntPtr), typeof(NativeHost), new PropertyMetadata(IntPtr.Zero));
    HwndSource _source;
    protected override HandleRef BuildWindowCore(HandleRef hwndParent)
    {
        _source = new HwndSource(0, WS_CHILD | WS_VISIBLE | LBS_NOTIFY, 0,0,0, (int)Width, (int)Height, "nativeHost", hwndParent.Handle);
        Handle = _source.Handle;
        return new HandleRef(this,Handle);
    }
    protected override void DestroyWindowCore(HandleRef hwnd)
    {
        _source.Dispose();
    }
    const int WS_CHILD = 0x40000000;
    const int WS_VISIBLE = 0x10000000;
    const int LBS_NOTIFY = 0x001;
}
```

-

# 二、使用方式

直接在[xaml](https://so.csdn.net/so/search?q=xaml&spm=1001.2101.3001.7020)中使用上述实现的控件：

```xml
<Window x:Class="WpfApp1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:sys="clr-namespace:System;assembly=mscorlib"
        xmlns:local="clr-namespace:WpfApp1" xmlns:interop="clr-namespace:System.Windows.Interop;assembly=PresentationFramework"
        mc:Ignorable="d"
        Title="MainWindow" Height="440" Width="640"   
        >
    <Grid>
          <!--控件有个Handle属性，可以绑定，使用OneWaytoSource赋值给viewModel-->
        <local:NativeHost x:Name="NH_Plane" Height="360" Width="640" ></local:NativeHost>
    </Grid>
</Window>
```

在Loaded事件中才能获取到句柄，在此事件之前句柄还没有生成。

```csharp
private void Window_Loaded(object sender, RoutedEventArgs e)
{
    //获取控件句柄
    var hwnd=NH_Plane.Handle
    //通过句柄进行渲染
}
```

-

# 三、示例

示例代码：  
[https://download.csdn.net/download/u013113678/40304426](https://download.csdn.net/download/u013113678/40304426)  
注：示例代码与文本所有代码基本一致，渲染部分在c++的dll不可见，请根据需要下载。  
效果预览：  
![在这里插入图片描述](https://img-blog.csdnimg.cn/da1f01e97cfb4990a559ddb3c32af416.png?x-oss-process=image/watermark,type_ZHJvaWRzYW5zZmFsbGJhY2s,shadow_50,text_Q1NETiBAQWxmcmVkLU4=,size_20,color_FFFFFF,t_70,g_se,x_16)

-

# 总结

通过HwndHost渲染视频，本质是获取Hwnd渲染视频，获取Hwnd后渲染方式可以有多种选择，用gdi、d3d、opengl都可以，其实就是相当于在MFC上渲染视频，很多方案都可以通用。但这种方法也有一些缺点，其渲染和wpf控件有冲突，无法同时存在，即视频上面无法放置任何控件、也无法做到圆角播放框。