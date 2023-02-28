using System;
using System.IO;

namespace HelperCollections
{
    /// <summary>
    /// 创建快捷方式(lnk和url)的帮助类。创建应用程序、文件夹和网络链接的快捷方式
    /// </summary>
    public class ShortcutHelper
    {
        /// <summary>
        /// 桌面文件夹路径
        /// </summary>
        public static string DesktopDir => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        /// <summary>
        /// 开始菜单文件夹路径
        /// </summary>
        public static string StartMenuDir => Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
        /// <summary>
        /// 通用开始菜单文件夹路径
        /// </summary>
        public static string CommonStartMenuDir => Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
        /// <summary>
        /// 自启动文件夹路径
        /// </summary>
        public static string StartupDir => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        /// <summary>
        /// 通用自启动文件夹路径
        /// </summary>
        public static string CommonStartupDir => Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);


        #region 方式一：写入文本文件 .url 实现快捷方式
        /// <summary>
        /// 创建链接url的快捷方式
        /// </summary>
        /// <param name="shortcutDir">快捷方式所在路径</param>
        /// <param name="linkUrl">快捷方式指向的url链接</param>
        /// <param name="shortcutName">快捷方式名</param>
        /// <param name="icon">快捷方式文件图标</param>
        public static void CreateUrlShortcut(string shortcutDir, string linkUrl, string shortcutName = null, string icon = null)
        {
            shortcutName = HandleShortcutName(linkUrl, shortcutName);
            HandleDirNotExists(shortcutDir);
            using (StreamWriter writer = new StreamWriter(Path.Combine(shortcutDir, shortcutName + ".url")))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + linkUrl);
                if (!string.IsNullOrWhiteSpace(icon) && System.IO.File.Exists(icon))
                {
                    writer.WriteLine("IconIndex=0");
                    writer.WriteLine("IconFile=" + icon);
                }
            }
        }

        /// <summary>
        /// 创建桌面上链接的快捷方式
        /// </summary>
        /// <param name="linkUrl">快捷方式指向的url链接</param>
        /// <param name="shortcutName">快捷方式名</param>
        /// <param name="icon">快捷方式文件图标</param>
        public static void UrlShortcutToDesktop(string linkUrl, string shortcutName = null, string icon = null)
        {
            CreateUrlShortcut(DesktopDir, linkUrl, shortcutName, icon);
        }

        /// <summary>
        /// 创建文件的快捷方式，如应用程序。【向指定目录创建指向源文件的快捷方式】
        /// </summary>
        /// <param name="shortcutDir">快捷方式所在的目标目录</param>
        /// <param name="targetFile">(源)文件完全路径，即快捷方式指向的目标，如果不存在将无法创建</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        /// <returns></returns>
        public static void CreateFileShortcut(string shortcutDir, string targetFile, string shortcutName = null, string icon = null)
        {
            if (string.IsNullOrWhiteSpace(targetFile) || !System.IO.File.Exists(targetFile))
            {
                throw new Exception("指向的目标文件不存在");
            }

            shortcutName = HandleShortcutName(targetFile, shortcutName);
            HandleDirNotExists(shortcutDir);

            using (StreamWriter writer = new StreamWriter(Path.Combine(shortcutDir, shortcutName + ".url")))
            {

                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + targetFile);
                writer.WriteLine("IconIndex=0");
                if (string.IsNullOrWhiteSpace(icon) || !System.IO.File.Exists(icon))
                {
                    icon = targetFile.Replace('\\', '/'); // 非必须。直接 IconLocation =targetFile 也可
                }
                writer.WriteLine("IconFile=" + icon);
            }
        }

        /// <summary>
        /// 创建文件的桌面快捷方式，如应用程序
        /// </summary>
        /// <param name="targetFile">(源)文件完全路径，即快捷方式指向的目标，如果不存在将无法创建</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        public static void FileShortcutToDesktop(string targetFile, string shortcutName = null, string icon = null)
        {
            CreateFileShortcut(DesktopDir, targetFile, shortcutName, icon);
        }

        /// <summary>
        /// 创建 当前运行程序 的桌面快捷方式
        /// </summary>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        public static void CurrShortcutToDesktop(string shortcutName = null, string icon = null)
        {
            // 获取正在执行的程序集，正在运行的程序
            //string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string app = System.Reflection.Assembly.GetEntryAssembly().Location;
            CreateFileShortcut(DesktopDir, app, shortcutName, icon);
        }
        /// <summary>
        /// 创建 当前运行程序 的快捷方式到指定Dir
        /// </summary>
        /// <param name="shortcutDir">快捷方式所在的目标目录</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="icon">图标路径</param>
        public static void CreateCurrShortcut(string shortcutDir, string shortcutName = null, string icon = null)
        {
            // 获取正在执行的程序集，正在运行的程序
            //string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string app = System.Reflection.Assembly.GetEntryAssembly().Location;
            CreateFileShortcut(shortcutDir, app, shortcutName, icon);
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
