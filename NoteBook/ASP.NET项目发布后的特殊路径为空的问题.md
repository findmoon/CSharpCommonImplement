**ASP.NET项目发布后的特殊路径为空的问题**


[why Environment.SpecialFolder.Desktop is empty](https://stackoverflow.com/questions/13850839/why-environment-specialfolder-desktop-is-empty) 中的介绍

没有使用相关的个人账户，就没有对应的特殊目录。App pool默认是以service账户运行。包括`Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)`路径，但是可以获取 `Environment.SpecialFolder.CommonApplicationData`、`Environment.SpecialFolder.CommonDesktopDirectory` 等公共的一些路径

If the site is not running as a user with Interactive Logon privilege, there will be no desktop associated with that user.

That will typically be the case for an application pool in IIS.

It would not be wise to run the application pool with Interactive Logon because it creates a security hole.


Likely the app pool is running under a service account, not your personal Windows account.
