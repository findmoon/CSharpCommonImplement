**Web前端中身份认证的常见方式【记录】**

[toc]

> 转载自 [Web身份认证的常见方式](https://zhuanlan.zhihu.com/p/590149659)

基于HTTP的身份认证是Web应用非常重要、非常基础的一环，

在安全、运维工作中，需要能快速识别应用系统所采用的身份认证机制，以便排查和解决问题。

在设计、研发过程中，需要准确理解各种身份认证机制，以便为系统选择合适的机制。

# Web身份认证

Web身份认证，是Web应用客户端用户与服务端之间的身份认证，包括标识、鉴权等基本过程。

标识/识别(Identification)​ 是用户所特有的识别信息，表明用户的身份。用户使用客户端，向服务端发生身份标识。用户使用的客户端，包括Web浏览器，或者其他客户端（APP、桌面客户端、httpclient）等。

服务端在收到用户的身份标识之后，需要对其标识的真实性、有效性进行鉴别确认。这个过程被称为身份鉴权或身份验证(Authentication)​。

从实践角度，流行的Web身份认证方式可以分为：登陆方式和令牌方式。

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

# 登录方式的认证

## 1、表单认证

这种方式是最基础、最常见的认证方式。

客户端通过post或get请求参数的方式，把用户名、密码上传到服务器端。

服务器端获得用户名、密码之后，进行验证。

这种方式和普通的表单传参没有太大区别。

为了安全性，需要对密码进行加密、单向散列、加盐等措施，避免传输过程中泄露原始密码。即便遭遇网络嗅探，攻击者拿到的也是散列后的密文，无法得到原始密码。

优点：简单灵活。

缺点：无标准规范，受限于安全性，也难以和其他系统快速对接。

## 2、HTTP身份验证框架

在介绍接下来几种认证方式之前，先介绍一些HTTP身份验证框架。

[RFC 7235](https://link.zhihu.com/?target=https%3A//datatracker.ietf.org/doc/html/rfc7235)定义了 HTTP 身份验证框架，服务器可以使用该框架来质询客户端请求，客户端按照服务器的要求，提供身份信息供服务端验证信息。

质询和响应流的工作方式如下：

1. 服务器响应具有401​​（未授权）响应状态给客户端，并提供有关如何使用至少包含一个质询的WWW-Authenticate​​响应标头进行授权的信息。（例如Basic​​、Digest​​）
2. 然后，想要向服务器进行身份验证的客户端可以通过在凭据中包含授权​​请求标头来实现。通常，这个过程伴随着，客户端向用户显示提示，由用户提供身份信息（比如用户名、密码），然后客户端按要求处理，发出包含正确标头的请求。

![](img/20230207133100.png)  

上述常规消息流对于大多数（并不是全部）身份验证方案是相同的。标头中的实际信息及其编码方式实际引用中会有所变化。


### 3、基本认证（Basic Auth）

在HTTP中，基本认证（Basic access authentication）是一种用来允许网页浏览器或其他客户端程序在请求时提供用户名和口令形式的身份凭证的一种登录验证方式。

该方案在[RFC 7617](https://link.zhihu.com/?target=https%3A//datatracker.ietf.org/doc/html/rfc7617) 中定义。

形式：Authorization: Basic <credentials>

<credentials> 为用户名拼接上一个冒号再加上密码，再对整个字符串进行 Base64编码得到。伪代码为：

```text
credentials=base64(user+":"+password);
```

示例：

```http
GET /list/ HTTP/1.1
 Host: www.XXX.com
 Authorization: Basic dXNlcjpwYXNzd29yZA==
```

优点：简单、通用性好。由于它不需要cookie，所有浏览器都支持、其他客户端也很容易支持。

缺点：不安全。简单的base64编码很容易被破解，得到用户名和原始密码。即便使用了https等传输层加密方案，也存在泄漏可能。

![](img/20230207133506.png)  

步骤：

1. 客户端发送http request 给服务器，
2. 因为request中没有包含Authorization header, 服务器会返回一个401 Unauthozied给客户端，并且在Response的 header "WWW-Authenticate" 中返回Basic标识，要求客户端进行基本认证。
3. 客户端把用户名和密码用BASE64加密后，放在Authorization header中发送给服务器，请求认证。
4. 服务器将Authorization header中的用户名密码取出，进行验证， 如果验证通过，将根据请求，发送资源给客户端

![](img/20230207134923.png)  

> realm 用来描述进行保护的区域，或者指代保护的范围。它可以是类似于“Access to the staging site”的消息，这样用户就可以知道他们正在试图访问哪一空间。

### 4、摘要认证（Digest Auth）

DIGEST 认证同样使用 质询/响应 的方式（challenge/response），但不会像 Basic认证 那样直接发送明文密码。

该方案在[RFC 7616](https://link.zhihu.com/?target=https%3A//datatracker.ietf.org/doc/html/rfc7616)中定义。

很明显，Digest Auth的安全性，比Basic Auth提高了很多。

```text
Authorization: Digest username=<username>,
    realm="<realm>",
    uri="<url>",
    algorithm=<algorithm>,
    nonce="<nonce>",
    nc=<nc>,
    cnonce="<cnonce>",
    qop=<qop>,
    response="<response>",
    opaque="<opaque>"
```

步骤：

1. 访问端发出请求。
2. 服务端返回401，以及Header字段WWW-Authenticate。WWW-Authenticate类型为Digest​​，并且需要包含realm 和nonce 信息。realm是域名称，nonce是随机字串。
3. 访问端提供身份标识信息，请求头字段 Authorization 内必须包含 username、realm、nonce、uri 和 response 的字段信息。其中，realm 和 nonce 就是之前从服务器接收到的响应中的字段。response是根据用户密码、nonce等经过摘要算法（可约定，一般为md5）形成的散列字串。
4. 服务端验证，成功返回200，失败返回401。

> 首部字段，Header字段。


### 5、Hawk Auth

Hawk认证与Digest认证的原理比较相似，差别主要在于两点：

- 服务端返回401响应的时候，首部字段WWW-Authenticate设定为Hawk​标识，标识需要采用Hawk认证。
- 客户端按要求在Authorization​字段中指定Hawk id、mac等信息，发送给服务端。其中mac为摘要。

# 令牌方式的认证

## 6、令牌 - Token

凭证，在不同的认证机制中可能采用不同的生成和验证机制，因此有不同的具体名称。

常见的凭证，就是Token，因此这里不做严格区分。


令牌（Token）是一种非常简单认证方式。

Token一般是一段字符串，是用户访问服务端的凭证，一般由服务端签发。

客户端在访问服务端时携带用户的Token，服务端会根据收到的Token验证用户身份以及访问权限。

Token使用时，需要有一定的安全保障。

Token一般由服务端签发。因此用户在首次登陆验证时，尚未获得token，需要采用表单认证、基本认证等方式。


Token也是一种无状态身份凭证。Token泄露后，攻击者可以凭借获得的Token访问服务。

因此，

1. Token需要由服务端发送给客户端，这个过程中建议采用https等方式传输，避免被网络嗅探窃取。
2. Token需要客户端妥善保存。前端程序一般会把Token保存在LocalStorage等位置。
3. Token一般需要定期更新。因此需要建立一个过期机制，由客户端定期获取服务端重新生成的Token。
4. Token可以设计校验机制。例如JWT。


因为Token的无状态特点，也比较适合于跨域场景。每个服务各自维护自己的用户Token关系。客户端用户只需要凭借自己有效的Token，就可以完成对跨域服务的授权访问。


## 7、Cookie 认证

session、cookie是利用浏览器和服务器之间的cookie机制，传递身份凭证，实现身份验证。

因为使用凭证，所以，这种方式不适用于登录认证。一般可用于登录成功之后的，身份认证。

‍

1. 登录成功后，响应头的Set-Cookie​​中返回令牌（这个场景下一般称为id）。令牌为键值对方式。
2. 浏览器在收到后，会自动将Set-Cookie​​中的键值对保存到本地cookie。
3. 下次请求时，浏览器会在请求头的Cookie​​自动携带上述令牌。
4. 服务端验证令牌

‍

很多框架实现了不同的键值对约定，比如：

- session\_id
- PHPSESSID
- ……  
    ‍  
优点：支持了会话管理（且对前端无感）。比较简单、灵活。  

缺点：依赖cookie，如果浏览器禁用或在支持不完善的移动端就无法使用。  


可以在中大型网站的web端，减少频繁。并发较大时，建议session在服务端使用redis等存储方案，减轻session缓存压力。


Session-Cookie 认证需要靠浏览器的 Cookie 机制实现，如果遇到原生 NativeAPP 时这种机制就不起作用了，或是浏览器的 Cookie 存储功能被禁用，也是无法使用该认证机制实现鉴权的。

## 8、Bearer Token（JWT）认证
‍  
JWT （JSON Web Token） 是目前最流行的跨域认证解决方案，是一种基于 Token 的认证授权机制。

从 JWT 的全称可以看出，JWT 本身也是 Token，一种规范化之后的 JSON 结构的 Token。  

一般建议将 JWT 作为 `Bearer token` 通过 Authorization header 传递给服务器。  

JWT 是基于开源标准([RFC 7519](https://link.zhihu.com/?target=https%3A//www.rfc-editor.org/rfc/rfc7519))自包含紧凑的通过 json 传递安全信息的一种方式。  

JWT采用令牌方式认证，不适用于登录认证。一般用于登录成功之后的身份认证。  
‍
```text
Authorization: Bearer <JWT>  
```

Bearer 为固定标识。  

JWT是一组字串，通过（.​​）切分成三个为 Base64 编码的部分：

  - Header : 描述 JWT 的元数据，定义了生成签名的算法以及 Token​​ 的类型。
  - Payload : 用来存放实际需要传递的数据
  - Signature（签名） ：服务器通过 Payload、Header 和一个密钥(Secret)使用 Header 里面指定的签名算法（默认是 HMAC SHA256）生成。



认证步骤：

1. 登录后，服务端返回一个加密、签名过的Token令牌，也就是JWT。具体的返回方式，没有明确规定。
2. 客户端以后每次向服务端发请求都在 Header 的 Authorization 中添加 JWT。
3. 服务端检查 JWT 并从中获取用户相关信息。

‍​
JWT认证的安全性比较高，且有标准规范支持，通用性较强。

JWT 自身包含了身份验证所需要的所有信息，比较适合使用RESTful API这类前后端分离的交互场景，可用于各种跨域认证场景。

同时，服务器也可以不额外存储 Session 信息。大大增加了系统的可用性和伸缩性，大大减轻了服务端的压力。


# 参考

[HTTP API 授权认证有哪些方式？(Authoration, Authentication) - 简书 (jianshu.com)](https://link.zhihu.com/?target=https%3A//www.jianshu.com/p/71254888b9cc)

[一文教你搞定所有前端鉴权与后端鉴权方案，让你不再迷惘 - 掘金 (juejin.cn)](https://link.zhihu.com/?target=https%3A//juejin.cn/post/7129298214959710244)

[HTTP的几种认证方式之DIGEST 认证（摘要认证） - wenbin\_ouyang - 博客园 (cnblogs.com)](https://link.zhihu.com/?target=https%3A//www.cnblogs.com/xy-ouyang/p/12609387.html)

[一文了解基于 HTTP 的认证模式 - 掘金 (juejin.cn)](https://link.zhihu.com/?target=https%3A//juejin.cn/post/7152140774920945677)

[http authorization 基本认证 - szchenrong - 博客园 (cnblogs.com)](https://link.zhihu.com/?target=https%3A//www.cnblogs.com/chenrong/articles/5818498.html)

[RFC 7235 - Hypertext Transfer Protocol (HTTP/1.1): Authentication (ietf.org)](https://link.zhihu.com/?target=https%3A//datatracker.ietf.org/doc/html/rfc7235)

[HTTP authentication - HTTP | MDN (mozilla.org)](https://link.zhihu.com/?target=https%3A//developer.mozilla.org/en-US/docs/Web/HTTP/Authentication%23authentication_schemes)

[Using HTTP cookies - HTTP | MDN (mozilla.org)](https://link.zhihu.com/?target=https%3A//developer.mozilla.org/en-US/docs/Web/HTTP/Cookies)

[JWT 基础概念详解 (javaguide.cn)](https://link.zhihu.com/?target=https%3A//javaguide.cn/system-design/security/jwt-intro.html)
