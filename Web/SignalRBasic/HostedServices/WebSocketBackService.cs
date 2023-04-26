using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.WebSockets;
using System.Runtime;

namespace SignalRBasic.HostedServices
{
    public class WebSocketBackService : BackgroundService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<WebSocketBackService> _logger;

        public WebSocketBackService(IHttpContextAccessor context,
                                     ILogger<WebSocketBackService> logger)
        {
            _context = context;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"WebSocketBackService is starting.");


            //// 注册 WebSocket 端点
            //_context.MapGet("/ws/backservice/WebSocketTest_GetCount", async context =>
            //{
            //    if (context.WebSockets.IsWebSocketRequest)
            //    {
            //        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //        await Echo(webSocket);
            //    }
            //    else
            //    {
            //        context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //    }
            //});

            // 后台耗时的任务
            while (!stoppingToken.IsCancellationRequested)
            {
            }

            _logger.LogDebug($"WebSocket background task is stopping.");
        }

        private static async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
