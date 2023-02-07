**C#一个功能全面、使用简单的ini配置文件读写操作的封装类**

[toc]

# ini文件简要介绍

`.ini`文件是 Initialization File 的缩写。其主要目的就是用于程序的一些初始化参数配置。文件后缀也可以为`.conf`、`.cfg`等。

ini文件的格式如下，由`[section]`和其下的具体配置项`key=value`组成。

```ini
[section]
key1=value1
key2=value2

[section2]
key3=value3
```

每个节之后的键值对都属于该section。一个节的开始就是上一个节的结束。


# 读写ini的操作类 - IniOperater

下面是全部的代码，简短清晰，直接放在了 System.Text 命名空间下，提供了读、写、删除和判断key是否存在的方法。

基本参考自[Reading/writing an INI file](https://stackoverflow.com/questions/217902/reading-writing-an-ini-file)中的回答。只不过`Path`改为公共只读，同时添加提供额外读写ini设置的静态方法。以及，获取所有section的方法`GetAllSections()`，和其他一些小改动。

代码不多，且都有注释，基本可以直接看懂并上手使用！

所有配置项必须在`[section]`下，无法获取或设置无section的配置项。因此，提供默认的section为`[程式名]`。

> 使用 win32 API GetPrivateProfileString/WritePrivateProfileString 对ini文件的读取操作。文件无需提前创建，也就是，如果文件不存在，在写入时会自动创建。且文件或配置项不存在，读取不会报错，默认返回空字符串""，也可以指定其他获取时的默认值。

> 读写基本功能，如果需要更多功能，可以参考其他相关的github项目。

```C#
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
```

# 使用测试

新建项目 `IniOperaterAndUse`，测试使用`IniOperater.cs`操作ini文件。

```C#
 var iniFile = new IniOperater("my.ini");

// ini中如果出现多个=，将有可能读取错乱
iniFile.Write("a=c", "http://127.0.0.1:8090?id=10");
var r=iniFile.Read("a=c");
var r_a=iniFile.Read("a");
Console.WriteLine(r);
Console.WriteLine(r_a);

var r_a_iniTest = iniFile.Read("a", "IniTest");
Console.WriteLine(r_a_iniTest);

staticTest.GetstaticTest();

// 获取所有section
var sections = iniFile.GetAllSections();
Console.WriteLine(string.Join(",", sections));

var sections_no = iniFile.GetAllSectionsWithOutDefault();

// 新指定文件，自动生成
var iniFile2 = new IniOperater("my2.ini");
iniFile2.Write("s","m");
Console.WriteLine(iniFile2.Read("s"));

var r2 = iniFile2.Read("s2",defaultValue: "m2");
var r3 = iniFile2.Read("s3", defaultValue: "m3");
Console.WriteLine(r2);
Console.WriteLine(r3);

Console.ReadLine();

//// 没有section 无法 设置读取
//iniFile2.WriteWithoutSection("s1", "m1");
//Console.WriteLine(iniFile2.ReadWithoutSection("s1"));
```