namespace API.Models
{
    public class Move
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
        public char Piece { get; set; }
        public char CapturedPiece { get; set; }

        public Move(int fromRow, int fromCol, int toRow, int toCol, char piece, char capturedPiece = '.')
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Piece = piece;
            CapturedPiece = capturedPiece;
        }

        public string ToNotation()
        {
            char[] files = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            return $"{char.ToUpper(Piece)}{files[FromCol]}{8 - FromRow} → {files[ToCol]}{8 - ToRow}";
        }

        public override string ToString()
        {
            return ToNotation();
        }
    }
}

