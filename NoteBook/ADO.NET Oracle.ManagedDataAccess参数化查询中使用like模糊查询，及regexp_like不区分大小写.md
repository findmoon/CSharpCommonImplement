**ADO.NET/Oracle.ManagedDataAccess参数化查询中使用like模糊查询，及Oracle中使用regexp_like正则匹配不区分大小写**

[toc]

默认 Oracle 的 LIKE 子句查询是区分大小写的。

要想在模糊查询（包括非模糊查询）中不区分大小写，可以采取的方式主要要三种：

1. 修改表字段或当前会话、当前查询语句的编码字符集，使用不区分大小写的字符集。
2. 字段和匹配的内容无大小写区别，比如将字段和匹配内容全变为大写或小写。
3. 使用支持不区分小写匹配的语句或函数，比如 `regexp_like` 正则匹配。

其实还有第4种方式，按字段和匹配内容的二进制进行比较，比如 MySQL 中在列名前指定`binary`关键字，不过不知道其他数据库是否提供此方式：

```sql
select * from table t where binary t.colum1 like '%A%';
```

或者，建表时加上 `binary` 标识，或，修改字段定义加上`binary`：

```sql
create table t{
    colum1 varchar(10) binary
}

ALTER TABLE <table_name> MODIFY COLUMN <column_name> VARCHAR(50) BINARY DEFAULT NULL;
```

两者查询结果相同：

select * from table t where binary t.colum1 like '%a%';
select * from table t where binary t.colum1 like '%A%';


COLLATE utf8mb4_unicode_ci

CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci


CREATE TABLE `CronTabs` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Closed` bit(1) NOT NULL,
  `TaskName` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `TaskInfo` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `CronExpression` varchar(120) COLLATE utf8mb4_unicode_ci NOT NULL,
  `EmailDefaultAccepters` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `RemakeEquipmentId` int(11) DEFAULT NULL,
  `Attachs` varchar(200) COLLATE utf8mb4_unicode_ci DEFAULT NULL COMMENT 'JSON格式的文件名数组',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_CronTabs_TaskName` (`TaskName`),
  KEY `IX_CronTabs_RemakeEquipmentId` (`RemakeEquipmentId`),
  CONSTRAINT `FK_CronTabs_RemakeEquipments_RemakeEquipmentId` FOREIGN KEY (`RemakeEquipmentId`) REFERENCES `RemakeEquipments` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci