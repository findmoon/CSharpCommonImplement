**Oracle Sqlplus命令登录的N种方式**

[toc]

> [Oracle Sqlplus命令登录的几种方式](https://blog.csdn.net/wwlhz/article/details/73296430)

> Sqlplus 命令行登陆执行sql脚本。

# [Oracle](https://so.csdn.net/so/search?q=Oracle&spm=1001.2101.3001.7020) Sqlplus命令登录的几种方式

## sqlplus 命令语法

```
sqlplus [ [<option>] [{logon | /nolog}] [<start>] ]
<option> 为: [-C <version>] [-L] [-M "<options>"] [-R <level>] [-S]

-C <version>   将受影响的命令的兼容性设置为<version> 指定的版本。该版本具有"x.y[.z]" 格式。例如, -C 10.2.0
-L             只尝试登录一次, 而不是 在出错时再次提示。
-M "<options>" 设置输出的自动 HTML 标记。选项的格式为:
               HTML [ON|OFF] [HEAD text] [BODY text] [TABLE text][ENTMAP {ON|OFF}] [SPOOL {ON|OFF}] [PRE[FORMAT] {ON|OFF}]
-R <level>     设置受限(restricted)模式, 以禁用与文件系统交互的SQL*Plus 命令。级别可以是 1, 2 或 3。最高限制级别为 -R 3, 该级别禁用与文件系统交互的所有用户命令。
-S             设置无提示(slient)模式, 该模式隐藏命令的提示和回显 的显示。

 <logon> 为: {<username>[/<password>][@<connect_identifier>] | / }[AS {SYSDBA | SYSOPER | SYSASM}] [EDITION=value]

 指定数据库帐户用户名, 口令和数据库连接的连接标识符。如果没有连接标识符, SQL*Plus 将连接到默认数据库。
 AS SYSDBA, AS SYSOPER 和 AS SYSASM 选项是数据库管理权限。

 <connect_identifier> 的形式可以是 Net 服务名或轻松连接。
   @[<net_service_name> | [//]Host[:Port]/<service_name>]
   <net_service_name> 是服务的简单名称, 它解析为连接描述符。
   示例: 使用 Net 服务名连接到数据库, 且数据库 Net 服务名为 ORCL。
      sqlplus myusername/mypassword@ORCL
   Host 指定数据库服务器计算机的主机名或 IP地址。
   Port 指定数据库服务器上的监听端口。
   <service_name> 指定要访问的数据库的服务名。
   示例: 使用轻松连接连接到数据库, 且服务名为 ORCL。
      sqlplus myusername/mypassword@Host/ORCL

 /NOLOG 选项可启动 SQL*Plus 而不连接到数据库。
 EDITION 指定会话版本的值。

<start> 为: @<URL>|<filename>[.<ext>] [<parameter> ...]
使用将分配给脚本中的替代变量的指定参数从 Web 服务器 (URL) 或本地文件系统 (filename.ext)运行指定的 SQL*Plus 脚本。

在启动 SQL*Plus 并且执行 CONNECT 命令后, 将运行站点概要文件 (例如, $ORACLE_HOME/sqlplus/admin/glogin.sql) 和用户概要文件例如, 工作目录中的 login.sql)。这些文件包含 SQL*Plus 命令。
```

支持的功能很全，但常用的几种连接方式也就几种：

**1\. sqlplus / as sysdba**

```
  sqlplus / as sysdba
```

无需数据库进入可用状态，就可用用该命令登录，运行startup来启动。

**2\. sqlplus “/as sysdba”**

```
sqlplus "/as sysdba"    
```

上一条命令的另一种形式，未发现两者有什么区别。

**3\. sqlplus username/pwd@host/service\_name**

```
sqlplus tiger/scott@localhost/orcl
sqlplus tiger/scott@172.16.10.1:1521/orcl
```

以用户名/密码、IP：Port、服务名 为参数登录。

**4\. sqlplus /nolog**

```
sqlplus /nolog
```

先使用sqlplus命令，而不连接数据库，然后用conn命令登录。

```
 conn tiger/scott
 conn tiger/scott@172.16.0.1/orcl
```

**这种方式比第3种方式安全，因为第3种方式登录后，通过ps查看到的进程是带用户名和密码的。**