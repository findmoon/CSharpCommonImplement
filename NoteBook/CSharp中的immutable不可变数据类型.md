**C#中的immutable不可变数据类型**

[toc]

# 不可变对象

**不可变(immutable)： 即对象一旦被创建初始化后，它们的值就不能被改变，之后的每次改变都会产生一个新对象。**

比如：

C#中的string是不可变的，`Substring`、`+`等操作返回的是一个新字符串值，而原字符串在共享域中是不变的。

```C#
var str="mushroomsir";
var str1 = str.Substring(0, 6);
var str2 = str+"拼接";

Console.WriteLine(str);
Console.WriteLine(str1);
Console.WriteLine(str2);

// 输出：
// mushroomsir
// mushro
// mushroomsir拼接
```

正是应为 string 类型的不可变性，每次变更值都会创建开辟新的内存空间，这对于频繁操作字符串的场景，将非常消耗性能。也因此，推荐使用另外一个字符串操作类 `StringBuilder`，它是可变的，用于对字符串的变更操作。

C#中的int也是不可变的，将一个值分配给int类型变量时，其所在内存的值不可以被修改，重新赋值和修改值是在栈中重新开辟空间完成操作和赋值，而不是使用的原来的内存及值。

