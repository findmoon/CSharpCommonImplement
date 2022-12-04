**Oracle使用expdp/impdp实现全库导入导出的整体流程**

Oracle的全库导入，首先一点必须先创建数据库，创建了数据库，才能往该数据库导入所有数据。

相对来说，使用Oracle进行数据导入导出还很有些“麻烦”的，大多数资料上来就是一堆命令或者命令的介绍，相关的、冗余的都杂糅在一起，看着往往让人越来越不清晰（深入熟悉了解Oracle架构、PL/SQL的大神除外）。

因此，对使用 expdp/impdp 命令工具对全库导出和导入的整体操作流程和主要步骤进行介绍和记录，最后，关于全库迁移的处理，还是推荐使用 `rman` 工具（一个看起来和使用起来似乎更加麻烦的工具，但功能应该很强）。

# 数据库导出

## 创建导出文件的目录directory

登陆数据库。

创建directory，名称为`DUMP_DIR`，用于在导出时指定导出到的目录：

```sql
create directory DUMP_DIR as 'D:\OracleExpdp';
```

> 注意，(路径)要用单引号，双引号会报错。

查看directory：

```sql
select * from dba_directories;    --查看directory
```

## 数据泵将数据库全部导出

如下，为 `expdp` 导出数据库全部数据的命令。

记得修改正确的`username/password`，数据库服务名`SID`，`dumpfile`为导出的数据文件名，默认名为`expdat.dmp`，逗号分割可以指定导出到多个文件，`logfile`为导出的日志文件名，若不指定，默认生成名为`export.log`的日志文件。

```sh
expdp username/password@SID directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

`FULL`表示导出整个数据库，默认N。

> 注：用户需要授权导出的权限，若是系统认证，或system/sys高级用户，则不需要。
> 
> 授予用户导入导出操作相关的权限：
> 
> ```sql
> grant read,write on directory DUMP_DIR to username;
> grant exp_full_database to username;
> ```

# 数据库导入

## 创建文件的目录directory

登陆数据库。同样创建directory，指定导入时文件所在的路径：

```sql
create directory DUMP_DIR as 'D:\OracleExpdp';
```

> DUMP_DIR 指向 dmp 导出的数据文件所在路径。

## 导入全部数据

```sh
impdp username/password@SID directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

同样，用户要有足够的权限

# 使用系统认证(或系统用户)进行数据库导出和导入

使用系统认证（sysdba角色）登陆数据库，免除用户权限的问题，可以更方便的直接执行导出或导入。

- 创建目录`DUMP_DIR`

```sql
create directory DUMP_DIR as 'D:\OracleExpdp';
```

- 全部导出

```sh
expdp '/@SID as sysdba' directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

> 如果报错，则使用`"'/@SID as sysdba'"`【实际使用没问题】形式，或`\'/@SID as sysdba\'`

或 `sys` 用户

```sh
expdp 'sys/admin@SID as sysdba' directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

> `"'sys/admin@SID as sysdba'"`

- 全部导入

```sh
impdp '/@SID as sysdba' directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

> `"'/@SID as sysdba'"`

或sys用户

```sh
impdp 'sys/admin@SID as sysdba' directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

> `"'sys/admin@SID as sysdba'"`

# expdp报错`UDE-00014: invalid value for parameter, ‘attach’`

这个原因在于`@SID`位置放置不正确。密码`password`和`@SID`之前不能有空格，并且要紧挨着。也就是上面所示的写法。系统认证的`/`也要写成`/@SID`。

参考[UDE-00014: invalid value for parameter, ‘attach’终极解决方案之一](https://blog.csdn.net/yzm272/article/details/86493791)

# 保证源和目标数据库的字符集一致

`select userenv('language') from dual;`查看数据库的字符集。

通常字符集在创建数据库时指定好，创建后则不应该修改。

如果导入导出时不一致，推荐的做法是，删除目标数据库，重新创建与源库字符集一样的数据库，而不是直接进行字符集的修改。

具体参见：[修改oracle数据库字符集编码](https://blog.csdn.net/beijirose/article/details/8599935)

# expdp/impdp相比exp/imp的优点

imp 需要先建立表空间、用户等再导入。impdp则不需要。

expdp/impdp 支持全库导入导出；效率相对更高。

# 表空间块大小与配置大小不一致导致导入时无法创建表空间的问题

impdp导入是会创建表空间、用户等，但是，如果表空间大小与配置的不一样就会报错`ORA-29339`

```sh
ORA-29339: 表空间块大小 16384 与配置的块大小不匹配
```

通常在直接用`plsql`创建表空间时，也有可能出现这个错误。这是由于创建表空间时指定的块大小与系统设置的块大小不一致。

解决办法通常是修改`db_Nk_cache_size`的大小。但是，有些块大小是和系统存储的块大小一致的，不允许修改。

如下为自己测试，有些能够修改，并且提示“已更改”，但是查看仍然没有变化。

```sql
SQL> alter system set db_4k_cache_size=8m;

系统已更改。

SQL> show parameter 4k;

NAME                                 TYPE                   VALUE
------------------------------------ ---------------------- ------------------------------
db_4k_cache_size                     big integer            32M
```

最终也是没有解决。

【其原因应该是源库没有使用默认的块大小（block_size）】

最开始的表空间导入失败，导致后面剩余的数据或信息的导入都失败。

> 正确的处理，应该是创建表空间，然后在依次执行用户、表、数据的导入，而不能直接进行整库的导入。
> 
> 具体通过各个部分进行导出导入，参见参考文章。

# 附：`expdp help=y`查看expdp的帮助信息


# 附：关于`grants=Y`参数

在[ORA-39083 - During Impdp](https://community.oracle.com/tech/developers/discussion/843166/ora-39083-during-impdp)中看到有使用`grants=Y`参数。其具体作用和使用暂时不清楚。

```sh
exp username/password tables=(mytables, moretables) file=mytable.dmp lpg=mytable.log  grants=Y

imp username/password tables=(mytables, moretables) file=mytable.dmp log=myimport.log grants=Y
```

# 附：全库(整体数据库)备份迁移推荐使用rman

全库或整体数据库的迁移，通常还是推荐使用 `rman` 工具。

# 附：`exp`、`imp`的示例

```sh
exp "'/@SID as sysdba'" file=D:\dumpfile.dmp full=y


imp "'/@SID as sysdba'" file=D:\dumpfile.dmp full=y

imp "'/@SID as sysdba'" file=D:\dumpfile.dmp full=y ignore=y
```

```sh
exp "'/ as sysdba'"@SID file=D:\dumpfile.dmp full=y


imp "'/ as sysdba'"@SID file=D:\dumpfile.dmp full=y

## imp "'/ as sysdba'"@SID file=D:\dumpfile.dmp full=y ignore=y
```

# 参考

- [Oracle数据库expdp用法以及注意事项](https://www.cnblogs.com/Jingkunliu/p/13705626.html)
- [oracle 数据泵导出导用户，多用户，整个库，指定表的数据](https://blog.csdn.net/qq_40203552/article/details/115627148)
- [Oracle数据泵导入导出备份数据库数据详解](https://www.zhangqiongjie.com/574.html)
- [oracle导出整个数据库和导入整个数据库命令](https://cloud.tencent.com/developer/article/1650129)
- [Oracle 表空间、权限、赋权、expdp导出、impdp导入等](https://www.jianshu.com/p/ed208c104851)

# 其他

[12c向19c迁移：使用数据泵（impdp）+dblink做全量迁移](https://blog.csdn.net/howard_shooter/article/details/126948861)