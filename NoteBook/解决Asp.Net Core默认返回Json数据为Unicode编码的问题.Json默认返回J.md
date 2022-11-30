**System.Text.Json序列化和反序列化详解，及解决ASP.Net Core使用内置的System.Text.Json默认返回Json数据为Unicode编码的问题**

[toc]

# ASP.Net Core中Json默认会返回Unicode字符的问题

## 问题介绍

ASP.Net Core 中返回`JsonResult`时，默认`System.Text.Json`会返回Unicode字符。

如下，返回的内容为`\"测试\"你好.|'`：

```C#
public IActionResult Index()
{
    return new JsonResult("\"测试\"你好.|'");
}
```

在未进行任何设置修改，使用默认设置的情况下，返回的结果将为：

```json
"\u0022\u6D4B\u8BD5\u0022\u4F60\u597D.|\u0027"
```

也就是，实际的内容被转义编码为Unicode返回。

使用 ASP.Net Core 的 JsonResult/ContentResult 等 Result 结果，默认返回的均是 `charset=utf-8` 编码，只是上面内容为 Unicode 字符
                    // 设置此处的UnicodeRanges.All，使内容为可以显示的正常结果

## 设置 Encoder 为 UnicodeRanges.All

将 `Encoder` 指定为 `UnicodeRanges.All` 是简单的处理方式：

```C#
services.AddControllers()
        .AddJsonOptions(option =>
        {
            option.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);                  
        });
```

这样可以防止中文等字符转义编码为Unicode，上面测试返回的结果将变为：

```json
"\u0022测试\u0022你好.|\u0027"
```

此设置不编码中文，比如 `new JsonResult("测试")` 结果为json字符串：`"测试"`，但是会编码 `"`、`'` 单双引号等。

## Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping【推荐】

`JavaScriptEncoder.UnsafeRelaxedJsonEscaping` 用于设置最大程度地减少转义编码

```C#
services.AddControllers()
        .AddJsonOptions(option =>
        {
            option.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;                  
        });
```

测试 `"\"测试\"你好.|'"` 转为 json 的结果为：

```json
"\"测试\"你好.|'"
```

单双引号正常输出显示。

## 另：web编码选项 WebEncoderOptions

`WebEncoderOptions`用于web数据（文本）的编码指定，参考如下，有需要可以设置

```C#
// web编码选项
services.Configure<WebEncoderOptions>(options =>
{
   options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});
```

# 序列化和反序列化的使用(非常简单)

- `JsonSerializer.Serialize(object)` 序列化一个对象为json字符串
- `JsonSerializer.Deserialize<T>(jsonString)` 反序列化json字符串为T类型的数据或实例。

基本使用就是这么简单，更多使用方法可以再探索。

# JsonSerializerOptions 的使用

`JsonSerializerOptions`用于按照一定格式或规则序列化对象，以获取到想要的格式的JSON。

## 指定序列化选项

`JsonSerializerOptions`一般常用的设置分为如下几项。作为web应用的主要数据格式，不需要没项都如此的介绍，可以参考使用 **`JsonSerializerDefaults.Web`和`Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping`的搭配(见后面介绍)。**

```C#
JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
{
    // 缩进打印
    WriteIndented = true,

    // 设置Json字符串支持的编码，默认情况下，序列化程序会转义所有非 ASCII 字符。 即 转换为 \uxxxx Unicode编码。 可以通过设置Encoder在序列化时不转义为Unicode 
    // 推荐使用如下的设置。
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,


    // 另，下面指定了基础拉丁字母和中日韩统一表意文字的基础Unicode 块(U+4E00-U+9FCC)。 基本涵盖了除使用西里尔字母以外所有西方国家的文字和亚洲中日韩越的文字
    // Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
    
    // 反序列化不区分大小写
    PropertyNameCaseInsensitive = true,
    
    // 序列化 键名 为 小写字母开头的驼峰命名
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

    // 对字典的键进行驼峰命名
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    
    // 序列化的时候忽略null值的属性（JSON结果中不包含此键）
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    
    // 忽略只读属性，因为只读属性只能序列化而不能反序列化，所以在以json为储存数据的介质的时候，序列化只读属性意义不大
    IgnoreReadOnlyFields = true,
    
    // 不允许结尾有逗号的不标准json
    AllowTrailingCommas = false,
    
    // 不允许有注释的不标准json
    ReadCommentHandling = JsonCommentHandling.Disallow,
    
    // 允许在反序列化的时候原本应为数字的字符串（带引号的数字）转为数字
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    
    // 处理循环引用，如下为忽略循环引用，比如Class1类里面有一个属性也是Class1类
    ReferenceHandler = ReferenceHandler.IgnoreCycles
};
```

注意，`JsonSerializerOptions`中很多取值已经不推荐使用，比如：忽略Null值的属性 - `IgnoreNullValues = true`，推荐设置`DefaultIgnoreCondition` 为 `JsonIgnoreCondition.WhenWritingNull`。以官方文档为准。

- 忽略循环引用`ReferenceHandler.IgnoreCycles`

序列化程序将循环引用属性设置为 `null`，在web处理中这是不叫常见的方式。此外就是保留引用。

