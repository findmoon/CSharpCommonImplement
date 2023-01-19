**2万字总结，体系化全面认识 Nginx【转】**

[toc]

> 原文 [万字总结，体系化带你全面认识 Nginx ！](https://juejin.cn/post/6942607113118023710)
> 
> 在此基础上有所补充。

# Nginx 概述

![](img/20230119153235.png)  

`Nginx` 是开源、高性能、高可靠的 Web 和 反向代理 服务器，支持热部署，几乎可以做到 7 * 24 小时不间断运行，即使运行几个月也不需要重新启动。可以在不间断服务的情况下对软件版本进行热更新。

性能是 `Nginx` 最重要的考量，占用内存少、并发能力强、能支持高达 5w 个并发连接数，最重要的是， `Nginx` 是免费的并可以商业化，配置使用也比较简单。

# Nginx 特点

- 高并发、高性能；
- 模块化架构使得它的扩展性非常好；
- 异步非阻塞的事件驱动模型这点和 `Node.js` 相似；
- 相对于其它服务器来说，它可以连续几个月甚至更长时间运行，而不需要重启服务器，使得它具有高可靠性；
- 热部署、平滑升级；
- 完全开源，生态繁荣；

# Nginx 作用

Nginx 的最重要的几个使用场景：

1. 静态资源服务，通过本地文件系统提供服务；
2. 反向代理服务，延伸出包括缓存、负载均衡等；
3. `API` 服务， `OpenResty` ；


对于前端来说 `Node.js` 并不陌生， `Nginx` 和 `Node.js` 的很多理念类似， `HTTP` 服务器、事件驱动、异步非阻塞等，且 `Nginx` 的大部分功能使用 `Node.js` 也可以实现，但 `Nginx` 和 `Node.js` 并不冲突，都有自己擅长的领域。 

**`Nginx` 擅长于底层服务器端资源的处理（静态资源处理转发、反向代理，负载均衡等）**， `Node.js` 更擅长上层具体业务逻辑的处理，两者可以完美组合。  

用一张图表示 `Nginx` 作用和服务处理应用中位置：

![](img/20230119153513.png)  

# # Nginx 安装

本文演示的是 `Linux` `centOS 7.x` 的操作系统上安装 `Nginx` ，至于在其它操作系统上进行安装可以网上自行搜索。  
  
使用 `yum` 安装 `Nginx` ：

```bash
yum install nginx -y
```

安装完成后，通过 **`rpm -ql nginx` 命令查看 `Nginx` 的安装信息**：

```bash
# Nginx配置文件
/etc/nginx/nginx.conf # nginx 主配置文件
/etc/nginx/nginx.conf.default

# 可执行程序文件
/usr/bin/nginx-upgrade
/usr/sbin/nginx

# nginx库文件
/usr/lib/systemd/system/nginx.service # 用于配置系统守护进程
/usr/lib64/nginx/modules # Nginx模块目录

# 帮助文档
/usr/share/doc/nginx-1.16.1
/usr/share/doc/nginx-1.16.1/CHANGES
/usr/share/doc/nginx-1.16.1/README
/usr/share/doc/nginx-1.16.1/README.dynamic
/usr/share/doc/nginx-1.16.1/UPGRADE-NOTES-1.6-to-1.10

# 静态资源目录
/usr/share/nginx/html/404.html
/usr/share/nginx/html/50x.html
/usr/share/nginx/html/index.html

# 存放Nginx日志文件
/var/log/nginx
```

主要关注的文件夹有两个：

1. `/etc/nginx/conf.d/` 是子配置项存放处， `/etc/nginx/nginx.conf` 主配置文件会默认把这个文件夹中所有子配置项都引入；
2. `/usr/share/nginx/html/` 静态文件都放在这个文件夹，也可以根据你自己的习惯放在其他地方；

# Nginx 常用命令

`systemctl` 系统命令：

```bash
# 开机配置
systemctl enable nginx # 开机自动启动
systemctl disable nginx # 关闭开机自动启动

# 启动Nginx
systemctl start nginx # 启动Nginx成功后，可以直接访问主机IP，此时会展示Nginx默认页面

# 停止Nginx
systemctl stop nginx

# 重启Nginx
systemctl restart nginx

# 重新加载Nginx，热重启
systemctl reload nginx

# 查看 Nginx 运行状态
systemctl status nginx

# 查看Nginx进程
ps -ef | grep nginx

# 杀死Nginx进程
kill -9 pid # 根据上面查看到的Nginx进程号，杀死Nginx进程，-9 表示强制结束进程
```

`Nginx` 应用程序命令：

```bash
nginx -s reload  # 向主进程发送信号，重新加载配置文件，热重启
nginx -s reopen	 # 重启 Nginx
nginx -s stop    # 快速关闭
nginx -s quit    # 等待工作进程处理完成后关闭
nginx -T         # 测试配置文件，并显示配置文件（这个命令可以快速查看配置文件）
nginx -t         # 检查（测试）配置是否有问题

nginx -v         # 查看版本
nginx -V         # 查看版本和nginx的配置选项
```

> **`Nginx`常见程序命令，可以通过 `nginx -h` 查看：**
> 
> ```sh
> # nginx -h
> nginx version: nginx/1.22.0
> Usage: nginx [-?hvVtTq] [-s signal] [-p prefix]
>              [-e filename] [-c filename] [-g directives]
> 
> Options:
>   -?,-h         : this help
>   -v            : show version and exit
>   -V            : show version and configure options then exit
>   -t            : test configuration and exit
>   -T            : test configuration, dump it and exit
>   -q            : suppress non-error messages during configuration testing
>   -s signal     : send signal to a master process: stop, quit, reopen, reload
>   -p prefix     : set prefix path (default: /usr/local/nginx/)
>   -e filename   : set error log file (default: logs/error.log)
>   -c filename   : set configuration file (default: conf/nginx.conf)
>   -g directives : set global directives out of configuration file
> ```

# Nginx 核心配置

## 配置文件结构

`Nginx` 的典型配置示例：

```conf
# main段配置信息
user  nginx;                        # 运行用户，默认即是nginx，可以不进行设置。可以指定 "用户 用户组"
worker_processes  auto;             # Nginx 进程数，一般设置为和 CPU 核数一样
worker_cpu_affinity auto;           # 将Nginx工作进程绑定到指定的CPU核心，默认Nginx是不进行进程绑定的

error_log  /var/log/nginx/error.log warn;   # Nginx 的错误日志存放目录
pid        /var/run/nginx.pid;      # Nginx 服务启动时的 pid 存放位置

# events段配置信息
events {
    use epoll;     # 使用epoll的I/O模型(如果你不知道Nginx该使用哪种轮询方法，会自动选择一个最适合你操作系统的)
    worker_connections 1024;   # 每个进程允许最大并发数
}

# http段配置信息
# 配置使用最频繁的部分，代理、缓存、日志定义等绝大多数功能和第三方模块的配置都在这里设置
http { 
    # 设置日志模式
    log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
                      '$status $body_bytes_sent "$http_referer" '
                      '"$http_user_agent" "$http_x_forwarded_for"';

    access_log  /var/log/nginx/access.log  main;   # Nginx访问日志存放位置

    sendfile            on;   # 开启高效传输模式
    tcp_nopush          on;   # 减少网络报文段的数量，内核会尽量把小数据包拼接成一个大的数据包（一个MTU）再发送出去。开启tcp_nopush，则必须要开启sendfile
    
    tcp_nodelay         on;   # 数据包立即发出，向内核递交的每个数据包都会立即发送出去。对于实时性要求较高的则应该开启

    # `tcp_nodelay、tcp_nopush`两个参数是“互斥”的，如果追求响应速度的应用推荐开启`tcp_nodelay`参数，如`IM`、金融等类型的项目。如果追求吞吐量的应用则建议开启`tcp_nopush`参数，如调度系统、报表系统等。
    
    keepalive_timeout   65;   # 保持连接的时间，也叫超时时间，单位秒
    types_hash_max_size 2048; # 默认值1024。nginx使用了一个散列表来保存MIME type与文件扩展名之间的映射。该参数指定了存储MIME type与文件扩展名的散列的最大大小，该值越大，散列的key就越稀疏，检索速度越快，但会占用更多的内存；该值越小，占用的内存越小，但是散列的冲突率就会上升，检索越慢。

    include             /etc/nginx/mime.types;      # 文件扩展名与类型映射表
    default_type        application/octet-stream;   # 默认文件类型（MIME类型）
    
    # server段配置信息
    server {
    	listen       80;       # 配置监听的端口
    	server_name  localhost;    # 配置的域名
      
    	# location段配置信息
    	location / {
    		root   /usr/share/nginx/html;  # 网站根目录
    		index  index.html index.htm;   # 默认首页文件
    		deny 172.168.22.11;   # 禁止访问的ip地址，可以为all
    		allow 172.168.33.44；# 允许访问的ip地址，可以为all
    	}
    	
    	error_page 500 502 503 504 /50x.html;  # 默认50x对应的访问页面
    	error_page 400 404 error.html;   # 同上
    }

    include /etc/nginx/conf.d/*.conf;   # 加载子配置项
}
```

- `main` 全局配置，对全局生效；
- `events` 配置影响 `Nginx` 服务器与用户的网络连接；
- `http` 配置代理，缓存，日志定义等绝大多数功能和第三方模块的配置；
- `server` **配置虚拟主机（Web网站、域名）的相关参数**，一个 `http` 块中可以有多个 `server` 块；
- `location` 用于配置匹配的 `uri`；
- `upstream` 配置后端服务器具体地址，负载均衡配置不可或缺的部分；

  
用一张图展示 Nginx 各个配置块的层级结构：

![](img/20230119163010.png)  

> **Nginx 的配置是分不同块（也叫做配置段）的，不同的块或段，有着不同的作用，不同的配置项也有各自适应的配置块范围。各个配置块综合起来，共同起作用。**

