**C#/.NET中实现单文件应用程序的方法总结一：官方的单文件发布PublishSingleFile**


[toc]

> 介绍生成 单个可执行程序、单个文件应用、独立exe程序 - 创建单个可执行程序文件 的方式。


# Single-file deployment

[Single-file deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview?tabs=cli)

[Build a "true" single executable in .NET 5.0 for Windows, Linux and MacOs](https://www.mihajakovac.com/build-dotnet-single-exe/)

## Sample project file

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <PublishSingleFile>true</PublishSingleFile>
    <SelfContained>true</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  </PropertyGroup>

</Project>
```

## Exclude files from being embedded

```xml
<ItemGroup>
  <Content Update="Plugin.dll">
    <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
    <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
  </Content>
</ItemGroup>
```

## Include PDB files inside the bundle

```xml
<PropertyGroup>
  <DebugType>embedded</DebugType>
</PropertyGroup>
```

## Native libraries

IncludeNativeLibrariesForSelfExtract 

# 介绍

- 单文件、自包含

```sh
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true
```

- 单文件、自包含、代码修剪(IL Trimming)

```sh
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true
```

- 单文件、自包含、单文件压缩

```sh
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:EnableCompressionInSingleFile=true
```


- WPF 等程序实现真正的单文件应用：

Thank you, it is really helpful. Just in case anyone needs, for WPF to create real single file app this shall be included
-p:IncludeNativeLibrariesForSelfExtract=yes
since native libraries will be separate .dll files which will be necessary to have alongside executable.

谢谢，这真的很有帮助。以防万一有人需要，对于WPF创建真正的单文件应用程序，应包括此内容
-p：IncludeNativeLibrariesForSelfExtract=yes
由于本机库将.dll文件分开，这些文件必须与可执行文件一起使用。


# 参考

- [Single File Apps In .NET 6](https://dotnetcoretutorials.com/2021/11/10/single-file-apps-in-net-6/)