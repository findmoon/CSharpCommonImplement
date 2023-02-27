using IWshRuntimeLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace System
{
    /// <summary>
    /// (非管理员)实现 设置或取消开机自启动、判断是否开机自启、创建快捷方式的功能
    /// Winform、WPF 均ok，具体参见 StartWithPC、StartWithPCWpf 项目的使用
    /// </summary>
    public static class StartWithPC
    {
        #region 开机自动启动
        #region 按钮状态变更的操作
        ///// <summary>
        ///// 设置 开启自启动且自动运行 按钮 的状态(文字、颜色)
        ///// </summary>
        ///// <returns></returns>
        //private bool SetStartupAutoRunButton()
        //{
        //    if (GetStartupShortCuts().Count > 0)
        //    {
        //        startWithPCAndAutoRunMenuItem.Text = "关闭开机启动且自动运行";
        //        startWithPCAndAutoRunMenuItem.BackColor = SystemColors.ActiveBorder;
        //        return true;
        //    }
        //    else
        //    {
        //        startWithPCAndAutoRunMenuItem.Text = "设置开机启动且自动运行";
        //        startWithPCAndAutoRunMenuItem.BackColor = Color.PaleTurquoise;
        //        return false;
        //    }
        //}
        //private void startWithPCAndAutoRunMenuItem_Click(object sender, EventArgs e)
        //{
        //    SetMeAutoStart(startWithPCAndAutoRunMenuItem.Text.StartsWith("设置"));
        //    SetStartupAutoRunButton();
        //}

        #endregion
        /// <summary>
        /// 是否已经开机自启动
        /// </summary>
        public static bool IsRunWithPC => GetStartupShortCuts().Count > 0;

        /// <summary>
        /// 设置或取消开机自动启动 - 只需要调用改方法就可以了，参数里面的bool变量是控制开机启动的开关的，默认为开启自启启动
        /// </summary>
        /// <param name="onOff">自启开关</param>
        /// <param name="isCommonStartup">是否放在 所有用户的启动文件夹 中，默认false，当前用户的启动文件夹</param>
        public static void SetMeAutoStart(bool onOff = true,bool isCommonStartup=false)
        {
            //获取启动路径应用程序快捷方式的路径集合
            List<string> shortcutPaths = GetStartupShortCuts();
            if (onOff)//开机启动
            {
                //存在2个以快捷方式则保留一个快捷方式-避免重复多于
                if (shortcutPaths.Count >= 2)
                {
                    for (int i = 1; i < shortcutPaths.Count; i++)
                    {
                        DeleteFile(shortcutPaths[i]);
                    }
                }
                else if (shortcutPaths.Count < 1)//不存在则创建快捷方式
                {
                    CreateStartupShortcut(isCommonStartup: isCommonStartup);
                }
            }
            else//开机不启动
            {
                //存在快捷方式则遍历全部删除
                foreach (var shortcutPath in shortcutPaths)
                {
                    DeleteFile(shortcutPath);
                }
            }
            //创建桌面快捷方式-如果需要可以取消注释
            //CreateDesktopQuick(desktopPath, QuickName, appAllPath);
        }
        /// <summary>
        /// 获取指定文件夹下指向目标应用程序的快捷方式路径集合
        /// </summary>
        /// <param name="directory">要查找的文件夹</param>
        /// <param name="targetFile">对应的目标应用程序路径</param>
        /// <returns>目标应用程序的快捷方式文件路径</returns>
        public static List<string> GetshortCutFilesFromFolder(string directory, string targetFile)
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
                tempStr = GetAppPathFromQuick(files[i]);
                if (tempStr == targetFile)
                {
                    tempStrs.Add(files[i]);
                }
            }
            return tempStrs;
        }
        /// <summary>
        /// 获取当前程序的开机启动的快捷方式文件
        /// </summary>
        /// <returns></returns>
        public static List<string> GetStartupShortCuts()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // 获取调用者主程序的文件名
            var targetFile = Process.GetCurrentProcess().MainModule.FileName;

            var shortCutNames = GetshortCutFilesFromFolder(directory, targetFile);

            var coomonDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
            shortCutNames.AddRange(GetshortCutFilesFromFolder(coomonDir, targetFile));

            return shortCutNames;
        }
        /// <summary>
        /// 根据路径删除文件-用于取消自启时，从计算机自启目录删除程序的快捷方式
        /// </summary>
        /// <param name="path">路径</param>
        private static void DeleteFile(string path)
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                System.IO.File.Delete(path);
            }
        }
        /// <summary>
        /// 获取快捷方式的目标文件路径-用于判断是否已经开启了自动启动
        /// </summary>
        /// <param name="shortcutPath"></param>
        /// <returns></returns>
        private static string GetAppPathFromQuick(string shortcutPath)
        {
            //快捷方式文件的路径 = @"d:\Test.lnk";
            if (System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortct = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                //快捷方式文件指向的路径.Text = 当前快捷方式文件IWshShortcut类.TargetPath;
                //快捷方式文件指向的目标目录.Text = 当前快捷方式文件IWshShortcut类.WorkingDirectory;
                return shortct.TargetPath;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        ///  创建开机自启(路径下)的快捷方式
        /// </summary>
        /// <param name="targetPath">(源)程序完全路径，如果为null将创建当前程序的开机自启(快捷方式)</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标地址</param>
        /// <param name="isCommonStartup">是否放在 所有用户的启动文件夹 中，默认false，当前用户的启动文件夹</param>
        /// <returns>成功或失败</returns>
        private static bool CreateStartupShortcut(string targetPath = null, string description = null, string iconLocation = null,bool isCommonStartup=false)
        {
            // 所有用户的启动文件夹；当前用户的启动文件夹
            var directory = Environment.GetFolderPath(isCommonStartup?Environment.SpecialFolder.CommonStartup: Environment.SpecialFolder.Startup);

            if (targetPath == null)
            {
                targetPath = Process.GetCurrentProcess().MainModule.FileName;
            }

            //var shortcutName = Application.ProductName;
            var shortcutName = Path.GetFileNameWithoutExtension(targetPath);

            return CreateShortcut(directory, targetPath, shortcutName, description, iconLocation);
        }
        /// <summary>
        /// 向指定目录创建指向源文件的快捷方式
        /// </summary>
        /// <param name="shortcutDir">快捷方式所在的目标目录</param>
        /// <param name="targetPath">(源)文件完全路径，即快捷方式指向的目标，如果不存在将无法创建，返回false</param>
        /// <param name="shortcutName">快捷方式名字，默认使用(源)文件名</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标地址</param>
        /// <returns>成功或失败</returns>
        public static bool CreateShortcut(string shortcutDir, string targetPath, string shortcutName = null, string description = null, string iconLocation = null)
        {
            if (string.IsNullOrWhiteSpace(targetPath) || !System.IO.File.Exists(targetPath))
            {
                return false;
            }

            if (!Directory.Exists(shortcutDir)) Directory.CreateDirectory(shortcutDir);                         //目录不存在则创建
            if (string.IsNullOrWhiteSpace(shortcutName))
            {
                shortcutName = Path.GetFileNameWithoutExtension(targetPath);
            }
            string shortcutPath = Path.Combine(shortcutDir, string.Format("{0}.lnk", shortcutName));          //合成路径
                                                                                                              //添加引用，Com 中搜索 Windows Script Host Object Model
            WshShell shell = new WshShell();
            IWshShortcut shortcut = shell.CreateShortcut(shortcutPath);    //创建快捷方式对象
            shortcut.TargetPath = targetPath;                                                               //指定目标路径
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);                                  //设置起始位置
            shortcut.WindowStyle = 1;                                                                       //设置运行方式，默认为常规窗口
            shortcut.Description = description;                                                             //设置备注
            shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;    //设置图标路径
            shortcut.Save();                                                                                //保存快捷方式
            return true;
        }
        #endregion

    }
}
