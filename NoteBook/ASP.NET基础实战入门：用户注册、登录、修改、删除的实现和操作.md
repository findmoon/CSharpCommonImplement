**ASP.NET基础实战入门：用户注册、登录、修改、删除的实现和操作**

[toc]

想要实现一下具有基础功能的 `ASP.NET` 项目，看到b站正好有一个课程 [ASP.NET入门实战，用户注册、登录、修改、删除全过程操作](https://www.bilibili.com/video/BV1rz4y1R7Xt)，实现了最简单的用户登陆、注册、修改等功能，可以对 `ASP.NET MVC` 有一个完整的了解，以及作为后续项目的小模板。

> 最好再加上角色管理、Auth 2.0认证。

此处记录主要实现过程，基于 SQL Server 数据库 和 ASP.NET

# 创建项目和初始化数据库

数据库使用SQL Server，下面是创建 DB 和 Table 的脚本（注意存放DB的路径要存在）：

```sql
CREATE DATABASE ASPNETSimple
 ON  PRIMARY 
( 
NAME = N'ASPNETSimple', FILENAME = N'D:\SoftWareDevelope\CSharp\DB\ASPNETSimple.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB 
)
 LOG ON 
( NAME = N'ASPNETSimple_log', FILENAME = N'D:\SoftWareDevelope\CSharp\DB\ASPNETSimple_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10% )

GO

USE ASPNETSimple;

GO

create table Users(
	Id nvarchar(50) NOT NULL PRIMARY KEY, -- 主键 id guid
	UserName nvarchar(50) NOT NULL UNIQUE, -- 用户名
	Password nvarchar(50) NOT NULL UNIQUE, -- 密码
	Name nvarchar(10), -- 姓名
	Gender	bit NOT NULL, -- 性别 0-男 1-女 
	Address nvarchar(50), -- 地址
	Email varchar(50), -- 邮箱
	EmailConfirmed bit, -- 邮箱确认
	PhoneNumber varchar(18), -- 手机号
	PhoneNumberConfirmed bit, -- 手机号确认
	UserState smallint, -- 用户状态 未登录、已登录、禁用、锁定 [枚举数字表示]
	AccessFailedCount smallint, -- 登陆失败次数
	AddTime datetime -- 注册时间
);

GO
```

> 注：SQL Server 中添加类的描述说明：
>
> `EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id，主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Id';`

打开 Visual Studio 2022，创建 `ASP.NET MVC` 项目 `ASPNETSimple`：

