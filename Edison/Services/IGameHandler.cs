using Edison.Abstractions;
using Edison.Contracts;

namespace Edison.Services
{
    public class GameHandler : IGameHandler
    {
        private readonly IGameService _gameService;
        public GameHandler(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task HandleGameAsync(GameState gameState, CancellationToken cancellationToken)
        {
            await _gameService.CreateGameAsync("playerAId", "playerBId");
            await Task.Delay(2);
        }
    }
}