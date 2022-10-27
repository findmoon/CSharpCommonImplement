SQLServer判断数据库、表、列是否存在


## COL_LENGTH 判断列是否存在【推荐】

不存在返回null。

```sql
select COL_LENGTH('shop.dbo.Product', 'sale_price');
```

## COLUMNPROPERTY 查询列属性，及问题

```sql
SELECT COLUMNPROPERTY( OBJECT_ID('Person.Person'),'LastName','PRECISION')
```

通过判断列的类型长度是否为null，可以判断是否存在列。

但是，**有很很严重的问题，`COLUMNPROPERTY`查询其他表中的列属性。只能判断当前连接的数据库中表的列的属性、是否存在。**

```sql
select OBJECT_ID('..Product');
select OBJECT_ID('shop..Product');

SELECT COLUMNPROPERTY( OBJECT_ID('Person.Person'),'LastName','PRECISION')

SELECT case when COLUMNPROPERTY(OBJECT_ID('{dbName}.{schema}.{tableName}'),'{columnName}','PRECISION') IS NULL THEN 0 ELSE 1 END;

SELECT case when COLUMNPROPERTY(OBJECT_ID('t.dbo.Product'),'sale_price','PRECISION') IS NULL THEN 0 ELSE 1 END;

select OBJECT_ID('shop.dbo.Product');
SELECT COLUMNPROPERTY(OBJECT_ID('shop.dbo.Product'),'sale_price','PRECISION');
```