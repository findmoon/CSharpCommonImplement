关于ASP.NET MVC中Request.IsAjaxRequest()判断是否为ajax请求的问题


`Request.IsAjaxRequest()`并总是准确，它是依据请求头header中的 `X-Requested-With:XMLHttpRequest` 来区分是不是ajax请求的，如果一个ajax请求没有这个请求头，就会判断错误。

目前测试，如果使用jquery ajax，其请求当前站点的url时，会有 `X-Requested-With:XMLHttpRequest` 请求头；而同样的方法，如果请求的是跨域的其他url，则没有`X-Requested-With:XMLHttpRequest`，这就导致判断错误。

(未测试是否是浏览器自己去掉的`X-Requested-With:XMLHttpRequest`，还是ajax方法自身去除的)

可以在 `$.ajax` 请求时，手动指定该请求头：

```js
    $.ajax({
        async:false,  // 同步请求ajax，极其不推荐
        type: "Post",
        dataType: "json",
        contentType: 'application/json',
        url: url,
        data: JSON.stringify(params),
        headers: {      // 指定请求头
            'X-Requested-With':'XMLHttpRequest'
        },
        success: function (json) {
        },
        error: function (json) {
        },
    });
```

# 参考

- [ASP.NET MVC中通过Request.IsAjaxRequest()来判断是否要加载公共视图](https://www.cnblogs.com/nele/p/4974840.html)
- [.net 使用IsAjaxRequest来判断ajax请求时遇到的问题，总返回false](https://blog.csdn.net/weixin_46382369/article/details/121459033)