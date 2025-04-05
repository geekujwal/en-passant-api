using Edison.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Controllers;

#region snippet_Controller_Connect
public class WebSocketController : ControllerBase
{
    // ws://localhost:5003/ws/2
    private readonly IWebSocketService _webSocketService;
    private readonly IRedisService _redisService;
    public WebSocketController(IWebSocketService webSocketService, IRedisService redisService)
    {
        _webSocketService = webSocketService;
        _redisService = redisService;
    }

    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString();

            await _webSocketService.AddSocketAsync(socketId, webSocket);

            await _redisService.SetAsync(socketId, true, TimeSpan.FromDays(1));

            await _webSocketService.ReceiveAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    #endregion
}