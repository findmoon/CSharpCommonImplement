如何通过C#启用或关闭Windows功能

> turn on or off windows feature

[How to Toggle Functionality in C# with Feature Flags](https://developer.okta.com/blog/2021/07/28/toggle-feature-flags-csharp)

[4 ways to enable the latest C# features](https://www.meziantou.net/4-ways-to-enable-the-latest-csharp-features.htm)

比如，安装IIS、IIS模块，都属于这一块的操作

[How Enable Windows Feature from C# Program?](https://www.codeproject.com/questions/855923/how-enable-windows-feature-from-csharp-program)

[Enable / disable Windows turn on or off feature in Win 10 in C#](https://social.msdn.microsoft.com/Forums/vstudio/en-US/575345b4-b6f5-4c42-829e-00dd18f737fe/enable-disable-windows-turn-on-or-off-feature-in-win-10-in-c?forum=csharpgeneral)

[Enable or Disable Windows Features Using DISM](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/enable-or-disable-windows-features-using-dism?view=windows-11)


[C#操作Windows控制面板](https://www.cnblogs.com/zhaotianff/p/7649877.html)

------------

[[C#.net资料]visual studio打包可安装的exe程序(添加配置文件)，新手小白最全教程](https://zhuanlan.zhihu.com/p/622419157)

关于 Microsoft Visual Studio Installer Projects 自定义的 install 安装操作，在覆盖安装时不会执行的问题，可能有另一种考虑或情况：覆盖安装时，只把更新的程序集（有更新版本的程序，进行覆盖），只需实现升级即可，只需扩展的自定义install属于额外操作，而不属于升级操作（尤其是版本升级）。那么，也就不需要执行自定义install。

也就是，固定好自定义的install/uninstall操作，后续只修改版本，那么覆盖升级就是比较正确的。

若，需要修改install/uninstall的操作，则必须卸载之前版本，或，不检测之前的版本，而是同时共存新的安装版本！

