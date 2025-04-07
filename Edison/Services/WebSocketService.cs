using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Edison.Abstractions;
using Edison.Contracts;

namespace Edison.Services
{
    public class WebSocketService : IWebSocketService
    {
        private static Dictionary<string, WebSocket> _clients = [];

        public async Task AddSocketAsync(string socketId, WebSocket socket)
        {
            _clients.Add(socketId, socket);
            await Task.Delay(2); // todo need to save this socketId allocating with userId in redis db
        }

        public async Task BroadcastMessageAsync(string message)
        {
            foreach (var kvp in _clients)
            {
                await SendMessageAsync(kvp.Key, message);
            }
        }

        public async Task CloseSocketAsync(string socketId, WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure, string statusDescription = "Closed")
        {
            if (!_clients.TryGetValue(socketId, out var socket))
                return;

            if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
            {
                await socket.CloseAsync(closeStatus, statusDescription, CancellationToken.None);
            }

            _clients.Remove(socketId);
        }

        public IEnumerable<WebSocket> GetAllSockets()
        {
            return _clients.Values;
        }

        public WebSocket GetSocketById(string socketId)
        {
            _clients.TryGetValue(socketId, out var webSocket);
            return webSocket;
        }

        public Task HandleMessageAsync(string socketId, string message)
        {
            throw new NotImplementedException();
        }

        public bool IsSocketOpen(string socketId)
        {
            if (_clients.TryGetValue(socketId, out var socket))
            {
                return socket.State == WebSocketState.Open;
            }
            return false;
        }

        public async Task<WebSocketMessage> ReceiveAsync(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() },
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    return JsonSerializer.Deserialize<WebSocketMessage>(message, options);
                }

            } while (!result.CloseStatus.HasValue);

            foreach (var kvp in _clients)
            {
                await CloseSocketAsync(kvp.Key, result.CloseStatus.Value, result.CloseStatusDescription);
            }
            return new();
        }

        public async Task HandleMessageAsync(WebSocket socket, string message)
        {
            Console.WriteLine($"Received message: {message}");
            await Task.Delay(1);
        }

        public async Task RemoveSocketAsync(string socketId)
        {
            _clients.Remove(socketId);
            await Task.Delay(2);
            // todo remove from db
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            if (_clients.TryGetValue(socketId, out var socket))
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(
                    new ArraySegment<byte>(bytes, 0, bytes.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }

        public bool SocketExists(string socketId)
        {
            return _clients.ContainsKey(socketId);
        }
    }
}