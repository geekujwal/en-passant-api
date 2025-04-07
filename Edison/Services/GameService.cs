using Edison.Abstractions;
using Edison.Contracts;

namespace Edison.Services
{
    public class GameService : IGameService
    {
        private readonly IWebSocketService _webSocketService;
        public GameService(IWebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        public async Task<string> CreateGameAsync(string playerWhiteId, string playerBlackId)
        {
            var gameId = Guid.NewGuid().ToString();
            await _webSocketService.BroadcastMessageAsync("Game created");
            return gameId;
        }

        public Task EndGameAsync(string gameId, string reason)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCurrentTurnAsync(string gameId)
        {
            throw new NotImplementedException();
        }

        public Task<GameState> GetGameStateAsync(string gameId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetLegalMovesAsync(string gameId, string playerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> JoinGameAsync(string gameId, string playerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MakeMoveAsync(string gameId, string playerId, string moveNotation)
        {
            throw new NotImplementedException();
        }
    }
}