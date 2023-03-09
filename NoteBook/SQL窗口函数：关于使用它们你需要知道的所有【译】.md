**SQL窗口函数：关于使用它们你需要知道的所有【译】**

[toc]

绝对好文：https://blog.csdn.net/kiritobryant/article/details/128022425

https://zhuanlan.zhihu.com/p/92654574?utm_id=0

https://mp.weixin.qq.com/s/yoC3RK_iFRNnAFqh8rzHyA

https://blog.csdn.net/hsabrina/article/details/126157064


[SQL Server下7种“数据分页”方案，全网最全](https://www.cnblogs.com/dotnet-college/archive/2023/01/03/17021701.html)

[SQL使用（一）：如何使用SQL语句去查询第二高的值](https://blog.csdn.net/xfw17397388089/article/details/125742901)

> [SQL Window Functions: All You Need to Know About Using Them](https://www.makeuseof.com/sql-window-functions/)

了解如何使用窗口函数仅通过一个 SQL 查询执行统计分析。

SQL作为DBMS查询语言，其多功能性多年来一直在提高。SQL广泛的实用性和多功能性使其成为每个数据分析师的最爱。

除了SQL的常规函数之外，还有相当多的高级函数。这些函数通常称为窗口函数。如果你正在处理复杂数据并希望执行高级计算，则可以使用它们来充分利用你的数据。

# 窗口函数的重要

SQL 中提供了几个窗口函数，每个窗口函数都可以执行一系列的计算。从创建分区，到对行进行排名或分配行号，这些窗口函数可以执行所有相关的操作。

对特定数据集或行集合应用聚合函数时，窗口函数非常有用。这些函数超越了 GROUP BY 提供的聚合函数。但是，**主要区别在于，与分组功能不同，你的数据不会合并到一行中**。

不能在 WHERE、FROM 和 GROUP BY 语句中使用窗口函数（原则上）。

# 窗口函数的语法

当使用任何窗口函数时，都必须遵循下面默认的语法结构，这样才能使函数正确。如果SQL命令的结构是错误的，将会得到错误并且代码运行失败。

默认的语法：

```sql
SELECT columnname1,
{window_function}(columnname2)
OVER([PARTITION BY columnname1] [ORDER BY columnname3]) AS new_column
FROM table_name;
```

此处

# ROW_NUMBER() 窗口函数

# 显示 依据某个字段排序的行号

```sql
SELECT ROW_NUMBER() OVER(ORDER BY 排序字段) AS RowNum,* FROM 表名
```

## ROW_NUMBER() 窗口函数实现分页

```sql
--pageIndex 表示指定页
--pageSize  表示每页显示的条数
SELECT * FROM
    (SELECT ROW_NUMBER() OVER(ORDER BY 排序字段) AS RowNum,* FROM 表名 ) AS r 
WHERE  RowNum  BETWEEN ((pageIndex-1)*pageSize + 1) AND (pageIndex * PageSize)
```