- 保留循环引用`ReferenceHandler.Preserve`

`ReferenceHandler.Preserve`设置会导致以下行为：

在序列化时：如果为复杂类型，序列化程序还会写入元数据属性（$id、$values 和 $ref）。

在反序列化时：需要元数据（虽然不是必需的），并且反序列化程序会尝试理解它。

如下是官网对`Preserve`用法的介绍，非常不错的示例：

```C#
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PreserveReferences
{
    public class Employee
    {
        public string? Name { get; set; }
        public Employee? Manager { get; set; }
        public List<Employee>? DirectReports { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            Employee tyler = new()
            {
                Name = "Tyler Stein"
            };

            Employee adrian = new()
            {
                Name = "Adrian King"
            };

            tyler.DirectReports = new List<Employee> { adrian };
            adrian.Manager = tyler;

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string tylerJson = JsonSerializer.Serialize(tyler, options);
            Console.WriteLine($"Tyler serialized:\n{tylerJson}");

            Employee? tylerDeserialized =
                JsonSerializer.Deserialize<Employee>(tylerJson, options);

            Console.WriteLine(
                "Tyler is manager of Tyler's first direct report: ");
            Console.WriteLine(
                tylerDeserialized?.DirectReports?[0].Manager == tylerDeserialized);
        }
    }
}

// Produces output like the following example:
//
//Tyler serialized:
//{
//  "$id": "1",
//  "Name": "Tyler Stein",
//  "Manager": null,
//  "DirectReports": {
//    "$id": "2",
//    "$values": [
//      {
//        "$id": "3",
//        "Name": "Adrian King",
//        "Manager": {
//          "$ref": "1"
//        },
//        "DirectReports": null
//      }
//    ]
//  }
//}
//Tyler is manager of Tyler's first direct report:
//True
```

## 推荐重用 JsonSerializerOptions 实例

官方文档在介绍`JsonSerializerOptions`实例时，第一个主题就是`重用JsonSerializerOptions实例`，并给出了重用和不重用两种条件下，性能差异（非常巨大）

```C#
using System.Diagnostics;
using System.Text.Json;

namespace OptionsPerfDemo
{
    public record Forecast(DateTime Date, int TemperatureC, string Summary);

    public class Program
    {
        public static void Main()
        {
            Forecast forecast = new(DateTime.Now, 40, "Hot");
            JsonSerializerOptions options = new() { WriteIndented = true };
            int iterations = 100000;

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                Serialize(forecast, options);
            }
            watch.Stop();
            Console.WriteLine($"Elapsed time using one options instance: {watch.ElapsedMilliseconds}");

            watch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                Serialize(forecast);
            }
            watch.Stop();
            Console.WriteLine($"Elapsed time creating new options instances: {watch.ElapsedMilliseconds}");
        }

        private static void Serialize(Forecast forecast, JsonSerializerOptions? options = null)
        {
            _ = JsonSerializer.Serialize<Forecast>(
                forecast,
                options ?? new JsonSerializerOptions() { WriteIndented = true });
        }
    }
}

// Produces output like the following example:
//
//Elapsed time using one options instance: 190
//Elapsed time creating new options instances: 40140
```

## 复制JsonSerializerOptions

`JsonSerializerOptions`的实例可以作为其构造函数的参数，从而实现复制`JsonSerializerOptions`实例。

```C#
JsonSerializerOptions options = new()
{
    WriteIndented = true
};

JsonSerializerOptions optionsCopy = new(options);
```

> `new()` 直接使用 new 关键字创建目标类型的对象（默认调用其构造函数），是 C#9.0 引入的。

## JsonSerializerDefaults.Web【推荐】

`JsonSerializerDefaults.Web`默认有如下的默认值：

- `PropertyNameCaseInsensitive = true`
- `JsonNamingPolicy = CamelCase`
- `NumberHandling = AllowReadingFromString`

**作为`ASP.NET Core`用于web的默认设置，此外还需要添加一个`Encoder`对于序列化时的转义处理，基本就是最广泛的`JsonOptions`设置了**，如下：

```C#
var JsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};
```

> 此外还可考虑添加 `WriteIndented = true` 格式化缩进。

## 关于如何设置全局默认选项？【不重要】

这是一个很不重要的小需求，应为在使用`JsonSerializerOptions`实例时，需要在每个调用`Serialize()`或`Deserialize()`方法的时候传递`JsonSerializerOptions`参数。如果能够全局设置一次，也会方便一些。

暂时原生为提供此设置，不过完全可以自己实现序列化处理方法（隐藏共用的`JsonSerializerOptions`及其调用时的传参）。

# System.Text.Json的几个特性

## JsonPropertyName指定属性对应的json键名

```C#
```

## JsonInclude包含字段或非公共访问器

## JsonNumberHandling

## JsonIgnore

## JsonPropertyOrder

## JsonConstructor

# 参考

- [How to serialize and deserialize (marshal and unmarshal) JSON in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to?pivots=dotnet-7-0)
- [How to instantiate JsonSerializerOptions instances with System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/configure-options?pivots=dotnet-7-0)
- [How to preserve references and handle or ignore circular references in System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/preserve-references?pivots=dotnet-7-0)