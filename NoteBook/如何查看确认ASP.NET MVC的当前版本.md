**如何查看确认ASP.NET MVC的当前版本？**

[toc]

# 运行时代码通过反射获取

```C#
typeof (Controller).Assembly.GetName().Version
```

# MVC项目中查看

在解决方案资源管理器中打开`packages.config`。

然后查找`ASP.NET MVC`包ID，如下所示：

```xml
  <package id="Microsoft.AspNet.Mvc" version="5.2.9" targetFramework="net462" />
  <package id="Microsoft.AspNet.Mvc.zh-Hans" version="5.2.9" targetFramework="net462" />
```

# Mvc.Diagnostics Nuget 包

该包可以生成一个查看所有程序集信息的比较格式化一些的页面。


# 参考出处

https://stackoverflow.com/questions/3008704