**CSharp.NET中实现单文件应用程序的方法总结二：资源嵌入与作为资源文件提取到临时文件夹执行.md**

[toc]

> 介绍生成 单个可执行程序、单个文件应用、独立exe程序 - 创建单个可执行程序文件 的方式。

创建项目 EmbedAssemblyAndAllResources AsResourceExtraToTempExec_Net6 AsResourceExtraToTempExec_Netfx


The solution I found for setting image to Msi file is ,

create a window application and add the msi file to that project as a content and write code in the windows application to write the msi file to a temp folder in the system and invoke it using the Process.Start method

