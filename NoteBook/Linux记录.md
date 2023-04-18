Linux记录.md

# [MySQL使用技巧: 如何查看mysql正在执行的SQL语句](https://www.coorw.com/3359.html)

# [linux下给网站目录添加sgid权限有哪些好处](https://www.coorw.com/2599.html)

今天这里主要讨论给网站根目录添加sgid权限

给目录添加sgid权限，可以使在该目录下创建文件或文件的所属组继承该目录的所属组。

假设该目录的所属组是www，那么添加了sgid权限后，无论你是用什么账户创建文件，其所属组都是www。

下面提供下给目录添加sgid权限的方法

chmod g+s .

ll -d .

看看所属组的权限有没有 s 位 ，比如

drwxr-sr-x 2 www www 4096 Apr 22 11:58

如此以后在网站根目录下创建文件的所属组就是www，无论你是用什么账户创建的。

# [/ect/fstab与/etc/mtab的区别](https://blog.51cto.com/u_15127623/3433220)

# [开机挂载mount etc/fstab与/etc/rc.d/rc.local区别](https://developer.aliyun.com/article/516428)


> 关于 `etc/fstab` 与 `/etc/rc.d/rc.local` 的区别：
> 
> fstab里面会在程序启动前加载上NFS文件系统，放到`rc.local`里往往造成程序启动加载时找不到路径。
> 
> 如果有程序依赖于NFS的话还是的放到fstab比较好。
>
> `网络文件系统 - Network File System(NFS)`

# [How to Install, Configure and Secure FTP Server in RHEL 8](https://www.tecmint.com/install-ftp-server-in-rhel-8/)

[Installing and Configuring Pure-FTPD on RHEL / CentOS 7](https://blog.momentumhosting.cloud/general-hosting/installing-and-configuring-pure-ftpd-on-rhel-centos-7/)

[centos pure-ftpd配置及错误解决](https://www.cnblogs.com/gaoxu387/p/8033796.html)

# [How to Install vsftpd (ftp server) on CentOS 8 / RHEL 8](https://www.linuxtechi.com/install-vsftpd-server-centos-8-rhel-8/)

# [linux如何mount挂载磁盘并设置开机自动mount](https://blog.csdn.net/mochou111/article/details/81298613) 格式化并挂载


# [硬件设计---认识时钟篇](https://blog.csdn.net/weixin_43813325/article/details/106924618)

[系统时钟在电子设备关机后如何持续记录时间？](https://www.zhihu.com/question/279200764)

[Linux时间系统之RTC时间](https://blog.csdn.net/u013686019/article/details/57126940)

[时间篇之linux系统时间和RTC时间](https://www.cnblogs.com/aozhejin/p/15978539.html)


[【STM32】RTC实时时钟，步骤超细详解，一文看懂RTC](https://blog.csdn.net/as480133937/article/details/105026033)


[UTC、RTC、UNIX时间戳、localtime 理解](https://blog.csdn.net/qq_37698947/article/details/115772329)

RTC基本上由晶体振荡器和振荡器电路组成。这种晶体单元是石英，具有在施加电压时高速振动的特性。

这个频率越高，手表的精度就越高，但由于RTC不需要达到那个级别的规格，所以大约是32.768kHz，月差（一个月偏差多少）在1分钟之内。这种振动被振荡电路转换成时钟信号，时间显示在显示器上。该RTC可以安装在CPU的主板上，也可以安装在外部IC上。

# [如何正确设计实时时钟RTC？](https://zhuanlan.zhihu.com/p/37226700)


---------------------------------------------------
--------------------------------------------------------------
-------------------------------------------------------------------------

# [Linux 软链接——ln命令详解](https://blog.csdn.net/annita2019/article/details/105481449)

https://www.runoob.com/linux/linux-comm-ln.html

https://www.cnblogs.com/my-show-time/p/14658895.html

https://blog.csdn.net/wangjie72270/article/details/122196213


https://cn.bing.com/search?q=crontab&aqs=edge..69i57&FORM=ANCMS9&PC=CNNDDB


https://blog.csdn.net/asmartkiller/article/details/109529391?ydreferer=aHR0cHM6Ly9jbi5iaW5nLmNvbS8%3D


https://blog.csdn.net/asmartkiller/article/details/109529391

https://zhuanlan.zhihu.com/p/575449787


https://cloud.tencent.com/developer/article/1546322


