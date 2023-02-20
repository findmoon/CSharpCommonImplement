使用SQLite 无法加载 DLL“SQLite.Interop.dll 找不到指定的模块

 

### 场景

最近弄个新项目，需要用到ORM。基本就是VS2017+C#+Dapper+[Sqlite](https://so.csdn.net/so/search?q=Sqlite&spm=1001.2101.3001.7020)这样的结构。从Nuget上下载相关的package，并关联好对应的数据结构。编译通过，但在运行时报错。如下图：  
![这里写图片描述](https://img-blog.csdn.net/20180911150208699?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2t1eXUwNQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

查看了下，packages目录下对应的System.Data.SQLite，System.Data.SQLite.Core文件内容都在，SQLite.Interop.dll也在。但就是没有生成到执行目录下。很费解。

### 解决方案

注意了下SQLite.Interop.dll所在目录，System.Data.SQLite.Core.1.0.109.1\\build\\net46下存在X64，X86两个文件夹。想到了项目工程编译选项设置的原因。于是：  
![这里写图片描述](https://img-blog.csdn.net/20180911150612742?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2t1eXUwNQ==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

根据需求选择对应的x86或x64，将会复制对应目录下的SQLite.Interop.dll至执行目录。成功运行。

错误很小，但希望能帮到更多的人。