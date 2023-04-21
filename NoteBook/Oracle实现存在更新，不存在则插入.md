**Oracle实现存在更新，不存在则插入**

[toc]

# 查询是否存在，并判断更新或插入

```sql
select count(1) from table_name where ****;
```

如果>=1则update table_name
如果==0则insert table_name

# MERGE INTO 语法

使用 MERGE INTO 实现 存在更新，不存在则插入 表数据。需要借助 dual 获取要比较的变量值：

```sql
merge into OPTIONS opt1
using (SELECT '小明' as "Option" FROM dual) opt2
on (opt1."Option"=opt2."Option")
when matched then
  update set opt1."Value" = '11'
when not matched then
  insert ("Option", "Value") values ('小明', '10');
```

MERGE INTO 的语法如下：

```sql
MERGE INTO table_name alias1 
USING (table|view|sub_query) alias2
 (join condition) 
WHEN MATCHED THEN 
UPDATE table_name 
SET col1 = col_val1, 
       col2 = col_val2 
WHEN NOT MATCHED THEN 
INSERT (column_list) VALUES (column_values);
```

如下的更新形式无效，即 using 中也使用要更新的表，不会执行更新或插入，提示合并行数为0或受影响行数为0：

```sql
merge into OPTIONS opt1
using (SELECT "Option" FROM OPTIONS WHERE "Option"='小明') opt2
on (opt1."Option"=opt2."Option")
when matched then
  update set opt1."Value" = '11'
when not matched then
  insert ("Option", "Value") values ('小明', '10');
```

> 由于列名 Option、Value 为 Oracle 中的保留关键字，所以使用`""`处理。否则报错。

# 参考

- [oracle实现添加数据时如果数据存在就更新，如果不存在就插入（merge into 的用法）](https://blog.csdn.net/yang5726685/article/details/92404719)