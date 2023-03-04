using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;

namespace HelperCollections
{
    /// <summary>
    /// 创建快捷方式(lnk和url)的帮助类。创建应用程序、文件夹和网络链接的快捷方式
    /// </summary>
    public class ShortcutHelper_WSH
    {
        /// <summary>
        /// 桌面文件夹路径
        /// </summary>
        public static string DesktopDir => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        /// <summary>
        /// 开始菜单的程序文件夹路径
        /// </summary>
        public static string StartMenuProgramsDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs");
        /// <summary>
        /// 通用开始菜单的程序文件夹路径
        /// </summary>
        public static string CommonStartMenuProgramsDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs");
        /// <summary>
        /// 自启动文件夹路径
        /// </summary>
        public static string StartupDir => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        /// <summary>
        /// 通用自启动文件夹路径
        /// </summary>
        public static string CommonStartupDir => Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);


        #region 方式二：WSH接口，Interop.IWshRuntimeLibrary.dll
        /// <summary>
        /// 向指定目录创建指向 源文件或链接 的快捷方式【链接url 或 文件 或 文件夹 快捷方式】
        /// </summary>
        /// <param name="shortcutDir">快捷方式所在的目标目录</param>
        /// <param name="targetPath">(源)文件完全路径，即快捷方式指向的目标，如果不存在将无法创建，返回false</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名，如果targetPath是带转义的查询字符串的url，则应该必须指定快捷方式名称</param>
        /// <param name="IconLocation">图标地址</param>
        /// <param name="description">描述</param>
        /// <returns>成功或失败</returns>
        public static bool CreateShortcut(string shortcutDir, string targetPath, string shortcutName = null, string IconLocation = null, string description = null)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                return false;
            }
            shortcutName = HandleShortcutName(targetPath, shortcutName);

            // 简单判断是否url

            //if (!isUrl && !System.IO.File.Exists(targetPath)) { return false; }

            HandleDirNotExists(shortcutDir);                         //目录不存在则创建

            string shortcutPath = Path.Combine(shortcutDir, $"{shortcutName}.lnk");          //合成路径
                                                                                             //添加引用，Com 中搜索 Windows Script Host Object Model
            WshShell shell = new WshShell();
            IWshShortcut shortcut = shell.CreateShortcut(shortcutPath);    //创建快捷方式对象。参数不能为.url
            shortcut.TargetPath = targetPath;                                                               //指定目标路径
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);                                  //设置起始位置
            //目标应用程序窗口类型(1.Normal window普通窗口,3.Maximized最大化窗口,7.Minimized最小化)
            shortcut.WindowStyle = 1;                                                                       //设置运行方式，默认为常规窗口
            shortcut.Description = description;                                                             //设置备注
            if (string.IsNullOrWhiteSpace(IconLocation) || !System.IO.File.Exists(IconLocation))
            {
                shortcut.IconLocation = targetPath;
            }
            else
            {
                shortcut.IconLocation = IconLocation;    //设置图标路径
            }

            //shortcut.Hotkey
            //shortcut.WorkingDirectory
            //shortcut.Arguments

            shortcut.Save();                                                                                //保存快捷方式
            return true;
        }

        /// <summary>
        /// 创建桌面快捷方式，如应用程序
        /// </summary>
        /// <param name="targetPath">(源)文件完全路径，即快捷方式指向的目标，如果不存在将无法创建</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        /// <param name="description">描述</param>
        public static void FileShortcutToDesktop(string targetPath, string shortcutName = null, string icon = null, string description = null)
        {
            CreateShortcut(DesktopDir, targetPath, shortcutName, icon, description);
        }

        /// <summary>
        /// 创建 当前运行程序 的桌面快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        /// <param name="description">描述</param>
        public static void CurrShortcutToDesktop(string shortcutName = null, string icon = null, string description = null)
        {
            // 获取正在执行的程序集，正在运行的程序
            //string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string app = System.Reflection.Assembly.GetEntryAssembly().Location;
            CreateShortcut(DesktopDir, app, shortcutName, icon, description);
        }
        /// <summary>
        /// 创建 当前运行程序 的快捷方式到指定Dir
        /// </summary>
        /// <param name="shortcutDir">快捷方式所在的目标目录</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        /// <param name="description">描述</param>
        public static void CreateCurrShortcut(string shortcutDir, string shortcutName = null, string icon = null, string description = null)
        {
            // 获取正在执行的程序集，正在运行的程序
            //string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string app = System.Reflection.Assembly.GetEntryAssembly().Location;
            CreateShortcut(shortcutDir, app, shortcutName, icon, description);
        }


        /// <summary>
        /// 获取指定文件夹下 指向目标Path 的 快捷方式文件路径集合(.lnk)
        /// </summary>
        /// <param name="directory">要查找的文件夹</param>
        /// <param name="targetPath">对应的目标应用程序路径</param>
        /// <returns>目标应用程序的快捷方式文件路径</returns>
        public static List<string> GetshortCutFilesFromFolder(string directory, string targetPath)
        {
            List<string> tempStrs = new List<string>();
            tempStrs.Clear();
            string[] files = Directory.GetFiles(directory, "*.lnk");
            if (files == null || files.Length < 1)
            {
                return tempStrs;
            }
            string tempStr = null;
            for (int i = 0; i < files.Length; i++)
            {
                //files[i] = string.Format("{0}\\{1}", shortcutDir, files[i]);
                tempStr = GetTargetPathFromQuick(files[i]);
                if (tempStr == targetPath)
                {
                    tempStrs.Add(files[i]);
                }
            }
            return tempStrs;
        }
        /// <summary>
        /// 获取快捷方式的目标Path路径
        /// </summary>
        /// <param name="shortcutFile"></param>
        /// <returns></returns>
        private static string GetTargetPathFromQuick(string shortcutFile)
        {
            //快捷方式文件的路径 = @"d:\Test.lnk";
            if (System.IO.File.Exists(shortcutFile))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortct = (IWshShortcut)shell.CreateShortcut(shortcutFile);
                //快捷方式文件指向的路径.Text = 当前快捷方式文件IWshShortcut类.TargetPath;
                //快捷方式文件指向的目标目录.Text = 当前快捷方式文件IWshShortcut类.WorkingDirectory;
                return shortct.TargetPath;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 一些公共私有方法
        /// <summary>
        /// 处理路径不存在
        /// </summary>
        /// <param name="dir"></param>
        private static void HandleDirNotExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 处理获取 shortcutName 快捷方式名
        /// </summary>
        /// <param name="linkUrlOrTargetPath"></param>
        /// <param name="shortcutName"></param>
        /// <returns></returns>
        private static string HandleShortcutName(string linkUrlOrTargetPath, string shortcutName)
        {
            if (string.IsNullOrWhiteSpace(shortcutName))
            {
                var temp = linkUrlOrTargetPath.Trim('/', '\\');
                var idx = temp.LastIndexOfAny(new char[] { '/', '\\' });
                if (idx >= 0)
                {
                    shortcutName = temp.Substring(idx + 1);
                }
                if (string.IsNullOrWhiteSpace(shortcutName))
                {
                    shortcutName = linkUrlOrTargetPath.Replace("\\", "").Replace("/", "");
                }
            }
            return shortcutName;
        }
        #endregion
    }
}
