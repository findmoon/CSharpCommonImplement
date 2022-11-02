

# SQLServerHelper - 简单易用的SQL Server DB访问帮助类

很简单的SQL Server帮助类，参数化的基本SQL语句执行、查询DataTable结果的数据、判断数据库和表是否存在、更改连接的DB。

支持.NET Framework 4.5和.NET Standard2.1

# 简要用法

- 基本使用

```cs
using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
{
    var resule = await sqlHelper.ExecuteQueryAsync("select * from product");
    // ...
}
```

```cs           
using (var sqlHelper = new SQLServerHelper())
{
    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
    {
        var resule = await sqlHelper.ExecuteQueryAsync("select * from test");
        // sqlHelper.Conn.Dispose();
        // ...
    }
}
```


```cs
using (var sqlHelper = new SQLServerHelper())
{
        if (sqlHelper.Initializer("ConnectionString"))
        {
            // do something
        }
}
```

- 判断数据库、表、列是否存在

```cs
using (var sqlHelper = SQLServerHelper.Init(ip, user, pwd, dbname))
{
    var tableExists1 = await sqlHelper.ExistsDBOrTableOrColAsync("t", "test");
    var dbExists2 = await sqlHelper.ExistsDBOrTableOrColAsync("shop","");
    var colExists3 = await sqlHelper.ExistsDBOrTableOrColAsync("", "product", "sale_price");
    var colExists4 = await sqlHelper.ExistsDBOrTableOrColAsync("", "product", "sale_price_No");
    var colExists5 = await sqlHelper.ExistsDBOrTableOrColAsync("shop", "product", "sale_price");
    var tableExists6 = await sqlHelper.ExistsDBOrTableOrColAsync("t", "test1");
}
```

- 参数化查询

```cs
// select * from Product where product_id='0006' or sale_price=8800; 
var selectSql = "select * from Product where product_id=@id or sale_price=@price;";
var resuleDt = await sqlHelper.ExecuteQueryAsync(selectSql, new SqlParameter[]
{
    new SqlParameter("@id","0006"),
    new SqlParameter("@price",8800)
});

foreach (DataColumn col in resuleDt.Columns)
{
    Debug.Write(col.ColumnName + "\t");
}
Debug.WriteLine(null);
foreach (DataRow row in resuleDt.Rows)
{
    foreach (DataColumn col in resuleDt.Columns)
    {
        
        Debug.Write(row[col.ColumnName].ToString() + "\t");
    }
    Debug.WriteLine(null);
}

// ...

// insert Product values('0009','圆珠笔','办公用品',15,10,GETDATE());
var insertSql = "insert into Product values(@id,@name,@type,@price,@purchase,@date);";
var rowNum = await sqlHelper.ExecuteNonQueryAsync(insertSql, new SqlParameter[]
{
    new SqlParameter("@id","0010"),
    new SqlParameter("@name","圆珠笔"),
    new SqlParameter("@type","办公用品"),
    new SqlParameter("@price",15),
    new SqlParameter("@purchase",10),
    new SqlParameter("@date",DateTime.Now)
});

Debug.WriteLine($"affect row num: {rowNum}");

// select product_id from Product where product_id='0010';
var selectOneSql = "select product_id from Product where product_id=@id;";
var resuleValue = await sqlHelper.ExecuteScalarAsync(selectOneSql, new SqlParameter[]
{
    new SqlParameter("@id","0010")
});
Debug.WriteLine($"product_id: {resuleValue}");
```

输出：

```sql
product_id	product_name	product_type	sale_price	purchase_price	regist_date	
0006	叉子	厨房用具	5000		2009-09-20 0:00:00	
0007	擦菜板	厨房用具	8800	395	2008-04-28 0:00:00	
affect row num: 1
product_id: 0010
```

> 注意，使用 DataTable.DataRow 获取单元格值时，使用 `row.Field<T>(name/idx/..)` 获取字段值，需要判断是否 `DBNull` -- `row[col.ColumnName] is DBNull`。 不为DBNull 且 必须指定正确的T数据类型 才能正确获取到值。
> 仅显示使用，推荐直接使用 `row[col.ColumnName].ToString()`

# 注意

`SQLServerHelper` 默认连接不使用数据加密，即连接字符串中 `Encrypt=False`，可以根据需要使用自己的字符串，用以设置数据加密 `Encrypt`、信任服务器证书 `TrustServerCertificate`/`Trust Server Certificate` 等。

# License

BSD-2-Clause(涉及第三方类库或代码，以第三方或兼容第三方类库的License为准)
