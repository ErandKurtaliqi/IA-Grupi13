namespace API.Models
{
    public class SATRequest
    {
        public int TotalGuests { get; set; } = 100;
        public int TotalTables { get; set; } = 10;
        public int SeatsPerTable { get; set; } = 10;
        public List<GuestPair>? CannotSitTogether { get; set; }
        public List<GuestPair>? MustSitTogether { get; set; }
        public List<Guest>? Guests { get; set; }
        public int MaxGenderDifference { get; set; } = 3;
    }

    public class GuestPair
    {
        public int Guest1 { get; set; }
        public int Guest2 { get; set; }
    }

    public class Guest
    {
        public int Id { get; set; }
        public string Gender { get; set; } = "M"; // M or F
    }

    public class SATResponse
    {
        public bool Success { get; set; }
        public Dictionary<int, List<int>>? TableAssignments { get; set; } // Table -> List of GuestIds
        public string? SATFormula { get; set; }
        public double ExecutionTimeMs { get; set; }
        public string? Message { get; set; }
        public int TotalClauses { get; set; }
    }
}

