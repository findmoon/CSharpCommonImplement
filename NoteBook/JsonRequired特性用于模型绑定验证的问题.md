**关于ASP.NET Core中使用System.Text.Json的JsonRequired特性用于模型绑定验证的问题**

[toc]

`Required`、`BindRequired` 只能用于 FromForm、FromQuery 等数据的模型绑定时的验证。

但是 如果是来自 请求体Body 的 JSON 数据，进行模型绑定时却不能使用 `Required`、`BindRequired` 用于模型验证。

`System.Text.Json` 在 .NET7 中引入了 `[JsonRequired]`特性 或 `required` 属性关键字(C# 11)，用于实现这个必须提供值的验证功能。

System.Text.Json 7.0 之前，没有很好的解决方法，只能在模型绑定的自己的处理方法中进行判断和实现。

测试在 ASP.NET Core 6 中使用 System.Text.Json 7.0 的 `[JsonRequired]` 特性，用于 JSON 模型对象的属性上。无法实现绑定验证。（不提供此属性键时，模型绑定的对象实例为空，仍然会进入 Action 的处理中，不会直接返回模型绑定失败信息）

比如下面：

```C#
public class TestModel
{
    //[JsonPropertyName(Name = "number")]
    [BindRequired]
    [Required(AllowEmptyStrings =false,ErrorMessage = "number 不能为空")][JsonRequired]
    public string Number { get; set; }

    //[JsonPropertyName(Name = "seq")]
    [BindRequired]
    [Required(AllowEmptyStrings = false, ErrorMessage = "seq 不能为空")]
    [JsonRequired]
    public int Seq { get; set; }
}
```

目前处理，**在 Action 中通过判断模型类对象是否为null，为null则表示绑定失败，请求的json格式不正确(也可能是原本就没有请求数据)**。

后面测试，在 .NET 7 中，使用 `[JsonRequired]` 和在 .NET 6 中的效果一样。

即使，在属性上使用 `required` 关键字也是一样的，绑定模型对象为null，进入 Action 方法执行。无法直接在绑定时就验证并返回错误。

```C#
public class TestModel
{
    //[JsonPropertyName(Name = "number")]
    [BindRequired]
    [Required(AllowEmptyStrings =false,ErrorMessage = "number 不能为空")][JsonRequired]
    public required string Number { get; set; }

    //[JsonPropertyName(Name = "seq")]
    [BindRequired]
    [Required(AllowEmptyStrings = false, ErrorMessage = "seq 不能为空")]
    [JsonRequired]
    public required int Seq { get; set; }
}
```

这一切的行为和结果，与 `ASP.NET Core 6` 中直接使用模型类（不指定`[JsonRequired]`特性、required关键字） 是一样的。

```C#
[HttpPost("[action]")]
public async Task<IActionResult> MyTest(TestModel model)
{
    var result = new ResultModel();
    // json格式不正确不会产生模型验证的错误，只会是结果为null
    // [JsonRequired] [BindRequired]都没法验证请求的json数据。只有json格式完全一致才能匹配模型；不符合时不会卡住，只会生成为null的模型对象
    if (model==null)
    {
        result.Status = 400;
        result.Message = "上传的Json数据或格式不正确，请确保正确后重试";
        return new JsonResult(result);
    }

    // ... 其它处理
}
```