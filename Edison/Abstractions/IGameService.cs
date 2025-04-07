using Edison.Contracts;

namespace Edison.Abstractions
{
    public interface IGameService
    {
        Task<string> CreateGameAsync(string playerWhiteId, string playerBlackId);

        Task<bool> JoinGameAsync(string gameId, string playerId);

        Task<bool> MakeMoveAsync(string gameId, string playerId, string moveNotation);

        Task<GameState> GetGameStateAsync(string gameId);

        Task<IEnumerable<string>> GetLegalMovesAsync(string gameId, string playerId);

        Task EndGameAsync(string gameId, string reason);

        Task<string> GetCurrentTurnAsync(string gameId);
    }
}