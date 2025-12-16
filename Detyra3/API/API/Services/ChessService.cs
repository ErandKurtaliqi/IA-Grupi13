using API.Models;

namespace API.Services
{
    public class ChessService
    {
        private readonly Dictionary<char, int> _pieceValues = new()
        {
            { 'P', 100 },   // Pawn
            { 'N', 320 },   // Knight
            { 'B', 330 },   // Bishop
            { 'R', 500 },   // Rook
            { 'Q', 900 },   // Queen
            { 'K', 20000 }, // King
            { 'p', -100 },
            { 'n', -320 },
            { 'b', -330 },
            { 'r', -500 },
            { 'q', -900 },
            { 'k', -20000 }
        };

        public bool IsWhitePiece(char piece)
        {
            return char.IsUpper(piece) && piece != '.';
        }

        public bool IsBlackPiece(char piece)
        {
            return char.IsLower(piece) && piece != '.';
        }

        public int EvaluateBoard(ChessBoard board)
        {
            int score = 0;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    char piece = board.Board[row, col];
                    if (piece != '.')
                    {
                        score += _pieceValues[piece];

                        // Add positional bonuses
                        score += GetPositionalBonus(piece, row, col);
                    }
                }
            }

            return score;
        }

        private int GetPositionalBonus(char piece, int row, int col)
        {
            // Central control bonus
            int centerBonus = 0;
            if (row >= 2 && row <= 5 && col >= 2 && col <= 5)
            {
                centerBonus = 10;
            }
            if (row >= 3 && row <= 4 && col >= 3 && col <= 4)
            {
                centerBonus = 20;
            }

            char lowerPiece = char.ToLower(piece);
            bool isWhite = char.IsUpper(piece);

            switch (lowerPiece)
            {
                case 'n': // Knight - prefer center
                    return isWhite ? centerBonus : -centerBonus;
                case 'q': // Queen - prefer center and mobility
                    return isWhite ? centerBonus / 2 : -centerBonus / 2;
                case 'k': // King - prefer corners in endgame
                    int kingBonus = (row == 0 || row == 7 || col == 0 || col == 7) ? 5 : 0;
                    return isWhite ? kingBonus : -kingBonus;
                default:
                    return 0;
            }
        }

        public List<Move> GenerateAllMoves(ChessBoard board, bool forWhite)
        {
            var moves = new List<Move>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    char piece = board.Board[row, col];
                    if (piece == '.') continue;

                    bool isPieceWhite = IsWhitePiece(piece);
                    if (isPieceWhite != forWhite) continue;

                    moves.AddRange(GenerateMovesForPiece(board, row, col, piece));
                }
            }

            return moves;
        }

        private List<Move> GenerateMovesForPiece(ChessBoard board, int row, int col, char piece)
        {
            var moves = new List<Move>();
            char lowerPiece = char.ToLower(piece);

            switch (lowerPiece)
            {
                case 'k':
                    GenerateKingMoves(board, row, col, piece, moves);
                    break;
                case 'q':
                    GenerateQueenMoves(board, row, col, piece, moves);
                    break;
                case 'r':
                    GenerateRookMoves(board, row, col, piece, moves);
                    break;
                case 'b':
                    GenerateBishopMoves(board, row, col, piece, moves);
                    break;
                case 'n':
                    GenerateKnightMoves(board, row, col, piece, moves);
                    break;
                case 'p':
                    GeneratePawnMoves(board, row, col, piece, moves);
                    break;
            }

            return moves;
        }

        private void GenerateKingMoves(ChessBoard board, int row, int col, char piece, List<Move> moves)
        {
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = row + dx[i];
                int newCol = col + dy[i];

                if (IsValidMove(board, row, col, newRow, newCol, piece))
                {
                    moves.Add(new Move(row, col, newRow, newCol, piece, board.Board[newRow, newCol]));
                }
            }
        }

        private void GenerateQueenMoves(ChessBoard board, int row, int col, char piece, List<Move> moves)
        {
            // Queen moves like rook + bishop
            GenerateRookMoves(board, row, col, piece, moves);
            GenerateBishopMoves(board, row, col, piece, moves);
        }

        private void GenerateRookMoves(ChessBoard board, int row, int col, char piece, List<Move> moves)
        {
            // Horizontal and vertical
            int[][] directions = { new[] { 0, 1 }, new[] { 0, -1 }, new[] { 1, 0 }, new[] { -1, 0 } };

            foreach (var dir in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int newRow = row + dir[0] * i;
                    int newCol = col + dir[1] * i;

                    if (!IsInBounds(newRow, newCol)) break;

                    if (board.Board[newRow, newCol] == '.')
                    {
                        moves.Add(new Move(row, col, newRow, newCol, piece));
                    }
                    else
                    {
                        if (IsValidMove(board, row, col, newRow, newCol, piece))
                        {
                            moves.Add(new Move(row, col, newRow, newCol, piece, board.Board[newRow, newCol]));
                        }
                        break;
                    }
                }
            }
        }

        private void GenerateBishopMoves(ChessBoard board, int row, int col, char piece, List<Move> moves)
        {
            // Diagonal
            int[][] directions = { new[] { 1, 1 }, new[] { 1, -1 }, new[] { -1, 1 }, new[] { -1, -1 } };

            foreach (var dir in directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    int newRow = row + dir[0] * i;
                    int newCol = col + dir[1] * i;

                    if (!IsInBounds(newRow, newCol)) break;

                    if (board.Board[newRow, newCol] == '.')
                    {
                        moves.Add(new Move(row, col, newRow, newCol, piece));
                    }
                    else
                    {
                        if (IsValidMove(board, row, col, newRow, newCol, piece))
                        {
                            moves.Add(new Move(row, col, newRow, newCol, piece, board.Board[newRow, newCol]));
                        }
                        break;
                    }
                }
            }
        }

        private void GenerateKnightMoves(ChessBoard board, int row, int col, char piece, List<Move> moves)
        {
            int[] dx = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] dy = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = row + dx[i];
                int newCol = col + dy[i];

                if (IsValidMove(board, row, col, newRow, newCol, piece))
                {
                    moves.Add(new Move(row, col, newRow, newCol, piece, board.Board[newRow, newCol]));
                }
            }
        }

        private void GeneratePawnMoves(ChessBoard board, int row, int col, char piece, List<Move> moves)
        {
            bool isWhite = IsWhitePiece(piece);
            // White pawns move UP the board (decreasing row index), Black pawns move DOWN (increasing row index)
            int direction = isWhite ? -1 : 1;

            // Forward move
            int newRow = row + direction;
            if (IsInBounds(newRow, col) && board.Board[newRow, col] == '.')
            {
                moves.Add(new Move(row, col, newRow, col, piece));

                // Double move from starting position
                // White pawns start at row 6 (rank 2), Black pawns start at row 1 (rank 7)
                int startRow = isWhite ? 6 : 1;
                int doubleRow = row + 2 * direction;
                if (row == startRow && board.Board[doubleRow, col] == '.')
                {
                    moves.Add(new Move(row, col, doubleRow, col, piece));
                }
            }

            // Captures
            int[] captureCols = { col - 1, col + 1 };
            foreach (int captureCol in captureCols)
            {
                if (IsInBounds(newRow, captureCol) && board.Board[newRow, captureCol] != '.')
                {
                    if (IsValidMove(board, row, col, newRow, captureCol, piece))
                    {
                        moves.Add(new Move(row, col, newRow, captureCol, piece, board.Board[newRow, captureCol]));
                    }
                }
            }
        }

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }

        private bool IsValidMove(ChessBoard board, int fromRow, int fromCol, int toRow, int toCol, char piece)
        {
            if (!IsInBounds(toRow, toCol)) return false;

            char targetPiece = board.Board[toRow, toCol];
            if (targetPiece == '.') return true;

            // Cannot capture own piece
            bool isPieceWhite = IsWhitePiece(piece);
            bool isTargetWhite = IsWhitePiece(targetPiece);

            return isPieceWhite != isTargetWhite;
        }

        public void MakeMove(ChessBoard board, Move move)
        {
            board.Board[move.ToRow, move.ToCol] = move.Piece;
            board.Board[move.FromRow, move.FromCol] = '.';
            board.IsWhiteTurn = !board.IsWhiteTurn;
        }

        public void UndoMove(ChessBoard board, Move move)
        {
            board.Board[move.FromRow, move.FromCol] = move.Piece;
            board.Board[move.ToRow, move.ToCol] = move.CapturedPiece;
            board.IsWhiteTurn = !board.IsWhiteTurn;
        }

        public bool IsCheckmate(ChessBoard board, bool forWhite)
        {
            var moves = GenerateAllMoves(board, forWhite);
            return moves.Count == 0;
        }

        public bool IsGameOver(ChessBoard board)
        {
            var whiteMoves = GenerateAllMoves(board, true);
            var blackMoves = GenerateAllMoves(board, false);
            return whiteMoves.Count == 0 || blackMoves.Count == 0;
        }
    }
}

