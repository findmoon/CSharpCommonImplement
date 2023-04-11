**CVM腾讯云服务器linux系统[内网] CentOS 8 挂载COS云对象存储的实现【cosfs、GooseFS-Lite、COS Migration】**

[toc]

COS 是否有内网域名？

对象存储（Cloud Object Storage，COS）的默认源站域名格式为：`<BucketName-APPID>.cos.<Region>.myqcloud.com`，默认已支持公网访问和同地域的内网访问。参见 [地域和访问域名](https://cloud.tencent.com/document/product/436/6224)。

同地域范围内的访问将会自动被指向到内网地址，即自动使用内网连接，在内网环境下通过存储桶的域名访问 COS 时，COS 会智能解析到内网 IP 上。内网流量不计费。


# 云服务器 判断 内网访问对象存储 方法

1. 获取存储桶访问域名，并记录该地址
2. 登录实例，并执行 nslookup 命令。假设 `examplebucket-1250000000.cos.ap-guangzhou.myqcloud.com` 为目标存储桶地址，则执行以下命令：

```sh
nslookup examplebucket-1250000000.cos.ap-guangzhou.myqcloud.com
```

返回如下图，其中`10.148.214.13`和`10.148.214.14`这两个 IP 就代表了是通过内网访问 COS。

![](img/20230403184905.png)

> **内网 IP 地址一般形如 `10.*.*.*`、`100.*.*.*` ，VPC 网络一般为`169.254.*.*` 等，这两种形式的 IP 都属于内网。**

> 虚拟私有云VPC（Virtual Private Cloud）
> 
> 虚拟专属私有网络（Virtual Private Cloud，简称 VPC 网络）是为用户在云上构建的私有网络空间，为用户创建的云资源提供隔离的虚拟网络环境。

具体参见 [云服务器通过内网访问对象存储](https://cloud.tencent.com/document/product/213/57441)

# CVM 挂载 COS 【不推荐】

访问 [CVM 挂载 COS](https://console.cloud.tencent.com/cos/bucket?bucket=pan-storage-bucket-1256319690&region=ap-beijing&type=csg) 

![](img/20230403171339.png)

这种方式是通过创建文件系统的形式实现CVM挂载COS，需要创建网关【费用不低】


# 腾讯云cosfs工具将COS的存储桶挂载到CentOS 8

## COSFS 工具 介绍

COSFS 工具支持将对象存储（Cloud Object Storage，COS）存储桶挂载到本地，像使用本地文件系统一样直接操作腾讯云对象存储中的对象， COSFS 提供的主要功能包括：

- 支持 POSIX 文件系统的大部分功能，如：文件读写、目录操作、链接操作、权限管理、uid/gid 管理等功能。
- 大文件分块传输功能。
- MD5 数据校验功能。
- 将本机数据上传至 COS，建议使用 [COS Migration 工具](https://cloud.tencent.com/document/product/436/15392) 或 [COSCMD 工具](https://cloud.tencent.com/document/product/436/10976)。

COSFS 并未提供 CentOS 8 的版本，因此，只能最好通过下载源码，通过编译安装的形式安装，应该是兼容的。目前最新版本 cosfs-1.0.20-centos7.0.x86_64.rpm

## 安装COSFS工具

- 获取源码

```sh
git clone https://github.com/tencentyun/cosfs /usr/cosfs
```

- 安装依赖软件

```sh
sudo yum install automake gcc-c++ git libcurl-devel libxml2-devel fuse-devel make openssl-devel fuse
```

- 编译和安装 COSFS

```sh
cd /usr/cosfs
./autogen.sh
./configure
make
sudo make install
cosfs --version  #查看 cosfs 版本号
```

如下：

```sh
# cosfs --version
Tencentyun Object Storage Service File System V1.0.20(commit:774e9ae) with OpenSSL
License GPL2: GNU GPL version 2 <http://gnu.org/licenses/gpl.html>
This is free software: you are free to change and redistribute it.
There is NO WARRANTY, to the extent permitted by law.
```

## 获取存储桶相关的挂载信息

登录腾讯云-控制台-对象存储-存储桶获取相关信息

- BucketName-APPID : test-1250000000 即存储桶名称（直接从存储桶列表中获取`存储桶名称`即可）
- SecretId : XXXXXX
- SecretKey : XXXXXX
- 区域地址 : `https://cos.ap-guangzhou.myqcloud.com` 存储桶所在的区域[域名](https://cloud.tencent.com/act/pro/domain-sales?from=20065&from_column=20065).

背景：https://cos.ap-beijing.myqcloud.com  可以通过存储桶信息的域名信息中获取。

## 配置密钥文件

在文件 `/etc/passwd-cosfs` 中，写入存储桶名称（格式为 BucketName-APPID），以及该存储桶对应的 `<SecretId>` 和 `<SecretKey>`，使用半角冒号隔开。为了防止密钥泄露，COSFS 要求您将密钥文件的权限值设置为640。

```sh
sudo su  # 切换到 root 身份，以修改 /etc/passwd-cosfs 文件；如果已经为 root 用户，无需执行该条命令。
echo <BucketName-APPID>:<SecretId>:<SecretKey> > /etc/passwd-cosfs
chmod 640 /etc/passwd-cosfs
```

示例：

```sh
echo examplebucket-1250000000:AKIDHTVVaVR6e3****:PdkhT9e2rZCfy6**** > /etc/passwd-cosfs
chmod 640 /etc/passwd-cosfs
```

## 挂载COS

将密钥文件中配置的存储桶挂载到指定目录：

```sh
cosfs <BucketName-APPID> <MountPoint> -ourl=http://cos.<Region>.myqcloud.com -odbglevel=info -oallow_other -onoxattr
```

- `<MountPoint>` 为本地挂载目录（例如`/mnt`）。
- `<Region>` 为地域简称， 例如 ap-guangzhou 、 eu-frankfurt 等。更多地域简称信息，请参见 [可用地域](https://cloud.tencent.com/document/product/436/6224)。
- `-odbglevel` 指定日志级别，默认为crit，可选值为crit、error、warn、info、debug。
- `-oallow_other` 允许非挂载用户访问挂载文件夹。
- `-onoxattr` 禁用 getattr/setxattr 功能，在1.0.9之前版本的 COSFS 不支持设置和获取扩展属性，如果在挂载时使用了 use_xattr 选项，可能会导致 mv 文件到 Bucket 失败。

> 通过 `-opasswd_file=[path]` 可以指定密钥文件的路径，这样可以将密钥保存在自定义的位置，同时密钥文件权限设置为 600。

创建想要挂载的路径：

```sh
mkdir -p /mnt/cosfs
```

挂载：

```sh
cosfs examplebucket-1250000000 /mnt/cosfs -ourl=http://cos.ap-beijing.myqcloud.com -onoxattr -oallow_other -odbglevel=error
```

也可以挂载子目录：

```sh
cosfs examplebucket-1250000000:/my-dir /mnt/cosfs -ourl=http://cos.ap-beijing.myqcloud.com -onoxattr -oallow_other -odbglevel=error
```

## 卸载COS

```sh
umount -l /mnt/cosfs
```

或 

```sh
fusermount -u /mnt
```

> fusermount 命令专用于卸载 FUSE 文件系统

或

```sh
umount /mnt
```

如果有程序引用文件系统中的文件时，`umount /mnt`进行卸载会报错。

## 日志文件

在 CentOS 中，COSFS 产生的日志存储在 `/var/log/messages` 中；

在 Ubuntu 中，COSFS 日志存储在 `/var/log/syslog` 中

## 设定 COSFS 开机自动挂载

- 先安装 fuse 包

```sh
sudo yum install -y fuse
```

- 在 `/etc/fstab` 文件中添加如下的内容，其中，`_netdev` 选项使得网络准备好后再执行当前命令：

```sh
cosfs#examplebucket-1250000000 /mnt/cosfs fuse _netdev,allow_other,url=http://cos.ap-beijing.myqcloud.com,dbglevel=error,onoxattr
```

## 设置挂载点下的文件以及目录的用户和用户组？

有些场景（例如 nginx 服务器），需要设置挂载点下的文件和目录的用户和用户组，例如 www 用户（uid=1002，gid=1002），则添加如下挂载参数：

```sh
-ouid=1002 -ogid=1002
```

# GooseFS-Lite 工具 挂载COS存储桶

COSFS 基于 S3FS 构建，读取和写入操作都经过磁盘中转。

相比于 COSFS，更建议您使用 GooseFS-Lite工具访问 COS，GooseFS-Lite 是一个轻量级单机 COS Fuse 工具，具有更好的读写性能和稳定性。大文件的读写速度更高，提升性能。

GooseFS-Lite 支持 POSIX 文件系统的主要功能，例如文件顺序/随机读，顺序写、目录操作等功能。

> GooseFS-Lite 不支持对文件进行随机写和 truncate 操作。


# COS Migration 工具

> 将本机数据上传至 COS，建议使用 [COS Migration 工具](https://cloud.tencent.com/document/product/436/15392) 或 [COSCMD 工具](https://cloud.tencent.com/document/product/436/10976)。

COS Migration 是一个集成了 COS 数据迁移功能的一体化工具，拥有 断点续传、分块上传、并行上传、迁移校验 等功能。支持 Windows、Linux 和 macOS 系统。

## 要求

- JDK 1.8 X64或以上，有关 JDK 的安装与配置请参见 [Java 安装与配置](https://cloud.tencent.com/document/product/436/10865)。
- Linux 环境需要 IFUNC 支持，确保环境 binutils 版本大于 2.20 。

### `java -version` java版本

### 检查 binutils 版本

```sh
ld --version
```

或

```sh
ar -v
```

```sh
# ld -v
GNU ld version 2.30-108.el8_5.1
# ar --version
GNU ar version 2.30-108.el8_5.1
```

## 安装 COS Migration 工具

- 下载 [COS Migration 工具](https://github.com/tencentyun/cos_migrate_tool_v5)

```sh
wget https://github.com/tencentyun/cos_migrate_tool_v5/archive/refs/heads/master.zip
```

- 解压缩

```sh
unzip master.zip -d cos_migrate_tool_v5 && cd cos_migrate_tool_v5
```

- 结构

解压后的 COS Migration 工具目录结构如下所示：

```sh
COS_Migrate_tool
|——conf  #配置文件所在目录
|   |——config.ini  #迁移配置文件
|——db    #存储迁移成功的记录
|——dep   #程序主逻辑编译生成的JAR包
|——log   #工具执行中生成的日志
|——opbin #用于编译的脚本
|——src   #工具的源码
|——tmp   #临时文件存储目录
|——pom.xml #项目配置文件
|——README  #说明文档
|——start_migrate.sh  #Linux 下迁移启动脚本
|——start_migrate.bat #Windows 下迁移启动脚本
```

> - db 目录主要记录工具迁移成功的文件标识，每次迁移任务会优先对比 db 中的记录，若当前文件标识已被记录，则会跳过当前文件，否则进行文件迁移。
> 
> - log 目录记录着工具迁移时的所有日志，若在迁移过程中出现错误，请先查看该目录下的 error.log。

## 修改 config.ini 配置文件

在执行迁移启动脚本之前，需先进行 config.ini 配置文件修改（路径：`./conf/config.ini`），config.ini 内容可以分为以下几部分：


### 迁移类型

type 表示迁移类型，用户根据迁移需求填写对应的标识。例如，**需要将本地数据迁移至 COS，则`[migrateType]`的配置内容是`type=migrateLocal`**。

```ini
[migrateType]
type=migrateLocal
```

### 迁移任务

用户根据实际的迁移需求进行相关配置，主要包括迁移至目标 COS 信息配置及迁移任务相关配置。

```sh
# 迁移工具的公共配置分节，包含了需要迁移到目标 COS 的账户信息。
[common]
secretId=COS_SECRETID
secretKey=COS_SECRETKEY
bucketName=examplebucket-1250000000
region=ap-guangzhou
storageClass=Standard
cosPath=/
https=off
tmpFolder=./tmp
smallFileThreshold=5242880
smallFileExecutorNum=64
bigFileExecutorNum=8
entireFileMd5Attached=on
daemonMode=off
daemonModeInterVal=60
executeTimeWindow=00:00,24:00
outputFinishedFileFolder=./result
resume=false
skipSamePath=false
```

| 名称 | 描述 | 默认值 |
| --- | --- | --- |
| secretId | 用户密钥 SecretId，请将`COS_SECRETID`替换为您的真实密钥信息。可前往 [访问管理控制台](https://console.cloud.tencent.com/cam/capi) 中的云 API 密钥页面查看获取 | \- |
| secretKey | 用户密钥 SecretKey，请将`COS_SECRETKEY`替换为您的真实密钥信息。可前往 [访问管理控制台](https://console.cloud.tencent.com/cam/capi) 中的云 API 密钥页面查看获取 | \- |
| bucketName | 目的 Bucket 的名称, 命名格式为 `<BucketName-APPID>`，即 Bucket 名必须包含 APPID，例如 examplebucket-1250000000 | \- |
| region | 目的 Bucket 的 Region 信息。COS 的地域简称请参照 [地域和访问域名](https://cloud.tencent.com/document/product/436/6224) | \- |
| storageClass | 数据迁移后的存储类型，可选值为 Standard（标准存储）、Standard\_IA（低频存储）、Archive（归档存储）、Maz\_Standard（标准存储多 AZ）、Maz\_Standard\_IA（低频存储多 AZ），相关介绍请参见 [存储类型概述](https://cloud.tencent.com/document/product/436/33417) | Standard |
| cosPath | 要迁移到的 COS 路径。`/`表示迁移到 Bucket 的根路径下，`/folder/doc/` 表示要迁移到 Bucket的`/folder/doc/` 下，若 `/folder/doc/` 不存在，则会自动创建路径 | / |
| https | 是否使用 HTTPS 传输：on 表示开启，off 表示关闭。开启传输速度较慢，适用于对传输安全要求高的场景 | off |
| tmpFolder | 从其他云存储迁移至 COS 的过程中，用于存储临时文件的目录，迁移完成后会删除。要求格式为绝对路径：<br> Linux 下分隔符为单斜杠，例如`/a/b/c`  <br> Windows 下分隔符为两个反斜杠，例如`E:\\a\\b\\c`  <br> 默认为工具所在路径下的 tmp 目录 | ./tmp |
| smallFileThreshold | 小文件阈值的字节，大于等于这个阈值使用分块上传，否则使用简单上传，默认5MB | 5242880 |
| smallFileExecutorNum | 小文件（文件小于 smallFileThreshold）的并发度，使用简单上传。如果是通过外网来连接 COS，且带宽较小，请减小该并发度 | 64 |
| bigFileExecutorNum | 大文件（文件大于等于 smallFileThreshold）的并发度，使用分块上传。如果是通过外网来连接 COS，且带宽较小，请减小该并发度 | 8 |
| entireFileMd5Attached | 表示迁移工具将全文的 MD5 计算后，存入文件的自定义头部 x-cos-meta-md5 中，用于后续的校验，因为 COS 的分块上传的大文件的 etag 不是全文的 MD5 | on |
| daemonMode | 是否启用 daemon 模式：on 表示开启，off 表示关闭。daemon 表示程序会循环不停的去执行同步，每一轮同步的间隔由 daemonModeInterVal 参数设置 | off |
| daemonModeInterVal | 表示每一轮同步结束后，多久进行下一轮同步，单位为秒 | 60 |
| executeTimeWindow | 执行时间窗口，时刻粒度为分钟，该参数定义迁移工具每天执行的时间段。例如： <br> 参数 03:30,21:00，表示在凌晨 03:30 到晚上 21:00 之间执行任务，其他时间则会进入休眠状态，休眠态暂停迁移并会保留迁移进度, 直到下一个时间窗口自动继续执行。注意后面的时间点必须大于前面的时间点。 | 00:00,24:00 |
| outputFinishedFileFolder | 这个目录保存迁移成功的结果，结果文件会按照日期命名，例如`./result/2021-05-27.out`，其中./result 为已创建的目录。文件内容每一行的格式为：绝对路径\\t文件大小\\t最后修改时间。设置为空，则不输出结果。 | ./result |
| resume | 是否接着最后一次运行的结果，继续往下遍历源的文件列表。默认从头开始。 | false |
| skipSamePath | 如果 COS 上已经有相同的文件名，是否直接跳过。默认不跳过，即覆盖原有文件。 | false |
| requestTryCount | 每个文件上传总的尝试次数。 | 5 |


### 数据源信息

根据`[migrateType]`的迁移类型配置相应的分节。

例如`[migrateType]`的配置内容是`type=migrateLocal`, 则用户只需配置`[migrateLocal]`分节即可。


若从本地迁移至 COS，则进行 本地数据源 `[migrateLocal]` 这部分的配置。

```sh
# 从本地迁移到 COS 配置分节
[migrateLocal]
localPath=E:\\code\\java\\workspace\\cos_migrate_tool\\test_data
excludes=
ignoreModifiedTimeLessThanSeconds=
```

| 配置项 | 描述 |
| --- | --- |
| localPath | 本地目录，要求格式为绝对路径：<br> - Linux 下分隔符为单斜杠，例如`/a/b/c` <br> - Windows 下分隔符为两个反斜杠，例如`E:\\a\\b\\c` <br> 注意：此参数只能填目录的路径，不能填具体文件的路径，否则会导致目标对象名解析错误，在 cosPath=/ 情况下，还会错误地解析成创桶请求 |
| excludes | 要排除的目录或者文件的绝对路径，表示将 localPath 下面某些目录或者文件不进行迁移，多个绝对路径之前用分号分割，不填表示 localPath 下面的全部迁移 |
| ignoreModifiedTimeLessThanSeconds | 排除更新时间与当前时间相差不足一定时间段的文件，单位为秒，默认不设置，表示不根据 lastmodified 时间进行筛选，适用于客户在更新文件的同时又在运行迁移工具，并要求不把正在更新的文件迁移上传到 COS，例如设置为300，表示只上传更新了5分钟以上的文件 |

## 运行迁移工具

通过运行 `start_migrate.sh` 实现迁移。支持两种形式的配置，一是默认从配置文件读取配置并迁移；二是可以接受从命令行传入的部分参数，结合配置文件实现迁移。

- 从 config.ini 配置文件读入配置，运行命令为：

```sh
sh start_migrate.sh
```

- 部分参数从命令行读入配置，运行命令为：

```sh
sh start_migrate.sh -Dcommon.cosPath=/savepoint0403_10/
```

# 参考

- [COSFS 工具](https://cloud.tencent.com/document/product/436/6883)

- [CentOS 7 挂载腾讯云COS对象存储教程](https://www.ioiox.com/archives/65.html)

- [COSFS 工具常见问题](https://cloud.tencent.com/document/product/436/30743)

- [GooseFS-Lite 工具](https://cloud.tencent.com/document/product/1424/73687)

- [Check binutils version](https://iq.opengenus.org/check-binutils-version/)