namespace API.Models.DTOs
{
    public class ChessAnalysisResponse
    {
        public string CurrentPosition { get; set; } = string.Empty;
        public bool IsWhiteTurn { get; set; }
        public int Evaluation { get; set; }
        public string? BestMove { get; set; }
        public string? BestMoveNotation { get; set; }
        public List<string> PrincipalVariation { get; set; } = new();
        public int NodesExplored { get; set; }
        public int NodesPruned { get; set; }
        public double ComputationTimeMs { get; set; }
        public List<MoveAnalysis> TopMoves { get; set; } = new();
        public string ResultMessage { get; set; } = string.Empty;
    }

    public class MoveAnalysis
    {
        public string Move { get; set; } = string.Empty;
        public string Notation { get; set; } = string.Empty;
        public int Evaluation { get; set; }
        public string EvaluationDescription { get; set; } = string.Empty;
    }
}

