**你应该知道的关于SQL子查询的每件事【译】**

[toc]

> 主要转自[Everything You Need to Know About SQL Sub-Queries](https://www.makeuseof.com/everything-you-need-to-know-about-sql-sub-queries/)，后续详细部分进行了非子查询的很多扩展。

**Curious to learn more about how you can use SQL sub-queries? You're in the right place.**

SQL查询是 RDBMS CRUD（创建，读取，更新，删除）的基础。但是，当你的应用程序或企业数据库增长时，对智能查询来说，检索有条件的、特定于要求的数据变得必不可少。

SQL 相对全面，包含许多功能，每个功能都非常适合各种业务用途。其中一个功能包括使用子查询（`sub-queries`）。

为了使代码高效且实用，你可以在 SQL 代码中使用子查询来获取数据、操作现有变量并一次性实现多个目标。

# 什么是 SQL 子查询？

子查询是一个嵌套查询，它用作另一个主查询中的参数。子查询是内部查询(`inner query`)，而主查询是外部查询(`outer query`)。

子查询将数据作为主查询的括号内参数返回，而主查询则进一步检索最终结果。

子查询嵌入在 **Select** 语句或 **Where** 子句中。这样的结构允许子查询表现为描述良好的数据筛选条件。

~~不过，子查询中只能使用 `Group By` 命令，而不能使用 `Order By` 命令（只有在主查询中才允许使用）~~

> ==**实际测试是可以在子查询中使用`Order By`的。**==

通常，每个子查询都包含一个带有 **Select** 子句的列。但是，在某些情况下，主查询具有多个列。一个子查询可以嵌套在另一个子查询中，使其成为嵌套子查询。

子查询的严格要求如下：

```sql
Select column_name from table where condition= 
(SELECT conditional_column FROM table) as alias;
```

举例，假设你有下面的表：

|   ID      |   First_name      |   Second_name     |   Agency_fee      |
|  -----    |   -----------     |   ----------      |   ---------       |
|   1       |   John            |   Wick            |   5000            |
|   2       |   Robert          |   Graham          |   4000            |
|   3       |   Stephen         |   Hicks           |   8000            |
|   4       |   Bob             |   Marley          |   1000            |
|   5       |   Mary            |   Ellen           |   9000            |

> ```sql
> CREATE TABLE Agent_details(
> AgentID INT PRIMARY KEY,FirstName char(255) NOT NULL,LastName char(255) NOT NULL, Agency_Fee int);
> 
> 
> insert into Agent_details (AgentID,FirstName,LastName,Agency_Fee) VALUES 
> 			(1, 'John','Wick', 5000),
> 			(2,'Robert','Graham',4000),
> 			(3,'Stephen','Hicks',8000),
> 			(4,'Bob','Marley',1000),
> 			(5, 'Mary','Ellen', 9000);
> ```

在此表中，若要提取收入高于平均代理费的人员的姓名，可以编写子查询，而不用编写多行代码。

查询类似如下所示：

```sql
Select * from agent_details
where Agency_Fee > (select avg(Agency_Fee) from agent_details);
```

`>`符号之前的命令是外部查询；`>`符号之后的为内部查询。

内查询（子查询）将计算 Agency_Fee 的平均值；外查询将显示所有大于计算的平均值的数据。

# 如何在SQL中使用子查询

有几种不同的可以使用子查询的方式。

## WHERE 子句中的子查询

查询结构：

```sql
select * from table_name 
where column_name = (select column_name from table_name);
```

### 子查询查找第二高的数据

比如，查询第二高的 agency fee 的数据：

```sql
select *, max(Agency_fee)
from agent_details
where Agency_fee < (select max(Agency_fee) from agent_details); 
```

> **注：这种写法，只有在 MySQL/MariaDB 中可用，且在默认为开启严格模式的情况下。**

只查询第二高的 agency fee ：

```sql
select max(Agency_fee) SecondHighestFee
from agent_details
where Agency_fee < (select max(Agency_fee) from agent_details); 
```

### LIMIT 或 TOP..NOT IN 或 OFFSET...FETCH NEXT 查找第二高的数据

```sql
select Agency_fee SecondHighestFee
from agent_details
order by Agency_fee desc
limit 1,1;
```

- SQL Server 中

```sql
-- 极其不推荐
SELECT TOP(1) Agency_fee SecondHighestFee
from agent_details
WHERE Agency_fee NOT IN(
			SELECT TOP(1) Agency_fee
			FROM agent_details
			ORDER by Agency_fee desc
		)
order by Agency_fee desc;

-- 或 SQL Server 2012以上
SELECT *
from agent_details
order by Agency_fee DESC
OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY;
```

## From 子句使用子查询

```sql
select a.* from (  
select agency_fee from agent_details  
where AgentID= 3) as a;
```

## insert into 语句中使用子查询

```sql
insert into table_name   
select * from table_name  
where column_name = conditions;  
```

## Update 中使用子查询

```sql
update table_name   
set column_name = new_value  
where column_name =   
(select column_name from table_name where = );
```

## DELETE 中使用子查询

```sql
delete from table_name where variable/column name =   
(select column_name from table_name where = condition);
``` 

举例：

```sql
Delete from agent_details   
where First_name IN   
(select First_name from agent_details where agency_fee = 9000);   
  
select * from agent_details; 
```

# 补充：尽量不要在 IN 子句中使用子查询

> 添加一个关联表orders：
> 
> ```sql
> CREATE TABLE orders(
> 	Id INT PRIMARY KEY auto_increment, -- 自增id，SQL Server 为 IDENTITY,
> 	agentId INT,
> 	amount DECIMAL,
> 	status VARCHAR(10),
> 	CONSTRAINT FK_orders_Agent_details FOREIGN KEY(agentId) REFERENCES Agent_details (AgentID)
> );
> 
> INSERT INTO orders (agentId,amount,status) VALUES
> 				(1,300,'Ok'),
> 				(2,500,'NO'),
> 				(3,600,'YES'),
> 				(4,300,'Ok'),
> 				(1,270,'Ok'),
> 				(3,500,'Ok');
> ```

通常，应该尽量避免在 SQL 查询中使用子查询，例如：

```sql
SELECT FirstName,LastName FROM Agent_details WHERE AgentID IN (SELECT agentId FROM orders WHERE status = 'OK' AND amount <= 500);
```

上面的查询非常低效、使用大量资源，并应该尽可能避免使用它。作为替代方法，可以使用合适的 join 表连接。

例如，上面的查询可以使用 `INNER JOIN`、`LEFT JOIN` 重写为：

```sql
SELECT DISTINCT a.FirstName, a.LastName FROM Agent_details a JOIN orders o ON o.agentId = a.AgentID WHERE o.status = 'OK' AND o.amount <= 500;

-- 或

SELECT DISTINCT a.FirstName, a.LastName FROM Agent_details a LEFT JOIN orders o ON o.agentId = a.AgentID WHERE o.status = 'OK' AND o.amount <= 500;
```

考虑可能的重复数据，保持结果一致，可以添加 `DISTINCT` 去重。

```sh
FirstName       LastName
John            Wick
Stephen         Hicks
Bob             Marley
```

# 附：关于

# 附：【WHERE 子句中的子查询】开始的原文

### Sub-Queries With Where Clause

One of the most basic structures of a sub-query in SQL is within the Where clause. It's the simplest way to define what you are searching for. The select statement returns values as per the sub-query condition(s) and uses it as a parameter for the main query.

Query structure:

```sql
select * from table_name  
  
where column_name = (select column_name from table_name);
``` 

Let's explain this with an example.

Suppose you want to find the second-highest agency fee from the agency\_details table. To do so, there are alternate functions within SQL; nonetheless, the best method is to use a sub-query.

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT3'); });

Here's how you can define the sub-query:

```sql
select *, max(Agency_fee)  
  
from agent_details   
  
where Agency_fee < (select max(Agency_fee) from agent_details); 
``` 

The resulting statement will show you **8000**, which is the second-highest fee in the given table. When the query runs, the sub-query calculates the maximum value from the list of fee. The highest fee amount (**9000**) is stored in memory.

  

Once this part is computed, the second part of the query is calculated, which finds the second-highest fee from the table (since the **<** sign is used). The end result is **8000**, which is the second-highest fee in the table.

### Sub-Queries Within the From Clause

Another variation within sub-queries is passing the condition in the **from** clause. As a similar concept, the inner query is processed first, and the outer query is processed afterwards. The inner query will filter on the data and show results where ID = 3.

Here's the query for reference:

```sql
select a.* from (  
select agency_fee from agent_details  
where ID= 3) as a;
```


This is a very basic structure; however, the more complex your data tables, you will get more rows of data, which match your conditions.

### Using Sub-Queries With Insert Into Statement

If you want to update an existing table with some new data rows, you can use the **Insert Into** statement. A sub-query can prove to be quite beneficial, if you want to add values based on a specific condition(s).

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT4'); });

Query structure:

```sql
insert into table_name   
select * from table_name  
where column_name = conditions;  
  
select * from table_name;
``` 

Here's an example on how you can use the insert into statement with the sub-query:

```sql
insert into agent_details  
  
select * from agent_details  
  
where agency_fee in (1000, 5000);  
  
select * from agent_details;
```

  

Once the query runs, the values matching the condition will be inserted into the existing table again. The **select \*** reference picks up all the columns together, and inserts it into the agent\_details table as it is. The **in** statement is used to define multiple filter conditions at once.

### Using Sub-Queries With Update Statement

There are situations wherein you want to update the underlying tables while running the queries. To do so, you can use the **update** statement along with the querying commands.

This is how you will write the sub-query to update the information in the table in one instance:

```sql
update table_name   
set column_name = new_value  
where column_name =   
(select column_name from table_name where = );
``` 

Here's an example demonstrating the use of the update statement:

```sql
UPDATE agent_details   
SET agency_fee = 35000  
WHERE agency_fee =   
(SELECT agency_fee FROM agent_details WHERE First_name='John');   
  
select * from agent_details;
``` 

The sub-query will filter on the column agency\_fee and single out the row(s) where First\_Name matches **John**. The outer query is executed next, wherein the agency fee is updated to 35000 for John Wick.

  

You can pass a **select \*** statement to check the final results; you will notice the agency fee for John Wick is updated to 35000, as there is only instance matching the conditions defined in the query.

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT5'); });

### Using Sub-Queries With Delete Statement

Just like the update statement, wherein you are updating the rows of data within an existing table, the **delete** statement deletes row(s) of data based on a condition.

The delete statement structure is:

```sql
delete from table_name where variable/column name =   
(select column_name from table_name where = condition);
``` 

Here's an example:

```sql
Delete from agent_details   
where First_name IN   
(select First_name from agent_details where agency_fee = 9000);   
  
select * from agent_details; 
``` 

  

## Using Sub-Queries Within SQL

Sub-queries are an excellent feature within SQL, which can save you from writing endless lines of unnecessary code. When you are able to use the basic functionalities of sub-queries to do your bidding, you would never want to worry about going into the complexities of SQL coding.

It's always best to enhance your existing SQL knowledge to ensure you are always on top of your game. Rest assured, SQL cheat sheets can give you a good idea on how to brush up on your basics in a single glance.

