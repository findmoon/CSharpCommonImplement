**C#中获取变量的堆栈内存地址，GCHandle介绍和管理释放内存，及直接操作内存数据**

[toc]

# C#获取变量的内存地址

> `&`取址符号，获取一个变量的内存地址，或获取一个变量的指针。

C#中的变量分为两种：**值类型与引用类型，值类型的数据位于栈中；引用类型的实际数据位于堆中，栈中存放的是该数据所在堆的内存地址。**

- 值类型的变量，通过`&`取址符号获取内存地址

```C#
unsafe
{
    int myInt;
    int* myIntPtr = &myInt;
}
```

- 引用类型的变量

引用类型的变量获取其内存地址，可以有两种方式，一种是通过其变量名，因为其变量在栈中存放的就是对象的内存地址；另一种是通过其内存中第一个数据元素或成员的内存地址，它就是整个对象所在内存的首地址。

```C#
int[] a = new int[5] { 10, 20, 30, 40, 50 };
unsafe
{
    // 获取数组a第一个元素的地址，数组指针
    fixed (int* p = &a[0])
    {
    }
    // 获取数组a变量的栈数据，数组指针
    fixed (int* p = a)
    {
    }
}
```

这种方式，对于 Class 等对象同样适用，可以获取第一个成员的内存地址 或 变量名中的地址。

> 以上，需要“允许不安全代码”。

# 不启用“允许不安全代码”的情况下获取内存地址【GCHandle】

就像之前说的，因为自动GC的作用，在不pin住对象的情况下，获取内存地址几乎没有任何意义，仅作查看使用。

`GCHandleType`需要使用`WeakTrackResurrection`或`Weak`。

通过`GCHandle.ToIntPtr` 获取 IntPtr 类型的内存地址，这种情况下不用启用“允许不安全代码”（没涉及到操作或使用指针）

> **指针和固定大小缓冲区只能在 不安全上下文 中使用**。

```C#
// 获取引用类型的内存地址方法
public static string getMemory(object obj)
{
    // GCHandle.Alloc(obj, GCHandleType.Pinned) // System.ArgumentException:“Object 包含非基元或非直接复制到本机结构中的数据。”
    GCHandle handle = GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
    IntPtr addr = GCHandle.ToIntPtr(handle);
    return $"0x{addr.ToString("X")}";
}
```

# C#获取对象的内存地址

c#是不能获取内存地址的，就算获取到了也没什么意义。

在unsafe的情况下，可以把内存pin在一定的地址防止回收，只有这样获取的地址才有意义。

`GCHandle`可以转化成`IntPtr`指针。

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