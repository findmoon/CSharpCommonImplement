**一个功能全面的Ini文件读写封装类**

下面是全部的代码，简短清晰，直接放在了 System.Text 命名空间下，提供了读、写、删除和判断key是否存在的方法，基本参考自[Reading/writing an INI file](https://stackoverflow.com/questions/217902/reading-writing-an-ini-file)中的回答。只不过Path改为公共只读，同时添加提供额外读写ini设置的静态方法。以及，添加获取所有section的方法`GetAllSections()`，和其他一些小改动。

代码不多，且都有注释，基本可以直接看懂并上手使用！

> （GetPrivateProfileString/WritePrivateProfileString）下面ini文件的读取操作。文件无需提前创建，也就是，如果文件不存在，在写入时会自动创建。且文件不存在，读取不会报错，返回空字符串""。

> 读写基本功能，如果需要更多功能，可以参考其他相关的github项目。

```cs
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace System.Text
{
    class IniFile
    {
        /// <summary>
        /// ini文件路径
        /// </summary>
        public string Path { get; }

        string _exe = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// 指定ini文件路径，默认为当前程序所在路径的程序名.ini文件
        /// </summary>
        /// <param name="IniPath"></param>
        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? _exe + ".ini").FullName;
        }

        /// <summary>
        /// 读取设置值
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <returns></returns>
        public string Read(string Key, string Section = null)
        {
            return ReadINISetting(Path, Section ?? _exe, Key);
        }
        /// <summary>
        /// 写入设置值
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="Section"></param>
        public void Write(string Key, string Value, string Section = null)
        {
            WriteINISetting(Path, Section ?? _exe, Key, Value);
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section);
        }
        /// <summary>
        /// 删除节section
        /// </summary>
        /// <param name="Section"></param>
        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section);
        }
        /// <summary>
        /// key是否存在
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <returns></returns>
        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        /// <summary>
        /// 获取所有的Section
        /// </summary>
        /// <returns></returns>
        public string[] GetAllSections()
        {
            var allLines = File.ReadAllLines(Path);
            var sections = new List<string>();
            foreach (var line in allLines)
            {
                var match = Regex.Match(line, "^[(.+?)]");
                if (match.Success)
                {
                    sections.Add(match.Groups[1].Value);
                }
            }
            return sections.ToArray();
        }

        /// <summary>
        /// 读取指定路径下的ini设置
        /// </summary>
        /// <param name="iniFilePath"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadINISetting(string iniFilePath, string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, iniFilePath);
            return retVal.ToString();
        }

        /// <summary>
        /// 写入指定路径下的ini设置
        /// </summary>
        /// <param name="iniFilePath"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteINISetting(string iniFilePath, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, iniFilePath);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
            StringBuilder lpReturnedString, uint nSize, string lpFileName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool WritePrivateProfileString(
        string lpAppName, string lpKeyName, string lpString, string lpFileName);
    }
}
```