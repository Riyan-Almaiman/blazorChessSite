using BlazorChess.Shared;
using System.Text;

namespace BlazorChess.Client.Pages
{
    public static class FormatService
    {
        // Parse Standard Algebraic Notation (SAN) to get the destination tile of the move
        public static string ParseSan(string san)
        {
            // Remove check (+) and checkmate (#) symbols
            san = san.TrimEnd('+', '#');

            // Remove capture (x) symbol
            san = san.Replace("x", "");

            // If it's a pawn promotion, only keep the destination tile
            if (san.Contains("="))
            {
                san = san.Split('=')[0];
                return san[^2..] + '=' ;

            }
            else
            {
                return san[^2..];


            }

            // The destination tile is the last two characters
        }

        public static string ConvertNotation(int row, int col)
        {
            // Convert row and column to algebraic notation
            char file = (char)('a' + col);
            int rank = 8 - row;
            return $"{file}{rank}";
        }
        public static string ToFen(GameState gameState)
        {
            var fen = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                int empty = 0;
                for (int j = 0; j < 8; j++)
                {
                    var tile = gameState.Board[i, j];
                    if (tile.Piece == "fa-sharp fa-solid")
                    {
                        empty++;
                    }
                    else
                    {
                        if (empty > 0)
                        {
                            fen.Append(empty);
                            empty = 0;
                        }
                        // Add piece to the FEN string
                        // Convert your Piece representation to a single character (p,n,b,r,q,k)
                        var pieceChar = ConvertPieceToChar(tile.Piece);
                        fen.Append(tile.PieceColor == "black" ? char.ToLower(pieceChar) : pieceChar);
                    }
                }
                if (empty > 0)
                {
                    fen.Append(empty);
                }
                fen.Append("/");
            }
            // Trimming last '/'
            fen.Remove(fen.Length - 1, 1);

            // Add other required FEN fields: active color, castling availability, en passant square, halfmove clock and fullmove number
            string activeColor = gameState.turnNumber % 2 == 0 ? "b" : "w";  // Assuming ActivePlayer is a new field in GameState
            fen.Append($" {activeColor} KQkq - 0 1");
            return fen.ToString();
        }

        public static char ConvertPieceToChar(string piece)
        {
            if (piece == Pieces.Pawn)
            {
                return 'P';
            }
            else if (piece == Pieces.Rook)
            {
                return 'R';
            }
            else if (piece == Pieces.Knight)
            {
                return 'N';
            }
            else if (piece == Pieces.Bishop)
            {
                return 'B';
            }
            else if (piece == Pieces.Queen)
            {
                return 'Q';
            }
            else if (piece == Pieces.King)
            {
                return 'K';
            }
            else
            {
                throw new Exception("Unknown piece: " + piece);
            }
        }
    }
}
