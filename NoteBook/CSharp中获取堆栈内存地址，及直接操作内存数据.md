**C#中获取变量的堆栈内存地址，及直接操作内存数据**

[toc]

# C#获取变量的内存地址

C#中的变量分为两种：**值类型与引用类型，值类型的数据位于栈中，引用类型的数据位于堆中，栈中存放的是该数据所在堆的内存地址。**

- 值类型的变量，通过`&`取址符号获取内存地址

```C#
```

# C#获取对象的内存地址

c#是不能获取内存地址的，就算获取到了也没什么意义。

在unsafe的情况下，可以把内存pin在一定的地址防止回收，只有这样获取的地址才有意义。

GCHandle可以转化成IntPtr指针。

```C#
using System.Runtime.InteropServices;
 using System;
 namespace ConsolePrototype
 {
     public class A
     {
     }
 
     class Program
     {
         static void Main(string[] args)
         {
 
             A a = new A();
             GCHandle hander = GCHandle.Alloc(a);
             var pin = GCHandle.ToIntPtr(hander);
 
             Console.WriteLine(pin);
         }
     }
 }
```

# 参考

- [c#获取类的内存地址](https://q.cnblogs.com/q/42721/)