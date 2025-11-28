namespace API.Models.DTOs
{
    public class ChessAnalysisRequest
    {
        public string FEN { get; set; } = "2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1";
        public int Depth { get; set; } = 4;
    }
}

