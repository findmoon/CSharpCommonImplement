MQTT（一）C#使用 MQTTnet 快速实现 MQTT 通信【转载-待看】

[toc]

> 原文：[MQTT（一）C#使用 MQTTnet 快速实现 MQTT 通信（文末有完整Demo下载）](https://blog.csdn.net/panwen1111/article/details/79245161)

## 目录

[MQTT（一）C#使用 MQTTnet 快速实现 MQTT 通信（文末有完整Demo下载）](https://blog.csdn.net/panwen1111/article/details/79245161)

[MQTT（二）在windows64位上安装Python环境](https://blog.csdn.net/panwen1111/article/details/79248328)

[MQTT（三）Python客户端+net客户端+net服务端 简单通信](https://blog.csdn.net/lordwish/article/details/85006228)

[MQTT（四）树莓派开机自动运行Python客户端](https://blog.csdn.net/panwen1111/article/details/80467128)

[MQTT（五）EMQ开源MQTT消息服务器](https://blog.csdn.net/panwen1111/article/details/81067689)

-

## 1 什么是 MQTT ？

> MQTT（Message Queuing Telemetry Transport，消息队列遥测传输）是 IBM 开发的一个即时通讯协议，有可能成为物联网的重要组成部分。MQTT 是基于二进制消息的发布/订阅编程模式的消息协议，如今已经成为 OASIS 规范，由于规范很简单，非常适合需要低功耗和网络带宽有限的 IoT 场景。[MQTT官网](http://mqtt.org/)

## 2 MQTTnet

[MQTTnet](https://github.com/chkr1011/MQTTnet) 是一个基于 MQTT 通信的高性能 .NET 开源库，它同时支持 MQTT 服务器端和客户端。而且作者也保持更新，目前支持新版的.NET core，这也是选择 MQTTnet 的原因。 MQTTnet 在 Github 并不是下载最多的 .NET 的 MQTT 开源库，其他的还 [MqttDotNet](https://github.com/stevenlovegrove/MqttDotNet)、[nMQTT](https://github.com/markallanson/nmqtt)、[M2MQTT](https://github.com/eclipse/paho.mqtt.m2mqtt) 等

> MQTTnet is a high performance .NET library for MQTT based communication. It provides a MQTT client and a MQTT server (broker). The implementation is based on the documentation from [http://mqtt.org/](http://mqtt.org/).

## 3 创建项目并导入类库

这里我们使用 Visual Studio 2017 创建一个空解决方案，并在其中添加两个项目，即一个服务端和一个客户端，服务端项目模板选择最新的 .NET Core 控制台应用，客户端项目选择传统的 WinForm 窗体应用程序。.NET Core 项目模板如下图所示：   
![.NET Core 控制台应用](https://imgconvert.csdnimg.cn/aHR0cDovL2ltYWdlczIwMTcuY25ibG9ncy5jb20vYmxvZy8xMzEwMzEvMjAxNzEwLzEzMTAzMS0yMDE3MTAyNDEzNTkwMjIyMy0xNjE3NDA1NjgzLnBuZw?x-oss-process=image/format,png)

在解决方案在右键单击-选择“管理解决方案的 NuGet 程序包”-在“浏览”选项卡下面搜索 MQTTnet，为服务端项目和客户端项目都安装上 MQTTnet 库，当前最新稳定版为 2.4.0。项目结构如下图所示：   
![项目结构](https://imgconvert.csdnimg.cn/aHR0cDovL2ltYWdlczIwMTcuY25ibG9ncy5jb20vYmxvZy8xMzEwMzEvMjAxNzEwLzEzMTAzMS0yMDE3MTAzMDE5Mzc1MDM3MS0xNzM1OTI1ODI5LnBuZw?x-oss-process=image/format,png)

## 4 服务端

MQTT 服务端主要用于与多个客户端保持连接，并处理客户端的发布和订阅等逻辑。一般很少直接从服务端发送消息给客户端（可以使用 `mqttServer.Publish(appMsg);` 直接发送消息），多数情况下服务端都是转发主题匹配的客户端消息，在系统中起到一个中介的作用。

### 4.1 创建服务端并启动

创建服务端最简单的方式是采用 `MqttServerFactory` 对象的 `CreateMqttServer` 方法来实现，该方法需要一个`MqttServerOptions` 参数。

```csharp
var options = new MqttServerOptions();var mqttServer = new MqttServerFactory().CreateMqttServer(options);
```

通过上述方式创建了一个 `IMqttServer` 对象后，调用其 `StartAsync` 方法即可启动 MQTT 服务。值得注意的是：之前版本采用的是 `Start` 方法，作者也是紧跟 C# 语言新特性，能使用异步的地方也都改为异步方式。

```csharp
await mqttServer.StartAsync();
```

4.2 验证客户端

在 `MqttServerOptions` 选项中，你可以使用 `ConnectionValidator` 来对客户端连接进行验证。比如客户端ID标识 `ClientId`，用户名 `Username` 和密码 `Password` 等。

```csharp
var options = new MqttServerOptions{    ConnectionValidator = c =>    {        if (c.ClientId.Length < 10)        {            return MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;        }         if (c.Username != "xxx" || c.Password != "xxx")        {            return MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;        }         return MqttConnectReturnCode.ConnectionAccepted;    }};
```

4.3 相关事件

服务端支持 `ClientConnected`、`ClientDisconnected` 和 `ApplicationMessageReceived` 事件，分别用来检查客户端连接、客户端断开以及接收客户端发来的消息。

其中 `ClientConnected` 和 `ClientDisconnected` 事件的事件参数一个客户端连接对象 `ConnectedMqttClient`，通过该对象可以获取客户端ID标识 `ClientId` 和 MQTT 版本 `ProtocolVersion`。

`ApplicationMessageReceived` 的事件参数包含了客户端ID标识 `ClientId` 和 MQTT 应用消息 `MqttApplicationMessage` 对象，通过该对象可以获取主题 `Topic`、QoS `QualityOfServiceLevel` 和消息内容 `Payload` 等信息。

## 5 客户端

MQTT 与 HTTP 不同，后者是基于请求/响应方式的，服务器端无法直接发送数据给客户端。而 MQTT 是基于发布/订阅模式的,所有的客户端均与服务端保持连接状态。

那么客户端之间是如何通信的呢？

具体逻辑是：某些客户端向服务端订阅它感兴趣（主题）的消息，另一些客户端向服务端发布（主题）消息，服务端将订阅和发布的主题进行匹配，并将消息转发给匹配通过的客户端。

### 5.1 创建客户端并连接

使用 MQTTnet 创建 MQTT 也非常简单，只需要使用 `MqttClientFactory` 对象的 `CreateMqttClient` 方法即可。

```csharp
var mqttClient = new MqttClientFactory().CreateMqttClient();
```

创建客户端对象后，调用其异步方法 `ConnectAsync` 来连接到服务端。

```csharp
await mqttClient.ConnectAsync(options);
```

调用该方法时需要传递一个 `MqttClientTcpOptions` 对象（之前的版本是在创建对象时使用该选项），该选项包含了客户端ID标识 `ClientId`、服务端地址（可以使用IP地址或域名）`Server`、端口号 `Port`、用户名 `UserName`、密码 `Password` 等信息。

```csharp
var options = new MqttClientTcpOptions{    Server = "127.0.0.1",    ClientId = "c001",    UserName = "u001",    Password = "p001",    CleanSession = true};
```

5.2 相关事件

客户端支持 `Connected`、`Disconnected` 和 `ApplicationMessageReceived` 事件，用来处理客户端与服务端连接、客户端从服务端断开以及客户端收到消息的事情。

### 5.2 订阅消息

客户端连接到服务端之后，可以使用 `SubscribeAsync` 异步方法订阅消息，该方法可以传入一个可枚举或可变参数的主题过滤器 `TopicFilter` 参数，主题过滤器包含主题名和 QoS 等级。

```csharp
mqttClient.SubscribeAsync(new List<TopicFilter> {    new TopicFilter("家/客厅/空调/#", MqttQualityOfServiceLevel.AtMostOnce)});
```

5.3 发布消息

发布消息前需要先构建一个消息对象 `MqttApplicationMessage`，最直接的方法是使用其实构造函数，传入主题、内容、Qos 等参数。

```csharp
mqttClient.SubscribeAsync(new List<TopicFilter> {    new TopicFilter("家/客厅/空调/#", MqttQualityOfServiceLevel.AtMostOnce)});
```

得到 `MqttApplicationMessage` 消息对象后，通过客户端对象调用其 `PublishAsync` 异步方法进行消息发布。

```csharp
mqttClient.PublishAsync(appMsg);
```

6 跟踪消息

`MQTTnet` 提供了一个静态类 `MqttNetTrace` 来对消息进行跟踪，该类可用于服务端和客户端。`MqttNetTrace` 的事件`TraceMessagePublished` 用于跟踪服务端和客户端应用的日志消息，比如启动、停止、心跳、消息订阅和发布等。事件参数`MqttNetTraceMessagePublishedEventArgs` 包含了线程ID `ThreadId`、来源 `Source`、日志级别 `Level`、日志消息 `Message`、异常信息 `Exception` 等。

```csharp
MqttNetTrace.TraceMessagePublished += MqttNetTrace_TraceMessagePublished; private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetTraceMessagePublishedEventArgs e){    Console.WriteLine($">> 线程ID：{e.ThreadId} 来源：{e.Source} 跟踪级别：{e.Level} 消息: {e.Message}");     if (e.Exception != null)    {        Console.WriteLine(e.Exception);    }}
```

同时 `MqttNetTrace` 类还提供了4个不同消息等级的静态方法，`Verbose`、`Information`、`Warning` 和 `Error`，用于给出不同级别的日志消息，该消息将会在 `TraceMessagePublished` 事件中输出，你可以使用 `e.Level` 进行过虑。

## 7 运行效果

以下分别是服务端、客户端1和客户端2的运行效果，其中客户端1和客户端2只是同一个项目运行了两个实例。客户端1用于订阅传感器的“温度”数据，并模拟上位机（如 APP 等）发送开关控制命令；客户端2订阅上位机传来的“开关”控制命令，并模拟温度传感器上报温度数据。

### 7.1 服务端

![服务端](https://imgconvert.csdnimg.cn/aHR0cDovL2ltYWdlczIwMTcuY25ibG9ncy5jb20vYmxvZy8xMzEwMzEvMjAxNzEwLzEzMTAzMS0yMDE3MTAzMDE5NDk0ODc0Ni0yMTI4OTg4NTgyLnBuZw?x-oss-process=image/format,png)

### 7.2 客户端1

![客户端1](https://imgconvert.csdnimg.cn/aHR0cDovL2ltYWdlczIwMTcuY25ibG9ncy5jb20vYmxvZy8xMzEwMzEvMjAxNzEwLzEzMTAzMS0yMDE3MTAzMDE5NDk1ODQ0OS0xMDIzMjMzMjU0LnBuZw?x-oss-process=image/format,png)

### 7.2 客户端2

![客户端2](https://imgconvert.csdnimg.cn/aHR0cDovL2ltYWdlczIwMTcuY25ibG9ncy5jb20vYmxvZy8xMzEwMzEvMjAxNzEwLzEzMTAzMS0yMDE3MTAzMDE5NTAwODYyMS0yODM0MjQyMjIucG5n?x-oss-process=image/format,png)

## 8 Demo代码

### 8.1 服务端代码

```csharp
using MQTTnet;using MQTTnet.Core.Adapter;using MQTTnet.Core.Diagnostics;using MQTTnet.Core.Protocol;using MQTTnet.Core.Server;using System;using System.Text;using System.Threading; namespace MqttServerTest{    class Program    {        private static MqttServer mqttServer = null;         static void Main(string[] args)        {            MqttNetTrace.TraceMessagePublished += MqttNetTrace_TraceMessagePublished;            new Thread(StartMqttServer).Start();             while (true)            {                var inputString = Console.ReadLine().ToLower().Trim();                 if (inputString == "exit")                {                    mqttServer?.StopAsync();                    Console.WriteLine("MQTT服务已停止！");                    break;                }                else if (inputString == "clients")                {                    foreach (var item in mqttServer.GetConnectedClients())                    {                        Console.WriteLine($"客户端标识：{item.ClientId}，协议版本：{item.ProtocolVersion}");                    }                }                else                {                    Console.WriteLine($"命令[{inputString}]无效！");                }            }        }         private static void StartMqttServer()        {            if (mqttServer == null)            {                try                {                    var options = new MqttServerOptions                    {                        ConnectionValidator = p =>                        {                            if (p.ClientId == "c001")                            {                                if (p.Username != "u001" || p.Password != "p001")                                {                                    return MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;                                }                            }                             return MqttConnectReturnCode.ConnectionAccepted;                        }                    };                     mqttServer = new MqttServerFactory().CreateMqttServer(options) as MqttServer;                    mqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;                    mqttServer.ClientConnected += MqttServer_ClientConnected;                    mqttServer.ClientDisconnected += MqttServer_ClientDisconnected;                }                catch (Exception ex)                {                    Console.WriteLine(ex.Message);                    return;                }            }             mqttServer.StartAsync();            Console.WriteLine("MQTT服务启动成功！");        }         private static void MqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)        {            Console.WriteLine($"客户端[{e.Client.ClientId}]已连接，协议版本：{e.Client.ProtocolVersion}");        }         private static void MqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)        {            Console.WriteLine($"客户端[{e.Client.ClientId}]已断开连接！");        }         private static void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)        {            Console.WriteLine($"客户端[{e.ClientId}]>> 主题：{e.ApplicationMessage.Topic} 负荷：{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");        }         private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetTraceMessagePublishedEventArgs e)        {            /*Console.WriteLine($">> 线程ID：{e.ThreadId} 来源：{e.Source} 跟踪级别：{e.Level} 消息: {e.Message}");            if (e.Exception != null)            {                Console.WriteLine(e.Exception);            }*/        }    }}
```

### 8.2 客户端代码

```csharp
using MQTTnet;using MQTTnet.Core;using MQTTnet.Core.Client;using MQTTnet.Core.Packets;using MQTTnet.Core.Protocol;using System;using System.Collections.Generic;using System.Text;using System.Threading.Tasks;using System.Windows.Forms; namespace MqttClientWin{    public partial class FmMqttClient : Form    {        private MqttClient mqttClient = null;         public FmMqttClient()        {            InitializeComponent();             Task.Run(async () => { await ConnectMqttServerAsync(); });        }         private async Task ConnectMqttServerAsync()        {            if (mqttClient == null)            {                mqttClient = new MqttClientFactory().CreateMqttClient() as MqttClient;                mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;                mqttClient.Connected += MqttClient_Connected;                mqttClient.Disconnected += MqttClient_Disconnected;            }             try            {                var options = new MqttClientTcpOptions                {                    Server = "127.0.0.1",                    ClientId = Guid.NewGuid().ToString().Substring(0, 5),                    UserName = "u001",                    Password = "p001",                    CleanSession = true                };                 await mqttClient.ConnectAsync(options);            }            catch (Exception ex)            {                Invoke((new Action(() =>                {                    txtReceiveMessage.AppendText($"连接到MQTT服务器失败！" + Environment.NewLine + ex.Message + Environment.NewLine);                })));            }        }         private void MqttClient_Connected(object sender, EventArgs e)        {            Invoke((new Action(() =>            {                txtReceiveMessage.AppendText("已连接到MQTT服务器！" + Environment.NewLine);            })));        }         private void MqttClient_Disconnected(object sender, EventArgs e)        {            Invoke((new Action(() =>            {                txtReceiveMessage.AppendText("已断开MQTT连接！" + Environment.NewLine);            })));        }         private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)        {            Invoke((new Action(() =>            {                txtReceiveMessage.AppendText($">> {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}{Environment.NewLine}");            })));        }         private void BtnSubscribe_ClickAsync(object sender, EventArgs e)        {            string topic = txtSubTopic.Text.Trim();             if (string.IsNullOrEmpty(topic))            {                MessageBox.Show("订阅主题不能为空！");                return;            }             if (!mqttClient.IsConnected)            {                MessageBox.Show("MQTT客户端尚未连接！");                return;            }             mqttClient.SubscribeAsync(new List<TopicFilter> {                new TopicFilter(topic, MqttQualityOfServiceLevel.AtMostOnce)            });             txtReceiveMessage.AppendText($"已订阅[{topic}]主题" + Environment.NewLine);            txtSubTopic.Enabled = false;            btnSubscribe.Enabled = false;        }         private void BtnPublish_Click(object sender, EventArgs e)        {            string topic = txtPubTopic.Text.Trim();             if (string.IsNullOrEmpty(topic))            {                MessageBox.Show("发布主题不能为空！");                return;            }             string inputString = txtSendMessage.Text.Trim();            var appMsg = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(inputString), MqttQualityOfServiceLevel.AtMostOnce, false);            mqttClient.PublishAsync(appMsg);        }    }}
```

### 9 本文的Demo下载地址

[点击下载 Demo](https://download.csdn.net/download/panwen1111/11018592)

[https://download.csdn.net/download/panwen1111/11018592](https://download.csdn.net/download/panwen1111/11018592)

-

## pw的其他原创文章导航

- ### **C#的MQTT系列**
    

[MQTT（一）C#使用 MQTTnet 快速实现 MQTT 通信（文末有完整Demo下载）](https://blog.csdn.net/panwen1111/article/details/79245161)

[MQTT（二）在windows64位上安装Python环境](https://blog.csdn.net/panwen1111/article/details/79248328)

[MQTT（三）Python客户端+net客户端+net服务端 简单通信](https://blog.csdn.net/lordwish/article/details/85006228)

[MQTT（四）树莓派开机自动运行Python客户端](https://blog.csdn.net/panwen1111/article/details/80467128)

[MQTT（五）EMQ开源MQTT消息服务器](https://blog.csdn.net/panwen1111/article/details/81067689)

- ### C#的阿里物联网平台
    

[阿里物联网平台（一）Windows系统+VS2017 模拟设备端接入](https://blog.csdn.net/panwen1111/article/details/88365636)

[阿里物联网平台（二）.net 实现移动端（WEB、HTML）与设备端通讯](https://blog.csdn.net/panwen1111/article/details/88367428)

- ### 落地项目
    

[落地项目-智慧海绵城市](https://blog.csdn.net/panwen1111/article/details/88368699)

[落地项目-智能焊机，钢塑管行业物联网应用](https://blog.csdn.net/panwen1111/article/details/88368699)

[手持安卓小票打印一体机，小票打印应用](https://blog.csdn.net/panwen1111/article/details/95050338)

[省城建设计院智慧海绵城市示范工程](https://blog.csdn.net/panwen1111/article/details/103184056)