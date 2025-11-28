namespace API.Models
{
    public class KillerSudokuRequest
    {
        public int[][]? Grid { get; set; } // 9x9 grid, 0 means empty
        public List<Cage>? Cages { get; set; }
    }

    public class Cage
    {
        public int Sum { get; set; }
        public List<CellPosition>? Cells { get; set; }
    }

    public class CellPosition
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }

    public class KillerSudokuResponse
    {
        public bool Success { get; set; }
        public int[][]? Solution { get; set; }
        public int BacktrackCount { get; set; }
        public double ExecutionTimeMs { get; set; }
        public string? Message { get; set; }
    }
}

