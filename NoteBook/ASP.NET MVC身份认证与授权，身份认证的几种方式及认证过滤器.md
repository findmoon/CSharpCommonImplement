**ASP.NET MVC身份认证与授权，身份认证的几种方式及认证过滤器**

[toc]

> 主要转载自 [ASP.NET MVC身份认证与授权](https://blog.csdn.net/dust__/article/details/106205738)，非常好的一篇文章。

# Web身份认证的方式

Web身份认证指的是Web应用客户端与服务器之间的身份认证，包括身份标识、用户鉴权（或用户验证`Authentication`）等基本过程。

由于HTTP协议是无状态的协议，对用户的认证和授权，都是基于Web服务器应用程序实现的。

流行的Web身份认证方式主要分为：登陆方式和令牌方式。

- 登录方式

  - 表单认证
  - HTTP身份验证框架

    - 基本认证
    - 摘要认证

      - Hawk认证

- 令牌方式

  - Session&Cookie认证
  - JWT认证

**登录方式**，一般是用户在客户端上首次访问服务端时使用的身份认证方式。用户通过发送表明身份的用户名、口令等信息，实现服务端登陆。登录成功后，服务端的会话管理机制会为用户的建立一个维持一段时间的会话。

**令牌方式**，一般是用户在登录后使用的身份认证方式。登录成功后，用户可以使用服务端提供的令牌去访问服务端。

在传统Web开发中，最常见的是使用表单认证，认证成功后生成Cookie发送到客户端，用于后续的用户会话维持。现代前后端分离及大前端的发展趋势，越来越多的采用JWT认证。


> 身份认证用于服务器资源的保护，只有登陆的用户、有权限的用户，才能访问站点内容。
>
> 身份认证中的 Session：
> 
> 基于Session保存用户状态和信息，比如：用户登录信息或登陆状态，相当于授权。
> 
> 在访问具体页面时，如果检测到没有登录，则禁止用户某些动作或操作。
> 
> session 的不足之处：
> 
> （1）Session具有生命周期，超过规定时间，用户就必须要重新登录
> （2）Session有各种丢失的可能，例如服务器重启，内存回收等，这样会影响用户的体验。

# ASP.NET身份验证

| 验证方式      |   说明   |
|   ---         |   ---    |
| Windows       | 使用windows操作系统和NTFS文件系统验证，适合公司内部站点使用，不适合大众商业站点 |
| Forms         | 利用网页向客户端发送凭证，客户端再把凭证提交给应用程序进行身份认证(使用最普遍) |
| Passport      | 一种单点登录标准(微软提供，使用需要付费，国内采用的较少) |
| Federated     | 一种单点登录标准(谷歌提供，一种联合身份验证标准) |
| HTTP框架认证  |           |

## Forms验证

（1） 在实际开发中应用最普遍  
（2） 最初由亚马逊网站开发使用，在内部使用Cookie来维护页面之间的状态  
（3） 在ASP.NET MVC中提供了一个`FormsAuthentication`类专门用于身份认证服务  
（4） `FormsAuthentication`类的一个功能就是写入一个标识用户身份的Cookie

### FormsAuthentication类

|   属性或方法      |       说明      |
|       ---         |       ---       |
| string LoginUrl   | 用户访问且验证不通过时，重定向到的登录页面的URL |
| TimeSpan TimeOut  | 获取身份验证票据的到期前的时间量 |
| void SetAuthCookie(string userName,bool createPersistentCookie) | 为提供的用户名创建一个身份验证票据，并将该票据添加到响应的Cookie集合或Url中，常用于登录 |
| viod SignOut() | 从服务器中删除Forms身份验证票据，常用于注销 |
| string Encrypt(FormsAuthenticationTicket ticket) | 将验证票据对象加密成一个字符串 |
| FormsAuthenticationTicket Decrypt(string encryptedTicket) | 将加密过的用户身份票据字符串解密成一个票据对象 |

## 验证案例

### 1.Forms验证案例

（1）编写带有身份验证的登录 Action 方法

```C#
[HttpPost]
public ActionResult UserLogin(UserInfor user)
{
    UserServer server = new UserServer();
    // 获取用户（主要是从数据库）
    UserInfor currentUser = server.GetUser(user);
    if (currentUser!=null)// 用户名、密码、验证码等验证一致的用户存在
    {
        //为当前用户名提供一个身份验证票据，并将该票据添加到Cookie
        FormsAuthentication.SetAuthCookie(currentUser.Name,false);
        ViewBag.Infor = $"欢迎您：{currentUser.Name}";
        return View("Index");
    }
    else
    {
        ViewBag.Infor = "用户名或密码错误！";
        return View("Login");
    }   
}
```

前端表单的HTML代码：

```html
<div> 
    <form action="~/Home/UserLogin" method="post">
        <table>
            <tr>
                <td>
                    用户名：
                </td>
                <td>
                    <input type="text" name="Id" value="" />
                </td>
            </tr>
            <tr>
                <td>
                    密码：
                </td>
                <td>
                    <input type="password" name="Pwd" value="" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input type="submit" value="登录" />
                </td>
            </tr>
        </table>
        <div>
            @ViewBag.Infor
        </div>
    </form>
</div>
```

`SetAuthCookie`方法中的两个参数：

1. 第一个参数给`currentUser.Name`当前登录人员的名字，也就是验证票据存在，那么验证票据中保存的就是登录人员的名字。

2. 第二个参数如果为true，表示永远不过期的Cookie，只要用户登录后，以后都不需要进行重新登录了，除非客户端主动把Cookie清除

### 2.使用User对象检查用户是否已验证

ASP.NET中内置的User对象封装了用户身份票据。可用于控制代码的权限

```C#
public ActionResult UserManager()
{
   if (this.User.Identity.IsAuthenticated)
   {
       string adminName = this.User.Identity.Name;//读取登陆的用户名
       ViewBag.adminName = adminName;
       UserServer server = new UserServer();
       ViewBag.AdminInfo = server.GetUserInfors();
       return View();
   }
   else
   {
       return RedirectToAction("Login", "Home");
   }
}
```

通过`User.Identity.IsAuthenticated`代码认证用户，只要是哪个页面需要做用户身份认证，Action方法中都要写一个判断，所以相对而言比较繁琐。

因此，实际中不会使用这种方式，而是使用 认证 特性 `[Authorize]`（本质是一个过滤器）。可以将其添加到  Action、Controller 或 Global 全局。

### 3.修改根目录中Web.config配置文件

`Web.config` 中添加`FormsAuthentication`认证的配置

```xml
<system.web>
    <compilation debug="true" targetFramework="4.7"/>
    <httpRuntime targetFramework="4.7"/>

    <!-- FormsAuthentication 认证 -->
    <authentication mode="Forms">
      <forms loginUrl="~/Home/Login" timeout="2880"></forms>
    </authentication>
</system.web>
```

(1) mode：身份验证的方式(Forms,None,Password,Windows)  

(2) loginurl：当前用户直接访问带有验证检测的页面时，如果没有验证通过，跳转到url(一般都设置的是登录页面)  

(3) timeout：Cookie的有效期，单位为“分钟”，2880表示两天时间  


### 4.用户注销

用户登录之后，根据需要可以注销当前用户票据

```C#
public ActionResult UserExit()
{
   FormsAuthentication.SignOut();
   return View("~/Home/Index");
}
```

# 四、身份授权

## Authorize特性实现授权

### Authorize特性

1. 更方便的实现身份授权

2. 应用在控制器上：拥有票据的用户具备访问这个控制器内所有Action方法的权限。

如果要使用则必须要考虑清楚，是否在控制器中的每个Action方法都需要进行身份验证，如果哪个方法不需要身份验证，则这个方法不应该出现在这个控制器中

3. 应用在Action方法上：拥有票据的用户具备访问该Action方法的权限。

和在每个Action方法中添加票据验证效果一样，哪个Action方法具有该特性则就会在访问时进行验证

### 注意

使用`Authorize`特性实现授权和User对象效果相同，但是`Authorize`可以实现更多复杂的功能

### Authorize实现高级授权案例

可以按照指定用户名进行授权

```C#
[Authorize(Users ="小红")]
public ActionResult UserManager()
{
    string adminName = this.User.Identity.Name;//读取写入的AdminName
    ViewBag.adminName = adminName;
    UserServer server = new UserServer();
    ViewBag.AdminList = server.GetUserInfors();
    return View();
}
```

只有当登录用户的名字是"小红"的时候才能通过身份验证，其他人没有权限进入管理界面。