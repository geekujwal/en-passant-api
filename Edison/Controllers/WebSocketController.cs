using System.Text.Json;
using Edison.Abstractions;
using Edison.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Controllers;

#region snippet_Controller_Connect
public class WebSocketController : ControllerBase
{
    private readonly IWebSocketService _webSocketService;
    private readonly IGameHandler _gameHandler;
    private readonly IRedisService _redisService;
    public WebSocketController(IWebSocketService webSocketService, IRedisService redisService, IGameHandler gameHandler)
    {
        _webSocketService = webSocketService;
        _redisService = redisService;
        _gameHandler = gameHandler;
    }

    // ws://localhost:5003/game
    [Route("/game")]
    public async Task Get(CancellationToken cancellationToken)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString();

            await _webSocketService.AddSocketAsync(socketId, webSocket);

            await _redisService.SetAsync(socketId, true, TimeSpan.FromDays(1));

            var data = await _webSocketService.ReceiveAsync(webSocket);
            switch (data.Event)
            {
                case Contracts.SocketMessageEvent.Game:
                    var game = JsonSerializer.Deserialize<GameState>(data.Payload);
                    await _gameHandler.HandleGameAsync(game, cancellationToken);
                    break;
                default:
                    throw new NotImplementedException("Method not implemented");
            }
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    #endregion
}
