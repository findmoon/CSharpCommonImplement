**Linux系统用户及权限管理详解，二进制数字权限，特殊权限和权限掩码【转】**

[toc]

> 原文 [Linux系统用户及权限管理详解](https://blog.csdn.net/qq_39565646/article/details/89345786)

# 用户

Linux操作系统对多用户的管理，是非常繁琐的，所以用组的概念来管理用户就变得简单，每个用户可以在一个独立的组，每个组也可以有零个用户或者多个用户。

Linux系统用户是根据用户ID来识别的，默认ID长度为32位，从默认ID编号从0开始，但是为了和老式系统兼容，用户ID限制在60000以下。

Linux用户分总共分为三种，分别如下：

- root用户  （ID 0）

- 系统用户  （ID 1-499）

- 普通用户  （ID 500以上）

**Linux系统中的每个文件或者文件夹，都有一个所属用户及所属组，使用id命令可以显示当前用户的信息，使用passwd命令可以修改当前用户密码。**

Linux操作系统用户的特点如下：

1. 每个用户拥有一个UserID，操作系统实际读取的是UID，而非用户名；

2. 每个用户属于一个主组，属于一个或多个附属组，一个用户最多有31个附属组；

3. 每个组拥有一个GroupID；

4. **每个进程以一个用户身份运行，该用户可对进程拥有资源控制权限**；

5. 每个可登陆用户拥有一个指定的Shell环境。

## Linux用户管理

Linux用户在操作系统中可以进行日常管理和维护，涉及到的相关配置文件如下：

- `/etc/passwd`     保存用户信息

- `/etc/shadow`     保存用户密码（以加密形式保存）

- `/etc/group`      保存组信息

- `/etc/login.defs`  用户属性限制,密码过期时间,密码最大长度等限制

- `/etc/default/useradd` 显示或更改默认的useradd配置文件

如需创建新用户，可以使用命令`useradd`，执行命令`useradd test1`即可创建test1用户，同时会创建一个同名的组test1，默认该用户属于test1主组。

`Useradd test1`命令默认创建用户test1，会根据如下步骤进行操作：

1. 在`/etc/passwd`文件中添加用户信息；

如使用passwd命令创建密码，密码会被加密保存在`/etc/shadow`中；

2. 为test1创建家目录：`/home/test1`；

 将`/etc/skel`中的`.bash`开头的文件复制至`/home/test1`家目录；

3. 创建与用户名相同的test1组，test1用户默认属于test1同名组；

test1组信息保存在`/etc/group`配置文件中。

## useradd 命令

在使用useradd命令创建用户时，可以支持如下参数：

用法：useradd [选项] 登录

useradd -D

useradd -D [选项]

选项：

-b, --base-dir BASE_DIR        指定新账户的家目录；

-c, --comment COMMENT          新账户的 GECOS 字段；

-d, --home-dir HOME_DIR        新账户的主目录；

-D, --defaults                 显示或更改默认的 useradd 配置；

-e, --expiredate EXPIRE_DATE   新账户的过期日期；

-f, --inactive INACTIVE        新账户的密码不活动期；

-g, --gid GROUP                新账户主组的名称或ID；

-G, --groups GROUPS            新账户的附加组列表；

-h, --help                     显示此帮助信息并推出；

-k, --skel SKEL_DIR            使用此目录作为骨架目录；

-K, --key KEY=VALUE            不使用 /etc/login.defs 中的默认值；

-l, --no-log-init         不要将此用户添加到最近登录和登录失败数据库；

-m, --create-home         创建用户的主目录；

-M, --no-create-home      不创建用户的主目录；

-N, --no-user-group       不创建同名的组；

-o, --non-unique               允许使用重复的 UID 创建用户；

-p, --password  PASSWORD        加密后的新账户密码；

-r, --system                   创建一个系统账户；

-R, --root CHROOT_DIR          chroot 到的目录；

-s, --shell SHELL              新账户的登录 shell；

-u, --uid UID                  新账户的用户 ID；

-U, --user-group               创建与用户同名的组；

-Z, --selinux-user SEUSER      为SELinux 用户映射使用指定 SEUSER。

# Linux组管理

所有的Linux或者Windows系统都有组的概念，通过组可以更加方便的管理用户，组的概念应用于各行行业，例如企业会使用部门、职能或地理区域的分类方式来管理成员，映射在Linux系统，同样可以创建用户，并用组的概念对其管理。

Linux组有如下特点：

1. 每个组有一个组ID；

2. 组信息保存在`/etc/group`中；

3. 每个用户至少拥有一个主组，同时还可以拥有31个附属组。

通过命令`groupadd`、`groupdel`、`groupmod`来对组进行管理，详细参数使用如下：

- **groupadd** 用法

-f, --force            如果组已经存在则成功退出；

并且如果 GID 已经存在则取消；

-g, --gid GID             为新组使用 GID；

-h, --help                显示此帮助信息并推出；

-K, --key KEY=VALUE       不使用 `/etc/login.defs` 中的默认值；

-o, --non-unique          允许创建有重复 GID 的组；

-p, --password PASSWORD   为新组使用此加密过的密码；

-r, --system              创建一个系统账户；

- **groupmod** 用法

-g, --gid GID             将组 ID 改为 GID；

-h, --help                显示此帮助信息并推出；

-n, --new-name NEW_GROUP  改名为 NEW_GROUP；

-o, --non-unique          允许使用重复的 GID；

-p, --password PASSWORD   将密码更改为(加密过的) PASSWORD；

- **groupdel** 用法

groupdel admin                 删除admin组；

# Linux权限管理

## 权限管理

Linux权限是操作系统用来限制对资源访问的机制，**权限一般分为读、写、执行**。

**系统中每个文件都拥有特定的权限、所属用户及所属组，通过这样的机制来限制哪些用户或用户组可以对特定文件进行相应的操作**。

**Linux每个进程都是以某个用户身份运行，进程的权限与该用户的权限一样，用户的权限越大，则进程拥有的权限就越大**。

Lnux中有的文件及文件夹都有至少一到三种权限，常见的权限如下所示:

|   **权限**     |    **对文件的影响**      |       **对目录的影响**    |
|   ----         |      ----------         |       ------------        |
|    r（读取）     |     可读取文件内容        |     可列出目录内容        |
|    w（写入）     |     可修改文件内容        |   可在目录中创建删除内容   |
|    x（执行）     |     可作为命令执行        |     可访问目录内容        |

**目录必须拥有 x 权限，否则无法查看其内容**


Linux权限授权，默认是授权给三种角色，分别是user、group、other。

Linux权限与用户之间的关联如下：

1. U代表User，G代表Group，O代表Other；

2. 每个文件的权限基于UGO进行设置；

3. 权限三位一组（rwx，即表示 **可读可写可执行**），同时需授权给三种角色：UGO；

4. 每个文件拥有一个所属用户和所属组，对应UG，不属于该文件所属用户或所属组使用O来表示；

在Linux系统中，可以通过`ls –l`查看peter.net目录的详细属性，如下所示：

```sh
drwxrwxr-x   2 peter1 peter1 4096 Dec 10 01:36 peter.net
```

`peter.net`目录属性参数详解如下：

- d 表示目录，同一位置如果为 `-` 则表示普通文件；

- rwxrwxr-x 表示三种角色的权限，每三位为一种角色，依次为u，g，o权限。如上则表示user的权限为rwx，group的权限为rwx，other的权限为r-x；

- **2 表示文件夹的链接数量，可理解为该目录下子目录/子文件的数量**；

- 从左到右，第一个peter1表示该用户名，第二个peter1则为组名，其他人角色默认不显示；

- 4096表示该文件夹占据的字节数；

- Dec 10 01:36 表示文件创建或者修改的时间；

- peter.net 为目录名，或者文件名。

## chmod 修改用户及组权限

**修改某个用户、组对文件的权限，用命令chmod实现，其中以 ugo 为代指。+、-、=代表加入、删除和等于对应权限**。

具体案例如下：

（1） 授予所属用户对peter.net目录拥有rwx权限

```sh
chmod  –R  u+rwx  peter.net
```

（2） 授予所属组对peter.net目录拥有rwx权限

```sh
chmod  –R  g+rwx  peter.net
```

（3） 授予用户、组、其他人对jpeter.net目录拥有rwx权限

```sh
chmod  –R  u+rwx,g+rwx,o+rwx  peter.net
```

（4） 撤销所属用户对peter.net目录拥有w权限

```sh
chmod  –R  u-w  peter.net
```

（5） 撤销用户、组、其他人对peter.net目录拥有x权限

```sh
chmod  –R  u-x,g-x,o-x peter.net
```

（6） 授予用户、组、其他人对peter.net目录只有rx权限

```sh
chmod  –R  u=rx,g=rx,o=rx  peter.net
```

## chmod二进制权限

Linux权限默认使用 rwx 来表示，为了更简化在系统中对权限进行配置和修改，Linux权限引入二进制表示方法。

**Linux权限可以将 rwx 用二进制来表示，其中有权限用1表示，没有权限用0表示**；

Linux权限用二进制显示如下：

rwx=111

r-x=101

rw-=110

r--=100

依次类推，转化为十进制，对应十进制结果显示如下：

rwx=111=4+2+1=7

r-x=101=4+0+1=5

rw-=110=4+4+0=6

r--=100=4+0+0=4

得出结论，**用 r=4,w=2,x=1 来表示权限**。

使用二进制方式来修改权限案例演示如下，其中默认 peter.nett 目录权限为755：

（1） 授予peter.net目录拥有的权限 所属用户rwx权限、所属组和其他r-x权限

```sh
chmod  –R  755 peter.net
```

（2） 授予peter.net目录拥有的权限 所属用户和所属组rwx权限、其他r-x权限

```sh
chmod  –R  775 peter.net
```

（3） 授予用户、组、其他人对peter.net目录拥有rwx权限

```sh
chmod  –R  777  peter.net
```

## Linux特殊权限及掩码

Linux权限除了常见的rwx权限之外，还有很多特殊的权限，细心的读者会发现，为什么 **Linux目录默认权限755，而文件默认权限为644** 呢，这是因为Linux权限掩码 umask 导致。

**每个Linux终端都拥有一个umask属性，umask属性可以用来确定新建文件、目录的默认权限**，默认系统权限掩码为022。

在系统中每创建一个文件或者目录，文件默认权限是666，而目录权限则为777，权限对外开放比较大，所以设置了权限掩码之后，默认的文件和目录权限减去umask值才是真实的文件和目录的权限。

- 对应目录权限为：777-022=755；

- 对应文件权限为：666-022=644；

**执行umask命令可以查看当前默认的掩码，`umask -S 023`可以设置默认的权限掩码**。

在Linux权限中，除了普通权限外，还有如下所示的 **Linux三个特殊权限**：

权限        |            对文件的影响                       |           对目录的影响
|   ----    |                   --------                  |            ----------      |
|   Suid    |   以文件的所属用户身份执行，而非执行文件的用户   |    无                      |
|   sgid    |   以文件所属组身份去执行                      |   在该目录中创建任意新文件的所属组与该目录的所属组相同
|   sticky  |   无                                        |   对目录拥有写入权限的用户仅可以删除其拥有的文件，无法删除其他用户所拥有的文件

**Linux中设置特殊权限方法如下**：

- 设置suid： `chmod u+s peter.net`

- 设置sgid： `chmod g+s peter.net`

- 设置sticky： `chmod o+t peter.net`

特殊权限与设置普通权限一样，可以使用数字方式表示：

 SUID    = 4

 SGID    = 2

 Sticky = 1

 可以通过 `chmod 4755 peter.net` 对该目录授予特殊权限为s的权限，Linux系统中s权限的应用常见包括：su、passwd、sudo。

