**C#中的函数指针**

[toc]

# 函数指针 Function pointer

**C# 提供委托类型来定义安全的函数指针对象。**

调用委托涉及实例化派生自 `System.Delegate` 的类型，并使用虚拟方法调用其 `Invoke` 方法，此虚拟调用使用 `callvirt` IL 指令（`callvirt IL instruction`）。在性能关键的代码路径中，使用 `calli` IL 指令效率更高。

> 函数指针的更多介绍 [Function Pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/function-pointers)

## delegate* 函数指针及调用

**可以使用`delegate*`语法定义函数指针，编译器将使用`calli`指令调用函数**，而不是实例化委托对象并调用`Invoke`。

下面的代码声明了使用`delegate`或`delegate*`组合相同类型的两个对象。第一种方法使用 `System.Func<T1，T2，TResult>` 委托类型；第二种方法使用`delegate*`，声明参数和返回类型相同：

```C#
public static T Combine<T>(Func<T, T, T> combinator, T left, T right) => 
    combinator(left, right);

public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) => 
    combinator(left, right);


public static T1 Combine2<T,T1>(Func<T, T, T1> combinator, T left, T right) =>
    combinator(left, right);
```

> **`delegate*<T, T, T>`用于表示一个`delegate*`指针。不过不知道为什么最后一个类型参数`T`表示当前的委托指针的返回类型，而不是输入类型。不确定是否有相关约定。**

下面演示声明一个使用本地函数，并使用指向该函数的指针调用`UnsafeCombine`方法：

```C#
unsafe class FunctionPointer
{
    /// <summary>
    /// 调用函数指针
    /// </summary>
    internal void InvokeFuncPtr()
    {
        int product = UnsafeCombine(&localMultiply, 3, 4);
        Console.WriteLine("函数指针调用的结果："+product);
    }
    static int localMultiply(int x, int y) => x * y;
    public static T UnsafeCombine<T>(delegate*<T, T, T> combinator, T left, T right) =>
        combinator(left, right);
}

// 调用函数指针执行
FunctionPointer functionPtr = new FunctionPointer();
functionPtr.InvokeFuncPtr();
```

**`delegate` 上的 `*` 后缀指示声明是函数指针。`&`和方法名一起分配或赋值给函数指针时（此处为`&localMultiply`分配给`delegate*<T, T, T>`类型的函数指针参数），表示操作获取方法的地址。**


## 函数指针的使用条件

使用函数指针访问函数时，需要遵循以下几点：

- 函数指针仅能在不安全上下文`unsafe context`中声明。
- 使用或返回`delegate*`的方法只能在不安全上下文中调用。
- `&`只能用于获取静态函数的地址（改规则适用于成员函数和本地函数）。

## delegate* 的托管和非托管调用

**使用 `managed` 和 `unmanaged` 关键字可以指定 `delegate*` 的调用约定。此外，对于非托管函数指针，也可以指定调用约定。**

下面的示例显示了调用约定的情况：

- 第一个声明使用`managed`调用约定，这是默认约定。
- 后面的四个使用`unmanaged`调用约定，分别指定 ECMA 335 规范中的 Cdecl、Stdcall、Fastcall 或 Thiscall 约定。
- 最后一个使用`unmanaged`调用约定，指定CLR选择平台的默认调用约定。CLR将在运行时选择`calling convention`。

```C#
public static T ManagedCombine<T>(delegate* managed<T, T, T> combinator, T left, T right) =>
combinator(left, right);

public static T CDeclCombine<T>(delegate* unmanaged[Cdecl]<T, T, T> combinator, T left, T right) =>
    combinator(left, right);
public static T StdcallCombine<T>(delegate* unmanaged[Stdcall]<T, T, T> combinator, T left, T right) =>
    combinator(left, right);
public static T FastcallCombine<T>(delegate* unmanaged[Fastcall]<T, T, T> combinator, T left, T right) =>
    combinator(left, right);
public static T ThiscallCombine<T>(delegate* unmanaged[Thiscall]<T, T, T> combinator, T left, T right) =>
    combinator(left, right);

public static T UnmanagedCombine<T>(delegate* unmanaged<T, T, T> combinator, T left, T right) =>
    combinator(left, right);
```

# 参考

- [Unsafe code, pointer types, and function pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code)