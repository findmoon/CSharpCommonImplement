C#创建快捷方式的N种方式、应用程序.lnk快捷方式、网页链接.url的快捷方式【最简单的方法】

[toc]

# 创建 shortcut 的N种方式

## 直接二进制写入

### URL 链接快捷方式

```C#
private void urlShortcutToDesktop(string linkName, string linkUrl)
{
    string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

    using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
    {
        writer.WriteLine("[InternetShortcut]");
        writer.WriteLine("URL=" + linkUrl);
    }
}
```

### 应用程序快捷方式

```C#
private void appShortcutToDesktop(string linkName)
{
    string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

    using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
    {
        string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
        writer.WriteLine("[InternetShortcut]");
        writer.WriteLine("URL=file:///" + app);
        writer.WriteLine("IconIndex=0");
        string icon = app.Replace('\\', '/');
        writer.WriteLine("IconFile=" + icon);
    }
}
```

## WSH（Windows Script Host Object Model）接口方法

## 【最简单方式】直接文本写入快捷方式文件

# 网页链接url的快捷方式

## .url 文件直接写入文本

最简单的方式是直接写入如下格式的文本到`.url`文件：

```ini
[InternetShortcut]
URL=http://www.xxx.com/path
IconIndex=0
IconFile=C:\xxx\favicon.ico
```

这样实现了快捷方式的效果。


