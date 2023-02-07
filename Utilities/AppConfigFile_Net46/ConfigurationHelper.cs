using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AppConfigFile_Net46
{
    /// <summary>
    /// 应用程序配置文件 App.config/Web.config 访问辅助类【访问的是 AppSettings 配置节下的配置项】
    /// </summary>
    internal class ConfigurationHelper
    {
        /// <summary>
        /// 读取string配置项
        /// </summary>
        /// <param name="appSettingKey">AppSetting配置项的Key值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回对应配置项的string类型value值</returns>
        public static string GetAppSettingValue(string appSettingKey, string defaultValue = "")
        {
            string result = ConfigurationManager.AppSettings[appSettingKey];
            if (string.IsNullOrEmpty(result))
            {
                return defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 读取bool配置项
        /// </summary>
        /// <param name="appSettingKey">AppSetting配置项的Key值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回对应配置项的bool类型value值</returns>
        public static bool GetAppSettingValue(string appSettingKey, bool defaultValue)
        {
            string result = ConfigurationManager.AppSettings[appSettingKey];
            if (String.IsNullOrEmpty(result))
            {
                return defaultValue;
            }
            bool boolResult = false;
            if (bool.TryParse(result, out boolResult))
            {
                return boolResult;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 读取int配置项
        /// </summary>
        /// <param name="appSettingKey">AppSetting配置项的Key值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回对应配置项的int类型value值</returns>
        public static int GetAppSettingValue(string appSettingKey, int defaultValue)
        {
            string result = ConfigurationManager.AppSettings[appSettingKey];
            if (String.IsNullOrEmpty(result))
            {
                return defaultValue;
            }
            int intResult = 0;
            if (int.TryParse(result, out intResult))
            {
                return intResult;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 读取double类型配置项
        /// </summary>
        /// <param name="appSettingKey">AppSetting配置项的Key值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回对应配置项的double类型value值</returns>
        public static double GetAppSettingValue(string appSettingKey, double defaultValue)
        {
            string result = ConfigurationManager.AppSettings[appSettingKey];
            if (String.IsNullOrEmpty(result))
            {
                return defaultValue;
            }
            double doubleResult = 0.0;
            if (double.TryParse(result, out doubleResult))
            {
                return doubleResult;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 修改App.config中AppSetttings中的配置项
        /// </summary>
        /// <param name="name">要修改的配置项的名称</param>
        /// <param name="value">要修改的配置项的值</param>
        /// <returns></returns>
        public static bool SetAppSettings(string name, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//当前应用程序的配置文件
                if (config != null)
                {
                    //AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
                    // if (appSettings.Settings.AllKeys.Contains(name))
                    if (config.AppSettings.Settings.AllKeys.Contains(name))
                    {
                        config.AppSettings.Settings[name].Value = value;
                    }
                    else
                    {
                        config.AppSettings.Settings.Add(name, value);
                    }
                    config.Save(); //保存配置文件  // config.Save(ConfigurationSaveMode.Full);
                    //ConfigurationManager.RefreshSection("appSettings"); // 刷新，更新缓存。无需重启，获取最新的配置值
                    ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("修改app.config配置{0}的值为{1}异常：{2}", name, value, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 修改App.config中AppSetttings中的配置项
        /// </summary>
        /// <param name="name">要修改的配置项的名称</param>
        /// <param name="value">要修改的配置项的值</param>
        /// <returns></returns>
        public static bool SetAppSettings(params KeyValuePair<string, string>[] keyValuePairs)
        {
            if (keyValuePairs == null || keyValuePairs.Length == 0)
            {
                throw new Exception("未指定设置的配置项");
            }
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//当前应用程序的配置文件
                if (config != null)
                {
                    for (int i = 0; i < keyValuePairs.Length; i++)
                    {
                        var name = keyValuePairs[i].Key;
                        if (config.AppSettings.Settings.AllKeys.Contains(name))
                        {
                            config.AppSettings.Settings[name].Value = keyValuePairs[i].Value;
                        }
                        else
                        {
                            config.AppSettings.Settings.Add(name, keyValuePairs[i].Value);
                        }
                    }
                    config.Save(); //保存配置文件  // config.Save(ConfigurationSaveMode.Full);
                    //ConfigurationManager.RefreshSection("appSettings"); // 刷新，更新缓存。无需重启，获取最新的配置值
                    ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("修改app.config配置时发生异常：{0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 删除appsetting节点下配置项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void DeleteAppSettings(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//当前应用程序的配置文件
                //如果当前节点存在，则删除当前节点
                config.AppSettings.Settings.Remove(key);
                config.Save(ConfigurationSaveMode.Modified);
                //ConfigurationManager.RefreshSection("appSettings");
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            else
            {
                Console.WriteLine("当前节点不存在");
            }
        }
    }
}
