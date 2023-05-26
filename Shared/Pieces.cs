namespace BlazorChess.Client.Pages
{
    public static class Pieces
    {
        public static string Pawn => "fa-sharp fa-solid fa-chess-pawn";
        public static string Knight => "fa-sharp fa-solid fa-chess-knight";
        public static string Bishop => "fa-sharp fa-solid fa-chess-bishop";
        public static string Rook => "fa-sharp fa-solid fa-chess-rook";
        public static string Queen => "fa-sharp fa-solid fa-chess-queen";
        public static string King => "fa-sharp fa-solid fa-chess-king";
        public static string Bounce => "fa-bounce";

        public static string InitialSetup(int row, int column)
        {
            if (row == 0)
            {
                if (column == 0 || column == 7)
                {
                    return Pawn;
                }
                else if (column == 1 || column == 6)
                {
                    return Pieces.Knight;
                }
                else if (column == 2 || column == 5)
                {
                    return Pieces.Bishop;
                }
                else if (column == 3)
                {
                    return Pieces.Queen;
                }
                else if (column == 4)
                {
                    return Pieces.King;
                }
            }
            else if (row == 1)
            {
                return Pieces.Pawn;
            }
            else if (row == 6)
            {
                return Pieces.Pawn;
            }
            else if (row == 7)
            {
                if (column == 0 || column == 7)
                {
                    return Pieces.Rook;
                }
                else if (column == 1 || column == 6)
                {
                    return Pieces.Knight;
                }
                else if (column == 2 || column == 5)
                {
                    return Pieces.Bishop;
                }
                else if (column == 3)
                {
                    return Pieces.Queen;
                }
                else if (column == 4)
                {
                    return Pieces.King;
                }
            }

            return "fa-sharp fa-solid";
        }

    }

}
