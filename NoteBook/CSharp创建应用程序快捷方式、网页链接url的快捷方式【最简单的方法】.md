C#创建快捷方式的N种方式、应用程序.lnk快捷方式、网页链接.url的快捷方式【最简单的方法】

[toc]

# 创建 shortcut 的N种方式

## 【最简单方式】直接文本写入(快捷方式文件)

### URL 链接快捷方式

```C#
private void urlShortcutToDesktop(string linkName, string linkUrl)
{
    string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

    using (StreamWriter writer = new StreamWriter(deskDir + "\\" + linkName + ".url"))
    {
        writer.WriteLine("[InternetShortcut]");
        writer.WriteLine("URL=" + linkUrl);
        writer.WriteLine("IconIndex=0");
        string icon = app.Replace('\\', '/');
        writer.WriteLine("IconFile=" + icon);
    }
}
```

### 文件快捷方式（如应用程序）

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

## WSH（Windows Script Host Object Model）接口方法【官方，推荐】

> 需要引用 `Interop.IWshRuntimeLibrary.dll`，`using IWshRuntimeLibrary;`。

> Project > Add Reference > COM > Windows Script Host Object Model.

官方提供的创建快捷方式的API，推荐使用。可以创建 **文件、文件夹、链接url** 快捷方式。

```C#
using IWshRuntimeLibrary;

private void CreateShortcut()
{
  object shDesktop = (object)"Desktop";
  WshShell shell = new WshShell();
  string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Notepad.lnk";
  IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
  shortcut.Description = "New shortcut for a Notepad";
  shortcut.Hotkey = "Ctrl+Shift+N";
  shortcut.TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\notepad.exe";
  shortcut.Save();
}
```

