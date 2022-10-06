using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CMCode.Register
{
    /// <summary>
    /// 注册文件类型，关联打开程序 【管理员权限】
    /// </summary>
    public static class FileTypeRegister
    {
        /// <summary>  
        /// 使文件类型与对应的图标及应用程序关联起来
        /// CreateSubKey(subKey, true) 从NF 4.6 开始支持创建或打开现有
        /// </summary>
        /// <param name="regInfo"></param>
        /// <param name="detectExist">是否检测存在，如果检测并存在，将不执行创建</param>
        public static void RegisterFileType(FileTypeRegInfo regInfo, bool detectExist = true)
        {
            if (detectExist && FileTypeRegistered(regInfo.ExtendName))
            {
                return;
            }

            //HKEY_CLASSES_ROOT/{ExtendName}
            using (RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName))
            {
                string relationName = regInfo.ExtendName.Substring(1).ToUpper() + "_FileType"; // ExtendName关联到打开该后缀的{relationName}注册表信息【原则上名称可以任意，"_FileType"后缀也不必须】
                fileTypeKey.SetValue("", relationName); // 注册表默认值
                fileTypeKey.Close();

                //HKEY_CLASSES_ROOT/{ExtendName-relation opened registry Name}
                using (RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName))
                {
                    relationKey.SetValue("", regInfo.Description);

                    //HKEY_CLASSES_ROOT/{ExtendName-relation opened registry Name}/DefaultIcon
                    RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
                    iconKey.SetValue("", regInfo.IconPath); // 值不能为null

                    //HKEY_CLASSES_ROOT/{ExtendName-relation opened registry Name}/Shell
                    RegistryKey shellKey = relationKey.CreateSubKey("Shell");

                    //HKEY_CLASSES_ROOT/{ExtendName-relation opened registry Name}/Shell/Open
                    RegistryKey openKey = shellKey.CreateSubKey("Open");

                    //HKEY_CLASSES_ROOT/{ExtendName-relation opened registry Name}/Shell/Open/Command
                    RegistryKey commandKey = openKey.CreateSubKey("Command");
                    commandKey.SetValue("", regInfo.ExePath + " \"%1\""); // " %1"表示将被双击的文件的路径传给目标应用程序
                    relationKey.Close();
                }
            }

            //调用SHChangeNotify才能更新文件/文件夹显示的图标（更新icon缓存） ，或者重启
            SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>  
        /// 更新指定文件类型关联信息  
        /// </summary>      
        public static bool RegisterFileTypeUpdate(FileTypeRegInfo regInfo)
        {
            if (!FileTypeRegistered(regInfo.ExtendName))
            {
                RegisterFileType(regInfo, false);
            }
            else
            {
                string extendName = regInfo.ExtendName;
                string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
                using (RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName, true))
                {
                    relationKey.SetValue("", regInfo.Description);
                    RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon", true);
                    iconKey.SetValue("", regInfo.IconPath);
                    RegistryKey shellKey = relationKey.CreateSubKey("Shell", true);
                    RegistryKey openKey = shellKey.CreateSubKey("Open", true);
                    RegistryKey commandKey = openKey.CreateSubKey("Command", true);
                    commandKey.SetValue("", regInfo.ExePath + " \"%1\"");
                    relationKey.Close();
                }
                // 调用SHChangeNotify才能更新文件 / 文件夹显示的图标（更新icon缓存） ，或者重启
                SHChangeNotify(0x8000000, 0, IntPtr.Zero, IntPtr.Zero);
            }
            return true;
        }

        /// <summary>  
        /// 获取指定文件类型关联信息  
        /// </summary>          
        public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
        {
            if (!FileTypeRegistered(extendName))
            {
                return null;
            }

            string relationName = extendName.Substring(1, extendName.Length - 1).ToUpper() + "_FileType";
            using (RegistryKey relationKey = Registry.ClassesRoot.OpenSubKey(relationName))
            {
                RegistryKey iconKey = relationKey.OpenSubKey("DefaultIcon");

                RegistryKey shellKey = relationKey.OpenSubKey("Shell");
                RegistryKey openKey = shellKey.OpenSubKey("Open");
                RegistryKey commandKey = openKey.OpenSubKey("Command");
                string temp = commandKey.GetValue("").ToString();
                var exePath = temp.Substring(0, temp.Length - 3);

                FileTypeRegInfo regInfo = new FileTypeRegInfo(extendName, exePath);
                regInfo.Description = relationKey.GetValue("").ToString();
                regInfo.IconPath = iconKey.GetValue("").ToString();
                return regInfo;
            }
        }

        /// <summary>  
        /// 指定文件类型是否已经注册  
        /// </summary>          
        public static bool FileTypeRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                return true;
            }
            return false;
        }

        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        
    }

    /// <summary>
    /// 文件类型注册信息
    /// </summary>
    public class FileTypeRegInfo
    {
        /// <summary>  
        /// 扩展名 必须是.开头的后缀名
        /// </summary>  
        public string ExtendName
        {
            get {
                if (!extendName.StartsWith("."))
                {
                    throw new FormatException("当前后缀名不正确，必须是.开头的后缀名");
                }
                return extendName;
            }
            set
            {
                if (value != null && value.StartsWith("."))
                {
                    extendName = value;
                }
                else
                {
                    throw new FormatException("必须是.开头的后缀名");
                }
            }
        }
        /// <summary>  
        /// 说明  
        /// </summary>  
        public string Description;
        /// <summary>  
        /// 文件类型关联的图标 扩展名文件在资源管理器中显示的文件图标，如果不指定(为空)，将使用[关联的可执行程序]的图标
        /// Application.ExecutablePath + ",0"; 使用程序图标
        /// 或 img 文件绝对路径（格式似乎不限）
        /// </summary>  
        public string IconPath {
            get
            {
                if (string.IsNullOrWhiteSpace(iconPath))
                {
                    return "";
                }
                return iconPath;
            }
            set
            {
                iconPath = value;
            }
        }
        /// <summary>  
        /// 应用程序路径  
        /// </summary>  
        public string ExePath
        {
            get
            {
                if (!File.Exists(exePath))
                {
                    throw new FormatException("后缀要关联的可执行程序不存在");
                }
                return exePath;
            }
            set
            {
                if (!File.Exists(value))
                {
                    throw new FormatException("后缀要关联的可执行程序不存在");
                }
                exePath = value;      
            }
        }

        private string extendName;
        private string exePath;
        private string iconPath;

        /// <summary>
        /// 创建FileTypeRegInfo实例 三个必须参数
        /// </summary>
        /// <param name="extendName">必须是.开头的后缀名</param>
        /// <param name="exePath">关联的可执行程序路径</param>
        public FileTypeRegInfo(string extendName, string exePath)
        {
            if (extendName != null)// 不应该在构造函数中发生异常，但赋值仍可能异常
            {
                this.ExtendName = extendName;
            }
            ExePath = exePath;
        }
    }
}
