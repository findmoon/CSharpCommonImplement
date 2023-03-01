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
    /// 注册文件类型，关联打开程序 【管理员权限】， 文件类型注册
    /// </summary>
    public static class FileTypeRegister
    {
        /// <summary>  
        /// 使文件类型与对应的图标及应用程序关联起来
        /// CreateSubKey(subKey, true) 从NF 4.6 开始支持创建或打开现有
        /// </summary>
        /// <param name="regInfo">文件类型注册信息类</param>
        /// <param name="detectExist">是否检测存在，如果检测并存在，将不执行创建</param>
        public static void RegisterFileType(FileTypeRegInfo regInfo, bool detectExist = true)
        {
            if (detectExist && FileTypeRegistered(regInfo.ExtendName,true))
            {
                return;
            }

            //HKEY_CLASSES_ROOT/{ExtendName}
            using (RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName))
            {
                string relationName = regInfo.RelationName; // ExtendName关联到打开该后缀的{relationName}注册表信息【原则上名称可以任意，"_FileType"后缀也不必须】
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
        /// <param name="regInfo">文件类型注册信息类</param>
        public static bool RegisterFileTypeUpdate(FileTypeRegInfo regInfo)
        {
            if (!FileTypeRegistered(regInfo.ExtendName))
            {
                RegisterFileType(regInfo, false);
            }
            else
            {
                string relationName = regInfo.RelationName;
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
        /// <param name="extendName">扩展名 必须是.开头的后缀名</param>
        public static FileTypeRegInfo GetFileTypeRegInfo(string extendName)
        {
            if (!FileTypeRegistered(extendName))
            {
                return null;
            }

            string relationName = FileTypeRegInfo.GetRelationName(extendName);
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
        /// <param name="extendName">扩展名 必须是.开头的后缀名</param>
        /// <param name="strictDetect">是否严格判断，推荐true</param>
        /// <returns></returns>
        public static bool FileTypeRegistered(string extendName,bool strictDetect=false)
        {
            // 重复执行问题，比如检查extendName、FileTypeRegistered，在很多地方都会检查，这些地方再调用它就会有重复再次执行检查。
            // 应该分离出来，对外公开的方法加入检查方法；对内，则考虑适当使用 无检查的方法 等等。
            DetectExtendName(extendName);
            using (RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName))
            {
                if (softwareKey != null)
                {
                    return true;
                }
                var value = (string)softwareKey.GetValue("");
                if (strictDetect)
                {
                    return value == FileTypeRegInfo.GetRelationName(extendName);
                }
                return !string.IsNullOrWhiteSpace(value);
            }
        }

        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// 检测后缀名是否正确合法
        /// </summary>
        /// <param name="extendName">扩展名 必须是.开头的后缀名</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        internal static bool DetectExtendName(string extendName)
        {
            if (string.IsNullOrWhiteSpace(extendName))
            {
                throw new ArgumentNullException(nameof(extendName));
            }
            if (!extendName.StartsWith("."))
            {
                throw new FormatException("当前后缀名不正确，必须是.开头的后缀名");
            }
            return true;
        }
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
                // FileTypeRegister.DetectExtendName(extendName);
                return extendName;
            }
            set
            {
                FileTypeRegister.DetectExtendName(extendName);
                extendName = value;
            }
        }
        /// <summary>
        /// 后缀类型在注册表中的关联注册表项的名字。此处统一为 '无.扩展名大写_FileType'
        /// </summary>
        public string RelationName => GetRelationName(extendName);
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
            //if (extendName != null)// 不应该在构造函数中发生异常，但赋值仍可能异常
                this.ExtendName = extendName;

            ExePath = exePath;
        }
        /// <summary>
        /// 获取 FileTypeRegInfo 标准的关联注册表项的名字。统一为 '无.扩展名大写_FileType'
        /// </summary>
        /// <param name="extendName">必须是.开头的后缀名</param>
        /// <returns></returns>
        public static string GetRelationName(string extendName)=>extendName.Substring(1).ToUpper() + "_FileType";
    }
}
