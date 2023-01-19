

- [选购机械硬盘的大坑，不看你就上当，详解SMR瓦楞式堆叠硬盘](https://www.bilibili.com/video/BV1rE411Q71m/?spm_id_from=333.337.search-card.all.click)
- [给大家科普下什么是叠瓦盘？顺便告诉你叠瓦盘为什么不能买？](https://zhuanlan.zhihu.com/p/378639081)

    
- [安装包制作工具 SetupFactory使用1 详解](https://www.cnblogs.com/SavionZhang/p/4106338.html)
- [Setup Factory](https://www.xitongtiandi.net/soft_wl/28729.html) 安装包制作
- [关于Setup Factory 9的一些使用方法](https://www.cnblogs.com/yply/p/9940017.html)


- [.NET软件开发与常用工具清单](https://www.cnblogs.com/SavionZhang/p/4033288.html)


- [惊 腾讯云、阿里云服务器无需备案配置域名访问方法](https://blog.csdn.net/qq_31628559/article/details/124496869)
好像没啥大用处

- [国内服务器免备案访问教程](https://blog.csdn.net/lmp5023/article/details/114078269)

- [【内网穿透服务器】公网环境访问内网服务器（以使用samba(smb)文件共享服务为例）](https://blog.csdn.net/deng_xj/article/details/88971573)
- [基于samba的远程目录共享服务搭建简易指南](https://blog.csdn.net/lczdk/article/details/114639097)


- 记录 nginx 配置



./configure --user=www --group=www --prefix=/www/server/nginx --add-module=/www/server/nginx/src/ngx_devel_kit --add-module=/www/server/nginx/src/lua_nginx_module --add-module=/www/server/nginx/src/ngx_cache_purge --add-module=/www/server/nginx/src/nginx-sticky-module --with-openssl=/www/server/nginx/src/openssl --with-pcre=pcre-8.43 --with-http_v2_module --with-stream --with-stream_ssl_module --with-stream_ssl_preread_module --with-http_stub_status_module --with-http_ssl_module --with-http_image_filter_module --with-http_gzip_static_module --with-http_gunzip_module --with-ipv6 --with-http_sub_module --with-http_flv_module --with-http_addition_module --with-http_realip_module --with-http_mp4_module --with-ld-opt=-Wl,-E --with-cc-opt=-Wno-error --with-ld-opt=-ljemalloc --with-http_dav_module --add-module=/www/server/nginx/src/nginx-dav-ext-module --add-module=/www/server/nginx/src/ngx_http_auth_basic_module

git clone --recursive https://github.com/arut/nginx-dav-ext-module

configure arguments: --user=www --group=www --prefix=/usr/local/nginx --with-http_stub_status_module --with-http_ssl_module --with-http_v2_module --with-http_gzip_static_module --with-http_sub_module --with-stream --with-stream_ssl_module --with-stream_ssl_preread_module --with-openssl=/root/lnmp1.9/src/openssl-1.1.1o --with-openssl-opt='enable-weak-ssl-ciphers' --with-ld-opt='-ljemalloc'


--user=www --group=www --prefix=/www/server/nginx --add-module=/www/server/nginx/src/ngx_devel_kit --add-module=/www/server/nginx/src/lua_nginx_module --add-module=/www/server/nginx/src/ngx_cache_purge --add-module=/www/server/nginx/src/nginx-sticky-module --with-openssl=/www/server/nginx/src/openssl --with-pcre=pcre-8.43 --with-http_v2_module --with-stream --with-stream_ssl_module --with-stream_ssl_preread_module --with-http_stub_status_module --with-http_ssl_module --with-http_image_filter_module --with-http_gzip_static_module --with-http_gunzip_module --with-ipv6 --with-http_sub_module --with-http_flv_module --with-http_addition_module --with-http_realip_module --with-http_mp4_module --with-ld-opt=-Wl,-E --with-cc-opt=-Wno-error --with-ld-opt=-ljemalloc --with-http_dav_module --add-module=/www/server/nginx/src/nginx-dav-ext-module



------------------

/usr/bin/nginx -V
nginx version: nginx/1.22.0
built by gcc 8.5.0 20210514 (Red Hat 8.5.0-4) (GCC)
built with OpenSSL 1.1.1o  3 May 2022
TLS SNI support enabled
configure arguments: --user=www --group=www --prefix=/usr/local/nginx --with-http_stub_status_module --with-http_ssl_module --with-http_v2_module --with-http_gzip_static_module --with-http_sub_module --with-stream --with-stream_ssl_module --with-stream_ssl_preread_module --with-openssl=/root/lnmp1.9/src/openssl-1.1.1o --with-openssl-opt='enable-weak-ssl-ciphers' --with-ld-opt='-ljemalloc' --with-http_dav_module --add-module=/usr/local/src/nginx-module/nginx-dav-ext-module
