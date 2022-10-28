

# OracleSQLHelper - 简单易用的Oracle DB访问帮助类

很简单的Oracle DB帮助类，参数化的基本SQL语句执行、查询DataTable结果的数据、判断数据库和表是否存在、更改连接的DB。

支持.NET Framework 4.5和.NET Standard2.1

# 简要用法

- 基本使用

```cs
using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
{
    var resule = sqlHelper.ExecuteQuery("select * from product");
    // ...
}
```

```cs           
using (var sqlHelper = new OracleSQLHelper())
{
    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
    {
        var resule = sqlHelper.ExecuteQuery("select * from test");
        // ...
    }
}
```


```cs
using (var sqlHelper = new OracleSQLHelper())
{
        if (sqlHelper.Initializer("ConnectionString"))
        {

        }
}
```

- 判断表、列是否存在

```cs
using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
{
    var testTableExists1 = sqlHelper.ExistsDBOrTableOrCol("", "t", "qty");
    var testTableExists2 = sqlHelper.ExistsDBOrTableOrCol("", "t", "qty_No");
    var testTableExists3 = sqlHelper.ExistsDBOrTableOrCol("", "t");
    var testTableExists4 = sqlHelper.ExistsDBOrTableOrCol("", "t2");
}
```


# License

BSD-2-Clause(涉及第三方类库或代码，以第三方或兼容第三方类库的License为准)