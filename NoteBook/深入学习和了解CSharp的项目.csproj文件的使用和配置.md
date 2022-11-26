**深入学习和了解C#的项目.csproj文件的使用和配置（文件格式、编译流程、.csproj 文件）**

[toc]

# csproj介绍

# 关于 CopyToOutputDirectory

`CopyToOutputDirectory`用于复制文件到输出目录

```xml
<!-- PreserveNewest 表示如果较新则复制 -->
<ItemGroup Condition="'$(Configuration)'=='Release'">
    <Content Include="custom_settings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
```

但是，似乎没有没有存在则不复制的取值。取值只有三个：

- `PreserveNewest`：较新则复制
- `Always`：总是复制
- `Never`：从不复制

关于配置文件根据开发、生产、稳定版本，可以参考类似`appsettings.xxx.json`配置文件的实现（不知能不能用于自定义配置文件，太菜了...），`xxx`包含Development, Production, Staging几个环境



# 