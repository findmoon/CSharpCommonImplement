**创建单个可执行程序文件（独立exe程序）的实现方法一：作为资源文件提取到临时文件夹中执行**

[toc]

The solution I found for setting image to Msi file is ,

create a window application and add the msi file to that project as a content and write code in the windows application to write the msi file to a temp folder in the system and invoke it using the Process.Start method

