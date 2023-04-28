**ASP.NET Core中System.Text.Json添加时间日期格式转换支持The JSON value could not be converted to System.DateTime**

[toc]

> 并且必须为大写的 T 和 Z

```C#
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace NeximOperateServer.Helper
{
    public class JsonDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
```

在 ASP.NET Core 的服务注册中，添加 AddJsonOptions 对json选项中 序列化器的添加`JsonSerializerOptions`

```C#
services.AddControllers(option =>
            {
                option.Filters.Add<APIModelFilter>();
            })
            .AddJsonOptions(option =>
            {
                option.AllowInputFormatterExceptionMessages = true;
                option.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

                // 添加 DateTime 类型转换
                option.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
            })
```

# 参考

- [DateTime and DateTimeOffset support in System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support?pivots=dotnet-7-0)
- [json无法正常解析DateTime格式](https://blog.csdn.net/M1234uy/article/details/108827107)
- [Deserialize JSON with C#](https://stackoverflow.com/questions/7895105/deserialize-json-with-c-sharp)
