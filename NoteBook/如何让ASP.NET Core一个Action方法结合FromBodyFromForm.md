**Web中Body和Form请求的不同，如何让ASP.NET Core一个Action方法结合 FromBody 和 FromForm 两种请求来源，同时使用[FromBody][FromForm]**

[toc]

- `[FromForm]` 接收的是来自 form 表单提交的数据，其 content type 为 `application/x-www-form-urlencoded`。
- `[FromBody]` 是解析模型的默认方式，其 content type 一般为 `application/json`，来自于请求体。

> - [FromQuery] - Gets values from the query string.
> - [FromRoute] - Gets values from route data.
> - [FromForm] - Gets values from posted form fields.
> - [FromBody] - Gets values from the request body.
> - [FromHeader] - Gets values from HTTP headers.

如果想要在一个Action中同时使用`[FromBody]``[FromForm]`，可以参考 [How to combine FromBody and FromForm BindingSource in ASP.NET Core?](https://stackoverflow.com/questions/51673361/how-to-combine-frombody-and-fromform-bindingsource-in-asp-net-core) 、[ASP.NET Core FromForm And FromBody Same Action](https://stackoverflow.com/questions/50453578/asp-net-core-fromform-and-frombody-same-action) 给出的示例或介绍

主要原理是通过 `[Consumes]`特性，或 自定义 Attribute 特性 处理不同的`content type`。

> 使用 `Consumes` 消费不同的 content type ：`[Consumes("application/x-www-form-urlencoded")]`、`[Consumes("application/json")]`

# 参考解决办法一

I love the solution proposed in the accepted answer, and even used it for a while, but now we have the `[Consumes]` attribute.

**And you can even map the two to the same route URL** which is great news.

```csharp
[HttpPost]
[Route("/api/Post")] //same route but different "Consumes"
[Consumes("application/x-www-form-urlencoded")]
public ActionResult Post([FromForm] Data data)
{
    DoStuff();
}

[HttpPost]
[Route("/api/Post")] //same route but different "Consumes"
[Consumes("application/json")]
public ActionResult PostJson([FromBody] Data data)
{
    Post(data); //just call the other action method
}
```

[https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-5.0#define-supported-request-content-types-with-the-consumes-attribute](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-5.0#define-supported-request-content-types-with-the-consumes-attribute)

# 参考解决办法二

You might be able to achieve what you're looking for with a custom [`IActionConstraint`](https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-2.1#understanding-iactionconstraint):

> Conceptually, IActionConstraint is a form of overloading, but instead of overloading methods with the same name, it's overloading between actions that match the same URL.

I've had a bit of a play with this and have come up with the following `IActionConstraint` implementation:

```csharp
public class FormContentTypeAttribute : Attribute, IActionConstraint
{
    public int Order => 0;

    public bool Accept(ActionConstraintContext ctx) =>
        ctx.RouteContext.HttpContext.Request.HasFormContentType;
}
```

As you can see, it's very simple - it's just checking whether or not the incoming HTTP request is of a form content-type. In order to use this, you can attribute the relevant action. Here's a complete example that also includes the idea suggested in this [answer](https://stackoverflow.com/questions/51430923/posting-form-data-to-mvc-core-api/51431788#51431788), but using your action:

```csharp
[HttpPost]
[FormContentType]
public ActionResult<Data> PostFromForm([FromForm] Data data) =>
    DoPost(data);

[HttpPost]
public ActionResult<Data> PostFromBody([FromBody] Data data) =>
    DoPost(data);

private ActionResult<Data> DoPost(Data data) =>
    new ActionResult<Data>(data);
```

`[FromBody]` is optional above, due to the use of `[ApiController]`, but I've included it to be explicit in the example.

Also from the docs:

> ...an action with an IActionConstraint is always considered better than an action without.

This means that when the incoming request is not of a form content-type, the `FormContentType` attribute I've shown will exclude that particular action and therefore use the `PostFromBody`. Otherwise, if it _is_ of a form content-type, the `PostFromForm` action will win due to it being "considered better".

I've tested this at a fairly basic level and it does appear to do what you're looking for. There may be cases where it doesn't quite fit so I'd encourage you to have a play with it and see where you can go with it. I fully expect that you may find a case where it falls over completely, but it's an interesting idea to explore nonetheless.

Finally, if you don't like having to use an attribute, it is possible to configure a convention that could e.g. use reflection to find actions with a `[FromForm]` attribute and automatically add the constraint. There are more details in this excellent [post](https://www.strathweb.com/2017/06/using-iactionconstraints-in-asp-net-core-mvc/) on the topic.

# 参考解决办法三：

> 结合了`[Consumes]`，实现只在一个Action中处理两个类型的请求

Please look at this library https://github.com/shamork/Toycloud.AspNetCore.Mvc.ModelBinding.BodyAndFormBinding

I used the source and modified a bit as there is an issue with .Net 5 with ComplexTypeModelBinderProvider being obsolete

https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.modelbinding.binders.complextypemodelbinderprovider?view=aspnetcore-5.0

In short, this prevents having to do this

```C#
    public class JsonRequestItem {
        public string jsonRequest { get; set; }
    }

    [HttpPost]
    [ActionName("NewRequest")]
    [Consumes("application/json")]
    public IActionResult NewRequestFromBody([FromBody]JsonRequestItem item) {
        return NewRequest(item.jsonRequest);
    }

    [HttpPost]
    [ActionName("NewRequest")]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult NewRequestFromForm([FromForm]JsonRequestItem item) {
        return NewRequest(item.jsonRequest);
    }

    private IActionResult NewRequest(string jsonRequest) {
        return new EmptyResult(); // example
    }
```

Now you can simplify as one action and get both FromBody and FromForm

```C#
    [HttpPost]
    [ActionName("NewRequest")]
    public IActionResult NewRequestFromBodyOrDefault([FromBodyOrDefault]JsonRequestItem item) {
        return new EmptyResult(); // example
    }
```

