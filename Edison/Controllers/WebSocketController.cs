using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Edison.Controllers;

#region snippet_Controller_Connect
public class WebSocketController : ControllerBase
{
    private static Dictionary<string, WebSocket> _clients = [];

    // ws://localhost:5003/ws/2

    [Route("/ws/{clientId}")]
    public async Task Get(string clientId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _clients[clientId] = webSocket;
            await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    #endregion

    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            if (_clients.TryGetValue("2", out var socket))
            {
                var bytes = Encoding.UTF8.GetBytes("Hello user123!");
                await socket.SendAsync(
                    new ArraySegment<byte>(bytes, 0, bytes.Length),
                    receiveResult.MessageType,
                    true,
                    CancellationToken.None);
            }


            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}