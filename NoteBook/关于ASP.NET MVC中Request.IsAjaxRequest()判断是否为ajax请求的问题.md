关于ASP.NET MVC中Request.IsAjaxRequest()判断是否为ajax请求的问题


`Request.IsAjaxRequest()`并总是准确，它是依据请求头header中的 `X-Requested-With:XMLHttpRequest` 来区分是不是ajax请求的，如果一个ajax请求没有这个请求头，就会判断错误。

目前测试，如果使用jquery ajax，其请求当前站点的url时，会有 `X-Requested-With:XMLHttpRequest` 请求头；而同样的方法，如果请求的是跨域的其他url，则没有`X-Requested-With:XMLHttpRequest`，这就导致判断错误。

(未测试是否是浏览器自己去掉的`X-Requested-With:XMLHttpRequest`，还是ajax方法自身去除的)