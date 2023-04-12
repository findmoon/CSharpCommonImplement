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