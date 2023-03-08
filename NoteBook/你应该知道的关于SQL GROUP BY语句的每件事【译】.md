**你应该知道的关于SQL GROUP BY语句的每件事【译】**

[toc]

> [Everything You Need to Know About the SQL GROUP BY Statement](https://www.makeuseof.com/sql-group-by/)

**GROUP BY is a key element of SQL queries. Here's everything you need to know about using the GROUP BY statement.**

关系数据库的大部分功能用于筛选数据和将表连接在一起。这就是为什么我们首先表示这些关系。但现代数据库系统提供了另一种有价值的技术：`grouping`分组。

分组允许从数据库中提取摘要信息，可以组合结果以创建有用的统计数据。分组使得我们不必为常见情况（如多列数字求平均）编写代码，从而使系统更高效。

# GROUP BY 子句做了什么？

顾名思义，`GROUP BY` 将结果分组到一个较小的集合中。结果由分组列的每个不同值对应一行组成。

我们可以通过查看一些示例数据来显示它的用法，这些数据具有多个有共同值的行。

下面是一个非常简单的包含两个表的数据库，表示唱片专辑数据（`albums`）。你可以为所选数据库系统编写基本schema来设置该数据库。

**albums** 表有九行，包含一个主键 id 列，以及用于姓名、艺术家、发行年份和销售的列：

```sh
+----+---------------------------+-----------+--------------+-------+
| id | name | artist_id | release_year | sales |
+----+---------------------------+-----------+--------------+-------+
| 1 | Abbey Road | 1 | 1969 | 14 |
| 2 | The Dark Side of the Moon | 2 | 1973 | 24 |
| 3 | Rumours | 3 | 1977 | 28 |
| 4 | Nevermind | 4 | 1991 | 17 |
| 5 | Animals | 2 | 1977 | 6 |
| 6 | Goodbye Yellow Brick Road | 5 | 1973 | 8 |
| 7 | 21 | 6 | 2011 | 25 |
| 8 | 25 | 6 | 2015 | 22 |
| 9 | Bat Out of Hell | 7 | 1977 | 28 |
+----+---------------------------+-----------+--------------+-------+
```

**artists** 表有七条数据，其结构更简单，只有 id 和 name 列：

```sh
+----+---------------+
| id | name |
+----+---------------+
| 1 | The Beatles |
| 2 | Pink Floyd |
| 3 | Fleetwood Mac |
| 4 | Nirvana |
| 5 | Elton John |
| 6 | Adele |
| 7 | Meat Loaf |
+----+---------------+
```

使用该简单的数据集你可以理解`GROUP BY`的过个方面。当然，实际的数据集会与很多很多行，但是原理都是相同的。

# 单个列分组

让我们查询每个艺术家(artist)有多少专辑(albums)。先看一个获取`artist_id`列的典型的 SELECT 查询：

```sql
SELECT artist_id FROM albums
```

如下，将会返回9行：

```sh
+-----------+
| artist_id |
+-----------+
| 1 |
| 2 |
| 3 |
| 4 |
| 2 |
| 5 |
| 6 |
| 6 |
| 7 |
+-----------+
```

通过 **GROUP BY artist_id** 以 artist 分组这些记过：

```sql
SELECT artist_id FROM albums GROUP BY artist_id
```

得到如下结果：

```sh
+-----------+
| artist_id |
+-----------+
| 1 |
| 2 |
| 3 |
| 4 |
| 5 |
| 6 |
| 7 |
+-----------+
```

结果集中有 7 行，这比 albums 表中的总共 9 行有所减少。每个唯一`artist_id`只有一行。最后，要获取实际的计数，请将 **COUNT(*)** 添加到所选列：

```sql
SELECT artist_id, COUNT(*)
FROM albums
GROUP BY artist_id

+-----------+----------+
| artist_id | COUNT(*) |
+-----------+----------+
| 1 | 1 |
| 2 | 2 |
| 3 | 1 |
| 4 | 1 |
| 5 | 1 |
| 6 | 2 |
| 7 | 1 |
+-----------+----------+
```

对 ID 为 2 和 6 的艺术家分别有两张专辑。

# 聚合函数中如何访问分组数据

你以前可能使用过 `COUNT` 函数，尤其是上面看到的 `COUNT(*)` 形式，它获取集合中的结果数。

可以使用它来获取表中的记录总数：

```sql
SELECT COUNT(*) FROM albums

+----------+
| COUNT(*) |
+----------+
| 9 |
+----------+
```

`COUNT` 是一个聚合函数(`aggregate function`)。**此术语是指将多行中的值转换为单个值的函数**。它们通常与 `GROUP BY` 语句结合使用。

除了计算行数，还可以将聚合函数应用于分组值：

```sql
SELECT artist_id, SUM(sales)
FROM albums
GROUP BY artist_id

+-----------+------------+
| artist_id | SUM(sales) |
+-----------+------------+
| 1 | 14 |
| 2 | 30 |
| 3 | 28 |
| 4 | 17 |
| 5 | 8 |
| 6 | 47 |
| 7 | 28 |
+-----------+------------+
```

上面显示的 artists 2 和 6 的总销售额，使它们多个专辑结合的销售额：

```sql
SELECT artist_id, sales
FROM albums
WHERE artist_id IN (2, 6)
+-----------+-------+
| artist_id | sales |
+-----------+-------+
| 2 | 24 |
| 2 | 6 |
| 6 | 25 |
| 6 | 22 |
+-----------+-------+
```

# 多列分组

也可以分组多个列，仅仅使用逗号分隔，包含多个列或表达式即可。结果将会按照这些列的组合分组。

```sql
SELECT release_year, sales, count(*)
FROM albums
GROUP BY release_year, sales
```

这通常会产生比按单列分组更多的结果：

```sh
+--------------+-------+----------+
| release_year | sales | count(*) |
+--------------+-------+----------+
| 1969 | 14 | 1 |
| 1973 | 24 | 1 |
| 1977 | 28 | 2 |
| 1991 | 17 | 1 |
| 1977 | 6 | 1 |
| 1973 | 8 | 1 |
| 2011 | 25 | 1 |
| 2015 | 22 | 1 |
+--------------+-------+----------+
```

注意，在我们的小示例中，仅仅有两个专辑(album)有相同的发行年份和销售额（28 in 1977）。


# 有用的聚合函数

除了 COUNT 之外，有几个函数可以很好地与 GROUP 配合使用。每个函数依据属于每个分组结果的记录返回一个值。

- `COUNT()` 返回匹配记录的总数。
- `SUM()` 返回给定列中所有值的总和。
- `MIN()` 返回给定列中的最小值。
- `MAX()` 返回给定列中的最大值。
- `AVG()` 返回平均值。它相当于 `SUM() / COUNT()`。

也可以在没有 GROUP 子句时单独使用这些函数。

```sql
SELECT AVG(sales) FROM albums

+------------+
| AVG(sales) |
+------------+
| 19.1111 |
+------------+
```

# 带 WHERE 子句的 GROUP BY

使用 WHERE 子句时，添加`GROUP BY`，会先按条件过滤，在分组结果集：

```sql
SELECT artist_id, COUNT(*)
FROM albums
WHERE release_year > 1990
GROUP BY artist_id

+-----------+----------+
| artist_id | COUNT(*) |
+-----------+----------+
| 4 | 1 |
| 6 | 2 |
+-----------+----------+
```

现在的结果是在1990年之后发行的专辑，按艺术家分组。

还可以将 连接 与 WHERE 子句一起使用，独立于 GROUP BY：

```sql
SELECT r.name, COUNT(*) AS albums
FROM albums l, artists r
WHERE artist_id=r.id
AND release_year > 1990
GROUP BY artist_id


+---------+--------+
| name | albums |
+---------+--------+
| Nirvana | 1 |
| Adele | 2 |
+---------+--------+
```

注意，如果你尝试基于一个聚合列过滤数据，将会得到一个错误！

```sql
SELECT r.name, COUNT(*) AS albums
FROM albums l, artists r
WHERE artist_id=r.id
AND albums > 2
GROUP BY artist_id;
```

错误：

```sh
ERROR 1054 (42S22): Unknown column 'albums' in 'where clause'
```

**基于聚合数据的列在 WHERE 子句中不可用。**

# 使用 HAVING 子句

那么，**如何在分组之后筛选结果集呢？ HAVING 子句用于处理此需求**：

```sql
SELECT r.name, COUNT(*) AS albums
FROM albums l, artists r
WHERE artist_id=r.id
GROUP BY artist_id
HAVING albums > 1;
```

注意，`HAVING` 子句位于 `GROUP BY` 之后。并不是简单地将 WHERE 替换为 HAVING。

结果是：

```sh
+------------+--------+
| name | albums |
+------------+--------+
| Pink Floyd | 2 |
| Adele | 2 |
+------------+--------+
```

仍然可以 **使用 WHERE 条件在分组之前过滤结果，与 HAVING 子句过滤分组之后的结果一起起作用**：

```sql
SELECT r.name, COUNT(*) AS albums
FROM albums l, artists r
WHERE artist_id=r.id
AND release_year > 1990
GROUP BY artist_id
HAVING albums > 1;
```

在我们的数据库中，仅有一个艺术家在1990之后发行超过一个专辑：

```sh
+-------+--------+
| name | albums |
+-------+--------+
| Adele | 2 |
+-------+--------+
```

# GROUP BY 合并结果

`GROUP BY` 语句是 SQL 语言中非常有用的一部分。它可以提供数据的摘要信息，例如内容页面。它是获取大量数据的绝佳替代方案。数据库可以很好地处理这种额外的工作负载，因为它的设计使其成为job的最佳选择。

了解分组以及如何联接多个表后，你将能够使用关系数据库的大部分功能。
