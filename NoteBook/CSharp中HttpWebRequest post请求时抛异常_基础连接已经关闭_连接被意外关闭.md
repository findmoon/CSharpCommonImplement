**C#中HttpWebRequest发送post请求，并解决异常-基础连接已经关闭: 连接被意外关闭**

[toc]

# HttpWebRequest发送post（分两步请求）

使用 POST 方法时，必须获取请求流，写入要发布的数据，然后关闭流。

When using the POST method, you must get the request stream, write the data to be posted, and close the stream. This method blocks waiting for content to post; if there is no time-out set and you do not provide content, the calling thread blocks indefinitely.

## 如果设置 ContentLength>0 或 SendChunked==true，则必须提供请求正文（即调用`GetRequestStream`）

在创建 HttpWebRequest 对象后，设置post请求的Header各个参数，发送后会报错：`如果设置 ContentLength>0 或 SendChunked==true，则必须提供请求正文。  在 [Begin]GetResponse 之前通过调用 [Begin]GetRequestStream，可执行此操作。`

因此，`[Begin]GetRequestStream`为post添加上下文，参见后面的代码。

> 根据提示，在不指定 `ContentLength` 或 `SendChunked==true` 时，可以省去 `GetRequestStream` 的请求，直接发送post。
> 
> 测试 **不指定 `ContentLength`，即，post发送内容为空，不设置请求体数据，可以不用调用 `GetRequestStream`，但对于post方法来说，很少会有发送空请求体内容的情况**。
> 
> `SendChunked==true` 属性设置未测试。

因此，调用`GetRequestStream`并设置请求体data数据的处理如下：

```C#
//data参数赋值
using (Stream stream = request.GetRequestStream())
{
   stream.Write(data, 0, data.Length);// 向当前流中写入字节序列
};
```

后面因为一些`小错误`，改为了后面调用`GetRequestStream`的形式，对`GetRequestStream Exception`异常进行处理。

## Post请求实现代码

post请求实际处理时分为两步的，第一步是发送option请求，获取response的100状态（100-continue）；第二步再发送实际的post请求及数据。

如下，是很早之前写的 HttpWebRequest 发送 POST 请求的过程，非常简陋，夹杂着各种注释：

