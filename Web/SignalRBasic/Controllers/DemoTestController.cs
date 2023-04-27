using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRBasic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoTestController : ControllerBase
    {
        /// <summary>
        ///  Polling 用于轮询测试
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public IActionResult ResetCount()
        {
            _count = 0;
            return Ok();
        }

        #region Polling
        /// <summary>
        ///  Polling 用于轮询测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public IActionResult PollingTest_GetCount(int id)
        {
            var count = GetLastedCount();
            if (count > 10)
            {
                return Ok(new { id, count, finished = true });
            }

            if (count > 8)
            {
                return NoContent();
                //return NotFound(); // 404
            }

            if (count > 3)
            {
                return Ok(new { id, count });
            }
            return NoContent();  // 404
            //return NotFound();  // 404
        }
        #endregion

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
            Thread.Sleep(1000 * new Random().Next(1,5) / 2);

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

        #region Server Sent Events (SSE)
        /// <summary>
        /// SSE 测试
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("[action]/{id}")]
        public async Task SSETest_GetCount(int id)
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            Response.ContentType = "text/event-stream";
            int count;
            do
            {
                Thread.Sleep(1000 * new Random().Next(1, 10) / 2);

                count = GetLastedCount();
                if (count % 3 == 0)
                {
                    //注意SSE返回数据的只能是字符串, 而且以data:开头, 后边要跟着换行符号, 否则EventSource会失败.
                    await HttpContext.Response.WriteAsync($"data:{JsonSerializer.Serialize(new { id, count, finished = count>15 }, jsonOptions)}\n\n");
                    await HttpContext.Response.Body.FlushAsync();
                }

            } while (count <= 21);

            // 直接返回，或 Body.Close()关闭，都会引发 js 中 EventSource 的 error 事件。
            // 需要在error事件方法内判断错误原因，如果是连接关闭，则关闭EventSource。否则，客户端默认会一直重新连接
            //return Content($"data:{JsonSerializer.Serialize(new { id, count, finished = count > 15 }, jsonOptions)}\n\n");

            // 直接关闭，js 的 EventSource 会 onerror，而不是关闭SSE
            Response.Body.Close();
        }
        #endregion

        #region WebSocket
        /// <summary>
        /// WebSocket 测试
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("[action]")]
        public async Task WebSocketTest_GetCount()
        {
            // 仅处理 WebSocket 通信
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await WebSocketHandle_Echo(webSocket);
            }
            else
            {
                // 也可以考虑同时支持普通的http请求
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest; // 400;
            }
        }
        /// <summary>
        /// 处理WebSocket，接收和发送数据
        /// Echo实现将接收的数据返回客户端
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private async Task WebSocketHandle_Echo(WebSocket webSocket)
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            try
            {
                var buffer = new byte[1024 * 4];
                var receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
                // 正确状态才获取
                //if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
                //{
                //    receiveResult = await webSocket.ReceiveAsync(
                //        new ArraySegment<byte>(buffer), CancellationToken.None);
                //}

                while (!receiveResult.CloseStatus.HasValue)
                {
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                        receiveResult.MessageType,
                        receiveResult.EndOfMessage,
                        CancellationToken.None);

                    var received = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);

                    #region echo的基础上额外发送一些数据
                    int count;
                    do
                    {
                        Thread.Sleep(1000 * new Random().Next(1, 10) / 2);

                        count = GetLastedCount();
                        if (count % 3 == 0)
                        {
                            var jsonStr = JsonSerializer.Serialize(new { received, count, finished = count > 15 }, jsonOptions);
                            await webSocket.SendAsync(
                                    buffer: new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(jsonStr), offset: 0, count: jsonStr.Length),
                                    messageType: WebSocketMessageType.Text,
                                    endOfMessage: true,
                                    cancellationToken: CancellationToken.None
                                );
                            break;
                        }

                    } while (count <= 21);
                    #endregion

                    receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                await webSocket.CloseAsync(
                    receiveResult.CloseStatus.Value,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                try
                {
                    if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived || webSocket.State == WebSocketState.CloseSent)
                    {
                        //await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done", CancellationToken.None);
                        await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, CancellationToken.None);
                    }
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine(ex2.Message);
                }
            }
        }
        #endregion

        private static int _count;
        public int GetLastedCount()
        {
            _count++;
            return _count;
        }

        // ------------------------------------------
        // ------------------------------------------
        // ------------------------------------------

        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
