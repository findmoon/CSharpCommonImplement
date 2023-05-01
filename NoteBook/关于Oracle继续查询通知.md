**关于Oracle继续查询通知**

# 权限不足

授予更改通知的权限：

```sql
GRANT CHANGE NOTIFICATION TO UserName;

-- REVOKE CHANGE NOTIFICATION FROM UserName;
```

# 报错 ORA-29983 

错误代码: ORA-29983
描述: 查询对于连续查询通知不受支持
原因: 无法为连续查询通知注册查询。
动作: 该查询具有一些使其与连续查询通知不兼容的构造，例如同义词或视图。请检查文档以获取完整列表。

Error code: ORA-29983
Description: Unsupported query for Continuous Query Notification
Cause: The query cannot be registered for Continuous Query Notification.
Action: The query has some constructs that make it incompatible with Continous Query Notification like synonyms or views. Please check the documentation for complete list.

# ORA-29979

Error code: ORA-29979
Description: Query registration not supported at current compatible setting
Cause: An attempt was made to register a query with the database compatible setting lower than 11.
Action: Increase the database compatible setting to 11.0.0.0.0

(Solution)解决方案就是 Action动作：升级数据库兼容设置到11。

# Oracle查询与修改数据库版本兼容性

查询数据库版本兼容性：

```sql
show parameter COMPATIBLE;

-- 或：
col NAME for a20
col VALUE for a15
col DESCRIPTION for a80
select name,value,description from v$parameter where name = 'compatible';
```

修改数据库版本兼容性：

```sql
alter system set compatible = "12.1.0" sope = spfile;
```

> Before upgrading to Oracle Database 19c, you must set the COMPATIBLE initialization parameter to at least 11.2.0, which is the minimum setting for Oracle Database 19c.
> 
> The compatible parameter must be at least 3 decimal numbers, separated by periods. For example:
>
> ```sql
> SQL> ALTER SYSTEM SET COMPATIBLE = '19.0.0' SCOPE=SPFILE;
> ```
> 
> Oracle recommends that you only raise the COMPATIBLE parameter after you have thoroughly tested the upgraded database.
> 
> After you increase the COMPATIBLE parameter, you cannot downgrade the database, and you cannot flash back to restore points.

# 参考

- [What Is Oracle Database Compatibility?](https://docs.oracle.com/en/database/oracle/oracle-database/19/upgrd/what-is-oracle-database-compatibility.html#GUID-4711E0D1-9FCF-4F35-85B5-52EBB437C00E)
- [What Is Oracle Database Compatibility? - 12c](https://docs.oracle.com/en/database/oracle/oracle-database/12.2/upgrd/what-is-oracle-database-compatibility.html#GUID-7FCE8614-8163-4393-AE66-2ADD1F73934F)
