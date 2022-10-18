**Oracle的一些相关操作记录**

[toc]

# 基础概念

一般Oracle数据库（Oracle Database）可以分为两部分，即实例（Instance）和数据库（Database）。
实例：是一个非固定的、基于内存的基本进程与内存结构。当服务器关闭后，实例也就不存在了。

数据库（Database）指的是固定的、基于磁盘的数据文件、控制文件、日志文件、参数文件和归档日志文件等。

一般情况下，Oracle数据库都是一个数据库对应一个实例。



# system、sys用户

> 一个数据库实例对应不同的(独立的)system、sys用户。因此，在创建数据库实例时，每次都需要指定system、sys的口令。

system是数据库内置的一个普通管理员，你手工创建的任何用户在被授予dba角色后都跟这个用户差不多。

sys用数据库的超级用户，数据库内很多重要的东西（数据字典表、内置包、静态数据字典视图等）都属于这个用户，sys用户必须以sysdba身份登录。

> 所有oracle的数据字典的基表和视图都存放在sys用户中，这些基表和视图对于oracle的运行是至关重要的，由数据库自己维护，任何用户都不能手动更改。
> 
> sys用户拥有dba，sysdba，sysoper等角色或权限，是oracle权限最高的用户。

> system用户用于存放次一级的内部数据，如oracle的一些特性或工具的管理信息。system用户拥有普通dba角色权限。

system用户以sysdba身份登录时就是sys，准确地说，任何用户以sysdba身份登录时都是sys（可以登陆后执行show user验证。）

`as sysdba` 就是以`sysdba`登录。

oracle登录身份有三种：

- normal 普通身份
- sysdba 系统管理员身份
- sysoper 系统操作员身份

每种身份对应不同的权限：

sysdba权限：

● 启动和关闭操作
● 更改数据库状态为打开/装载/备份，更改字符集
● 创建数据库
● 创建服务器参数文件spfile
● 日志归档和恢复
● 包含了“会话权限”权限

sysoper权限：

● 启动和关闭操作
● 更改数据库状态为打开/装载/备份
● 创建服务器参数文件spfile
● 日志归档和恢复
● 包含了“会话权限”权限
 
# 用户管理

## 创建用户

```sql
create user user_name identified by password;
```

## 修改用户密码

```sql
alter user user_name identified by password;
```

## 删除用户

```sql
drop user user_name;
```

若用户拥有对象，则不能直接删除，否则将返回一个错误值。指定关键字`cascade`,可删除用户所有的对象，然后再删除用户。

```sql
drop user user_name cascade;
```

## 查看用户是否被锁定

```sql
select username, account_status, lock_date from dba_users;
```

## 锁定和解锁用户

```sql
alter user user_name account lock;


alter user user_name account unlock;
```

> `system`用户就有可能经常是被锁定的

# 角色及授权

oracle为兼容以前版本，提供三种标准角色（role）：connect、resource和dba.

## connect role(连接角色)

临时用户，特指不需要建表的用户，通常只赋予他们connect role。

connect使用oracle简单权限，这种权限只对其他用户的表有访问权限，包括select/insert/update和delete等。

拥有 connect role 的用户还能够创建表、视图、序列（sequence）、簇（cluster）、同义词(synonym)、回话（session）和其他数据的链接（link）

## resource role(资源角色)

更可靠和正式的数据库用户可以授予resource role。

resource提供给用户另外的权限以创建他们自己的表、序列、过程(procedure)、触发器(trigger)、索引(index)和簇(cluster)。

##  dba role(数据库管理员角色)

dba role拥有所有的系统权限。包括无限制的空间限额和给其他用户授予各种权限的能力。

## 授权和撤销权限

```cs
grant connect, resource to user_name;
```

```sql
revoke connect, resource from user_name;
```

## 自定义角色

除了三种系统角色----connect、resource和dba，用户还可以在oracle创建自己的role。

用户创建的role可以由表或系统权限或两者的组合构成。为了创建role，用户必须具有create role系统权限。

1. 创建角色

语法： create role 角色名;

例子： create role testRole;

2. 授权角色

语法： grant select on class to 角色名;

列子： grant select on class to testRole;

注：现在，拥有testRole角色的所有用户都具有对class表的select查询权限

