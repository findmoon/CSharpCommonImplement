**设置yum源多个repo仓库文件的优先级**

[toc]

实际配置`.repo`文件时，突然想到一个问题，即有多个yum源（多个.repo文件）时，如何执行rpm安装包所在仓库的优先级？如何保证优先使用官方的或更推荐的软件仓库？

# 包的策略

包的策略。一共有两个选项，newest和last，这个作用是如果你设置了多个repository，而同一软件在不同的repository中同时存在，yum应该安装哪一个，如果是newest，则yum会安装最新的那个版本。如果是last，则yum会将服务器id以字母表排序，并选择最后的那个 服务器上的软件安装。

一般都是选newest。

# 安装 yum-plugin-priorities 配置.repo优先级

yum install yum-plugin-priorities

 

确认配置文件内容

cat /etc/yum/pluginconf.d/priorities.conf

[main]

enabled = 1

安装完插件后，只需要在yum源配置文件*.repo里指定优先级即可，如：添加priority=n的配置项，n值越小优先级越高，默认99，

[local]
name=local yum
baseurl=file:///mnt
enabled=1
gpgcheck=0
priority=1


# 附：yum 安装网络源的包时可以考虑使用http替换https

通常网络仓库镜像源都是https的地址，在实际使用中可能会遇到网络问题，可以考虑替换为 http。

比如：

 1 [salt-latest]

 2 name=SaltStack Latest Release Channel for RHEL/Centos $releasever

 3 baseurl=**http**`://repo.saltstack.com/yum/redhat/6/$basearch/latest`

 4 failovermethod=priority

 5 enabled=1

 6 gpgcheck=1

 7 gpgkey=file:///etc/pki/rpm-gpg/saltstack-signing-key


# 参考

- [.repo文件共存与优先级，.repo文件的修改，https的repo改成http](https://blog.csdn.net/fantaxy025025/article/details/84918201)