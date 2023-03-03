**SQL Server中获得EXEC后面的sql语句和存储过程的返回值，获取存储过程或函数中SELECT查询结果的方法**

[toc]

> 主要转自 [SQL中获得EXEC后面的sql语句或者存储过程的返回值的方法](https://blog.csdn.net/j_jake/article/details/1691738) 和 [SQLSERVER中 获取存储过程里 SELECT出来的结果](https://blog.csdn.net/u013986317/article/details/102458918)

前言：在数据库程序开发的过程中,我们经常会碰到利用EXEC来执行一段需要返回某些值的sql语句（通常是构造动态sql语句时使用）；

或者在一个存储过程中利用EXEC调用另一个有返回值的存储过程（必须获得返回值）；

再或者，存储过程中通过SELECT语句查询出结果显示，如果想获取查询的结果集。

那么，如何获取返回值 或者 查询结果 呢？

# EXEC执行sql语句的情况

```sql
declare   @rsql   varchar ( 250 )
declare   @csql   varchar ( 300 )
declare   @rc   nvarchar ( 500 )
declare   @cstucount   int
declare   @ccount   int
set   @rsql = ' (select Classroom_id from EA_RoomTime where zc= ' + @zc + '  and xq= ' + @xq + '  and T ' + @time + ' = '' 否 '' ) and ClassroomType= '' 1 '''
-- exec(@rsql)
set   @csql = ' select @a=sum(teststucount),@b=sum(classcount) from EA_ClassRoom where classroom_id in  '
set   @rc = @csql + @rsql
exec  sp_executesql  @rc ,N ' @a int output,@b int output ' , @cstucount  output, @ccount  output -- 将exec的结果放入变量中的做法
-- select @csql+@rsql
-- select @cstucount
```

上面的@rc这个sql语句的功能是找出特定时间段里所有有空的教室数量以及这些教室所能容纳的学生人数,因为涉及到动态的sql语句（@csql这句里条件中有一个列名是动态变化的）的构造,所以要放在exec里执行，但是同时我又要返回2个结果，所以执行时的代码为：

```sql
exec  sp_executesql  @rc ,N ' @a int output,@b int output ' , @cstucount  output, @ccount  output -- 将exec的结果放入变量中的做法
```

这样就将返回值放到了，@cstucount，@ccount两个变量中，得到了我们想要的结果。

# exec执行带返回值的存储过程的情况  
  
我们来看一个简单的存储过程：

```sql
create   procedure  ProTest
(
        @name   varchar ( 10 ),
        @money   int  output
)
as
begin
        if ( @name = ' 1 ' )
                set   @money = 1000
        else
                set   @money = 2000
end
```
  
这个只是一个简单的示例，这个存储过程返回的是@money 这个参数的值，那么当我们在另外一个存储过程中调用此存储过程的时候如何获取这个参数呢，方法如下：  
  
```sql
declare   @m   int   -- -用来接收返回值的变量
exec  ProTest  @name = ' 1 ' , @money = @m  output  -- 一定要注名是output
```

  
就这么简单，我们就获得了返回值，然后就可以利用它了

# 获取存储过程里 SELECT出来的结果

**通过 `INSERT INTO` 语句，可以获取 EXECUTE 执行时，存储过程或函数内SELECT查询出来的结果。**

```sql
-- 将返回结果插入表变量中
INSERT INTO @TempTable EXECUTE [dbo].[HaveSelectDataProcedure] @ParamID = 1;
 
SELECT * FROM @TempTable;
```


创建一个存储过程如下，存储过程中select出集合：

```sql
CREATE PROCEDURE dbo.HaveSelectDataProcedure
@ParamID int
AS
BEGIN
	DECLARE @TempTable Table
	(
		ID int,
		SomeValue NVARCHAR(50)
	)
	INSERT INTO @TempTable
	VALUES(@ParamID, 'ceshi1')
 
	INSERT INTO @TempTable
	VALUES(@ParamID, 'ceshi2')
 
	SELECT * FROM @TempTable
END
```

接受存储过程中返回的表结果（SELECT查询的结果），可以这样写：

```sql
DECLARE @ParamID int --存储过程参数
 
DECLARE @TempTable Table --定义一个与存储过程返回结果结构一致的表变量，用于接受结果
	(
		ID int,
		SomeValue NVARCHAR(50)
	)

--将返回结果插入表变量中
INSERT INTO @TempTable EXECUTE [dbo].[HaveSelectDataProcedure] @ParamID = 1
 
SELECT * FROM @TempTable
GO
```

注意： `Insert into` 语句不能嵌套用于存储过程，即 存储过程A内部中使用了如上的`insert into`来获取另一个存储过程B返回的集合 ，那么不能再使用 `insert into` 来获取存储过程A返回的集合。

