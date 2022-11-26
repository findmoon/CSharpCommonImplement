**关于ASP.NET Core中使用System.Text.Json的JsonRequired特性用于json模型绑定验证的问题**

[toc]

> 本篇主要介绍`System.Text.Json 7.0` 中引入的 `JsonRequired`，测试到后面发现，可以使用 `Required`、`Range` 等验证特性及其 `ErrorMessage` 实现对 `JsonRequired` 的替代，即同样可以实现对JSON请求数据的模型绑定的验证、错误消息的自定义等。
> 
> 对 `JsonRequired` 的使用和探索源于“误导”（`SuppressModelStateInvalidFilter`使用导致），以及 `Newtonsoft.Json` 库中 `[JsonProperty]` 特性可以设置 `Required` 指定属性是必需的。
> 
> `JsonRequired` 的替代仅在于`ASP.NET Core`中的模型绑定中（这也是使用场景最多的），对于想要在JSON数据反序列化时指定必需的属性，则应该使用`JsonRequiredAttribute`或 C# 11 的`required`修饰符（所有非`ASP.NET Core`模型绑定情况下的json序列化和反序列化中）。
> 
> **在反序列化的场景下，指定必需属性，通常推荐使用`JsonRequiredAttribute`**。


# JSON请求体的模型绑定验证

`Required`、`BindRequired` 只能用于 `FromForm`、`FromQuery` 等数据的模型绑定时的验证。

但是 如果是来自 请求体Body 的 JSON 数据，进行模型绑定时却不能使用 `Required`、`BindRequired` 用于模型验证。

`System.Text.Json` 在 .NET7 中引入了 `[JsonRequired]`特性 或 `required` 属性关键字(C# 11)，用于实现这个必须提供值的验证功能。

`System.Text.Json 7.0` 之前，没有很好的解决方法，只能在模型绑定的反序列化处理方法中进行判断和实现。

## `[JsonRequired]`特性

测试在 ASP.NET Core 6 中，通过安装 System.Text.Json 7.0 nuget包，使用 `[JsonRequired]` 特性，用于 JSON 模型对象的属性上。

比如下面：

```C#
public class TestModel
{
    //[JsonPropertyName(Name = "id")]
    [Required(AllowEmptyStrings =false,ErrorMessage = "id 不能为空")]
    [JsonRequired]
    public string Id { get; set; }

    //[JsonPropertyName(Name = "seq")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "seq 不能为空")]
    [JsonRequired]
    public int Seq { get; set; }
}
```

MVC项目中，模型绑定是否成功，需要在 `Action` 方法中通过 `ModelState.IsValid` 判断；Web API 中，`[ApiController]`会自动处理绑定失败时的 Http 400 错误，不会进入 `Action` 方法而是直接返回`BadRequest`。

**JSON格式的模型类对象在绑定失败时为null，通常为请求的json格式数据无法序列化为模型类对象(也可能是原本就没有请求数据)，`JsonRequired`不满足时结果也为null**。

> 注意，`SuppressModelStateInvalidFilter=true` 会禁用自动 HTTP 400 响应，也就是模型绑定失败时仍会进入 Action 方法执行。

## C# 11的 required 关键字

在属性上使用 `required` 关键字也是一样的，JSON中不提供该属性键时，会验证失败。web api 中会直接返回错误。

```C#
public class TestModel
{
    //[JsonPropertyName(Name = "id")]
    [Required(AllowEmptyStrings =false,ErrorMessage = "id必须指定且不能为空")]
    public required string Id { get; set; }

    //[JsonPropertyName(Name = "seq")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "seq必须指定且不能为空")]
    public required int Seq { get; set; }
}
```

# Action 方法内判断模型类对象

在 MVC 或禁用了自动400错误响应的情况下，可以通过在 Action 方法内，**判断模型验证的结果`ModelState.IsValid`**，或者，**判断模型类对象是否为`null`**，处理模型绑定失败的情况。如下：

```C#
[HttpPost("[action]")]
public async Task<IActionResult> MyTest(TestModel model)
{
    var result = new ResultModel();
    // json格式不正确模型绑定结果为null
    if (model==null)
    {
        result.Status = 400;
        result.Message = "上传的Json数据或格式不正确，请确保正确后重试";
        return new JsonResult(result);
    }

    // ... 其它处理
}
```

# Web 模型验证中 对 JsonRequired 的替代实现

> 注：**`JsonRequired` 的替代实现仅在于`ASP.NET Core`中的模型绑定中，如果是使用`System.Text.Json`序列化反序列化json，想要指定必需属性，则应该使用`JsonRequiredAttribute`或`required`修饰符。**

如下，仅使用 `[Required]`、`[Range]` 模型验证特性的情况下，请求的json数据反序列化为模型对象后，还是会进行模型的验证，从而实现自定义错误消息。

```C#
public class TestModel
{
    [Required(AllowEmptyStrings =false,ErrorMessage = "id必须指定且不能为空")]
    public string Id { get; set; }

    [Range(1,int.MaxValue,ErrorMessage = "seq必须指定且大于0")]
    public int Seq { get; set; }
}
```

测试请求模型验证失败时。

- json请求不指定seq键、或者其值小于等于0时，会返回如下错误：

```json
{
    "status": 400,
    "message": "seq必须指定且大于0"
}
```

- json请求 **不指定id键、或者为null，或者为空字符串，或者为空白字符的字符串(" ") 时，都会验证失败**。返回如下错误：

```json
{
    "status": 400,
    "message": "id必须指定且不能为空"
}
```

相对来说，这种`替代实现`比`JsonRequired`的验证：必须指定json键，效果要好，不过其验证(失败)不是发生在JSON反序列化时，而是在反序列化为模型类对象后的模型数据验证时。

# 