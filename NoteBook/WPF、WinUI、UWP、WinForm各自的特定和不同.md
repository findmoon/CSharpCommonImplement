**WPF、WinUI、UWP、WinForm各自的特定和不同**

[toc]

转载自 [WPF、WinUI、UWP、WinForm 在项目中到底是怎么区别的？](https://www.zhihu.com/question/498398314)

先单独说明Win32，Win32有3个含义

1\. Win32 UI：指的是界面上的每一个控件都是原生Windows控件，每个控件都是一个HWND

2\. Win32 API：指C形式的，大部分都不符合UWP沙盒环境的API

3\. Win32 程序：程序的生命周期和表现形式，除了Win32形式则是UWP形式，所以Win32形式包含了WPF，WinForm。UWP程序的生命周期形式有一个默认的特点，最小化之后，程序暂停运行。

## 辨别方法

Win32 UI，WPF、WinUI、UWP、WinForm等等名词，都可以用三方面来总结区别，一个是程序依赖什么，一个是UI绘制方式，一个是运行权限。

从他们出生的先后顺序开始说起

### 1\. Win32 UI

### 1.1 依赖

包含了一大批稳定的，兼容的C语言版本Win32API，比如 [GetWindowTextA](https://link.zhihu.com/?target=https%3A//docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowtexta)，用来获取窗口文字，非常非常早期不少程序也用这个函数来获取QQ密码框控件中的密码，虽然密码框中文本表现为“**\***”，但是实际存储的为密码的内容。

### 1.2 UI绘制方式

界面上的元素都是由Windows操作系统提供的控件，比如编辑框，文本框，每一个控件都有自己的HWND形式的HANDLE，其他程序可以通过[遍历控件](https://www.zhihu.com/search?q=%E9%81%8D%E5%8E%86%E6%8E%A7%E4%BB%B6&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)找到HWND后，利用HWND对控件进行操作，比如可以使得灰色不能点击的按钮可以点击的函数[EnableWindow](https://link.zhihu.com/?target=https%3A//docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-enablewindow)。整个应用的所有控件形成的树形列表，都是被Windows操作系统掌握的。

### 1.3 权限

利用Win32API开发出来的程序，具备超级大的权限，比如上面提到的获取密码，让灰色按钮可以点击，除此之外还可以自动扫描U盘，还可以修改其它进程的行为，这样容易产生很多坏程序，于是需要[杀毒软件](https://www.zhihu.com/search?q=%E6%9D%80%E6%AF%92%E8%BD%AF%E4%BB%B6&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)很有必要存在，或者部分API需要管理员才能运行（这一招大部分没用，大部分用户会放行），即使实在目前Windows10/11的Windows Defender开启的情况下，仍然不敢随意下载和运行不明来历的程序。

### 2\. WinFrom

### 2.1 依赖

因为Win32API的开发是C/C++语言的，需要了解比较多的知识和内存管理，于是在C#语言上封装了大部分Win32API，特别是窗口部分的API形成了WinForm开发

### 2.2 UI绘制方式

[同Win32程序](https://www.zhihu.com/search?q=%E5%90%8CWin32%E7%A8%8B%E5%BA%8F&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)

### 2.3 权限

同Win32程序，C# API不足的部分，可以利用C#调用C函数的方式实现

### 3\. WPF

### 3.1 依赖

无

### 3.2 UI绘制方式

进程只剩下一个主窗口，主窗口内的所有元素形成的[树状列表](https://www.zhihu.com/search?q=%E6%A0%91%E7%8A%B6%E5%88%97%E8%A1%A8&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)，对Windows操作系统来讲是不了解的，WPF程序自己接管了每一个控件的绘制，和事件响应，意味着，WPF程序对窗口消息循环仅仅依赖主窗口的消息，然后自己对主窗口的所有消息进行转发，不依赖Windows操作系统来转发，这样有一个好处是程序更安全，别人再也获取不到程序中的密码框控件，因为密码框不存在HWND，无法被遍历到。还有一个好处得益于GPU的帮忙，界面更漂亮了。

### 3.3 权限

同Win32程序，API不足的部分，可以利用C#调用C函数的方式实现

### 4\. UWP

### 4.1 依赖

上面提到的几个程序，权限都可以很大，移动时代的[沙盒机制](https://www.zhihu.com/search?q=%E6%B2%99%E7%9B%92%E6%9C%BA%E5%88%B6&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)需要一种方案，使得安装，卸载，权限控制明确。

UWP上可以用的函数包括：

\- WinRT运行时

\- 部分Win32API可以用（比如[socket函数](https://www.zhihu.com/search?q=socket%E5%87%BD%E6%95%B0&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)[connect](https://link.zhihu.com/?target=https%3A//docs.microsoft.com/en-us/windows/win32/api/winsock2/nf-winsock2-connect)）

\- 大部分COM组件可以用

\- 还有的部分需要看微软提供的列表

### 4.2 UI绘制方式

原理和API都很相似WPF，界面API主要是做了减少，使得程序能运行在较差的设备上，同时[命名空间](https://www.zhihu.com/search?q=%E5%91%BD%E5%90%8D%E7%A9%BA%E9%97%B4&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)发生变化。

> 控件举例 Windows.UI.Xaml.Controls.Grid，在我的电脑上，它来自于E:\\Windows Kits\\10\\References\\10.0.19041.0\\Windows.Foundation.UniversalApiContract\\10.0.0.0\\Windows.Foundation.UniversalApiContract.winmd，更新频率同操作系统，而且旧版本的操作系统不再更新，常常遇到在新版本开发的xaml文件，在旧版本中运行crash，因为旧版本缺失控件的某个属性，为了解决以上问题，有了后面的WinUI

### 4.3 权限

类似于手机应用，默认只能访问网络，无法和当前计算机（不包含当前计算机的虚拟机）中的其他应用进行TCP/IP通信，未经用户同意，不得扫描其他盘符（D: E:), 权限得到的控制 缺点很多，所以开发人员不开心：

\- 部分API性能比传统Win32程序差，比如[遍历文件](https://www.zhihu.com/search?q=%E9%81%8D%E5%8E%86%E6%96%87%E4%BB%B6&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)，因为需要中间应用来传递查找到的数据，加上API都是异步的存在线程的切换。

\- 默认最小化会暂停应用，这样使得网络请求也暂停了

\- 无法把图标放入系统托盘

\- socket无法与本机应用通讯

### 5\. WinUI

### 5.1 依赖

WinUI2依赖UWP

WinUI3不依赖UWP

[微软WinUI的介绍](https://link.zhihu.com/?target=https%3A//docs.microsoft.com/zh-cn/windows/apps/winui/)

### 5.2 UI绘制方式

无论是WinUI2还是WinUI3，无论是WinUI3的Win32程序还是UWP程序，绘制方式同WPF：自绘

> 控件举例 Microsoft.UI.Xaml.Controls，在我的电脑上，它来自于C:\\Users\\xxx\\.nuget\\packages\\microsoft.ui.xaml\\2.7.0\\lib\\uap10.0\\Microsoft.UI.Xaml.winmd，更新很快，更新后可以运行与旧版本的操作系统

### 5.3 权限

WinUI2权限同UWP

WinUI3权限要看创建的是不是Win32程序，如果是Win32程序，最小化后不会暂停应用，如果是UWP程序，则同UWP

### 6.MAUI

一套期望用一个库来抽象Windows/iOS/Android平台的界面库，在Windows端它依赖WinUI 3,在iOS端，它依赖xamarin.iOS，在Andoird端，它依赖xamarin.Android。xamarin.iOS和xamarin.Android属于对应平台的原生开发，不属于自绘。

## 题主提到的疑问

**问: 那么是否意味着我只要把这个包换一下 (以及对应的 xaml 的包), 我的应用就变成另一种类型了?**

答: 应用类型定义在csproj文件中，需要文本编辑这个文件中的属性来更换类型，一般更换比较麻烦，如果想更换，还是新建一个工程，然后把文件拷贝过去比较好

**问：那么 UWP 限制调用 win32 的 api 是怎么限制的呢?**

答：所有涉及到可能跳出沙盒的API都会限制，限制方式有两种

\- 一种是利用宏定义使得API没有定义，从而编译不过，达到不允许调用，

\- 一种是调用后，被调用的函数判断当前是不是UWP环境，如果是UWP环境则不起作用

win32 API能不能用，看具体API文档最准确，文档最下方都会表明是否可以用于UWP，也有这样的页面来表明了哪一类不能用，[比如文档这在里](https://link.zhihu.com/?target=https%3A//docs.microsoft.com/en-us/uwp/win32-and-com/win32-and-com-for-uwp-apps)

**问：按照现在的条件，我只要单独写个类引个其他的 [nuget](https://www.zhihu.com/search?q=nuget&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D) 包就能间接调用 win32 的 api 了呀**

答：的确可以这么做，如果引用的API都可以在UWP中使用，是没有问题的，不幸的是，大多数[nuget包](https://www.zhihu.com/search?q=nuget%E5%8C%85&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)都包含一些不能在UWP中使用的API，使得UWP应用无法提交。

**问：WinUI 说的支持 Win32 程序是什么意思呢？**

代表支持“程序的生命周期和表现形式”，这种情况下最小化不暂停，不代表Win32 UI，是否代表Win32 API看利用WinUI3创建什么类型的程序了(UWP 还是 Win32).

## 额外说明

如果Win32应用被打包到应用商店，则部分API会失效，比如其他答主提到的注册表操作其实是和操作系统的注册表隔离的，这是依靠API内部实现的，会判断当前运行的APP是不是UWP应用。

Windows Store既可以上传UWP应用，也可以上传Win32应用，如果上传的是Win32应用，部分API还是会被禁止使用，比如创建进程，其他可能有问题的API需要向Microsoft说明为什么要使用。

那么[WinUI 3](https://www.zhihu.com/search?q=WinUI+3&search_source=Entity&hybrid_search_source=Entity&hybrid_search_extra=%7B%22sourceType%22%3A%22answer%22%2C%22sourceId%22%3A2221576589%7D)和WPF到底有什么区别呢？难道只是为了统一WPF和UWP的UI开发？也许是吧。

由于本人对MFC环境下的Win32程序和UWP很熟悉，其他都是不深入，可能有错误，欢迎讨论。

[编辑于 2021-11-18 11:45](https://www.zhihu.com/question/498398314/answer/2221576589)

