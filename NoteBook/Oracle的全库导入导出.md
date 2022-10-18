**Oracle的全库导入导出**

Oracle的全库导入，首先一点必须先创建数据，创建了数据库，才能往该数据库导入所有数据。

# 数据库导出

## 创建导出文件的目录directory

登陆数据库。

```sql
create directory DUMP_DIR as 'D:\OracleExpdp';
```

## 数据泵将数据库全部导出

记得修改正确的`username/password`，数据库服务名`SID`，`dumpfile`为保存的文件名

```sh
expdp username/password@SID directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

`FULL`表示导出整个数据库，默认N。

> 注：用户需要授权导出的权限，若是系统认证，或system/sys高级用户，则不需要。
> 
> ```sql
> grant read,write on directory DUMP_DIR to username;
> grant exp_full_database to username;
> ```

# 数据库导入

## 创建文件的目录directory

登陆数据库。

```sql
create directory DUMP_DIR as 'D:\OracleExpdp';
```

> DUMP_DIR 指向dmp导出的数据文件所在路径。

## 导入全部数据

```sh
impdp username/password@SID directory=DUMP_DIR dumpfile=文件名.dmp logfile=文件名.log full=y
```

同样，用户要有足够的权限

# 使用系统认证的数据库导出和导入

使用系统认证免除问用户权限的问题，可以更方便的直接执行导出或导入。

