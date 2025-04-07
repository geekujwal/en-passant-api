namespace Edison.Contracts
{
    public class GameState
    {
        public string GameId { get; set; }

        public string PlayerWhiteId { get; set; }

        public string PlayerBlackId { get; set; }

        public string Fen { get; set; }

        public string CurrentTurnPlayerId { get; set; }

        public List<string> MoveHistory { get; set; } = [];

        public bool IsGameOver { get; set; }

        public string Result { get; set; }
    }
}