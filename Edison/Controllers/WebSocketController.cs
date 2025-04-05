using Edison.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Controllers;

#region snippet_Controller_Connect
public class WebSocketController : ControllerBase
{
    // ws://localhost:5003/ws/2
    private readonly IWebSocketService _webSocketService;
    public WebSocketController(IWebSocketService webSocketService)
    {
        _webSocketService = webSocketService;
    }

    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString();

            await _webSocketService.AddSocketAsync(socketId, webSocket);

            // var buffer = new byte[1024 * 4];
            // WebSocketReceiveResult result;

            // do
            // {
            //     result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //     if (result.MessageType == WebSocketMessageType.Text)
            //     {
            //         await _webSocketService.BroadcastMessageAsync("Hello world");
            //         await _webSocketService.ReceiveAsync(webSocket, result, buffer);
            //     }
            // } while (!result.CloseStatus.HasValue);
            await _webSocketService.ReceiveAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    #endregion
}