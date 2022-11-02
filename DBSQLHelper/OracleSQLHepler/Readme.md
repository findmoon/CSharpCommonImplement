

# OracleSQLHelper - 简单易用的Oracle DB访问帮助类

很简单的Oracle DB帮助类，参数化的基本SQL语句执行、查询DataTable结果的数据、判断数据库和表是否存在、更改连接的DB。

支持.NET Framework 4.5和.NET Standard2.1

# 简要用法

- 基本使用

```cs
using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
{
    var resule = await sqlHelper.ExecuteQueryAsync("select * from product");
    // ...
}
```

```cs           
using (var sqlHelper = new OracleSQLHelper())
{
    if (sqlHelper.Initializer(new SQLInitModel(ip, user, pwd, dbname)))
    {
        var resule = await sqlHelper.ExecuteQueryAsync("select * from test");
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
    var testTableExists1 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t", "qty");
    var testTableExists2 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t", "qty_No");
    var testTableExists3 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t");
    var testTableExists4 = await sqlHelper.ExistsDBOrTableOrColAsync("", "t2");
}
```

- 参数化查询

> **注意：Oracle中参数化查询的需要使用 `:`**

```cs
using (var sqlHelper = OracleSQLHelper.Init(ip, user, pwd, dbname))
{
    // CREATE 语句要单独执行，不能和 INSERT 一起
    var createSql = @"CREATE TABLE Product(
product_id char(4) NOT NULL PRIMARY KEY,
product_name varchar(100) NOT NULL,
product_type varchar(32) NOT NULL,
sale_price int NULL,
purchase_price int NULL,
regist_date date NULL
)";
    var cresult = await sqlHelper.ExecuteNonQueryAsync(createSql);
    // 创建语句返回 -1
    Debug.WriteLine($"create result: {cresult}");

    // 不能一次执行多个INSERT语句，要单独执行
    var iniInsertSql = "INSERT INTO Product values('0006','叉子','厨房用具',5000,NULL,TO_DATE('2009-09-20','yyyy-mm-dd'))";
    var inResult = await sqlHelper.ExecuteNonQueryAsync(iniInsertSql);
    iniInsertSql = "INSERT INTO Product values('0007','擦菜板','厨房用具',8800,395,TO_DATE('2008-04-28','yyyy-mm-dd'))";
    inResult = await sqlHelper.ExecuteNonQueryAsync(iniInsertSql);
    Debug.WriteLine($"insert result: {inResult}");

    // select * from Product where product_id='0006' or sale_price=8800; 
    var selectSql = "select * from Product where product_id=:id or sale_price=:price";
    var resuleDt = await sqlHelper.ExecuteQueryAsync(selectSql, new OracleParameter[]
    {
        new OracleParameter(":id","0006"),
        new OracleParameter(":price",8800)
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
            // 使用 row.Field<T>(name/idx/..) 获取字段值，需要判断是否DBNull -- row[col.ColumnName] is DBNull。 不为DBNull才能转换 且 必须指定正确的T数据类型

            Debug.Write(row[col.ColumnName].ToString() + "\t");
        }
        Debug.WriteLine(null);
    }

    // ...

    // insert Product values('0009','圆珠笔','办公用品',15,10,GETDATE());

    // 参数化查询中使用的 参数名 不能为 Oracle相关的关键字，比如 date type 等，否则报错
    //var insertSql = "insert into Product values(:id,:name,:mytype,:price,:purchase,TO_DATE(:mydate,'yyyy-mm-dd'))";
    var insertSql = "insert into Product values(:id,:name,:mytype,:price,:purchase,:mydate)";
    var rowNum = await sqlHelper.ExecuteNonQueryAsync(insertSql, new OracleParameter[]
    {
        // new OracleParameter(":id","0010"),
        new OracleParameter(":id","0011"),
        new OracleParameter(":name","圆珠笔"),
        new OracleParameter(":mytype","办公用品"),
        new OracleParameter(":price",15),
        new OracleParameter(":purchase",10),
        //new OracleParameter(":mydate",DateTime.Now.ToString("yyyy-MM-dd"))
        new OracleParameter(":mydate",DateTime.Now)
    });

    Debug.WriteLine($"affect row num: {rowNum}");

    // select product_id from Product where product_id='0010';
    var selectOneSql = "select product_id from Product where product_id=:id";
    var resuleValue = await sqlHelper.ExecuteScalarAsync(selectOneSql, new OracleParameter[]
    {
    new OracleParameter(":id","0010")
    });
    Debug.WriteLine($"product_id: {resuleValue}");
  
}
```

# 注意

1. 执行SQL语句结尾不能有`;`，否则报错
1. Oracle中参数化查询的要使用 `:`
2. CREATE 等 DLL 语句要单独执行，不能和 INSERT 等dml 或 其他dll语句 一起，否则报错 “ORA-00911: 无效字符”；INSERT 等dml语句也是，每次只能执行一条语句。【目前所知似乎，每次只能执行一条语句，否则报错】
3. 不能一次执行多个INSERT语句，要单独执行。否则报错
4. 参数化查询中使用的 参数名 不能为 Oracle相关的关键字，比如 date type 等，否则报错

# License

BSD-2-Clause(涉及第三方类库或代码，以第三方或兼容第三方类库的License为准)
