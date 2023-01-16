centos查找nginx安装的路径以及相关安装操作命令

1、查看nginx安装目录

输入命令

# ps  -ef | grep nginx

返回结果包含安装目录

root     2662    1 0 07:12 ?       00:00:00 nginx: master process /usr/sbin/nginx


直接执行 nginx 命令，比如 `nginx -s relaod` 需要在nginx目录下或者指定nginx目录
