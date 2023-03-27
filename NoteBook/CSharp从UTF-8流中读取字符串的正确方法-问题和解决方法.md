**C# 从 UTF-8 流中读取字符串的正确方法-问题和解决方法**

[toc]

> 这篇的起因来自 [C# 从 UTF-8 流中读取字符串的正确方法](https://blog.csdn.net/wanghao72214/article/details/121460606) 的介绍。
> 
> 通过 Stream 读取 UTF-8 编码的字符串时（任何多字节编码都一样有这个问题），很大概率会出现读取到字节数量（或字节位置），并不是正好等于字节编码正确编码长度。
> 
> 比如，UTF-8 使用 1 到 4 个字节来表示一个 Unicode 字符，如果读取到缓冲区的字节不是完整的UTF-8字符，那么就没正确解码为正确的字符，出现乱码形式！
> 
> 文中给出了两种解决办法：**1. 读取完全部的字节数据后，才将总的字节数组转换为字符串；2. 通过`StreamReader`使用指定编码读取整个stream流或文件。**
> 
> 此外还提到，`System.Text.Decoder`类正确解码缓冲区内的字符，使用`PipeReader`、`Rune`类来以内存性能优化的方式读取数据。未做具体介绍。

> 其实，其实除了上面提到的方法，还有一个更简洁、更好用的解决办法：
> 
> 以 4 的倍数 长度为单位，读取stream流的字节，这样就能保证 读取到的都是正确位置的UTF-8编码字节（因为它的范围就是1-4个字节）。不会出现截断的情况！


我们下面的代码是从一个流 [stream](https://so.csdn.net/so/search?q=stream&spm=1001.2101.3001.7020) 中读取 UTF-8 编码的字符串。我们可以先考虑一下其中存在的潜在问题。

```csharp
string ReadString(Stream stream)
{
    var sb = new StringBuilder();
    var buffer = new byte[4096];
    int readCount;
    while ((readCount = stream.Read(buffer)) > 0)
    {
        var s = Encoding.UTF8.GetString(buffer, 0, readCount);
        sb.Append(s);
    }

    return sb.ToString();
}
```

问题出在：某些情况下返回的字符串与与原始编码的字符串并不同。  
例如，笑脸符号😊 有时会被解码为 4 个未知字符：

```csharp
编码字符串: 😊
解码字符串: ????
```

我们知道：UTF-8 可以使用 1 到 4 个字节来表示一个 Unicode 字符，有关字符串编码的知识可以参考 ​​[字符编码​​​](http://www.codebaoku.com/charset/charset-index.html) 一文。

​​Stream.Read​​​ 方法可以把从 1 到​​ messageBuffer.Length​​​ 字节返回，这意味着缓冲区可能包含不完整的 UTF-8 字符。

一旦缓冲区中的最后一个字符的 UTF-8 编码不完整，那么 ​​Encoding.UTF8.GetString​​ 就是转换一个无效的 UTF-8 字符串。在这种情况下，该方法返回一个无效字符串，因为它无法猜测丢失的字节。

我们使用以下代码演示以上行为：

```csharp
var bytes = Encoding.UTF8.GetBytes("?");
// bytes = new byte[4] { 240, 159, 152, 138 }

var sb = new StringBuilder();
// 模拟逐个字节地读取数据流
for (var i = 0; i < bytes.Length; i++)
{
    sb.Append(Encoding.UTF8.GetString(bytes, i, 1));
}

Console.WriteLine(sb.ToString());
// "????" 代替了 "😊"

Encoding.UTF8.GetBytes(sb.ToString());
// new byte[12] { 239, 191, 189, 239, 191, 189, 239, 191, 189, 239, 191, 189 }
 
```

## 如何修复代码

有多种方法可以修复代码。  
第一种方法：只有当你得到全部数据时，才将字节数组转换为字符串。

```csharp
string ReadString(Stream stream)
{
    using var ms = new MemoryStream();
    var buffer = new byte[4096];
    int readCount;
    while ((readCount = stream.Read(buffer)) > 0)
    {
        ms.Write(buffer, 0, readCount);
    }

    return Encoding.UTF8.GetString(ms.ToArray());
}
```

第二种方法：可以把流包进一个具有正确编码的 StreamReader 对象中。

```csharp
string ReadString(Stream stream)
{
    using var sr = new StreamReader(stream, Encoding.UTF8);
    return sr.ReadToEnd();
}
```

另外，还可以使用System.Text.Decoder类来正确解码缓冲区内的字符。在需要性能的情况下，可以使用PipeReader、Rune类来以内存优化的方式读取数据。

**参考资料：**

1. ​​[字符编码​ ​​](http://www.codebaoku.com/charset/charset-index.html)
2. [C#教程​](http://www.codebaoku.com/csharp/csharp-index.html)

