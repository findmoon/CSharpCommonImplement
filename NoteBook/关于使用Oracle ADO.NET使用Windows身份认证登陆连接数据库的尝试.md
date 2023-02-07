**关于使用Oracle ADO.NET使用Windows身份认证登陆连接数据库的尝试**

> 最终算是失败的，只是差一点成功。感觉最后再测试下应该能成功（找个低版本的Oracle，如11g。或，Windows低版本）。只是由于各方面的原因、时间、精力等，暂时放弃了。本来这种应用场景，就只能在Windows的本地上使用。最后借助客户端的cmd执行命令实现本地登陆和执行SQL语句。
>
> 至少，从官方文档上看，ADO.NET 2.0 的版本是可以实现Windows认证登陆的。

[ODP.NET Windows\passthru Authentication without oracle client installed](https://forums.oracle.com/ords/apexds/post/odp-net-windows-passthru-authentication-without-oracle-clie-4162)

[Oracle Data Provider for .NET, Managed Driver Configuration](https://docs.oracle.com/en/database/oracle/oracle-database/19/odpnt/InstallManagedConfig.html#GUID-29A85CBD-948D-4C9F-A89D-A96A99EFF2D7)

[Using Windows Native Authentication (NTS)](https://docs.oracle.com/cd/E63277_01/win.121/e63268/featConnecting.htm#CJAEGCDF)



[Oracle® Data Provider for .NET Developer's Guide - ADO.NET 2.0 Features](https://docs.oracle.com/cd/E20434_01/doc/win.112/e23174/featADO20.htm#BABDGGGE)

https://docs.oracle.com/cd/E20434_01/welcome.html

[Additional Oracle .NET Information Resources](https://www.oracle.com/database/technologies/appdev/dotnet-resources.html)

[Authenticating Database Users with Windows](https://docs.oracle.com/cd/E11882_01/win.112/e10845/authen.htm#NTQRF120)

https://docs.oracle.com/cd/E11882_01/win.112/e10845/toc.htm

[3.3 Connecting to Oracle Database](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/18.3/odpnt/featConnecting.html)

https://docs.oracle.com/en/database/oracle/oracle-data-access-components/18.3/odpnt/featConnecting.html#GUID-BCF2F215-C25F-403C-8D18-B03A69BC7104

[3.3.8 Using Windows Native Authentication (NTS)](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/18.3/odpnt/featConnecting.html#GUID-51D1ADDB-D6B2-44A8-858F-336BE63C9064)

https://stackoverflow.com/questions/65633537/oracle-manageddataaccess-client-cannot-connect-using-windows-authentication

https://stackoverflow.com/questions/20348529/entity-framework-datacontext-to-oracle-database

[.Net ManagedDataAccess fails to connect using Windows Native Authentication](https://forums.oracle.com/ords/apexds/post/net-manageddataaccess-fails-to-connect-using-windows-nativ-5867)

[Using Windows user authentication using Oracles OracleConnection](https://www.connectionstrings.com/oracle-data-provider-for-net-odp-net/using-windows-user-authentication/)


