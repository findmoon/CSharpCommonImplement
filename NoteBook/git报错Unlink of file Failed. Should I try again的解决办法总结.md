**git报错Unlink of file Failed. Should I try again? 的正确解决办法总结**

[toc]

先说一下我遇到这个问题的解决办法：**以管理员身份运行 Git Bash 即可解决**。

在执行`git pull`命令时遇到这烦人的。网上绝大多数介绍是由于文件占用导致的`Unlink of file`，其本质错误原因是由于 git 无法连接修改该文件导致。【无权限修改文件、文件被占用无法修改文件等】

最终参考 stackoverflow 的[Unlink of file Failed. Should I try again?](https://stackoverflow.com/questions/4389833/unlink-of-file-failed-should-i-try-again)回答，里面基本包含了所有可能的解决办法。

1. 文件被占用。关闭使用文件的软件，释放文件占用，比如 VS、IDEA、eclipse、Word等。也可以考虑使用 `Process Explorer` 查找占用文件的进程是哪个。

2. 使用`git gc`命令，执行git的垃圾回收，移除临时的、不必须要的文件等。也可以执行`git gc --auto`命令。

3. 以管理员身份运行解决此问题。该问题发生在`a non-elevated command line`下，针对Windows解决。参考[此处](https://stackoverflow.com/a/12280076/184176)

重启、关闭所有软件等都不好使。

> Git 往磁盘保存对象时默认使用的格式叫松散对象 (loose object) 格式。Git 时不时地将这些对象打包至一个叫 packfile 的二进制文件以节省空间并提高效率。当仓库中有太多的松散对象则就会提示你运行 ' git gc '。
> 
> 可以运行 'find .git/objects -type f' 命令，查看一下 objects 目录里有多少对象。
> 
> 然后在运行 'git gc' 命令后，在执行刚才的命令，看下 object 目录里面还剩下多少对象。
> 
> 相关文章：https://www.cnblogs.com/ayseeing/p/4226471.html 、  https://gitbook.liuhui998.com/4_10