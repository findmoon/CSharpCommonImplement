**ASP.NET MVC 和 Web API 设置返回的JsonResult为驼峰命名的Json数据**

[toc]

# ASP.NET MVC 5 返回小驼峰命名Json

ASP.NET MVC 5 不受全局设置的影响。Json() 方法 和 创建 JsonResult 对象，都需要单独指定。

要实现需要处理两个地方，一个是 Json() 方法，通过 重写（Override）实现；一个是 JsonResult 类，自定义继承JsonResult的新类（比如JsonNetResult），在`ExecuteResult`实现小驼峰的序列化，后续使用`JsonNetResult`替换`JsonResult`。

```C#
    /// <summary>
    /// 自定义 JsonResult，特定序列化格式
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">序列化的对象</param>
        public JsonNetResult(Object data)
        {
            Data = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            // 未检查Data为null

            // If you need special handling, you can call another form of SerializeObject below
            var serializedObject = JsonConvert.SerializeObject(Data);
            response.Write(serializedObject);
        }
    }
```

# ASP.NET Web API 全局设置返回Json为小驼峰命名

在 `Application_Start()` 方法内，添加全局的Json格式化设置：

```C#
GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings=new JsonSerializerSettings()
{
    //日期类型默认格式化处理
    DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
    DateFormatString = "yyyy-MM-dd HH:mm:ss",

    //空值处理
    //NullValueHandling = NullValueHandling.Ignore;

    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
};
```

# 继承 JsonResult 自定义序列化格式



# Newtonsoft.Json 设置全局的Json格式

```C#
JsonConvert.DefaultSettings = () =>
{
    return new Newtonsoft.Json.JsonSerializerSettings()
    {
        //日期类型默认格式化处理
        DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
        DateFormatString = "yyyy-MM-dd HH:mm:ss",

        //空值处理
        //NullValueHandling = NullValueHandling.Ignore;

        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),

    };
};
```

# Application_Start() 方法设置全局格式化

`Application_Start()` 方法设置全局序列化时的格式化，可以支持 Web API，然后MVC中使用 Newtonsoft.Json 的序列化，而不使用 Json() 或 JsonResult。

```C#
#region Json序列化设置
// System.Web.Mvc.Controller Json() JsonResult 不受全局序列化格式化设置的影响
var jsonSettings = new JsonSerializerSettings()
{
    //日期类型默认格式化处理
    DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
    DateFormatString = "yyyy-MM-dd HH:mm:ss",

    //空值处理
    //NullValueHandling = NullValueHandling.Ignore;

    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),

    //PreserveReferencesHandling= PreserveReferencesHandling.None,
    ReferenceLoopHandling= ReferenceLoopHandling.Ignore,

};
// JsonResult 全局序列化设置
GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
// Newtonsoft.Json 全局默认设置
JsonConvert.DefaultSettings = () => jsonSettings; 
#endregion
```

# 参考

- [Using JSON.NET as the default JSON serializer in ASP.NET MVC 3 - is it possible?](https://stackoverflow.com/questions/7109967/using-json-net-as-the-default-json-serializer-in-asp-net-mvc-3-is-it-possible)
- [Change Default JSON Serializer Used In ASP MVC3 [duplicate]](https://stackoverflow.com/questions/6883204/change-default-json-serializer-used-in-asp-mvc3)
- [Serialize with DefaultSettings](https://www.newtonsoft.com/json/help/html/DefaultSettings.htm)
- [How to make MVC5 JsonResult use NewtonSoft.json CustomSerializer](https://learn.microsoft.com/en-us/answers/questions/600350/how-to-make-mvc5-jsonresult-use-newtonsoft-json-cu)
- [ASP.NET MVC、WebApi 设置返回Json为小驼峰命名](https://blog.csdn.net/q646926099/article/details/80169601)

其他

[Newtonsoft.Json 全部配置](https://www.cnblogs.com/rash/p/8489666.html)

[.NET Core / .NET 5 JSON 返回格式配置 踩坑日记](https://blog.csdn.net/qq_42798138/article/details/116424196)