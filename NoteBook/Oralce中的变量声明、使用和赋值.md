Oralce中的变量声明、使用和赋值

# 变量的声明初始化和赋值

1. 声明的变量在 begin 中必须使用，否则保存，至少使用一个，不必全部使用。
2. declare 声明变量的关键字只能有一个，不能每个变量一个关键字
3. 声明变量后必须在 begin...end 中使用。
4. 变量的赋值必须位于 begin..end 内

```sql
DECLARE 
    page int :=1;
    pagesize int :=2;
begin
    pagesize:=2; -- 变量的赋值必须位于begin...end内
    select 1 into page from dual;
end;
```


其他示例

```sql
declare
   l_dept    integer := 20;
   currtime  date := sysdate;
   l_nam     varchar2(20) := to_char(add_months(trunc(sysdate),-12),'yyyymmdd');  -- to_char(sysdate,'MM')-13;

   type num_list is varray(4) of number;
   arr_id num_list := num_list(100,101,123,33,234);

 begin
      l_dept := 30;
      dbms_output.put_line(l_dept);
      dbms_output.put_line(currtime);
      dbms_output.put_line(l_nam);
      dbms_output.put_line(arr_id(1));      
 end;
```

```sql
declare
op nvarchar2(100);
i int ;
j int :=100;
begin
  while j<200 loop
  select nvl(max(id),0) +1 into i from t4;
  insert into t4 values(i,j,'test'||i);
  dbms_output.put_line(i);
  j:=j+1;
  end loop;
end;
```

# 报错 PLS-00428: 在此 SELECT 子句中缺少 INTO 子句

begin end块中只能添加insert、update、delete之类的，不能添加纯粹的select语句。

比如如下这么写就会报错

```sql
begin
    select 1 from dual;
end;
```

# 参考

[Oracle变量的定义、赋值及使用](https://www.cnblogs.com/mq0036/p/4155774.html)