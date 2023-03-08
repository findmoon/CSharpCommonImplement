**编写mySQL数据库模式的初学者指南【译】**

[toc]

只需一个文本编辑器和基本结构大纲或“模式”，即可创建你自己的 mySQL 数据库。

> basic structure outline, or "schema."

![](img/20230308103949.png)

在开发软件项目时，最重要、最基础和最内在的方面之一是结构合理的数据库模式。这相当于在建造房屋时，你需要确保正确铺设地基，否则建造优质房屋的机会会大大降低。

令人惊讶的是，学习如何编写架构良好的数据库模式的各个方面，比人们想象的要容易。

# CREATE TABLE 语法

作为开始，打开你最喜欢的文本编辑器。创建一个数据库模式，除了文本文件之外不需要任何其他东西。

一个数据库由多个表组成、每个表有多个列组成。`CREATE TABLE`语法用于创建单个表。

基本的示例如下：

```sql
CREATE TABLE users (
  id INT NOT NULL,
  is_active TINY INT NOT NULL,
  full_name VAR CHAR(100) NOT NULL,
  email VARCHAR(100) NOT NULL
);
```

此处创建由四个列组成的`users`数据库表。

这是一个相当简单的 SQL 语句，以 `CREATE TABLE` 开头，后跟数据库表的名称，然后在括号内用逗号分隔的列。

# 使用正确的列类型

如上所示，表将包含的列用逗号分隔。每个列定义由三个相同的部分组成：

```sql
COL_NAME TYPE [OPTIONS]
```

列名，后跟列类型，然后是其它可选参数。稍后将介绍可选参数，但重点介绍列类型。

下面列出了最常用的可用列类型：

<table border="1" cellpadding="1" cellspacing="1"><thead style="font-weight: bold;font-size:18px"><tr><th><p>Type</p> </th><th><p>Description</p> </th> </tr></thead><tbody><tr><td><p>INT</p> </td><td><p>Integer, supports values up to (+/-) 2.14 billion. Most communly used integer type, but the following with respective ranges are also available:</p><ul> <li> TINYINT - 128. Great for booleans (1 or 0). </li> <li> SMALLINT - 32k </li> <li> MEDIUMINT - 3.8 million </li> <li> BIGINT - 9.3 quintillion. </li><p> </p> </ul><p> </p> </td> </tr><tr><td><p>VARCHAR(xxx)</p> </td><td><p>Variable length string that supports virtually all non-binary data. The <strong>xxx</strong> within parentheses is the maximum length the column can hold.</p> </td> </tr><tr><td><p>DECIMAL(x,y)</p> </td><td><p>Stores decimal / float values, such as prices or any numeric values that aren't whole numbers. The numbers within the parentheses of <strong>(x,y)</strong> define the maximum length of the column, and the number of decimal points to store. For example, <strong>DECIMAL(8,2)</strong> would allow numbers to be maximum six digits in length plus formatted to two decimal points.</p> </td> </tr><tr><td><p>DATETIME / TIMESTAMP</p> </td><td><p>Both hold the date and time in YYY-MM-DD HH:II:SS format. You should use TIMESTAMP for all row meta data (ie. created at, lst updated, etc.) and DATETIME for all other dates (eg. date of birth, etc.).</p> </td> </tr><tr><td><p>DATE</p> </td><td><p>Similar to DATETIME except it only stores the date in YYY-MM-DD format, and does not store the time.</p> </td> </tr><tr><td><p>TEXT</p> </td><td><p>Large blocks of text, can store up to 65k characters. The following are also available with their respective ranges:</p><ul> <li> MEDIUMTEXT - 16.7 million characters. </li> <li> LONGTEXT - 4.2 billion characters. </li><p> </p> </ul><p> </p> </td> </tr><tr><td><p>BLOB</p> </td><td><p>Used to store binary data such as images. Supports maximum size of 64kb, and the following with respective size limits are also supported:</p><ul> <li> TINYBLOG - 255 bytes </li> <li> MEDIUMBLOB - 16MB </li> <li> LONGBLOG - 4GB </li><p> </p> </ul><p> </p> </td> </tr><tr><td><p>ENUM(opt1, opt2, opt3...)</p> </td><td><p>Only allows the value to be one of the pre-defined values specified within the parentheses. Good for things such as a status column (eg. active, inactive, pending).</p> </td> </tr></tbody> </table>