3. 删除角色

语法： drop role 角色名;

例子： drop role testRole;

注：与testRole角色相关的权限将从数据库全部删除

# SQL Plus登陆连接数据库

## SQL Plus登陆Oracle的完整语法

```sh
sqlplus user/pwd@[//]Host[:Port]/<service_name> [as {SYSDBA | SYSOPER | SYSASM}]
```

`[]`中为可以省略的选项。

```sh
sqlplus user/pwd@Host[:Port]/<service_name>;

# 或

sqlplus user/pwd@<net_service_name>;
```

`service_name`服务名或`net_service_name`网络服务名。

登陆本地的Oracle，则可以直接`@service_name`，不用指定ip和端口。

Oracle的默认端口为`1521`。

## SQL Plus登陆本地Oracle

登陆本地的Oracle，默认不需要安装Client，只有在客户端（其他电脑）连接Oracle数据库服务器时，才需要安装Oracle。

SQL Plus登陆Oracle，即使不连接任何数据库，至少也应保证Oracle上创建有一个数据库实例（未严格测试，但是如果登陆不上，可以使用`DBCA`随便创建个数据库再登陆测试）

### 以DBA权限登陆（不连接数据库）

#### 方式一

```sh
sqlplus /nolog
```

```sh
conn sys/admin as sysdba
```

> `conn user_name/password` 不指定角色连接。

#### 方式二

```sh
sqlplus sys/admin as sysdba
```

#### 方式三

`sqlplus / as sysdba` 或 `sqlplus /nolog`--`conn / as sysdba` 不指定用户名密码作为`sysdba`登陆的方式，使用的是windows系统认证。因此，要保证开启系统认证登陆。

> `sqlplus "/as sysdba"` 的形式也可以登陆。

#### sqlplus后输入用户名密码登陆

- `sqlplus`或`sqlplus @ip:port/service_name`，然后输入用户名密码登陆

```sh
> sqlplus @localhsot/orcl

SQL*Plus: Release 19.0.0.0.0 - Production on 星期二 10月 18 09:44:03 2022
Version 19.3.0.0.0

Copyright (c) 1982, 2019, Oracle.  All rights reserved.

SP2-0310: 无法打开文件 "localhsot/orcl.sql"
请输入用户名:
```

- `sqlplus`和`请输入用户名:  sys as sysdba`

以`sysdba`角色登陆

```sh
> sqlplus

SQL*Plus: Release 19.0.0.0.0 - Production on 星期二 10月 18 09:40:02 2022
Version 19.3.0.0.0

Copyright (c) 1982, 2019, Oracle.  All rights reserved.

请输入用户名:  sys as sysdba
输入口令:

连接到:
Oracle Database 19c Standard Edition 2 Release 19.0.0.0.0 - Production
Version 19.3.0.0.0

SQL>
```

#### 使用数据库实例连接

前面的登陆连接未指定数据库（实例），连接的是默认新建的第一个数据库。

查看当前连接的数据库：

```sql
SQL> select name from v$database;

NAME
------------------
ORCL
```

连接到ORCL实例：

```sh
> sqlplus / as sysdba@orcl

SQL*Plus: Release 19.0.0.0.0 - Production on 星期二 10月 18 09:32:45 2022
Version 19.3.0.0.0

Copyright (c) 1982, 2019, Oracle.  All rights reserved.


连接到:
Oracle Database 19c Standard Edition 2 Release 19.0.0.0.0 - Production
Version 19.3.0.0.0

SQL>
```

> 或`sqlplus /@orcl as sysdba`。

## SQL Plus登陆远程Oracle

远程登陆，需要安装Oracle客户端，配置好网络连接。

> 主要在client的安装目录下，找到`network/admin/tnsnames.ora`文件，如果没有则新建。
> 
> `tnsnames.ora`的网络连接配置如下（可从Oracle数据库安装包或所在安装目录下找到示例文件）：
> 
> ```sh
> ORCL =
> (DESCRIPTION =
>   (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
>   (CONNECT_DATA =
>     (SERVER = DEDICATED)
>     (SERVICE_NAME = orcl)
>   )
> )
> ```
> 
> TNS通用配置示例：
> 
> ```sh
> IPTrans =
> (DESCRIPTION =
>   (ADDRESS_LIST =
>     (ADDRESS = (PROTOCOL = TCP)(HOST = xx.xx.xx.xx)(PORT = 1521))
>   )
>   (CONNECT_DATA =
>     (SERVICE_NAME = xxx)
>   )
> )
> ```
> 
> - IPTrans：连接的网络服务器名，自己自定义名字
> - SERVICE_NAME ：要连接的实例名（或服务名）   

或者，安装配置`Oracle Data Access Components (ODAC)`，使用其他工具连接。

```sh
sqlplus tiger/scott@localhost/orcl
sqlplus tiger/scott@172.16.10.1:1521/orcl
```

> 如果在Oracle服务器的本地也安装了客户端，有时可能会出现`ORA-12560: TNS: 协议适配器错误`，可以查看`Path`环境变量中 Oracle_db 和 Oracle_client 的顺序（服务器端通常不需要安装客户端），`dbhome`在前面，`client`在后面。
> 
> 具体可参见 [sqlplus 登录Oracle，出现：ORA-12560: TNS: 协议适配器错误](https://blog.csdn.net/aganliang/article/details/85227283)

# Windows下sqlplus / as sysdba登录出现ora-01017用户名/口令无效的问题

在Windows系统下的命令行工具中，使用 `sqlplus / as sysdba`（或`sqlplus /nolog`--`connect / as sysdba`） 登陆Oracle时报错“ORA-01017: 用户名/口令无效; 登录被拒绝”

```sh
> sqlplus /nolog

SQL*Plus: Release 19.0.0.0.0 - Production on 星期一 10月 17 15:32:12 2022
Version 19.3.0.0.0

Copyright (c) 1982, 2019, Oracle.  All rights reserved.

SQL> connect / as sysdba
ERROR:
ORA-01017: 用户名/口令无效; 登录被拒绝
```

```sh
> sqlplus / as sysdba

SQL*Plus: Release 19.0.0.0.0 - Production on 星期一 10月 17 15:37:28 2022
Version 19.3.0.0.0

Copyright (c) 1982, 2019, Oracle.  All rights reserved.

ERROR:
ORA-01017: 用户名/口令无效; 登录被拒绝
```

如果操作系统用户是`administrator`，已经是管理员权限，则，不存在权限不够的问题，应该就是操作系统认证的环节出现了问题，查看`sqlnet.ora`文件（Oracle软件根目录`product\11.2.0\dbhome_1\network\admin\`或安装包目录下`network\admin\`）。

但是，由于安装Oracle时，选择的是“仅设置软件”（“仅安装软件”）：

![](img/20221017165617.png)  

> “仅配置软件”表示仅仅将配置当前的安装软件，将当前安装包所在的Oracle软件配置，并不是完整意义上的Oracle数据库（即使后面创建了数据库实例）
> 
> 创建和配置单实例数据库的作用在于“创建启动数据库”【启动数据库即默认的全局数据库】：
> 
> ![](img/20221017172619.png)  
>
> 而且，对于软件来说，似乎解压后的安装包，在安装后，是作为Oracle软件存在的（具体不太清楚）
> 
> ![](img/20221017173117.png)  

> 启动数据库：也叫全局数据库，是数据库系统的入口，它会内置一些高级权限的用户如SYS，SYSTEM等

在Oracle安装包中`network\admin\`下的`sqlnet.ora`文件中，可以看到其内容：

```ini
# sqlnet.ora Network Configuration File: D:\download\Oracle\WINDOWS.X64_193000_db_home\NETWORK\ADMIN\sqlnet.ora
# Generated by Oracle configuration tools.

# This file is actually generated by netca. But if customers choose to 
# install "Software Only", this file wont exist and without the native 
# authentication, they will not be able to connect to the database on NT.

SQLNET.AUTHENTICATION_SERVICES= (NTS)

NAMES.DIRECTORY_PATH= (TNSNAMES, ONAMES, HOSTNAME)
```

SQLNET.AUTHENTICATION_SERVICES有3个参数：

- SQLNET.AUTHENTICATION_SERVICES = (NONE) 关闭操作系统认证
- SQLNET.AUTHENTICATION_SERVICES = (ALL) 开启LINUX和AIX操作系统认证
- SQLNET.AUTHENTICATION_SERVICES = (NTS) 开启windows操作系统认证

> 似乎"Software Only时，无法使用本地系统认证登陆（即使后面创建了数据库）。

后面只能暂时卸载Oracle，重新安装时选择“创建并配置单实例数据库”。等待安装完成，直接使用`sqlplus / as sysdba`登陆成功。

```sh
> sqlplus / as sysdba

SQL*Plus: Release 19.0.0.0.0 - Production on 星期一 10月 17 17:56:57 2022
Version 19.3.0.0.0

Copyright (c) 1982, 2019, Oracle.  All rights reserved.


连接到:
Oracle Database 19c Standard Edition 2 Release 19.0.0.0.0 - Production
Version 19.3.0.0.0

SQL> exit
从 Oracle Database 19c Standard Edition 2 Release 19.0.0.0.0 - Production
Version 19.3.0.0.0 断开
```

> Oracle 19c相关配置文件比如`sqlnet.ora`、监听文件`listener.ora`、实例数据库配置文件`tnsnames.ora`等，在安装完成后，都位于原安装包所在路径下的`network\admin\`中。
> 
> 也就是，安装包原文件已经作为Oracle软件被配置和使用，和之前的版本有所不同。

# 关于电脑名导致的Oracle安装内存不可用问题

电脑名字太长、有连字符、有中文，或者安装路径（包括安装包所在路径）有中文，都有可能导致安装失败，或者一直提示内存不可用，则必须修改电脑名字、所在路径名称等。

# oracle 查看用户名

## 查看当前用户名

```sql
show user;
select user from dual;
```

## oracle 查看所有用户名

```sql
select * from all_users;
```

```sql
select * from dba_users;
```

## 查看用户拥有的角色或权限

```sql
select * from dba_role_privs where grantee='用户名'；
```

# Oracle查看当前的实例及切换实例

## 查看当前实例

### show parameter name

查看当前登录数据库的配置参数，里面可以看到实例`instance_name`（此外还有`service_names`等）：

```sql
SQL> set linesize 800
SQL> show parameter name

NAME                                 TYPE                   VALUE
------------------------------------ ---------------------- ------------------------------
cdb_cluster_name                     string
cell_offloadgroup_name               string
db_file_name_convert                 string
db_name                              string                 orcl
db_unique_name                       string                 orcl
global_names                         boolean                FALSE
instance_name                        string                 orcl
lock_name_space                      string
log_file_name_convert                string
pdb_file_name_convert                string
processor_group_name                 string

NAME                                 TYPE                   VALUE
------------------------------------ ---------------------- ------------------------------
service_names                        string                 orcl
```

### select name from v$database;

```sql
SQL> select name from v$database;

NAME
------------------
ORCL
```

### show parameter instance_name;

只查看实例参数（实例名）

## 指定连接的实例

```sh
sqlplus /@ORACLE_SID as sysdba;
# 或
sqlplus user/pwd@ORACLE_SID as sysdba;
```

## 查看所有实例

```sql
select instance_name from v$instance;
```

```sql
select name from v$database ;
```

## 查看所有的表空间

```sql
select tablespace_name from dba_tablespaces;
```

## 查看用户及其表空间

```sql
select default_tablespace, temporary_tablespace, d.username  
from dba_users d;
```

# 理解Oracle中user、scheme、表空间、数据库、实例之间的关系

## 数据库和实例

完整的Oracle数据库通常由两部分组成：Oracle数据库和数据库实例。

1) 数据库是一系列物理文件的集合（数据文件，控制文件，联机日志，参数文件等）； 
2) Oracle数据库实例则是一组Oracle后台进程/线程以及在服务器分配的共享内存区。

在启动Oracle数据库服务器时，实际上是在服务器的内存中创建一个Oracle实例（即在服务器内存中分配共享内存并创建相关的后台内存），然后由这个Oracle数据库实例来访问和控制磁盘中的数据文件。Oracle有一个很大的内存块，成为全局区（SGA）。

> 实例是访问Oracle数据库所需的一部分计算机内存和辅助处理后台进程，是由进程和这些进程所使用的内存(SGA)所构成一个集合。
> 
> 实例关联了数据库文件才可以访问，如果没有，就会得到实例不可用的错误。

服务器、数据库、实例的对应关系可以为：

- 服务器：数据库——1：n

- 数据库：实例——1：n

- 实例：用户——1：n

多个实例可以对应一个数据库，他们共同操作同一数据文件，一个数据库可被许多实例同时装载和打开(即RAC)，RAC环境中实例的作用能够得到充分的体现！

在任何时刻，一个实例只能有一组相关的文件（与一个数据库关联）。大多数情况下，反过来也成立：一个数据库上只有一个实例对其进行操作。不过，Oracle的真正应用集群（Real Application Clusters，RAC）是一个例外，这是Oracle提供的一个选项，允许在集群环境中的多台计算机上操作，这样就可以有多台实例同时装载并打开一个数据库（位于一组共享物理磁盘上）。由此，可以同时从多台不同的计算机访问这个数据库。Oracle RAC能支持高度可用的系统，可用于构建可扩缩性极好的解决方案。

**实例名指的是用于响应某个数据库操作的数据库管理系统的名称。同时也叫SID。实例名是由参数instance_name决定的。**

## 表空间

表空间(tablespace)是数据库的逻辑划分，每个数据库至少有一个表空间（称作SYSTEM表空间）。为了便于管理和提高运行效率，可以使用一些附加表空间来划分用户和应用程序。例如：USER表空间供一般用户使用，RBS表空间供回滚段使用。**一个表空间只能属于一个数据库**。

Oracle数据库通过表空间来管理（使用和存储）物理表，一个数据库实例可以有N个表空间，一个表空间下可以有N张表。

表空间只和数据文件（ORA或者DBF文件）发生关系，数据文件是物理的，一个表空间可以包含多个数据文件，而一个数据文件只能隶属一个表空间。

## 数据文件（dbf、ora）：

数据文件是数据库的物理存储单位。

数据库的数据存储在表空间中，真正是在某一个或者多个数据文件中。而一个表空间可以由一个或多个数据文件组成，一个数据文件只能属于一个表空间。

一旦数据文件被加入到某个表空间后，就不能删除这个文件，如果要删除某个数据文件，只能删除其所属于的表空间才行。

## 用户

Oracle数据库建好后，要想在数据库里建表，必须先为数据库建立用户，并为用户指定表空间。

一个用户至少要有一个默认表空间，此外，还可以管理其他表空间。

用户需要有表空间足够的权限，才能操作表空间

**用户是在实例下建立的。不同实例可以建相同名字的用户。**

> 表的数据，是由用户放入某一个表空间的，而这个表空间会随机把这些表数据放到一个或者多个数据文件中。
> 
> oracle的数据库不是普通的概念，oracle是由用户和表空间对数据进行管理和存放的。但是表不是由表空间去查询的，而是由用户去查的。因为不同用户可以在同一个表空间建立同一个名字的表！这里区分就是用户了！



# 参考

- [Oracle内置账户sys/system详解，角色normal/sysdba/sysoper详解及创建用户、角色、授权](https://blog.csdn.net/wqh0830/article/details/87874380)
- [windows平台 sqlplus / as sysdba登录出现ora-01017错误](https://blog.csdn.net/m0_37625564/article/details/112920445)
- [Oracle Sqlplus命令登录的几种方式](https://blog.csdn.net/wwlhz/article/details/73296430)

- [Oracle 数据库服务器，数据库，实例，用户之间的关系](https://blog.csdn.net/u011519658/article/details/9986813)
- [Oracle数据库、实例、用户、表空间、表之间的关系](https://blog.csdn.net/MINGDE_SKILL/article/details/102365698)

# Oracle安装配置好文推荐

- [Oracle---windows下安装oracle19c](https://www.cnblogs.com/zdyang/p/12580263.html)
- [Oracle19c的安装、卸载配置教程](https://blog.csdn.net/Evening_breeze_/article/details/113988231)，尤其注意卸载Oracle后删除注册表中的内容

# 其他好文

- [关系型数据库Oracle之架构详解](https://blog.csdn.net/qq_41036232/article/details/84500594)
- [Oracle架构、原理、进程](https://cloud.tencent.com/developer/article/1531025)