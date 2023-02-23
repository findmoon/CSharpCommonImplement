**安装包制作全讲解三：Setup Project制作安装包添加各种快捷方式，解决已安装了该产品另一个版本的问题，实现重复多次安装及升级**

[toc]

> 在自定义操作中，添加的安装类库中添加的组件无法显示使用。

# 关于：已经安装了该产品的另一个版本，无法继续安装此版本。

![](img/20230208222355.png)  

应该可以通过设置，实现覆盖安装，允许程序安装后再次运行安装包。暂未处理。


# 生成一个网页快捷方式

# 无法在生成后打开程序或打开一个文件【坑】

该功能无法实现，坑死人了！！！



# 添加的用户界面对话框，无法实现必填项的效果

# 重点注意项

## CustomActionData 传递数据到自定义操作的类库中

传递生产商、产品名、作者名、版本信息等：`/manufacturer="[Manufacturer]" /productName="[ProductName]" /author="[Author]" productVersion="[ProductVersion]"`（Author和ProductVersion未测试）

> **注意不要大写**

如果 `CustomActionData` 中**参数的格式错误**，将会在安装时直接报错：`Eoor 1001用法：InstallUtil [/u | /uninstall] [option [...]] assembly [[[option [...]] assembly] [...]]`
`InstallUtil 执行每个给定程序集中的安装程序。如果指定 /u 或 /uninstall 开关，则它卸载程序集；反之，则安装它们。`

![](img/20230223114538.png)  

**解决办法就是修改 `CustomActionData` 的值为正确的格式，不要有错误，不要有错误！！！**

## 不要直接复制程序文件到安装项目的文件系统(Application Folder)中

为了方便操作，后续看到有文章介绍可以直接将程序文件复制粘贴到 文件系统的`Application Folder`下。

相对来说，这是一种非常方便的制作安装包时处理源程序文件的方法，因为这样不会存在主输出中无法处理加载子文件夹文件的问题。

直接复制粘贴（Ctrl+C,V）：

![](img/20230223095956.png)  

**但是，但是，但是，这可能会产生隐藏的程序加载的问题**。

我测试的是Web发布后的程序文件，使用这种方式部署后，Web访问产生了报错：`未能加载文件或程序集"System.Data.Common"或它的某-个依赖项。找到的程序集清单定义与程序集引用不匹配。(异常来自 HRESULT:0x8131040)`。

> 查看安装后的该dll文件与源程序文件，确实不一样。不知道打包时是如何处理。

但是，这个问题应该不总是出现。不过还是，**不建议直接复制粘贴程序文件**。

> 复制粘贴的程序打包处理方式，应该和“内容文件”不一样。
>
> 并且，复制粘贴后打包的内容体积明显增多，不知道额外增加了哪些文件。。。

## 关于打包时的压缩问题

右键安装项目->属性，可以看到压缩处理，默认为“优化速度”：

![](img/20230223100641.png)  

同样是 `ASP.NET`(MVC) 项目，默认采用压缩时，竟然在安装部署完成后，报错编译时错误：`Compiler Error Message: The compiler failed with error code -532462766`（`"/"应用程序中的服务器错误`）

> **解决办法就是不要采用压缩！！！**
>
> 估计问题原因是在安装解压缩时`Web.config`文件中的特殊字符处理有问题，没有完全还原。
>
> 这也是一个不常遇到的问题。

针对该问题最简单的解决办法是删除`Web.config`中的`<system.codedom>`，如下，删除所示的所有行：

```xml
<system.codedom>
    <compilers>
      <compiler language="c#;cs;csharp" extension=".cs" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:6 /nowarn:1659;1699;1701" />
      <compiler language="vb;vbs;visualbasic;vbscript" extension=".vb" type="Microsoft.CodeDom.Providers.DotNetCompilerPlatform.VBCodeProvider, Microsoft.CodeDom.Providers.DotNetCompilerPlatform, Version=1.0.3.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" warningLevel="4" compilerOptions="/langversion:14 /nowarn:41008 /define:_MYTYPE=\&quot;Web\&quot; /optionInfer+" />
    </compilers>
</system.codedom>
```

> 参考自 [The compiler failed with error code -532462766](https://9to5answer.com/compiler-error-message-the-compiler-failed-with-error-code-532462766)



