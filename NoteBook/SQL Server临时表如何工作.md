SQL Server临时表如何工作？【译】

[toc]

> 原文 [How SQL Server Temp Tables Work](https://www.makeuseof.com/sql-server-temp-tables/)


**对于简化复杂的查询和操作，临时表是一种聪明的解决方案。**

SQL Server临时表存储临时数据。你可以在临时表上执行类似 SELECT、INSERT、DELETE、UPDATE 等常规的SQL表的操作。

临时表位于`tempdb`数据库，并且仅仅在连接期间可见。当你终止连接时，SQL Server会删除临时表。你也可以在任何时间显式的删除它。

# 临时表的类型

有两种类型的临时表：本地和全局。

## Local Temp Table

本地临时表仅仅对于创建它的连接可见。当连接结束，或用户从SQL Server实例断开连接，本地临时表会自动清除。

在`CREATE TABLE`语句中，表名以单个`#`号开头，就可以创建一个本地临时表。

语法如下：

```sql
CREATE TABLE #TempTable (
   Column1 INT,
   Column2 VARCHAR(50)
);
```

下面的示例创建一个名为`#TempCustomer`的临时表，有name和email字段：

```sql
CREATE TABLE #TempCustomer (
   ID int NOT NULL PRIMARY KEY
   FullName VARCHAR(50),
   Email VARCHAR(50)
);
```

## Global Temp Table

全局临时表对所有的连接和用户可见，当所有的连接和引用该表的用户都断开连接后，SQL Server才会删除它。

在`CREATE TABLE`语句中，表名以双`##`号开头，就可以创建一个全局临时表。

```sql
CREATE TABLE ##TempTable (
   Column1 INT,
   Column2 VARCHAR(50)
);
```

下面的代码创建一个名为`##TempCustomer`的全局临时表。

```sql
CREATE TABLE ##TempCustomer (
   ID int NOT NULL PRIMARY KEY
   FullName VARCHAR(50),
   Email VARCHAR(50)
);
```

然后，就可以使用标准的SQL命令添加或操作临时表中的数据了。

# 如何删除临时表

当所有使用临时表的用户都断开连接时，SQL Server实例会自动删除临时表。

作为最佳实践，应该总是显式的删除你的临时表，以释放`tempdb`内存。

使用`DROP TABLE IF EXISTS`后跟临时表表名的语句，用以删除临时表。

删除`#TempCustomer`表：

```sql
DROP TABLE IF EXISTS #TempCustomer
```

删除`##TempCustomer`表：

```sql
DROP TABLE IF EXISTS ##TempCustomer
```

# SQL临时表的典型用法

当需要存储用以进一步处理的复杂查询的中间结果时，临时表非常有用。

例如，在创建报表时，可能需要创建临时表存储来自多个数据库的查询结果。然后，可以通过对临时表运行查询来生成最终报告。

临时表有用的另一种情况是，当需要一个表的查询结果来运行另一个查询时。你可以将结果存储在临时表中，然后在新查询中引用它。实质上，你将临时表用作工作表或缓冲区表，以保存执行特定任务所需的数据。


