**ASP.NET MVC 5 若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet**

[toc]

ASP.NET MVC 5 设置返回 Json 类型时报错 若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet

```C#
[HttpGet]
public async Task<ActionResult> GetAll()
{
    var users = await _userService.GetAll();
    return Json(users);
}
```

```sh
应用程序中的服务器错误。
此请求已被阻止，因为当用在 GET 请求中时，会将敏感信息透漏给第三方网站。若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet。
说明: 执行当前 Web 请求期间，出现未经处理的异常。请检查堆栈跟踪信息，以了解有关该错误以及代码中导致错误的出处的详细信息。

异常详细信息: System.InvalidOperationException: 此请求已被阻止，因为当用在 GET 请求中时，会将敏感信息透漏给第三方网站。若要允许 GET 请求，请将 JsonRequestBehavior 设置为 AllowGet。
```

原因是，对于 Json 类型，框架默认请求方式是Post，认为Get不安全，需要在返回Json时指定JsonRequestBehavior.AllowGet才可以成功访问。

```C#
[HttpGet]
public async Task<ActionResult> GetAll()
{
    var users = await _userService.GetAll();
    return Json(users, JsonRequestBehavior.AllowGet);
}
```
