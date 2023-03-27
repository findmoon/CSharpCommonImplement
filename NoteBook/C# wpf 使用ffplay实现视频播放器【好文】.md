**C# wpf 使用ffplay实现视频播放器【好文】**

[toc]

> 原文 [C# wpf 使用ffplay实现视频播放器](https://blog.csdn.net/u013113678/article/details/123907480)

> 收费资源：[C# wpf 使用ffplay实现视频播放器](https://download.csdn.net/download/u013113678/85437645?utm_medium=distribute.pc_relevant_download.none-task-download-2~default~OPENSEARCH~Rate-7-85437645-download-2584369.topnsimilar_compare_v2&depth_1-utm_source=distribute.pc_relevant_download.none-task-download-2~default~OPENSEARCH~Rate-7-85437645-download-2584369.topnsimilar_compare_v2&dest=https%3A%2F%2Fdownload.csdn.net%2Fdownload%2Fu013113678%2F85437645&spm=1003.2020.3001.6616.10)

# [ffplay](https://so.csdn.net/so/search?q=ffplay&spm=1001.2101.3001.7020)自定义系列

[第一章 自定义播放器接口](https://blog.csdn.net/u013113678/article/details/114266843)  
[第二章 倍速播放](https://blog.csdn.net/u013113678/article/details/124742856)  
[第三章 dxva2硬解渲染](https://blog.csdn.net/u013113678/article/details/124773187)  
[第四章 提供C#接口](https://blog.csdn.net/u013113678/article/details/124759757)  
[第五章 制作wpf播放器(本章)](https://blog.csdn.net/u013113678/article/details/123907480)

-

### 文章目录

- [ffplay自定义系列](https://blog.csdn.net/u013113678/article/details/123907480#ffplay_1)
- [前言](https://blog.csdn.net/u013113678/article/details/123907480#_13)
- [一、播放模块](https://blog.csdn.net/u013113678/article/details/123907480#_18)
- [二、界面](https://blog.csdn.net/u013113678/article/details/123907480#_23)
- - [1、关键实现](https://blog.csdn.net/u013113678/article/details/123907480#1_26)
    - - [(1)、圆角边框](https://blog.csdn.net/u013113678/article/details/123907480#1_27)
        - [(2)、拖动移动调整大小](https://blog.csdn.net/u013113678/article/details/123907480#2_30)
        - [(3)、播放](https://blog.csdn.net/u013113678/article/details/123907480#3_34)
        - [(4)、停止](https://blog.csdn.net/u013113678/article/details/123907480#4_55)
        - [(4)、进度条](https://blog.csdn.net/u013113678/article/details/123907480#4_68)
        - [(5)、关闭播放](https://blog.csdn.net/u013113678/article/details/123907480#5_70)
    - [2、效果预览](https://blog.csdn.net/u013113678/article/details/123907480#2_90)
- [三、下载](https://blog.csdn.net/u013113678/article/details/123907480#_100)
- [总结](https://blog.csdn.net/u013113678/article/details/123907480#_109)

-

# 前言

有了[《WPF视频渲染系列》](https://blog.csdn.net/u013113678/article/details/121275982)的视频渲染方法，再结合笔者已有的一个定制化ffplay播放器[《基于ffplay改造成自定义多开播放器》](https://blog.csdn.net/u013113678/article/details/114266843)，我们可以很容易的在wpf中实现一个播放器软件，这个播放器可以支持本地播放、摄像头播放、网络点播、rtmp和rtsp拉流。

-

# 一、播放模块

参考[第四章 提供C#接口](https://blog.csdn.net/u013113678/article/details/124759757)

-

# 二、界面

由于使用[wpf](https://so.csdn.net/so/search?q=wpf&spm=1001.2101.3001.7020)制作界面所以很多酷炫的效果都可以做，界面可以做的比较好看。

## 1、关键实现

### (1)、[圆角边框](https://so.csdn.net/so/search?q=%E5%9C%86%E8%A7%92%E8%BE%B9%E6%A1%86&spm=1001.2101.3001.7020)

圆角边框需要设置窗口透明，如果使用AllowsTransparency=“True”,会严重**影响性能**，可能导致渲染视频卡顿。我使用的是WindowChrome.WindowChrome实现圆角边框。具体代码就不贴出了，网上可以找到解决方案。  
还有一个关键点是不能使用窗口阴影，会影响d3dImage渲染性能。

### (2)、拖动移动调整大小

由于使用了WindowChrome.WindowChrome实现无边框圆角窗口，所有移动和调整大小功能基本要自己实现了。参考[《C# wpf 附加属性实现任意控件（包括窗口）拖动》](https://blog.csdn.net/u013113678/article/details/121397550)、[《C# wpf 附加属性实现任意控件拖动调整大小》](https://blog.csdn.net/u013113678/article/details/121719278)

### (3)、播放

由于在界面上渲染视频，且播放中再次Start内部会先调用Stop,Stop是同步实现的，在渲染或停止事件有Invoke时停止容易造成死锁，所以需要判断播放中时异步Stop后再Start。

```csharp
 async void StartPlay(string url)
 {   
     if (_isStarted)
     {
         await Task.Run(() =>
         {
             _play.Stop();
         });
         _play.Start(url);
         _isStarted = true;    
     }
     else
     {
         _play.Start(url);
         _isStarted = true;    
     }
 }
```

### (4)、停止

与上面相同，停止的时候需要异步停止。

```csharp
private async void Stop_Button_Click(object sender, RoutedEventArgs e)
{
    await Task.Run(() =>
  {
      _play.Stop();
  });
  _isStarted = false;  
}
```

### (4)、进度条

参考[《C# wpf slider实现显示进度、拖动定位、点击定位功能》](https://blog.csdn.net/u013113678/article/details/123832482)

### (5)、关闭播放

关闭窗口时，使用同步接口停止播放器会导致死锁：主线程等待渲染线程结束，同时渲染线程等待主线程invoke。所以我们需要异步停止来防止死锁。

示例代码如下：

```csharp
private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
    if (_play == null)
        return;
    e.Cancel = true;
    //异步退出，防止死锁
    await Task.Run(() =>
    {
        _play.Dispose();
    });
    _play = null;
    Close();
}
```

## 2、效果预览

![在这里插入图片描述](https://img-blog.csdnimg.cn/6a8b855cf60240ada98523b9bd6d8e2c.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAQWxmcmVkLU4=,size_20,color_FFFFFF,t_70,g_se,x_16)

![在这里插入图片描述](https://img-blog.csdnimg.cn/9d6b49ca5cbf41d68e3cdb0671dfc73f.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAQWxmcmVkLU4=,size_20,color_FFFFFF,t_70,g_se,x_16)

![在这里插入图片描述](https://img-blog.csdnimg.cn/9353f00fd7b04820b40ce76546d9a6b6.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAQWxmcmVkLU4=,size_20,color_FFFFFF,t_70,g_se,x_16)  
![在这里插入图片描述](https://img-blog.csdnimg.cn/ab61f0e4e770444ea1147b40acf6690c.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAQWxmcmVkLU4=,size_20,color_FFFFFF,t_70,g_se,x_16)

-

# 三、下载

[https://download.csdn.net/download/u013113678/85437645](https://download.csdn.net/download/u013113678/85437645)

**注：资源只包含C#源码，C语言播放器模块只提供x86dll，请根据需要下载。C#源码包含了[《WPF视频渲染系列》](https://blog.csdn.net/u013113678/article/details/121275982)的所有内容，切勿重复下载。**

-

# 总结

实现一个视频播放器基于ffplay，功能是很强大的同时也具有足够的稳定性，但是其内部实现代码庞杂，改造起来还是不太容易的，而且其功能也不是绝对完善的很多地方需要拓展和优化，比如精准定位、倍速播放、硬解渲染。有一个定制化的底层播放器后，利用wpf的界面优势很容易的就做出一个可以日常使用的播放器。当前版本的播放器功能相对简单，还不足以当成日常使用，还缺少平滑拖动定位、音轨选择、字幕播放以及声音放大等功能。