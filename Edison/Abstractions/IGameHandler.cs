using Edison.Contracts;

namespace Edison.Abstractions
{
    public interface IGameHandler
    {
        Task HandleGameAsync(GameState gameState, CancellationToken cancellationToken);
    }
}