```C#
/// <summary>
/// 向指定url发送JSON格式的Post请求
/// </summary>
/// <param name="jsonData">发送的JSON字符串</param>
/// <param name="url">post url地址</param>
/// <param name="recursion">发送失败时是否重复发送，及重复次数</param>
void PostDataToWeb(string jsonData, string url, int recursion = 0)
{
    if (jsonData == null)
    {
        return;
    }
    string logInfo = string.Empty;

    #region //不缓存的httpwebrequest
    try
    {
        //为"http:"或 "https"设置一个默认策略级别
        HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
        HttpWebRequest.DefaultCachePolicy = policy;

        //创建请求
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //为此次请求设不缓存
        HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        request.CachePolicy = noCachePolicy;
        request.ContentType = "application/json";//传送数据的格式

        #region 解决 "基础连接已经关闭: 连接被意外关闭" 问题。目前测试，仅设置 HttpVersion.Version10 即可解决该报错
        // 添加 user agent 
        //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
        //request.KeepAlive = false;  //设置不建立持久性连接连接
        request.ProtocolVersion = HttpVersion.Version10; //http的版本有2个,一个是1.0,一个是1.1 具体更具实际情况测试替换

        #endregion

        //开始请求
        logInfo = $"begin post production info";
        DealLogInfo(logInfo); // 记录log

        //POST Data
        request.Method = "POST";//POST请求
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        request.ContentLength = data.Length;

        #region GetRequestStream 提供请求上下文
        //data参数赋值
        //using (Stream stream = request.GetRequestStream())
        //{
        //    stream.Write(data, 0, data.Length);// 向当前流中写入字节序列
        //};
        
        Stream stream = null;
        try
        {
            //NotSupportedException 请求缓存验证程序指示对此请求的响应可从缓存中提供；但是写入数据的请求不得使用缓存。 如果您正在使用错误实现的自定义缓存验证程序，则会发生此异常。
            stream = request.GetRequestStream();
        }
        catch (WebException ex)
        {
            HttpWebResponse response = (HttpWebResponse)ex.Response;
            if (response == null)
            {
                DealLogInfo($"GetRequestStream Exception:{ex.Message}");
                return;
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError || (int)response.StatusCode > 500)//400
            {
                if (recursion < 6)
                {
                    DealLogInfo($"500错误，重试{recursion + 1}次");
                    PostDataToMES(jsonData, ip, recursion + 1);
                }
                else
                {
                    DealLogInfo($"内部服务器异常(500)错误下，重新上传次数达到5次，请确认后重试");
                }
            }
            else
            {
                DealLogInfo($"GetRequestStream Exception:{ex.Message}");
                //BeginInvoke(new Action(()=> MessageBox.Show(ex.Message)));

            }
            return;
        }
        catch (Exception ex)
        {
            DealLogInfo($"GetRequestStream Exception:{ex.GetType()}");
            if (stream == null)
            {
                DealLogInfo("无法处理POST参数");
            }
            return;
        }
        finally
        {
            if (stream != null)
            {
                stream.Write(data, 0, data.Length);// 向当前流中写入字节序列
                stream.Close();
            }
        } 
        #endregion

        //{"如果设置 ContentLength>0 或 SendChunked==true，则必须提供请求正文。  在 [Begin]GetResponse 之前通过调用 [Begin]GetRequestStream，可执行此操作。"}
        //POST方法必须先获取请求上下文
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            //File.AppendAllText("response.txt",(new StreamReader(response.GetResponseStream())).ReadToEnd());
            if (response.StatusCode == HttpStatusCode.OK)//200
            {
                logInfo = $"Success processed";
            }
            else if (response.StatusCode == HttpStatusCode.Created)
            {
                logInfo = $"Post Successd 201-{response.StatusCode}";
            }
            else
            {
                logInfo = $"response code {response.StatusCode}";
            }
            DealLogInfo(logInfo);
        }
    }
    catch (WebException ex)
    {
        if (ex.Status == WebExceptionStatus.Timeout)
        {//已经建立上下文连接后，POST参数写入请求流前，网线断开，发生异常：{"请求被中止: 操作超时。"}。ex.Response==null 会报错未将对象引用设置到对象实例
            //
            logInfo = $"Timeout {ex.Message}";
        }
        else
        {
            HttpWebResponse response = (HttpWebResponse)ex.Response;
            if (response.StatusCode == HttpStatusCode.InternalServerError || (int)response.StatusCode > 500)//400
            {
                if (recursion < 6)
                {
                    DealLogInfo($"500错误，重试{recursion + 1}次");
                    PostDataToMES(jsonData, ip, recursion + 1);
                }
                else
                {
                    DealLogInfo($"内部服务器异常(500)错误下，重新上传次数达到5次，请确认后重试");
                }
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)//400
                {
                    logInfo = $"400-URL incorrect or request data invalid";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)//404
                {
                    logInfo = $"404-Specified sensor dose not exists";
                }
                else
                {
                    logInfo = $"response code {response.StatusCode}";
                }
                DealLogInfo(logInfo);
            }
        }

        //BeginInvoke(new Action(()=> MessageBox.Show(ex.Message)));
        return;
    }
    catch (Exception ex)
    {
        //log处理
        DealLogInfo(ex.Message);
        BeginInvoke(new Action(() => MessageBox.Show(ex.Message)));
    }
    #endregion
}
```

## 解决 "基础连接已经关闭: 连接被意外关闭" 问题。

改问题一般原因是因为，服务器不允许与 Internet 资源建立持久性连接连接，或者，http的版本造成。

目前测试，**仅设置 `HttpVersion.Version10` 即可解决该报错**，如下：

```C#
request.ProtocolVersion = HttpVersion.Version10; //http的版本有2个,一个是1.0,一个是1.1 具体更具实际情况测试替换
```

- `HttpWebRequest.ProtocolVersion` 的默认值为 `Version11`
- `HttpWebRequest.KeepAlive`的默认值为 `true`

此外如果不能解决该报错，可以参考设置`KeepAlive=false`、添加`UserAgent`等。

```C#
// 添加 user agent 
request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
request.KeepAlive = false;  //设置不建立持久性连接连接
```

> 另，有一篇 [C# wpf HttpClient“基础连接已经关闭: 连接被意外关闭。“](https://blog.csdn.net/qq_33435149/article/details/120079983)


# HttpWebRequest.Timeout

获取或设置 `GetResponse()` 和 `GetRequestStream()` 方法的超时值（以毫秒为单位），默认值为 100，000 毫秒(100 秒)。 

# 参考

- [c#中 HttpWebRequest请求抛异常,基础连接已经关闭: 连接被意外关闭](https://www.cnblogs.com/hycms/p/5020103.html)
- [HttpWebResponse 类 (System.Net) | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/api/system.net.httpwebresponse?view=net-6.0)