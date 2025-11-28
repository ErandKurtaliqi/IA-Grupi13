namespace API.Models
{
    public class ChessBoard
    {
        public char[,] Board { get; set; }
        public bool IsWhiteTurn { get; set; }

        public ChessBoard()
        {
            Board = new char[8, 8];
            IsWhiteTurn = true;
        }

        public ChessBoard Clone()
        {
            var clone = new ChessBoard
            {
                IsWhiteTurn = this.IsWhiteTurn,
                Board = new char[8, 8]
            };

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    clone.Board[i, j] = this.Board[i, j];
                }
            }

            return clone;
        }

        public void InitializeFromFEN(string fen)
        {
            var parts = fen.Split(' ');
            var position = parts[0];
            var turn = parts.Length > 1 ? parts[1] : "w";

            IsWhiteTurn = turn == "w";

            // Initialize empty board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i, j] = '.';
                }
            }

            var rows = position.Split('/');
            for (int i = 0; i < rows.Length; i++)
            {
                int col = 0;
                foreach (var c in rows[i])
                {
                    if (char.IsDigit(c))
                    {
                        col += (c - '0');
                    }
                    else
                    {
                        Board[i, col] = c;
                        col++;
                    }
                }
            }
        }

        public string ToFEN()
        {
            var fen = "";
            for (int i = 0; i < 8; i++)
            {
                int emptyCount = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] == '.')
                    {
                        emptyCount++;
                    }
                    else
                    {
                        if (emptyCount > 0)
                        {
                            fen += emptyCount.ToString();
                            emptyCount = 0;
                        }
                        fen += Board[i, j];
                    }
                }
                if (emptyCount > 0)
                {
                    fen += emptyCount.ToString();
                }
                if (i < 7) fen += "/";
            }

            fen += IsWhiteTurn ? " w - - 0 1" : " b - - 0 1";
            return fen;
        }
    }
}