> 链接url的快捷方式，后缀也需要为lnk，否则创建时报错。
> 
> 官方示例中 [CreateShortcut Method](https://learn.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/windows-scripting/xsy6k3ys(v=vs.84)?redirectedfrom=MSDN) 使用的是vb，可以直接创建`.url`后缀的文件。

## 利用反射获取WScript.Shell，避免添加项目引用

利用反射获取创建对象，避免添加COM引用到项目中（具体参见参考文章）： 

```C#
Type m_type = Type.GetTypeFromProgID("WScript.Shell");
object m_shell = Activator.CreateInstance(m_type);
IWshShortcut shortcut = (IWshShortcut)m_type.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, m_shell, new object[] { fileName });
shortcut.Description = description;
shortcut.Hotkey = hotkey;
shortcut.TargetPath = targetPath;
shortcut.WorkingDirectory = workingDirectory;
shortcut.Arguments = arguments;
if (!string.IsNullOrEmpty(iconPath))
     shortcut.IconLocation = iconPath;
shortcut.Save();
```

或

```C#
/// <summary>
/// 创建一个快捷方式
/// </summary>
/// <param name="lnkFilePath">快捷方式的完全限定路径。</param>
/// <param name="workDir"></param>
/// <param name="args">快捷方式启动程序时需要使用的参数。</param>
/// <param name="targetPath"></param>
public static void CreateShortcut(string lnkFilePath, string targetPath, string workDir, string args = "")
{
    var shellType = Type.GetTypeFromProgID("WScript.Shell");
    dynamic shell = Activator.CreateInstance(shellType);
    var shortcut = shell.CreateShortcut(lnkFilePath);
    shortcut.TargetPath = targetPath;
    shortcut.Arguments = args;
    shortcut.WorkingDirectory = workDir;
    shortcut.Save();
}

/// <summary>
/// 读取一个快捷方式的信息
/// </summary>
/// <param name="lnkFilePath"></param>
/// <returns></returns>
public static ShortcutDescription ReadShortcut(string lnkFilePath)
{
    var shellType = Type.GetTypeFromProgID("WScript.Shell");
    dynamic shell = Activator.CreateInstance(shellType);
    var shortcut = shell.CreateShortcut(lnkFilePath);
    return new ShortcutDescription()
    {
        TargetPath = shortcut.TargetPath,
        Arguments = shortcut.Arguments,
        WorkDir = shortcut.WorkingDirectory,
    };
}
```

## 其他

### 借助于 VBS

> 参考自 [C#两种创建快捷方式的方法](https://www.cnblogs.com/linmilove/archive/2009/06/10/1500989.html)

C#中通过代码生成vbs文件到临时目录，然后通过Process进程类执行。

**在写入VBS文件时,一定要用UnicodeEncoding，否则可能产生乱码**

主要代码如下:

```C#
//生成VBS代码
string vbs = this.CreateVBS();
//以文件形式写入临时文件夹
this.WriteToTemp(vbs);
//调用Process执行
this.RunProcess();

///
/// 创建VBS代码
///
///
private string CreateVBS()
{
    string vbs = string.Empty;
    vbs += ("set WshShell = WScript.CreateObject(\"WScript.Shell\")\r\n");
    vbs += ("strDesktop = WshShell.SpecialFolders(\"Desktop\")\r\n");
    vbs += ("set oShellLink = WshShell.CreateShortcut(strDesktop & \"\\D4S.lnk\")\r\n");
    vbs += ("oShellLink.TargetPath = \"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"\r\n");
    vbs += ("oShellLink.WindowStyle = 1\r\n");
    vbs += ("oShellLink.Description = \"ChinaDforce YanMang\"\r\n");
    vbs += ("oShellLink.WorkingDirectory = \"" + System.Environment.CurrentDirectory + "\"\r\n");
    vbs += ("oShellLink.Save");
    return vbs;
}
///
/// 写入临时文件
///
///
private void WriteToTemp(string vbs)
{
    if (!string.IsNullOrEmpty(vbs))
    {
        //临时文件
        string tempFile = Environment.GetFolderPath(Environment.SpecialFolder.Templates) + "[url=file://\\temp.vbs]\\temp.vbs[/url]";
        //写入文件
        FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
        try
        {
            //这里必须用UnicodeEncoding. 因为用UTF-8或ASCII会造成VBS乱码
            System.Text.UnicodeEncoding uni = new UnicodeEncoding();
            byte[] b = uni.GetBytes(vbs);
            fs.Write(b, 0, b.Length);
            fs.Flush();
            fs.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "写入临时文件时出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            //释放资源
            fs.Dispose();
        }
    }
}
///
/// 执行VBS中的代码
///
private void RunProcess()
{
    string tempFile = Environment.GetFolderPath(Environment.SpecialFolder.Templates) + "\\temp.vbs";
    if (File.Exists(tempFile))
    {
        //执行VBS
        Process.Start(tempFile);
    }
}
private void btn退出_Click(object sender, EventArgs e)
{
    Application.Exit();
    //清除临时文件
    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Templates) + "\\temp.vbs");
}
```

### Shell Links

> [Shell Links](https://learn.microsoft.com/en-us/windows/win32/shell/links) 是一个 Win32 API，通常被用来创建快捷方式。
> 
> [ShellLink.cs](http://www.vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp)是对该API进行的包装类。

小示例：

```C#
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace TestShortcut
{
    class Program
    {
        static void Main(string[] args)
        {
            IShellLink link = (IShellLink)new ShellLink();

            // setup shortcut information
            link.SetDescription("My Description");
            link.SetPath(@"c:\MyPath\MyProgram.exe");

            // save it
            IPersistFile file = (IPersistFile)link;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            file.Save(Path.Combine(desktopPath, "MyLink.lnk"), false);
        }
    }

    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLink
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLink
    {
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
        void GetIDList(out IntPtr ppidl);
        void SetIDList(IntPtr pidl);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out short pwHotkey);
        void SetHotkey(short wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        void Resolve(IntPtr hwnd, int fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }
}
```

### 调用cmd创建

```C#
Process.Start("cmd.exe", $"/c mklink {linkName} {applicationPath}");
```

### P/Invoke 调用 Win32 API 实现

参见 [CreateSymbolicLinkW function](https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-createsymboliclinkw)


# 手动 往 .url 文件直接写入文本

更简单的方式是直接写入如下格式的文本到`.url`文件：

```ini
[InternetShortcut]
URL=http://www.xxx.com/path
IconIndex=0
IconFile=C:\xxx\favicon.ico
```

即可实现快捷方式的效果。


# 参考

- [Create a shortcut on Desktop](https://stackoverflow.com/questions/4897655/create-a-shortcut-on-desktop)
- [C# 创建或读取快捷方式](https://www.cnblogs.com/jasongrass/p/12144799.html)