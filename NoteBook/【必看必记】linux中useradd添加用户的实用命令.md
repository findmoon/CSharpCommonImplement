**linux中useradd添加用户的实用命令【必看必记】**

[toc]

`useradd` 命令用于添加用户，本篇主要记录一些比较实用的添加用户的命令。

> 不同的 Unix-like 系统，可能命令会为 `adduser`

`useradd` 的语法如下：

```sh
useradd [options] username
```

# 创建一个新用户

```sh
useradd user1
```

# 为用户指定密码

```sh
passwd user1
```

# 查看用户文件

`/etc/passwd` 文件用于存储用户信息；`/etc/shadow` 文件用于存储密码信息。

查看用户文件：

```sh
# cat /etc/passwd
user1:x:1003:1003::/home/user1:/bin/bash
```

每条用户信息包含一组七个以冒号分隔的字段，每个字段都有自己的含义：

- `Username`：用于登录系统的用户登录名。它的长度应该在 1 到 32 个字符之间。
- `Password`: 用户密码（或 x 字符）存储在`/etc/shadow`加密格式的文件。
- `User ID (UID)`: 每个用户必须有一个用户`ID`(`UID`) 用户识别号。默认情况下，`UID 0`为 root 用户和 UID 保留，范围从`1-99`保留用于其他预定义帐户。进一步的 UID 范围从`100-999`为系统帐户和组保留。
- `Group ID (GID)`: 主要组 ID (`GID`) ，组标识号存储在`/etc/group`文件。
- `User Info`：此字段是可选的，允许您定义有关用户的额外信息。例如，用户全名。该字段由 “finger” 命令填充。
- `Home Directory`: 用户家目录的绝对位置。
- `Shell`：用户`shell`的绝对位置，即`/bin/bash`

# `-d` 指定家目录 或 登入目录

```sh
useradd -d /data/testhome user2
```

通常 `-m -d` 会一起使用：

```sh
useradd -m -d /data/testhome user2
```

# 查看指定用户的信息

```sh
cat /etc/passwd | grep user2
```

# 创建指定用户ID的用户

Linux中，每个用户都有自己的 `UID`(`Unique Identification Number`)。`-u`选项可以在创建用户时指定id值。

```sh
useradd -u 1012 user3
```

# id username 查看用户id信息

```sh
# id user1
uid=1003(user1) gid=1003(user1) groups=1003(user1)
```

# `-g`创建指定组的用户

创建用户时，`-g`选项用于指定组。

```sh
useradd -g user1 user4

# 或 gid
useradd -g 1003 user4
```

# id -gn username 查看用户的组

```sh
# id -gn user4
user1
```

# `-G`将用户添加到多个附属组

`-G` 选项可以将用户添加到其他组，每个组名用逗号分隔，中间不能有空格。

```sh
# groupadd group1
# groupadd group2
# groupadd developers
```

```sh
useradd -G group1,group2,developers user5
```

# usermod -a -G 修改用户添加附属组

```sh
usermod -a -G group1,group2,developers user1
```

id 查看添加的组信息：

```sh
# id user1
uid=1003(user1) gid=1003(user1) groups=1003(user1),1005(group1),1006(group2),1007(developers)
```

# `-M`添加没有家目录的用户

`-M`选项指定创建用户时，不创建家目录。【不自动建立用户的登入目录】

```sh
useradd -M user6
```

因此，家目录不存在：

```sh
# ls -l /home/user6
ls: cannot access '/home/user6': No such file or directory
```

但是用户文件中，用户信息仍然会显示家目录路径：

```sh
# cat /etc/passwd | grep user6
user6:x:1007:1009::/home/user6:/bin/bash
```

由于没有家目录，登陆时会提示，并切换到根目录下。

```sh
Could not chdir to home directory /home/user6: No such file or directory
[user6@localhost /]$ pwd
/
```

# `-e`创建一个有账户到期日的用户

`useradd` 默认创建永不过期的账户。过期日期为`0`。

`-e` 可以指定过期的日期：

```sh
useradd -e 2023-12-27 user7
```

