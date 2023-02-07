/*
 * License：BSD-3-Clause License https://github.com/Bluegrams/SettingsProviders/blob/master/LICENSE
 */

using Bluegrams.Application;
using System.IO;
using System.Reflection;

// 如果是单独使用 PortableSettings cs文件，按以下步骤进行：
// 第0步 ：复制 PortableSettings.cs、PortableSettingsProvider.cs、PortableSettingsProviderBase.cs 这三个文件到自己项目
// 第一步：添加 System.Configuration 引用
// 第二步：在程序main函数最开始。初始化 PortableSettings，传入自己的Settings设置，如：new PortableSettings(Properties.Settings.Default); 【只是初始化不需要接受或使用new的对象，后面可以修改为静态方法，而不是创建实例】
// 之后  ：剩下就和平时使用 Properties.Settings 一样

// 如果是引用 PortableSettings 类库，引用后，直接从上面的第一步开始

namespace System.Configuration
{
    public class PortableSettings
    {
        /// <summary>
        /// 指定设置文件的路径，为空时将保存在程序所在目录下 必须提供至少一个Settings 
        /// </summary>
        /// <param name="settingFilePath"></param>
        public PortableSettings(string settingsFilePath, string settingsFileName, ApplicationSettingsBase setting, params ApplicationSettingsBase[] settingsList)
        {
            try
            {
                var psProvider = new PortableSettingsProvider(settingsFilePath, settingsFileName);

                psProvider.ApplyProvider(setting);
                psProvider.ApplyProvider(settingsList);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to initialize PortableSettings!{ex.Message}");
            }
        }
        /// <summary>
        /// 指定设置文件名，用于保存在不同的文件或多个文件中 必须提供至少一个Settings 
        /// </summary>
        /// <param name="settingFilePath"></param>
        public PortableSettings(string settingsFileName, ApplicationSettingsBase setting, params ApplicationSettingsBase[] settingsList) : this(Path.Combine(LocalAppPath, AssemblyCompany, AssemblyName), settingsFileName, setting, settingsList)
        {
        }
        /// <summary>
        /// 必须提供至少一个Settings，保存在LocalAppData目录
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="settingsList"></param>
        public PortableSettings(ApplicationSettingsBase setting, params ApplicationSettingsBase[] settingsList) : this(Path.Combine(LocalAppPath, AssemblyCompany, AssemblyName), "app.config", setting, settingsList)
        {
        }

        //public string Con
        /// <summary>
        /// 执行的程序集公司
        /// </summary>
        internal static string AssemblyCompany
        {
            get
            {
                //var companyAttrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                //var companyAttrs = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                var companyAttrs = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (companyAttrs.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)companyAttrs[0]).Company;
            }
        }
        /// <summary>
        /// 执行的程序集名
        /// </summary>
        internal static string AssemblyName
        {
            get
            {
                //return Assembly.GetExecutingAssembly().GetName().Name;
                return Assembly.GetEntryAssembly().GetName().Name;
            }
        }
        /// <summary>
        /// 当前用户的%LocalAppData%路径
        /// </summary>
        internal static string LocalAppPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
        }

    }
}
