**ASP.NET MVC添加全局统一的模型验证错误处理自动返回而不进入Action方法（Filter中如何访问ModelState）**

[toc]

> `Request.IsAjaxRequest()`并总是准确，它是依据请求头header中的 `X-Requested-With:XMLHttpRequest` 来区分是不是ajax请求的，如果一个ajax请求没有这个请求头，就会判断错误。
>
> 目前测试，如果使用jquery ajax，其请求当前站点的url时，会有 `X-Requested-With:XMLHttpRequest` 请求头；而同样的方法，如果请求的是跨域的其他url，则没有`X-Requested-With:XMLHttpRequest`，这就导致判断错误。
> 
> (未测试是否是浏览器自己去掉的`X-Requested-With:XMLHttpRequest`，还是ajax方法自身去除的)

```C#
    /// <summary>
    /// [ajax请求]添加统一模型验证错误的自动返回过滤器
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // var request = filterContext.RequestContext.HttpContext.Request;            
            // if (request.IsAjaxRequest())
            // {
                //    request.HttpMethod == "POST";
                var viewData = filterContext.Controller.ViewData;

                if (!viewData.ModelState.IsValid)
                {
                    filterContext.Result = new JsonNetResult(new APIModel()
                    {
                        Status = 400,
                        Message = string.Join(";", viewData.ModelState.Values.Where(v => v.Errors.Count > 0).SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
                    });
                }

            // }

            base.OnActionExecuting(filterContext);
        }
    }
```

添加到全局过滤器：

```C#
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new ValidateModelAttribute());

        // ...
    }
}
```

ASP.NET MVC 返回Json对象 JsonNetResult 对中文的支持，乱码处理

http://192.168.104.12:10518/Core/NeximReport/LPWarning

```json
{"status":404,"message":"璧勬簮涓嶅瓨鍦�","data":{"httpMethod":"GET"}}
```