# chage 命令 查看账户更改和过期日期

`chage -l username`：

```sh
# chage -l user7
Last password change                                    : Apr 12, 2023
Password expires                                        : never
Password inactive                                       : never
Account expires                                         : Dec 27, 2023
Minimum number of days between password change          : 0
Maximum number of days between password change          : 99999
Number of days of warning before password expires       : 7
```

# -f 创建密码过期后指定天数不可用的用户【需要 设置密码过期 才生效】

`-f` 指定密码过期的 **天数**。默认情况下，密码过期值为`-1`，意味着永不过期。

创建 `-e 2023-12-1` 账户过期，`-f 50` 密码过期50天后不可用 的 用户：

```sh
useradd -e 2023-12-1 -f 50 user8
```

`-f` 选项的解释如下：

-f, --inactive INACTIVE       set password inactive after expiration to INACTIVE


如下，未设置密码过期时，查看 `chage -l` 密码过期和不可用(`inactive`) 均为 never：

```sh
# chage -l user8
Last password change                                    : Apr 12, 2023
Password expires                                        : never
Password inactive                                       : never
Account expires                                         : Dec 01, 2023
Minimum number of days between password change          : 0
Maximum number of days between password change          : 99999
Number of days of warning before password expires       : 7

```

# `chage -M` 指定密码过期天数

`chage -M days username` 可以实现用户密码过期的天数。

修改用户 user8 50天后密码过期：

```sh
chage -M 50 user8
```

此时，`-f`指定的inactive才会生效。

`chage -l` 查看密码过期和不可用(`inactive`)：

```sh
# chage -l user8
Last password change                                    : Apr 12, 2023
Password expires                                        : Jun 01, 2023
Password inactive                                       : Jul 21, 2023
Account expires                                         : Dec 01, 2023
Minimum number of days between password change          : 0
Maximum number of days between password change          : 50
Number of days of warning before password expires       : 7
```

- 直接修改 `/etc/shadow` 文件，也可以修改密码过期。

```sh
# cat /etc/shadow | grep user8
user8:!!:19459:0:50:7:50:19692:
```

`:` 分割的倒数第三个就是密码过期天数。修改后保存文件即可。 **99999 表示永不过期**。


# `-c`添加自定义注释的用户

`-c` 允许添加自定义注释，常用来表示`full name`、`phone number`等信息。

```sh
useradd -c "我是注释评论，可以添加用户全名、手机号等" user9
```

查看用户信息：

```sh
# tail -1 /etc/passwd
user9:x:1010:1012:我是注释评论，可以添加用户全名、手机号等:/home/user9:/bin/bash

```

# `-s /sbin/nologin` 创建无法登陆的用户

`-s` 指定用户登陆时的 shell。`/sbin/nologin` 表示没有登陆shell，禁止用户登陆。

```sh
useradd -s /sbin/nologin user10
```

也可以指定其他无效的 shell：

```sh
useradd -s /etc user10
```

# `-r` 或 `--system` 创建系统用户

# `-N` / `--no-user-group` 创建没有组的用户

# 拒绝用户登录，可以将其 shell 设置为 /usr/sbin/nologin 或者 /bin/false

```sh
usermod -s | --shell /usr/sbin/nologin username

# 或

usermod -s | -shell /bin/false username
```

- `/bin/false`：什么也不做只是返回一个错误状态，然后立即退出。将用户的 shell 设置为 /bin/false，用户会无法登录，并且不会有任何提示。

- `/usr/sbin/nologin`：nologin 会礼貌的向用户显示一条信息，并拒绝用户登录：This account is currently not available.

有一些软件，**比如一些 ftp 服务器软件，对于本地非虚拟账户，只有用户有有效的 shell 才能使用 ftp 服务。这时候就可以使用 nologin 使用户即不能登录系统，还能使用一些系统服务**，比如 ftp 服务。`/bin/false` 则不行，这是二者的重要区别之一。

> 如果存在 `/etc/nologin` 文件，则系统只允许 root 用户登录，其他用户全部被拒绝登录，并向他们显示 `/etc/nologin` 文件的内容。

