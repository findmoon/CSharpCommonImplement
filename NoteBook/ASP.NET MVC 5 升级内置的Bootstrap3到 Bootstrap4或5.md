**ASP.NET MVC 5 升级内置的Bootstrap3到 Bootstrap 4 或者 Bootstrap 5**

[toc]

由于Bootstrap 4到5之间的跨越比较大，如果要升级到 Bootstrap 5 要做好html大改的准备。

> 至少测试直接升级到 Bootstrap 5 会无法运行，Razor视图内很多C#标识要删改。

[upgrading to bootstrap 4 CSS minifier breaks in asp.net mvc](https://stackoverflow.com/questions/52207265/upgrading-to-bootstrap-4-css-minifier-breaks-in-asp-net-mvc)


[ASP.NET With Bootstrap 5 & Scaffolding](https://sdwh.dev/posts/2022/01/ASPNET-MVC-With-Bootstrap5/)



[ASP.NET MVC使用Bootstrap系列（1）——开始使用Bootstrap](https://www.cnblogs.com/OceanEyes/p/get-started-with-bootstrap.html) 

https://www.bootstrapdash.com/blog/asp-net-core-with-bootstrap-4

https://aspnetcore.readthedocs.io/en/stable/client-side/bootstrap.html

https://hub.packtpub.com/getting-started-aspnet-core-and-bootstrap-4/


Bootstrap快速搭建登录界面：

```html
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>用户登录</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        body
        {
            width: 100%;
            height: 100%;
            overflow: hidden;
            background: url(Images/bg.jfif) no-repeat;
            background-size: 100% 100%;
            background-attachment: fixed;
        }

        form
        {
            width: 400px;
            margin: 200px auto;
            padding: 20px 30px;
            background-color: rgba(255, 255, 255, 0.8);
        }

            form div
            {
                margin-bottom:10px;
            }

        a:link, a:visited, a:hover, a:active
        {
            text-decoration: none;
            color: rgba(0, 0, 0, 0.5);
        }
    </style>
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/popper.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <div class="container">
        <form action="Handlers/LoginHandler.ashx" method="post" onsubmit="return login()">
            <div style="margin-bottom:20px;">
                <h2 style="text-align:center;">用户登录</h2>
            </div>
            <div class="form-group has-success has-feedback">
                <input type="text" class="form-control" id="username" placeholder="用户名">
                <span class="glyphicon glyphicon-user form-control-feedback"></span>
            </div>
            <div class="form-group has-success has-feedback">
                <input type="password" class="form-control" id="password" placeholder="密码">
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
            </div>
            <div class="form-check">
                <label class="form-check-label">
                    <input type="checkbox" class="form-check-input" value=""><span style="font-size:15px;">记住我</span>
                </label>
            </div>
            <div>
                <button type="submit" class="btn btn-primary btn-block">登录</button>
            </div>
            <div>
                <a href="#" style="display:inline;font-size:15px;">还未注册</a>
                <a href="#" style="display:inline;font-size:15px;margin-left:185px;">忘记密码？</a>
            </div>
        </form>
    </div>
</body>
</html>
```