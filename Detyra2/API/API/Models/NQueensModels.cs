namespace API.Models
{
    public class NQueensRequest
    {
        public int N { get; set; }
        public List<Position>? BlockedPositions { get; set; }
        public string HeuristicType { get; set; } = "conflicts"; // "conflicts", "attacking", "combined"
    }

    public class Position
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }

    public class NQueensResponse
    {
        public bool Success { get; set; }
        public List<Position>? Solution { get; set; }
        public int NodesExplored { get; set; }
        public double ExecutionTimeMs { get; set; }
        public string HeuristicUsed { get; set; } = string.Empty;
        public string? Message { get; set; }
    }

    public class NQueensState
    {
        public int[]? Queens { get; set; } // Queens[row] = col
        public int N { get; set; }
        public HashSet<string>? BlockedPositions { get; set; }
        public int Row { get; set; }
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => GCost + HCost;

        public string GetKey()
        {
            return Queens != null ? string.Join(",", Queens) : "";
        }

        public NQueensState Clone()
        {
            return new NQueensState
            {
                Queens = Queens?.ToArray(),
                N = N,
                BlockedPositions = BlockedPositions,
                Row = Row,
                GCost = GCost,
                HCost = HCost
            };
        }
    }
}

