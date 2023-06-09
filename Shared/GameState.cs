using BlazorChess.Client.Pages;

namespace BlazorChess.Shared
{
    public class GameState
    {
        public string? player1ConnectionID { get; set; }

        public string? player2ConnectionID { get; set; }

        public int turnNumber { get; set; } = 1; 

        public List<string>? capturedPieces { get; set; }

        public bool IsGameOver()
        {
            return capturedPieces?.Any(p => p.Equals("k", StringComparison.OrdinalIgnoreCase)) ?? false;
        }
        public Tile[,] Board { get; set; }

        public GameState()
        {
            Board = new Tile[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i, j] = new Tile
                    {
                        Piece = Pieces.InitialSetup(i, j),
                        PieceColor = i < 2 ? "black" : (i > 5 ? "white" : ""),
                        Row = i,
                        Col = j
                    };
                }
            }
        }

   

        public class Tile
        {
            public string? Piece { get; set; }
            public string? PieceColor { get; set; }
            public int Row { get; set; }
            public int Col { get; set; }
            public string Id => $"{Row}-{Col}";
        }
    }
}
