Oracle中的分页查询和分页排序

# Oracle 中的分页查询，及排序后的分页

```sql
SELECT * FROM
    (SELECT temp.*, ROWNUM RN   
    FROM (SELECT BAR,STG,SLT,RMT,RMB FROM T_Table ORDER BY RMT) temp  
    WHERE ROWNUM <=(page*pagesize))  
WHERE RN >(page-1)*pagesize;
```

比如 查询第二页，每页5条 的数据：

```sql
SELECT * FROM
    (SELECT temp.*, ROWNUM RN   
    FROM (SELECT BAR,STG,SLT,RMT,RMB FROM T_Table ORDER BY RMT) temp  
    WHERE ROWNUM <=(2*5))  
WHERE RN >(2-1)*5;
```

如果不进行排序后分页，则可以简化为如下形式：

```sql
SELECT * FROM
    (SELECT T_Table.*, ROWNUM RN   
    FROM T_Table WHERE ROWNUM <=(page*pagesize))
WHERE RN >(page-1)*pagesize;
```

比如 查询第二页，每页5条 的数据：

```sql
SELECT * FROM
    (SELECT T_Table.*, ROWNUM RN   
    FROM T_Table WHERE ROWNUM <=(2*5))  
WHERE RN >(2-1)*5;
```

实在无法知道 Oracle中如何通过变量使用select语句，并把结果显示出来。如下，无法使用result接受查询的变量，会报错 ORA-00932: 数据类型不一致。

也就是，如何接受查询出来的表结构的数据？

> 真是坑，难用的Oracle（或，难用的PL/SQL），真难用！！！

```sql
set serveroutput on --打开 output ，用于使用 DBMS_OUTPUT

DECLARE
    v_sql varchar2(1000); 
    page int :=1;
    pagesize int :=2;
    
    result varchar2(200); 
begin
    -- -- 方式一[不推荐]
    -- v_sql := 'SELECT * FROM (SELECT temp.*, ROWNUM RN FROM (SELECT BAR,STG,SLT,RMT,RMB FROM T_Table ORDER BY RMT) temp WHERE ROWNUM <=('||page||'*'||pagesize||')) WHERE RN >('||page||'-1)*'||pagesize||'';
    -- execute immediate v_sql;

    -- -- 方式二[推荐 - 绑定变量]
    -- execute immediate 'SELECT * FROM
    --     (SELECT temp.*, ROWNUM RN   
    --     FROM (SELECT BAR,STG,SLT,RMT,RMB FROM T_ ORDER BY RMT) temp  
    --     WHERE ROWNUM <=(:page * :pagesize))  
    -- WHERE RN >(:page - 1)* :pagesize' using page,pagesize,page,pagesize;

    -- 如何实现？ 执行SQL接受结果 并输出
    execute immediate 'SELECT * FROM
        (SELECT temp.*, ROWNUM RN   
        FROM (SELECT BAR,STG,SLT,RMT,RMB FROM T_ ORDER BY RMT) temp  
        WHERE ROWNUM <=(:page * :pagesize))  
    WHERE RN >(:page - 1)* :pagesize' into result using page,pagesize,page,pagesize;

    DBMS_OUTPUT.put_line(result); 
end;
```

# 分页查询时获取 ROWID

```sql
SELECT * FROM
    (SELECT temp.*, ROWNUM RN   
    FROM (SELECT ROWID CURRID,BAR,STG,SLT,RMT,RMB FROM T_Table ORDER BY RMT) temp  
    WHERE ROWNUM <=(page*pagesize))  
WHERE RN >(page-1)*pagesize;
```

比如 查询第二页，每页5条 的数据：

```sql
SELECT * FROM
    (SELECT temp.*, ROWNUM RN   
    FROM (SELECT ROWID CURRID,BAR,STG,SLT,RMT,RMB FROM T_Table ORDER BY RMT) temp  
    WHERE ROWNUM <=(2*5))  
WHERE RN >(2-1)*5;
```

# oracle 借助ROWNUM获取第一条数据

```sql
SELECT * FROM tableName where fd_rt = 'A' 
--and rownum=1 
ORDER BY fd_date DESC 
```

涉及排序时，如下方式获取排序后的第一条数据是错误的，因为 rownum 表示的是排序后的行号，与上面分页查询时一样的原理。

```sql
-- 错误，获取排序后的第一条
SELECT * FROM tableName where fd_rt = 'A' 
and rownum=1 
ORDER BY fd_date DESC 
```

正确的处理为：

```sql
SELECT t.* from
(
SELECT * FROM tableName where fd_rt = 'A' ORDER BY fd_date DESC
) t WHERE rownum = 1
```

# 报错 PLS-00428: 在此 SELECT 子句中缺少 INTO 子句

begin end块中只能添加insert、update、delete之类的，不能添加纯粹的select语句。

比如如下这么写就会报错

```sql
begin
    select 1 from dual;
end;
```

# execute immediate sql语句字符串 into 变量

```sql
declare /*存储过程，不需要声明*/ 
   v_sql varchar2(200); 
   v_revc_row varchar2(200); 
begin 
    v_sql:='select v_revc_row  from dfgz_send where revc_id=22150';
 
    --v_revc_row 赋值 
    execute immediate v_sql into v_v_revc_row ;
 
     --打印结果(Oracle自带日志打印功能) 
     DBMS_OUTPUT.put_line(v_revc_row); 
end;
```

# 参考

主要参考自 [Oracle数据库中分页排序](https://blog.csdn.net/github_34013496/article/details/74938788)

其他 [ORACLE分页查询SQL语句(最有效的分页)](https://blog.csdn.net/lchmyhua88/article/details/121076227)、[Oracle中进行分页查询的三种方法](https://blog.csdn.net/qq_42449963/article/details/105922287)

[ORA-01446: 无法使用distinct, group by 等子句从试图中选择rowid或采样](https://blog.csdn.net/u012459917/article/details/17511425)

[oracle 获取第一条数据](https://blog.csdn.net/laybarbarian/article/details/97766751)

[Oracle 实现分页查询](https://blog.csdn.net/weixin_43525116/article/details/85006795) 介绍了如下形式的分页语法，使用到了 start、end，实际测试报错，至少 Oracle 11g 不支持这种语法。

```sql
-- 格式1(推荐)
SELECT * FROM   
(  
SELECT temp.*, ROWNUM RN   
FROM (SELECT * FROM 表名) temp  
WHERE ROWNUM <=end (page*pagesize)  
)  
WHERE RN >start (page-1)*pagesize
```
