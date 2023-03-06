**Nginx设置最简单的反向代理，以及修改请求体、请求头等内容**

[toc]


设置Nginx反向代理的起因是，使用 Apifox 本地Mock功能，无法在本地以外使用。想着可以通过简单的反向代理实现，将网络请求转发到本地地址。

# 简单的反向代理实现

Nginx设置反向代理非常简单，如下所示：

```conf
server {
        listen        5011;
        server_name  0.0.0.0;

        location / {
            proxy_pass  http://127.0.0.1:4523/m1/1623420-0-default/;
        }
}
```

通过 `proxy_pass` 指令，将当前端口的请求直接转发到Mock地址。

> 注意：**proxy_pass 后面的地址结尾`/`加与不加的区别。**

# Nginx简单修改请求体

同样还是因为Mock服务器，本地Mock的接口只能接收json数组或对象。否则报错`only json objects or arrays allowed`。

`proxy_set_body` 指令可以修改请求体的内容。如下所示，构造一个字符串请求体为JSON格式的数组或对象：

```conf
server {
        listen        5011;
        server_name  0.0.0.0;

        location ~ /(GetSN|GetReel) {
            # json 对象
            proxy_set_body "{\"nginxModify\":$request_body}";
            # 无效的json  ----  proxy_set_body "{nginxModify:$request_body}";
            # json 数组
            # proxy_set_body "[$request_body]";
  
            # proxy_pass  http://127.0.0.1:4523/m1/1623420-0-default/GetSN;
            proxy_pass  http://127.0.0.1:4523/m1/1623420-0-default/$1;
        }

        location / {
            proxy_pass  http://127.0.0.1:4523/m1/1623420-0-default/;
        }
}
```

通过正则匹配 `/GetSN` 或 `/GetReel`，修改请求体，并将该请求转发到对应的路径下。

> `$request_body` 变量为请求体内容。

# 附：使用lua脚本修改Nginx的请求体

Nginx通过OpenResty可以使用lua脚本处理用户请求包体。在nginx.conf中添加如下配置：

```conf
location / {
    content_by_lua_block {
        ngx.req.read_body()
        local data = ngx.req.get_body_data()
        ngx.say("The body is: ", data)
    }
}
```

`content_by_lua_block`指令加载lua脚本，在脚本中使用`ngx.req.read_body()`读取请求包体，并使用`ngx.say()`打印出请求包体内容。

# 附：修改请求头

```conf
location /post_data {
    proxy_pass http://backend;
    proxy_set_body "data=$request_body"; 
    proxy_set_header Content-Type application/x-www-form-urlencoded;
}
```

# 参考

[Nginx接收用户请求包体的处理方式](https://www.bytenote.net/article/167946144695975937)

推荐： [nginx系列之修改请求参数](https://blog.csdn.net/chunyuan314/article/details/55292661)