> **ENUM(opt1,opt2...)类型最值得关注。**

上述列类型基本是编写构造良好的 mySQL 数据库模式所需的全部内容。

# 定义列可选参数

定义列时你可以指定多个可选的变量。

下面是 `CREATE TABLE` 语句的另外示例：

```sql
CREATE TABLE users (
  id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
  username VARCHAR(100) NOT NULL UNIQUE,
  status ENUM('active','inactive') NOT NULL DEFAULT 'active',
  balance DECIMAL(8,2) NOT NULL DEFAULT 0,
  date_of_birth DATETIME,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

以上可能看起来有点令人生畏，但不要担心，这很简单。细分一下，上述声明表示的含义如下：

- **应始终对所有可能的列使用 `NOT NULL`，以帮助提高表的速度和性能。** 它表示在插入行或修改列的值时，列不能留空/null。

- **始终尽量保持列的大小尽可能小，因为这有助于提高速度和性能。**

- id 列是一个整数，也是主键（在表中是唯一的），并且会在每一条记录插入时自增。

主键id列应该用于你创建的所有表中，这样，你可以很容易的引用表中的任何单个行。

- status 列是一个 ENUM 类型，取值为"active"或"inactive"。如果没有指定值，新行将使用默认值"active"。

- balance 列默认值为0，是一个格式为两个小数点的金额。

- date_of_birth 列是简单的 DATE 类型，但是在创建时如果不知道出生日期，也允许空值null。

- created_at 是一个 TIMESTAMP 类型，默认是数据行插入的当前时间。

以上是一个结构良好的数据库表的示例，应该用作以后的示例基础。

# 使用外键约束将表连接在一起

使用关系数据库（如mySQL）的最大优势之一是它对外键约束和级联的出色支持。当你通过一个列将两个表链接在一起，形成父子关系时，如果删除父行，也会自动删除必要的子行或者阻止删除父行；修改时同理。

此处是一个示例：

```sql
CREATE TABLE users (
  id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
  username VARCHAR(100) NOT NULL UNIQUE,
  full_name VARCHAR(100) NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
) engine=InnoDB;
 
CREATE TABLE orders (
  id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
  userid INT NOT NULL,
  amount DECIMAL(8,2) NOT NULL,
  product_name VARCHAR(200) NOT NULL,
  FOREIGN KEY (userid) REFERENCES users (id) ON DELETE CASCADE
) engine=InnoDB;
```

> `ON DELETE CASCADE` 表示级联删除。通常不应该设置级联删除，而是阻止删除。

你可能注意到 `FOREIGN KEY` 子句是最后一行。此行声明当前表包含的子行 `userid` 列链接到其父行（即 `users` 表的行）的`id` 列，并且启用级联删除。这意味着，每当从 users 表中删除一行时，mySQL 都会自动从 orders 表中删除所有相应的行，帮助确保数据库中的结构完整性。

> structural integrity

另请注意上述语句末尾的 `engine=InnoDB`。虽然`InnoDB`现在是默认的mySQL表类型，但它并非总是如此，所以为了安全起见，应该添加它，因为，**级联仅适用于`InnoDB`表**。

# 充满信心的设计 - Design With Confidence

你现在正在构建可靠、结构良好的 mySQL 数据库模式。使用上述知识，你现在可以编写组织良好的架构，以提供性能和结构完整性。

schema 就绪后，就可以使用基本 SQL 命令了。
