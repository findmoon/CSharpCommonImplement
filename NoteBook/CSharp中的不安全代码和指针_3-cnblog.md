**C#中固定大小的缓冲区和使用指针复制数据**

[toc]

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

# 参考

- [Unsafe code, pointer types, and function pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code)