**C#中GCHandle使用介绍和释放内存，获取变量的堆栈内存地址，及直接操作内存数据**

[toc]

# GCHandle 结构 介绍

## GCHandle 结构

`GCHandle` 是 Struct 结构类型，它的作用是 **提供从非托管内存访问托管对象的方法**。

**在平台互操作的场景中，通常会遇到调用非托管代码的时候，要传递给非托管代码相关的托管对象，用以非托管代码执行时访问该对象或数据，这时就要注意自动垃圾回收，是否会收集或移动托管对象，导致内存访问错误。**

最常见的，比如非托管代码需要回调函数，可以通过 `委托（安全的函数指针）` 传递回调，回调中引用 C# 的某个对象并访问操作，就需要小心，如果非托管代码用到的托管对象被GC回收了，就会报内存错误。

因此，需要将这个托管对象 `pin`(钉住)，固定它的内存地址，不会垃圾收集移动或回收。即使用`GCHandle.Alloc`向特定对象分配一个`handle`，使其不受GC的影响而被回收或移动。

`pin`住的托管对象不会被GC回收，因此，如果确认不再使用，需要我们自己管理释放内存。即使用`GCHandle.Free`释放`GCHandle`。否则，可能会发生内存泄露(`memory leak`)。

> This GCHandle must be released with Free() when it is no longer needed.

## GCHandle.Alloc 和 Free 方法的使用

官网提供了一个使用`GCHandle.Alloc`为托管对象创建`handle`句柄的示例，非托管函数`EnumWindows`接收传递的委托和托管对象的`GCHandle`转换的`IntPtr`，**非托管函数将该`IntPtr`传递回调用方的回调函数参数。**

调用完后，执行`Free()`释放创建的`GCHandle`。

```C#
public delegate bool CallBack(int handle, IntPtr param);

internal static class NativeMethods
{
    // passing managed object as LPARAM
    // BOOL EnumWindows(WNDENUMPROC lpEnumFunc, LPARAM lParam);

    [DllImport("user32.dll")]
    internal static extern bool EnumWindows(CallBack cb, IntPtr param);
}

public class App
{
    public static void Main()
    {
        Run();
    }

    public static void Run()
    {
        TextWriter tw = Console.Out;
        GCHandle gch = GCHandle.Alloc(tw);

        CallBack cewp = new CallBack(CaptureEnumWindowsProc);

        // platform invoke will prevent delegate to be garbage collected
        // before call ends

        NativeMethods.EnumWindows(cewp, GCHandle.ToIntPtr(gch));
        gch.Free();
    }

    private static bool CaptureEnumWindowsProc(int handle, IntPtr param)
    {
        GCHandle gch = GCHandle.FromIntPtr(param);
        TextWriter tw = (TextWriter)gch.Target;
        tw.WriteLine(handle);
        return true;
    }
}
```

> 另外，还可以使用 `GC.KeepAlive` 方法 确保对对象的引用存在【未具体测试，是否能确保pin住不被GC移动】

## GCHandle.ToIntPtr 获取对象的内存地址

`GCHandle.ToIntPtr(gchandleObj)` 获取 `IntPtr` 类型的内存地址，它是`GCHandle`类的静态方法，参数为GCHandle对象。

## GCHandle.FromIntPtr 从IntPtr类型指针获取GC句柄

```C#
GCHandle gch = GCHandle.FromIntPtr(test_intPtr);
```

## Target 属性获取GC句柄对应的托管对象

`Target`属性用于获取 GC句柄对象 对应的托管对象。

`gcHandleObject.Target`。

```C#
var test = new OnlyForTest();
// 固定内存地址，获取托管对象的GC句柄
GCHandle gch = GCHandle.Alloc(test);

// 从GC句柄获取内存地址 IntPtr 指针
var test_intPtr = GCHandle.ToIntPtr(gch);

Debug.WriteLine($"从GC句柄获取内存地址：{test_intPtr}");

// 从 IntPtr 指针 获取GC句柄
var gch2 = GCHandle.FromIntPtr(test_intPtr);

// 从GC句柄获取对应的 托管对象
var test2 = (OnlyForTest)gch2.Target!;

Debug.WriteLine($"托管对象和GC句柄相互转换后是否相等：{Object.ReferenceEquals(test, test2)}");

Debug.WriteLine($"通过intPtr转换后GC句柄是否相等：{Object.ReferenceEquals(test, test2)}");

gch.Free();
gch2.Free();


// --- 输出
// 从GC句柄获取内存地址：1679464217032
// 托管对象和GC句柄相互转换后是否相等：True
// 通过intPtr转换后GC句柄是否相等：True

// ....

class OnlyForTest{}
```

## AddrOfPinnedObject 获取pinned对象的内存地址

`gchandleObj.AddrOfPinnedObject()` 用于获取pinned对象的`GCHandle`实例的 `IntPtr` 类型的内存地址，它是GCHandle实例的方法。

并且 GCHandle 一定要为 `GCHandleType.Pinned` 类型 pin住的对象，否则获取时将会报错。

```C#
#region AddrOfPinnedObject() 需要获取的是pin住对象地址，非pinned对象获取时报错
GCHandleType.Pinned pin住对象时报错 System.ArgumentException:“Object contains non-primitive or non-blittable data. Arg_ParamName_Name”
只能pin基元类型或可复制(传输)类型。对象引用为 non-blittable 类型
var test_forPinned = new OnlyForTest();
GCHandle gch_pined = GCHandle.Alloc(test_forPinned, GCHandleType.Pinned);
// 获取pin住的GC句柄的内存地址 IntPtr 指针
var test_intPtr = gch_pined.AddrOfPinnedObject(); 
#endregion

// ....

class OnlyForTest{}
```

`System.ArgumentException:“Object contains non-primitive or non-blittable data. Arg_ParamName_Name”` 要 pin住 的对象必须为基元类型或可复制(传输)类型，否则无法创建 pinned对象 的GC句柄。

**对象引用为 non-blittable 类型。**

## `Object.ReferenceEquals` 比较对象的引用是否相等

# C#获取内存地址的方法

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

通过 `GCHandle.ToIntPtr` 获取 `IntPtr` 类型的内存地址，这种情况下不用启用“允许不安全代码”（没涉及到操作或使用指针）

> **指针和固定大小缓冲区只能在 不安全上下文 中使用**。

```C#
// 获取引用类型的内存地址方法
public static string GetMemory(object obj)
{
    // GCHandle.Alloc(obj, GCHandleType.Pinned) // System.ArgumentException:“Object 包含非基元或非直接复制到本机结构中的数据。”
    GCHandle handle = GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
    IntPtr addr = GCHandle.ToIntPtr(handle);
    return $"0x{addr.ToString("X")}";
}
```

# C#获取对象的内存地址

> 前面已经介绍，使用的是 `GCHandle.ToIntPtr`。
> 
> 只是当时这部分也是参考的 [c#获取类的内存地址](https://q.cnblogs.com/q/42721/) ，因此再出

c#是不能获取内存地址的，就算获取到了也没什么意义。

在unsafe的情况下，可以把内存pin在一定的地址防止回收，只有这样获取的地址才有意义（后语后续操作）。

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

- [GCHandle Struct](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.gchandle?view=net-6.0)
- [《你不常用的c#之二》:略谈GCHandle](https://www.cnblogs.com/zhaox583132460/p/3402243.html)