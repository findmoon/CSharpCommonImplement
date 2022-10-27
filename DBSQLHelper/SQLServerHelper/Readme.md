

# SQLServerHelper - 简单易用的SQL Server DB访问帮助类

很简单的SQL Server帮助类，参数化的基本SQL语句执行、查询DataTable结果的数据、判断数据库和表是否存在、更改连接的DB。

支持.NET Framework 4.5和.NET Standard2.1

# 简要用法

- 基本使用

```cs
using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
{
    var resule = sqlHelper.ExecuteQuery("select * from product");
    // ...
}
```

```cs           
using (var sqlHelper = new SQLServerHelper())
{
    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
    {
        var resule = sqlHelper.ExecuteQuery("select * from test");
        // ...
    }
}
```


```cs
using (var sqlHelper = new SQLServerHelper())
{

        if (sqlHelper.Initializer("ConnectionString"))
        {

        }
}
```

- 判断数据库、表、列是否存在

```cs
using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
{
    var tableExists1 = sqlHelper.ExistsDBOrTableOrCol("t", "test");
    var dbExists2 = sqlHelper.ExistsDBOrTableOrCol("shop","");
    var colExists3 = sqlHelper.ExistsDBOrTableOrCol("", "product", "sale_price");
    var colExists4 = sqlHelper.ExistsDBOrTableOrCol("", "product", "sale_price_No");
    var colExists5 = sqlHelper.ExistsDBOrTableOrCol("shop", "product", "sale_price");
    var tableExists6 = sqlHelper.ExistsDBOrTableOrCol("t", "test1");
}
```

# 注意

默认连接不使用数据加密，即连接字符串中 `Encrypt=False`，可以根据需要使用自己的字符串，用以设置数据加密 `Encrypt`、信任服务器证书 `TrustServerCertificate`/`Trust Server Certificate` 等。

# License

BSD-2-Clause(涉及第三方类库或代码，以第三方或兼容第三方为准)