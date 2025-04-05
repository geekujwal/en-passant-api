using System.Net.WebSockets;

namespace Edison.Abstractions
{
    public interface IWebSocketService
    {
        Task AddSocketAsync(string socketId, WebSocket socket);

        Task RemoveSocketAsync(string socketId);

        IEnumerable<WebSocket> GetAllSockets();

        bool IsSocketOpen(string socketId);

        Task SendMessageAsync(string socketId, string message);

        Task BroadcastMessageAsync(string message);

        // Task<WebSocketReceiveResult> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
        Task ReceiveAsync(WebSocket socket);

        Task HandleMessageAsync(string socketId, string message);

        Task CloseSocketAsync(string socketId, WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure, string statusDescription = "Closed");

        bool SocketExists(string socketId);
    }
}