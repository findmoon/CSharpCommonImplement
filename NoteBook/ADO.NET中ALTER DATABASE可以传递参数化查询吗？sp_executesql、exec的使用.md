**ADO.NET中修改数据库系统属性级别的语句ALTER DATABASE可以传递参数化查询吗？sp_executesql的使用**

[toc]

比如 想要通过语句 `ALTER DATABASE @dbname SET SINGLE_USER;` 修改数据库为单用户模式，ADO.NET中通过参数化查询传递数据，会报语法错。

[Can I pass parameter to an ALTER DATABASE command](https://social.msdn.microsoft.com/Forums/sqlserver/en-US/578d87fa-9939-4cb0-bb72-e37cee8abf25/can-i-pass-parameter-to-an-alter-database-command?forum=transactsql) 介绍到`ALTER DATABASE`无法参数化查询（简单的回答）。

不仅如此，在SSMS的T-SQL编辑器中，也无法通过变量，执行`ALTER DATABASE`等命令。

但是，可以通过`动态sql`的执行实现变量参数的传递。

# sp_executesql 执行动态sql，使用变量

比如，设置数据库离线：`ALTER DATABASE @dbname SET OFFLINE WITH ROLLBACK IMMEDIATE`。

通过`sp_executesql`执行动态SQL，将要执行的语句作为变量传递。

```sql
DECLARE @dbname NVARCHAR(125)

SET @dbname = 'Adventures'

DECLARE @sql NVARCHAR(max)
SET @sql = N'ALTER DATABASE ' + @dbname + N' SET OFFLINE WITH ROLLBACK IMMEDIATE'
EXEC sp_executesql @sql
```

# Exec 动态执行

```sql
Declare @dbname NVARCHAR(MAX), @strSQL NVarchar(Max)

SET @dbname = N'Adventures'

Set @strSQL = N'ALTER DATABASE ' + @dbname + N' SET OFFLINE WITH ROLLBACK IMMEDIATE'

Exec(@strSQL)  --- execute(@strSQL)
```

> 这样，也能实现在 ADO.NET 中的参数化查询。

> quotename(@dbname)


# 另：获取存储过程的返回代码值

使用 RETURN 语句指定过程的返回代码。 与输出参数一样，执行过程时必须将返回代码保存到变量中，才能在调用程序中使用返回代码值。

```sql
DECLARE @result int;

EXECUTE @result = my_proc;
```


```sql
DECLARE @result int;
EXEC @result = sp_detach_db @dbname;
```