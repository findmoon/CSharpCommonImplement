**ASP.NET Core 6 使用SignalR实现实时应用入门与学习**

[toc]

> 新建 .NET 6 的 ASP.NET Core Web API 项目 `SignalRBasic` ，用于代码测试和实现。

# ASP.NET Core SignalR 简介

## 什么是 SignalR？

ASP.NET Core SignalR 是一种开放源代码库，可简化应用程序实现或集成实时 web 功能。

> 实时 web 功能指的是即时获取web数据，服务端可以立即将内容推送到客户端。

适用场景如下：

- 需要从服务器进行高频率更新的应用。比如 游戏、社交网络、投票、拍卖、地图和 GPS 应用。
- 仪表板和监视应用。比如 公司仪表板、即时销售更新或旅行警报。
- 协作应用。比如 白板应用和团队会议软件。
- 需要通知的应用。比如 社交网络、电子邮件、聊天、游戏、旅行警报，和其他需要及时获取消息的应用场景。

SignalR提供了一个用于创建服务器到客户端的远程过程调用(RPC)的API，来自服务器端 .NET Core 代码的RPC会调用客户端的函数。

> API for creating server-to-client remote procedure calls (RPC)

Here are some features of SignalR for ASP.NET Core:

- Handles connection management automatically.
- Sends messages to all connected clients simultaneously. For example, a chat room.
- Sends messages to specific clients or groups of clients.
- Scales to handle increasing traffic.
- [SignalR Hub Protocol](https://github.com/dotnet/aspnetcore/blob/main/src/SignalR/docs/specs/HubProtocol.md)

## 传输

SignalR 支持以下的实时通信技术（按顺序优雅回退，自动选择最佳的传输方法）：

- [WebSockets](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/websockets)
- Server-Sent Events 【SSE】
- Long Polling 【长轮询】

## Hubs

SignalR 使用 Hubs(集线器) 在客户端和服务器之间进行通信。

Hub是一种允许客户端和服务器相互调用方法的高级管道。SignalR 自动处理跨计算机边界的调度，使客户端能够在服务器上调用方法，反之亦然。可以传递强类型参数给启用了模型绑定的方法。

SignalR 提供了两种内置的Hub协议：基于 JSON 的文本协议和基于 [MessagePack](https://msgpack.org/) 的二进制协议。与 JSON 相比，MessagePack 通常会创建较小的消息。

> 较早的浏览器必须支持 [XHR level 2](https://caniuse.com/#feat=xhr2) 才能提供 MessagePack 协议支持。

Hub通过发送包含客户端方法的名称和参数的消息来调用客户端代码。作为方法参数发送的对象将使用配置的协议进行反序列化。客户端尝试将名称与客户端代码中的方法匹配。当匹配时，会调用该方法并向其传递反序列化的参数数据。

ASP.NET Core SignalR 支持的客户端有：

- [JavaScript client](https://learn.microsoft.com/en-us/aspnet/core/signalr/javascript-client?view=aspnetcore-6.0)
- [.NET client](https://learn.microsoft.com/en-us/aspnet/core/signalr/dotnet-client?view=aspnetcore-6.0)
- [Java client](https://learn.microsoft.com/en-us/aspnet/core/signalr/java-client?view=aspnetcore-6.0)

> 如果不支持ES6，see [Getting Started with ES6 – Transpiling ES6 to ES5 with Traceur and Babel](https://weblogs.asp.net/dwahlin/getting-started-with-es6-%E2%80%93-transpiling-es6-to-es5).

> Flutter下有非官方支持的 signalr_flutter、signalr_netcore 等各种客户端包。

# 主流的Web端实时通信的技术和原理

SignalR用到的实时通信技术，基本都是主流的Web端即时通信技术，几乎可以做到所有浏览器或Web端环境的支持，并提供支持其它类型的客户端。

传统的Web应用工作流程是这样的：浏览器发送HTTP请求到ASP.NET Core Web服务器, Web服务器处理请求并返回响应, 浏览器客户端接收返回的数据。

![](img/20230424170613.png)

但这种工作方式却无法满足实时web的需求。

实时Web需要服务器能够主动发送消息给客户端(浏览器)，以及客户端主动发送消息给服务器（实现全双工通信(`duplex`)）：

![](img/20230424170900.png)

数据发生变化，Web服务器主动发送给客户端，或，通知客户端。

> duplex transmission,duplex traffic:    双工传输
> duplex, full duplex:    全双工
> auto duplex:    自动双工

## Polling

### Polling

Polling是实现实时Web的一种笨方法, 它就是通过**定期的向服务器发送请求**, 来查看服务器的数据是否有变化.

如果服务器数据没有变化, 那么就返回204 No Content; 如果有变化就把最新的数据发送给客户端:

![](img/20230424174202.png)

> Polling 轮询 被称为 短轮询、定期轮询。

![](img/20230425192551.png)

### Polling 代码测试实现

下面是Polling的一个实现, 非常简单:

1. Web API项目中，新增一个控制器 DemoTestController 代码如下

```C#
    [Route("api/[controller]")]
    [ApiController]
    public class DemoTestController : ControllerBase
    {
        #region Polling

        /// <summary>
        ///  Polling 用于轮询测试
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult PollingTest_GetCount(int id)
        {
            var count = GetLastedCount();
            if (count > 10)
            {
                return Ok(new { id, count, finished = true });
            }
            
            if (count > 8)
            {
                return NotFound();
            }
            
            if (count > 3)
            {
                return Ok(new { id, count });
            }
            return NotFound();
        }
        #endregion

        private static int _count;
        public int GetLastedCount()
        {
            _count++;
            return _count;
        }
    }
```

`PollingTest_GetCount`方法会调用 `GetLastedCount()`，获取全局自动的 _count 变量。

Action `PollingTest_GetCount` 获取的 count，会在 `count<=3` 和 `8<count<=10` 时返回 No Content；`3<count<=8` 时返回对象；`count>10`返回中增加finished标志。

2. PollingTest.html 前端页面实现polling（定时轮询/短轮询），请求`PollingTest_GetCount`接口。

> html、js位于同一文件

```html
    <button id="btnStart" type="button">开始Polling</button>
    <span id="result" style="color:red;font-weight:bolder;"></span>
    <br>
    <br>
    <br><br>
    <button id="btnReset" type="button">重置</button>
    <script>
        window.onload=()=>{
            const resultDiv = document.getElementById("result");
            let intervalId = -1;

            function poll(id) {
                fetch('/api/DemoTest/PollingTest_GetCount/'+id)
                    .then(function (response) {
                        if (response.status === 200) {
                            return response.json().then(j => {
                                let txt=`id(${j.id})-count(${j.count})`
                                resultDiv.innerHTML = txt;
                                if (j.finished) {
                                    clearInterval(intervalId);
                                    resultDiv.innerHTML = txt + ",已结束";
                                }
                            })
                        }
                    });
            }

            document.getElementById('btnStart').addEventListener('click', () => {
                intervalId = setInterval(() => {
                    poll(parseInt(1 + 20 * Math.random()))
                }, 1000)
            });

            // 重置
            document.getElementById('btnReset').addEventListener('click', () => {
                fetch('/api/DemoTest/ResetCount', {
                    method: 'PUT', // *GET, POST, PUT, DELETE, etc.
                    })
                    .then(res=>{
                        console.log(res);
                        // return res.json();
                        return res.text();
                    })
                    .then(obj=>console.log(`重置结束 ${obj}.`));
            });
        }
</script>
```

功能很简单：点击按定后，定时每秒发送一次请求，获取数据，并对数据进行处理和显示！

![](img/20230425185139.png)

当请求到 `count>10` 的时候结束。

这就是Polling - 定时轮询，因为频繁建立连接与发送请求，相对，比较浪费资源，同时增加服务器负担。

> **SignalR没有采用Polling这种技术**


## Long Polling

### Long Polling

Long Polling和Polling有类似的地方, 客户端都是发送请求到服务器. 但是不同之处是: 

**如果服务器没有新数据要发给客户端的话, 那么服务器会继续保持连接, 直到有新的数据产生, 服务器才把新的数据返回给客户端。**

**如果请求发出后一段时间内没有响应, 那么请求就会超时. 这时, 客户端会再次发出请求。**

![](img/20230425191119.png)

![](img/20230425191137.png)

相比 Polling 做了优化。

![](img/20230425193013.png)

### 关于超时时间

#### Kestrel 服务器的默认超时时间，设置超时时间

#### ASP.NET Core 部署到 IIS 的超时时间设置

#### Visual Studio IIS Express调试时的超时时间设置

#### fetch 请求的超时时间及设置

> 默认情况下，`fetch()` 请求的超时时间，在Chrome中为300秒，而在Firefox中为90秒。
>
> 300秒、甚至90秒都远远超过了用户对完成一个简单网络请求的期望。

`fetch()` API本身不允许以编程方式取消一个请求，通过也没有设置超时时间的参数。

> 终止`fetch()`请求的实现，本质是通过一个 终止控制器(`AbortController`)，将其`signal`属性(`AbortSignal`对象) 传递给 fetch 方法，fetch会监听`signal`的状态，一旦状态改变且请求未结束，就会终止 fetch 方法的请求。


`fetchWithTimeout()` 是 `fetch()` 的改进版，可创建具有可配置超时的请求。

```js
async function fetchWithTimeout(resource, options = {}) {
  const { timeout = 10000 } = options;
  
  const controller = new AbortController();
  const id = setTimeout(() => controller.abort(), timeout);
  const response = await fetch(resource, {
    ...options,
    signal: controller.signal  
  });
  clearTimeout(id);
  return response;
}
```

- `const { timeout = 10000 } = options` 从 options 对象中提取以毫秒为单位的超时参数（默认为10秒）。

- `const controller = new AbortController()` 创建一个[中止控制器](https://developer.mozilla.org/en-US/docs/Web/API/AbortController)实例。这个控制器用于停止 fetch 的请求。

注意，每一个请求都必须创建一个新的中止控制器。换句话说，控制器是不可重复使用的。

- `const id = setTimeout(() => controller.abort(), timeout)` 启动一个计时功能。在 `timeout` 时间之后，如果计时函数没有被清除，执行 `controller.abort()` 中止（或取消）获取请求。 

- `await fetch(resource, { ...option, signal: controller.signal })` 执行获取请求。

注意分配给 signal 属性的特殊 `controller.signal` 值：它将`fetch()` 与中止控制器连接起来。

- 最后`clearTimeout(id)`，如果请求完成的速度比 `timeout` 的时间快，则清除终止的计时功能。

`fetchWithTimeout`超时终止请求的实际示例：

```js
async function loadGames() {
  try {
    const response = await fetchWithTimeout('/games', {
      timeout: 6000
    });
    const games = await response.json();
    return games;
  } catch (error) {
    // Timeouts if the request takes longer than 6 seconds
    console.log(error.name === 'AbortError');
  }
}
```

#### XMLHttpRequest 对象指定超时时间 (ajax) 

`XMLHttpRequest`对象有指定超时时间的API，可以直接指定超时时间和超时处理。

> 显式调用 `XMLHttpRequest.abort()` 可以终止请求；`abort`终止请求事件。

```js
// 具有超时的ajax请求
function send(url,options) {
  const {
    method='GET' // GET、POST、PUT、DELETE
    success=res=>console.log(res),
    error=res=>console.error(res),
    timeout=10000, // 超时时间
    timeoutFunc=e=>console.error("Timeout!!"),
    async=true, // 是否异步执行操作，默认true
    user=null,  // 可选的用户名用于认证用途；默认为 null
    password=null, // 可选的密码用于认证用途；默认为 null
} = options;

 // 创建对象
  let xhr = new XMLHttpRequest()
  // 初始化请求
  xhr.open(method, url, async,user,password)
  xhr.timeout = timeout // 超时时间，单位是毫秒
  xhr.onreadystatechange = function() {
    if (xhr.readyState == 4) {
      if (xhr.status == 200) {
        //如果返回成功
      }

      const res = {
        response:xhr.response,
        status:xhr.status,
        responseType:xhr.responseType
      }
      if(xhr.status>=400) error(res);
      else success(res);
    }
  }
  xhr.ontimeout = timeoutFunc;
  // 发送请求
  xhr.send()
}


send()


send() //不管成功还是失败都会发下一次请求
```

### Long Polling 代码测试实现

在 DemoTestController 控制器中，新增一个名称为`LongPollingTest` 的 Action：

```C#
#region Long Polling
/// <summary>
/// 长轮询测试
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[HttpGet("[action]/{id}")]
public IActionResult LongPollingTest_GetCount(int id)
{
   // 模式等待数据
   Thread.Sleep(1000 * new Random().Next(1,120) / 2);

   int count = GetLastedCount();
   if (count > 5)
   {
       return Ok(new { id, count, finished = true });
   }
   else
   {
       return Ok(new { id, count, finished = false });
   }
}
#endregion
```

`LongPollingTest_GetCount` 用于模拟长轮询的接口地址。通过模拟一定时间的等待，在符合要求的数据出现之前，保持连接开放，当有新数据后，才会返回给客户端。

`LongPollingTest.html`前端页面的实现：

```html

```

# 附：网络通信中单工（Simplex Communication）、半双工（Half-duplex Communication）、全双工（Full-duplex Communication）

![](img/20230424171741.png)

- 单工通信只支持信号在一个方向上传输（正向或反向）
- 半双工通信允许信号在两个方向上传输，但某一时刻只允许信号在一个信道上单向传输。(实际上是一种可切换方向的单工通信)
- 全双工通信允许数据同时在两个方向上传输，即有两个信道，因此允许同时进行双向传输。

> 参见 [网络通信中的单工（Simplex Communication）、半双工（Half-duplex Communication）、全双工（Full-duplex Communication）是什么意思？](https://blog.csdn.net/Dontla/article/details/126603052)

# 参考

- [ASP.Net Core 3.1 使用实时应用SignalR入门](https://www.cnblogs.com/hudean/p/14172852.html)

- [javascript:如何对fetch()请求进行超时处理](https://juejin.cn/post/7126043782172114951)

- [ASP.NET Core如何设置请求超时时间](https://www.cnblogs.com/OpenCoder/p/10307882.html)

# 其他

- [Web 实时推送技术的总结](https://segmentfault.com/a/1190000018496938) -- 绝对好文
- [这是即时通讯的 4 种实现方案](https://juejin.cn/post/7057687288154685470)
- [前端实时通信的8种方式及其优缺点和实现方式](https://blog.csdn.net/weixin_43236062/article/details/107756103)
- [实时通信技术大乱斗](https://zhuanlan.zhihu.com/p/419044910)
- [即时通讯程序总结](https://blog.csdn.net/godelgnis/article/details/100431494)