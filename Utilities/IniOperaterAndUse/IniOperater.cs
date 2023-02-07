using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace System.Text
{
    /// <summary>
    /// ini文件操作类。
    /// 使用 win32 的 GetPrivateProfileString、WritePrivateProfileString。无法读写操作 没有section 的配置项，未指定section时，默认为[程序名]
    /// </summary>
    public class IniOperater
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
        public IniOperater(string IniPath = null)
        {
            var iniFile = IniPath ?? _exe;
            if (!iniFile.EndsWith(".ini"))
            {
                iniFile += ".ini";
            }
            Path = new FileInfo(iniFile).FullName;
        }

        /// <summary>
        /// 读取设置值
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section">默认为[程式名] section</param>
        /// <param name="defaultValue">未读取到时的默认值</param>
        /// <returns></returns>
        public string Read(string Key, string Section = null, string defaultValue = "")
        {
            return ReadINISetting(Path, Section ?? _exe, Key, defaultValue);
        }
        /// <summary>
        /// 写入设置值，如果未指定Section，默认写入到[程序名]Section下。
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="Section">默认为[程式名] section</param>
        public void Write(string Key, string Value, string Section = null)
        {
            WriteINISetting(Path, Section ?? _exe, Key, Value);
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section">默认为[程式名] section</param>
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? _exe);
        }
        /// <summary>
        /// 删除节section
        /// </summary>
        /// <param name="Section">默认为[程式名] section</param>
        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? _exe);
        }
        /// <summary>
        /// key是否存在
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <returns></returns>
        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section ?? _exe).Length > 0;
        }
        /// <summary>
        /// 获取除了默认section [程式名] 之外的所有section
        /// </summary>
        /// <returns></returns>
        public string[] GetAllSectionsWithOutDefault()
        {
            return GetAllSections(_exe);
        }

        /// <summary>
        /// 获取所有的Section,排除指定的section
        /// </summary>
        /// <param name="excludes">排除的section</param>
        /// <returns></returns>
        public string[] GetAllSections(params string[] excludes)
        {
            var exs = new List<string>(excludes);
            var allLines = File.ReadAllLines(Path);
            var sections = new List<string>();
            foreach (var line in allLines)
            {
                var match = Regex.Match(line, @"^\[(.+?)\]");
                if (match.Success)
                {
                    if (!exs.Contains(match.Groups[1].Value))
                    {
                        sections.Add(match.Groups[1].Value);
                    }
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
        /// <param name="defaultValue">未读取到时的默认值</param>
        /// <returns></returns>
        public static string ReadINISetting(string iniFilePath, string section, string key, string defaultValue = "")
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, retVal, 255, iniFilePath);
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
