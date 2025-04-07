using System.Text.Json.Serialization;

namespace Edison.Contracts;
public class WebSocketMessage
{
    [JsonPropertyName("event")]
    public SocketMessageEvent? Event { get; set; }

    [JsonPropertyName("payload")]
    public string Payload { get; set; }
}

public enum SocketMessageEvent
{
    Game,
    Chat
}
