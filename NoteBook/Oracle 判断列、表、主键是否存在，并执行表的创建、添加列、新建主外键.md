**Oracle 判断列、表、主键是否存在，并执行表的创建、添加列、新建主外键【转】**

> 原文 [Oracle 判断列、表、主键是否存在](https://blog.csdn.net/infernalboy/article/details/87720610)

# 判断表是否存在，不存在则执行创建

```sql
declare 
    n_count number;   --声明变量存储要查询的表是否存在  
begin   
     select count(1) into n_count  from user_tables t where t.table_name = upper('表名'); --从系统表中查询当表是否存在  
     if n_count  = 0 then --如果不存在，使用快速执行语句创建新表  
         execute immediate  
         'create table 表名 --创建测试表  
         (ID number not null,Name = varchar2(20) not null)';  
     end if;  
end;
```

# 判断列是否存在，不存在则新增列

```sql
declare 
    n_count number;   --声明变量存储要查询的表中的列是否存在  
begin   
        --从系统表中查询表中的列是否存在  
        select count(1) into n_count from user_tab_columns t where t.table_name = upper('表名')  and t.column_name = upper('列名');       
        --如果不存在，使用快速执行语句添加列  
        if n_count = 0 then   
           execute immediate  
           'alter table 表名 add 列名 number not null';  
        end if;  
end;
```

# 判断主键是否存在，不存在则添加主键

```sql
declare 
    n_count number;   --声明变量存储要查询的表中的主键是否存在  
begin   
        --从系统表中查询表是否存在主键（因一个表只可能有一个主键，所以只需判断约束类型即可）  
        select count(1) into n_count from user_constraints t where t.table_name = upper('表名') and t.constraint_type = 'P';       
        --如果不存在，使用快速执行语句添加主键约束  
        if n_count  = 0 then   
        execute immediate  
        'alter table 表名 add constraint PK_表名_ID primary key(id)';  
        end if;  
end;
```

# 判断是否存在外键，不存在则新建外键

```sql
declare 
    n_count number;   --声明变量存储要查询的表中的外键是否存在  
begin    
        select count(1) into n_count from user_constraints t where t.table_name = upper('表名') and t.constraint_type = 'R' and t.constraint_name = '外键约束名称';       
        --如果不存在，使用快速执行语句添加外键约束  
        if n_count = 0 then   
           execute immediate  
           'alter table 表名 add constraint 外键约束名称 foreign key references 外键引用表(列)';  
        end if;  
end;
```

# 另：Oracle增加字段

> [Oracle增加字段](https://blog.csdn.net/qq_37834380/article/details/106137878)

```sql
-- 添加字段
ALTER TABLE 表名
ADD 字段1 类型(字段长度)
ADD 字段2 类型(字段长度);

-- 添加字段的注释
COMMENT ON COLUMN 表名.字段1 IS '字段1的名称';
COMMENT ON COLUMN 表名.字段2 IS '字段2的名称';
```

```sql
ALTER TABLE BF_PROJECT ADD F_ID VARCHAR2(32) 
ADD F_CITY VARCHAR2(32);
COMMENT ON COLUMN BF_PROJECT.F_ID IS '项目id';
COMMENT ON COLUMN BF_PROJECT.F_CITY IS '项目城市';
```