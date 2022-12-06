**C#中的不安全代码、指针使用和分配内存的方方面面**

[toc]

平时所写的C#代码都是“可验证的安全代码”（`verifiably safe code`），这表示 .NET 工具可以验证代码是安全的。

安全代码不能使用指针直接访问内存，并且也不分配原始的内存，而是创建托管对象（`managed objects`）。

> 本篇主要参考翻译自官网文档，参见文末的参考列表

# unsafe 不安全上下文

通过`unsafe`关键字可以启用不安全的上下文，用以编写`不能验证的`代码。在`unsafe`上下文的代码中可以使用指针、分配和释放内存块、使用函数指针调用方法等。

不安全代码不代表是危险的，仅仅表示代码的安全性不能验证。

不安全代码有下面的几种属性：

- 方法、类型和代码块可以定义为`unsafe`不安全
- 一些情况下，不安全代码通过移除数组边界检查可以提高应用程序的性能
- 当调用需要指针的本地代码时，需要使用不安全代码
- 不安全代码引入了安全性和稳定性的挑战
- 必须启用`AllowUnsafeBlocks`编译器选项才能编译包含不安全代码块的代码

如果要使用不安全代码，需要启用`AllowUnsafeBlocks`，否则会报错`CS0227`。

![](img/20221130162827.png)  

`AllowUnsafeBlocks`的启用分为以下几种方式：

修改`.csproj`项目文件，添加如下即可：

```xml
<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
```

或者，修改vs的项目属性，在“生成”中勾选`不安全代码``允许使用"unsafe"关键字编译的代码`。

![](img/20221130101804.png)  

或者`允许不安全代码`（.Net Framework）

![](img/20221130101849.png)  

# 不安全代码的使用

## unsafe 修饰符的使用

通过`unsafe`关键字表示不安全上下文，通常指定一个代码块。

```C#
unsafe{
    // unsafe code block
}
```

可以在属性、方法、类的声明中使用`unsafe`修饰符，表示类型或成员的整个正文范围均被视为不安全上下文。

`fixed`语句用于禁止垃圾回收器重定位可移动的变量（垃圾回收可能不可预知的移动变量地址），主要针对的引用类型的变量；`fixed`修饰符还可用于创建固定大小的缓冲区。

`fixed`语句或修饰符只能出现在不安全的上下文中。

在C#中使用指针时，只能操作struct，不能操作class，不能在泛型类型代码中使用未定义类型的指针。

## 指针类型（Pointer types）

### 指针类型的介绍和声明

在不安全的上下文中，除了值类型（`value type`）和引用类型（`reference type`）之外，还可以使用`指针类型`（`Pointer type`）。指针类型的声明为：

```C#
type* identifier;
void* identifier; //及其不推荐
```

> 不能操作`void*`类型的指针，但是可以将其转换为其他类型的指针；同样其他任何类型的指针都可以转换为`void*`。
> 
> 指针可以是`null`，但是操作空指针可能会有未预见的行为。


**指针类型中`*`之前的类型被称为`referent type`，只有非托管类型可以是`referent type`。**

指针类型并不继承自`object`，并且指针类型和`object`之间不能转换，装箱、拆箱不支持指针。但是，**可以在不同的指针类型之间、指针类型和整型之间进行转换**。

同一声明中声明多个指针时，直接将星号`*`与基础类型写在一起即可，不用作为每个指针名的前缀。

```C#
int* p1, p2, p3;   // Ok
int *p1, *p2, *p3;   // C#中无效
```

### 指针类型的含义

**`MyType*`类型的指针变量的值，是一个`MyType`类型的变量的地址。**

下面是几种指针类型声明的示例：

- `int* p`：`p`是一个指向 `integer` 的指针。
- `int** p`: `p`是一个指向 `integer`指针 的指针。
- `int*[] p`: `p`是一个指向 `integers` 的一维数组指针。
- `char* p`: `p`是一个指向 `char` 的指针。
- `void* p`: `p`是一个指向未知类型的指针。

## `*`间接寻址运算符

`*`除了声明指针变量，还可以作为`指针间接寻址运算符`，用于访问指针变量指向位置的内容。

如下，使用`int*`类型的执行，并`*`寻址获取内容：

