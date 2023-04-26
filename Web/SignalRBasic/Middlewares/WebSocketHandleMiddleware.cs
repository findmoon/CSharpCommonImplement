using System.Globalization;
using System.Net.WebSockets;

namespace SignalRBasic.Middlewares
{
    public class WebSocketHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws/middleware/WebSocketTest_GetCount")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await Echo(webSocket);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                // Call the next delegate/middleware in the pipeline.
                await _next(context);
            }

            
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

    public static class WebSocketHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketHandle(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketHandleMiddleware>();
        }
    }
}
