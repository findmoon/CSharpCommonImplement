**C#自定义App.config/Web.config配置文件、自定义配置节、多文件**

[toc]

[C#配置文件之App.config和.settings](https://blog.csdn.net/yizhou2010/article/details/123007459)

[在C#类库中使用App.config文件自定义配置](https://www.cnblogs.com/shalves/archive/2013/03/11/use_appconfig_on_csharp_library.html)

[C#封装系列---②App.config配置文件详解（如何对配置文件的配置节进行分组取值）](https://blog.csdn.net/elsa15/article/details/103748267)

[一步一步教你玩转.NET Framework的配置文件app.config](https://www.cnblogs.com/tonnie/archive/2010/12/17/appconfig.html)


[在Web.config或App.config中的添加自定义配置](https://www.cnblogs.com/yukaizhao/archive/2011/12/02/net-web-config-costom-config-implement.html)

[C#自定义配置文件（一）](https://www.cnblogs.com/yangyongdashen-S/p/YiRenXiAn_CSharp_Config_1.html)

[如何在配置文件app.config中自定义节点——附C#源码](https://blog.csdn.net/yangwohenmai1/article/details/89393608)


[C#配置文件configSections详解](https://www.cnblogs.com/lxshwyan/p/10828305.html)


如果你的程序是对其它程序的配置文件进行操作,代码如下：
ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
filemap.ExeConfigFilename = filePath;//配置文件路径
config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
if (AppSettingsKeyExists("Refresh", config))
{
config.AppSettings.Settings["Refresh"].Value = M_TimeRead.ToString();
} if (AppSettingsKeyExists("MachineNo", config))
{
config.AppSettings.Settings["MachineNo"].Value = M_MachineNo; }
config.Save(ConfigurationSaveMode.Modified);
ConfigurationManager.RefreshSection("appSettings");
config.ConnectionStrings.ConnectionStrings["connectionString"].ConnectionString = M_ConnectionString;
config.Save(ConfigurationSaveMode.Modified);
ConfigurationManager.RefreshSection("connectionStrings"); 
数据库字符串加密
ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
filemap.ExeConfigFilename = Application.ExecutablePath + ".Config"; //filePath;
config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
//指定我所要的节点
ConfigurationSection section = config.ConnectionStrings;
if ((section.SectionInformation.IsProtected == false) && (section.ElementInformation.IsLocked == false))
{
//制定节点加密
section.SectionInformation.ProtectSection(protect);
//即使没有修改也保存设置
section.SectionInformation.ForceSave = true;
//配置文件内容保存到xml
config.Save(ConfigurationSaveMode.Full);
} 

————————————————
版权声明：本文为CSDN博主「wzk456」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
原文链接：https://blog.csdn.net/wzk456/article/details/49001391

[Add Custom Configuration Elements In .NET](https://www.c-sharpcorner.com/article/add-custom-configuration-elements-in-net/)

[Creating Custom Configuration Sections in App.config](https://blog.ivankahl.com/creating-custom-configuration-sections-in-app-config/)