```C#
unsafe
{
    int myInt;
    int* myIntPtr=&myInt;

    // 间接寻址运算符
    *myIntPtr = 11;

    Console.WriteLine($"myInt值：{myInt}");
    Console.WriteLine($"*myIntPtr取值：{*myIntPtr}");
    Console.WriteLine($"myInt地址：{(int)myIntPtr}");
}

// 输出：
// myInt值：11
// *myIntPtr取值：11
// myInt地址：3623348284
```

## 引用类型变量使用指针

**指针不能指向引用类型或包含引用类型的结构体(`struct`)**，因为，即使有指针指向，引用对象也可以被垃圾回收。

垃圾回收不会跟踪对象是否有任何的指针指向。

除了垃圾回收之外，引用对象在使用中，还有可能发生位置移动，对象所在的堆地址有可能会发生变化。

因此，要使用引用类型的指针，需要将对象pin住（固定住，钉住），防止其位置移动。C# 提供了`fixed`语句实现pin堆中的对象，从而在使用内部指针时，对象不会移动。

下面的示例来自官方，使用 unsafe 关键字 和 fixed 语句，显示如何增加内部指正和取值赋值：

> `p + n` 和 `n + p` 表达式表示`T*`类型的指针`p`，在其地址上加上`n * sizeof(T)`，即产生新的指针，指向指针`p`之后的第n个元素数据的首地址。
> 
> `n * sizeof(T)`表达式则表示`T*`类型的指针`p`，在其地址上减去`n * sizeof(T)`，指向指针`p`之前的第n个元素地址。

```C#
// Normal pointer to an object.
int[] a = new int[5] { 10, 20, 30, 40, 50 };
// Must be in unsafe code to use interior pointers.
unsafe
{
    // Must pin object on heap so that it doesn't move while using interior pointers.
    fixed (int* p = &a[0])
    {
        // p is pinned as well as object, so create another pointer to show incrementing it.
        int* p2 = p;
        Console.WriteLine(*p2);
        // Incrementing p2 bumps the pointer by four bytes due to its type ...
        p2 += 1;
        Console.WriteLine(*p2);
        p2 += 1;
        Console.WriteLine(*p2);
        Console.WriteLine("--------");
        Console.WriteLine(*p);
        // Dereferencing p and incrementing changes the value of a[0] ...
        *p += 1;
        Console.WriteLine(*p);
        *p += 1;
        Console.WriteLine(*p);
    }
}

Console.WriteLine("--------");
Console.WriteLine(a[0]);

/*
Output:
10
20
30
--------
10
11
12
--------
12
*/
```

**方法之间不应该传递指针**。如果一个方法通过`in`、`out`、`ref`参数或函数返回值返回一个本地变量的指针，如果该指针位于固定块`fixed block`内，则变量将会不再固定，从而产生未定义的行为。


