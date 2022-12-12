**C#使用stackalloc分配堆栈内存和非托管类型**

[toc]

# stackalloc 表达式

`stackalloc`表达式在栈(`stack`)上分配内存块。

在方法执行期间创建的栈中分配的内存块会在方法返回时自动丢弃。不能显式释放使用 `stackalloc` 分配的内存。`stackalloc`分配的内存块不受垃圾回收的影响，也不必通过 `fixed` 语句固定。

> 栈内存，栈内存开辟快速高效但资源有限，通常限制1M。

可以将 `stackalloc` 表达式的结果分配给以下任一类型的变量：

## stackalloc 分配 `System.Span<T>` 或 `System.ReadOnlySpan<T>` 类型

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

## stackalloc 分配 指针类型

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

**新分配内存的内容是未定义的。必须在使用之前初始化。** 比如，**可以使用 `Span<T>.Clear` 方法设置所有的元素项为类型`T`的默认值**。

也可以使用数组初始化器定义新分配内存的内容。

```C#
Span<int> first = stackalloc int[3] { 1, 2, 3 };
Span<int> second = stackalloc int[] { 1, 2, 3 };
ReadOnlySpan<int> third = stackalloc[] { 1, 2, 3 };
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

- [stackalloc expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/stackalloc)
- [Unmanaged types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/unmanaged-types)