> **关于指针加减运算的含义：**
>  
> 加减算术运算：对于指向数组的指针变量，可以加上或减去一个整数n，指针变量加或减一个整数n的意义：是把指针指向的当前位置(指向某数组元素)向前或向后移动n个位置，指向该位置的数组元素。
>
> 因为数组有不同的类型，各种类型的数组元素所占的字节长度不同；如 **指针变量加 1，即向后移动 1 个位置表示指针变量指向下一个数组元素的首地址，而不是原地址基础上加 1。**
> 
> **指针变量的加减运算只能对数组指针变量进行，对指向其它类型变量的指针变量作加减运算是毫无意义的(虽然可以运算)。**
> 
> 两个指针变量之间的运算只有指向同一数组的两个指针变量之间才能进行运算
> 
> 两指针变量相减：所得之差是两个指针所指数组元素之间相差的元素个数；
> 
> 两指针变量进行关系运算：指向同一数组的两指针变量进行关系运算可表示他们所指数组元素之间的关系：`pf1 == pf2`表示pf1和pf2指向同一数组元素；`pf1 > pf2`表示pf1处于高地址位置；`pf1 < pf2`表示pf2处于高地址位置。
> 
> 设 p 为指针变量，则 `p == 0` 表明 p 是空指针，不指向任何变量； `p != 0` 表示不是空指针；指针变量未赋值时，可以是任意值，但不能使用。
>
> 参考自 [c语言指针向前移动i个位置,C语言指针](https://blog.csdn.net/weixin_29156679/article/details/117104497)


## 指针成员访问操作符`->`

> Pointer member access operator ->

`->` 是指针间接寻址和成员访问的综合。

**如果`x`是一个`T*`类型的指针，`y`是类型`T`的可访问成员，则`x->y`表达式等同于`(*x).y`。**

`->`的使用示例

```C#
public struct Coords
{
    public int X;
    public int Y;
    public override string ToString() => $"({X}, {Y})";
}

public class PointerMemberAccessExample
{
    public static unsafe void Main()
    {
        Coords coords;
        Coords* p = &coords;
        p->X = 3;
        p->Y = 4;
        Console.WriteLine(p->ToString());  // output: (3, 4)
    }
}
```

## 指针元素访问操作符`[]`

> Pointer element access operator []

**`p[n]`指针`p`元素访问等同于`*(p+n)`。**

下面通过指针和`[]`访问数组元素：

```C#
unsafe
{
    char* pointerToChars = stackalloc char[123];

    for (int i = 65; i < 123; i++)
    {
        pointerToChars[i] = (char)i;
    }

    Debug.Write("Uppercase letters: ");
    for (int i = 65; i < 91; i++)
    {
        Debug.Write(pointerToChars[i]);
    }
    Debug.WriteLine("");
    Debug.Write("Lowercase letters: ");
    for (int i = 97; i < 123; i++)
    {
        Debug.Write(pointerToChars[i]);
    }
}

// 输出:
// Uppercase letters: ABCDEFGHIJKLMNOPQRSTUVWXYZ
// Lowercase letters: abcdefghijklmnopqrstuvwxyz
```

> 注意，不检查`out-of-bounds`错误。

## 指针类型转换

下面将`int*`类型的指针强制转换为`byte*`指针，并获取`byte`数据。

> **`sizeof(type)`方法计算一个类型占用的空间大小（字节个数）。**

```C#
int number = 1024;

unsafe
{
    // Convert to byte:
    byte* p = (byte*)&number;

    Debug.Write("整型的四个字节:");

    // Display the 4 bytes of the int variable:
    for (int i = 0; i < sizeof(int); ++i)
    {
        Debug.Write((*p).ToString("X2")+" ");
        // Increment the pointer:
        p++;
    }
    Debug.WriteLine("");
    Debug.WriteLine("整型的值: {0}", number);

    /* Output:
        整型的四个字节: 00 04 00 00
        整型的值: 1024
    */
}
```

# 固定大小的缓冲区 - Fixed-size buffers

可以使用 `fixed` 关键字创建在数据结构中具有固定大小的数组的缓冲区(`buffer`)。**当编写与其他语言或平台的数据源进行互操作的方法时，固定大小的缓冲区很有用**。

在可以使用常规结构成员(`regular struct members`)的任何属性或修饰符上都可以使用`fixed-size buffer`。唯一的限制是数组类型必须为基础类型 bool、byte、char、short、int, long、sbyte、ushort、uint、ulong、float 或 double。

比如：

```C#
private fixed char name[30];
```

在安全代码中，包含数组的C#结构不会包含任何数组元素，结构只是包含到元素的引用。

在`unsafe`代码块中，可以为`struct`添加固定大小的数组。

下面 `PathArray` `struct` 的大小不取决于数组元素的数量，因为是`pathName`引用变量：

```C#
public struct PathArray
{
    public char[] pathName;
    private int reserved;
}
```

不安全代码中，`struct`结构可以包含嵌入的数组。下面的示例中，`fixedBuffer`数组有一个固定大小，使用`fixed`语句获取第一个元素的指针，并通过该指针访问数组元素。**`fixed`语句pin实例字段`fixedBuffer`到内存中的一个特定位置。**

```C#
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}

internal unsafe class Example
{
    public Buffer buffer = default;
}

private static void AccessEmbeddedArray()
{
    var example = new Example();

    unsafe
    {
        // Pin the buffer to a fixed location in memory.
        fixed (char* charPtr = example.buffer.fixedBuffer)
        {
            *charPtr = 'A';
        }
        // Access safely through the index:
        char c = example.buffer.fixedBuffer[0];
        Console.WriteLine(c);

        // Modify through the index:
        example.buffer.fixedBuffer[0] = 'B';
        Console.WriteLine(example.buffer.fixedBuffer[0]);
    }
}
```

> **128个元素的`char`数组大小是256个字节。固定大小的`char`缓冲区中每个字符固定占两个字节，不管采用哪种编码**。
>
> 数组的大小是相同的，即使char缓冲区被封送到 API 方法或结构`struct`设置为`CharSet = CharSet.Auto` / `CharSet = CharSet.Ansi`
> 
> 固定大小的`bool`数组，其元素占用的大小固定为1字节。`bool`数组不适合创建位数组或缓冲区。
> 
> [CharSet](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.charset)

固定大小的缓冲区使用 `System.Runtime.CompilerServices.UnsafeValueTypeAttribute` 进行编译，指示公共语言运行时 (CLR) 某个类型包含可能溢出的非托管数组。

**使用 `stackalloc` 分配的内存还会在 CLR 中自动启用缓冲区溢出检测功能**。

`unsafe struct` 中可以使用固定大小的缓冲区。比如前面的`Buffer`：

```C#
internal unsafe struct Buffer
{
    public fixed char fixedBuffer[128];
}
```

编译器会生成下面带特性的`Buffer`：

```C#
internal struct Buffer
{
    [StructLayout(LayoutKind.Sequential, Size = 256)]
    [CompilerGenerated]
    [UnsafeValueType]
    public struct <fixedBuffer>e__FixedBuffer
    {
        public char FixedElementField;
    }

    [FixedBuffer(typeof(char), 128)]
    public <fixedBuffer>e__FixedBuffer fixedBuffer;
}
```

固定大小的缓冲区不同于常规数组的几个方式如下：

- 只能在`unsafe`上下文中使用
- 只能作为结构的实例字段
- 始终是矢量或一维数组。
- 声明应包括长度，如 `fixed char id[8]`，不能使用 `fixed char id[]`。

# 如何使用指针复制字节数组

下面的通过`unsafe`关键字使Copy方法中可以使用指针；`fixed`语句用于声明`source`和`destination`数组的指针。

`fixed`语句pin源和目标数组在内存中的位置不被垃圾收集移动。当`fixed`块执行结束后，数组的内存块会解除固定(`unpinned`)。

该示例使用索引(`indices`-指标)而不是第二个非托管指针访问两个数组的元素。`pSource` 和 `pTarget` 指针的声明固定数组。

```C#
static unsafe void Copy(byte[] source, int sourceOffset, byte[] target,
    int targetOffset, int count)
{
    // If either array is not instantiated, you cannot complete the copy.
    if ((source == null) || (target == null))
    {
        throw new System.ArgumentException("source or target is null");
    }

    // If either offset, or the number of bytes to copy, is negative, you
    // cannot complete the copy.
    if ((sourceOffset < 0) || (targetOffset < 0) || (count < 0))
    {
        throw new System.ArgumentException("offset or bytes to copy is negative");
    }

    // If the number of bytes from the offset to the end of the array is
    // less than the number of bytes you want to copy, you cannot complete
    // the copy.
    if ((source.Length - sourceOffset < count) ||
        (target.Length - targetOffset < count))
    {
        throw new System.ArgumentException("offset to end of array is less than bytes to be copied");
    }

    // The following fixed statement pins the location of the source and
    // target objects in memory so that they will not be moved by garbage
    // collection.
    fixed (byte* pSource = source, pTarget = target)
    {
        // Copy the specified number of bytes from source to target.
        for (int i = 0; i < count; i++)
        {
            pTarget[targetOffset + i] = pSource[sourceOffset + i];
        }
    }
}
```

# stackalloc 表达式

`stackalloc`表达式在栈(`stack`)上分配内存块。在方法执行期间创建的栈中分配的内存块会在方法返回时自动丢弃。不能显式释放使用 `stackalloc` 分配的内存。`stackalloc`分配的内存块不受垃圾回收的影响，也不必通过 `fixed` 语句固定。

> 栈内存，栈内存开辟快速高效但资源有限，通常限制1M。

可以将 `stackalloc` 表达式的结果分配给以下任一类型的变量：

## `System.Span<T>` 或 `System.ReadOnlySpan<T>` 类型

```C#
int length = 3;
Span<int> numbers = stackalloc int[length];
for (var i = 0; i < length; i++)
{
    numbers[i] = i;
}
```

**将`stack分配内存块`赋值给 `System.Span<T>` 或 `System.ReadOnlySpan<T>` 类型的变量不必使用不安全上下文(`unsafe context`)。**

可以在表达式允许的任何地方使用`stackalloc`，并且在需要`分配内存`时，推荐尽可能的使用 `Span<T>` 或 `ReadOnlySpan<T>` 类型。

```C#
int length = 1000;
Span<byte> buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];
```

```C#
Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
Console.WriteLine(ind);  // output: 1
```

## 指针类型

如下示例，对于指针类型，`stackalloc`表达式只能用于本地变量声明的初始化中。

```C#
unsafe
{
    int length = 3;
    int* numbers = stackalloc int[length];
    for (var i = 0; i < length; i++)
    {
        numbers[i] = i;
    }
}
```

使用指针类型，必须使用不安全上下文(`unsafe context`)。

## stackalloc分配内存的注意点

堆栈可用的内存数量是有限的，如果分配太多内存，则可能发生`StackOverflowException`异常。因此需要注意以下几点：

- **限制使用`stackalloc`分配的内存数量。**

例如，如果预期的缓冲区大小低于某个限制，则可以在堆栈上分配内存；否则，请使用所需长度的数组。如下代码所示：

```C#
const int MaxStackLimit = 1024;
Span<byte> buffer = inputLength <= MaxStackLimit ? stackalloc byte[MaxStackLimit] : new byte[inputLength];
```

> stack 上可用内存数量取决于代码的执行环境。

- **避免在循环内部使用`stackalloc`。在循环外部`allocate`分配内存块，并在循环内部重用。**

**新分配内存的内容时未定义的。必须在使用之前初始化。** 比如，**可以使用 `Span<T>.Clear` 方法设置所有的元素项为类型`T`的默认值**。

也可以使用数组初始化器定义新分配内存的内容。

```C#
Span<int> first = stackalloc int[3] { 1, 2, 3 };
Span<int> second = stackalloc int[] { 1, 2, 3 };
ReadOnlySpan<int> third = stackalloc[] { 1, 2, 3 };
```

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

# 非托管类型 Unmanaged type

在定义指针、`stackalloc T[n]`时，其类型只能是非托管类型。(虽然在使用和形式上，非托管类型与C#的原始类型几乎没有区别，但，还是可以了解下)。

以下类型的属于或也属于非托管类型：

- `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `char`, `float`, `double`, `decimal`, or `bool`
- 任何 `enum` 类型
- 任何 `pointer` 类型
- 任何 只包含非托管类型字段的用户定义(`user-defined`)的 `struct` 类型

使用非托管泛型约束`unmanaged`，指定类型参数为非指针、不可为空的非托管类型。

仅包含非托管类型字段的构造结构类型（`constructed struct type`）也是非托管的。如下示例所示，`DisplaySize<T>()`方法的泛型约束为`unmanaged`，在调用时`Coords<int>`、`Coords<double>`作为非托管类型使用：

```C#
using System;

public struct Coords<T>
{
    public T X;
    public T Y;
}

public class UnmanagedTypes
{
    public static void Main()
    {
        DisplaySize<Coords<int>>();
        DisplaySize<Coords<double>>();
    }

    private unsafe static void DisplaySize<T>() where T : unmanaged
    {
        Console.WriteLine($"{typeof(T)} is unmanaged and its size is {sizeof(T)} bytes");
    }
}
// Output:
// Coords`1[System.Int32] is unmanaged and its size is 8 bytes
// Coords`1[System.Double] is unmanaged and its size is 16 bytes
```

泛型结构`Coords<T>`可以是非托管和托管构造类型。当然也可以限制为非托管类型，如下：

```C#
public struct Coords<T> where T : unmanaged
{
    public T X;
    public T Y;
}
```

# 参考

- [Unsafe code, pointer types, and function pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code)
- [Pointer related operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/pointer-related-operators)
- [stackalloc expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/stackalloc)
- [Unmanaged types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/unmanaged-types)