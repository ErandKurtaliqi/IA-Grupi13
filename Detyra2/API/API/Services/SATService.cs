using API.Models;
using System.Diagnostics;
using System.Text;

namespace API.Services
{
    public class SATService : ISATService
    {
        public async Task<SATResponse> SolveAsync(SATRequest request)
        {
            return await Task.Run(() => Solve(request));
        }

        private SATResponse Solve(SATRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            
            // Generate SAT formula
            var (formula, totalClauses) = GenerateSATFormula(request);
            
            // Solve using backtracking with constraint propagation
            var assignment = new Dictionary<int, int>(); // Guest -> Table
            var tableGuests = new Dictionary<int, List<int>>(); // Table -> List of Guests
            
            for (int i = 0; i < request.TotalTables; i++)
            {
                tableGuests[i] = new List<int>();
            }

            bool success = SolveSAT(request, assignment, tableGuests, 0);
            
            stopwatch.Stop();

            if (success)
            {
                return new SATResponse
                {
                    Success = true,
                    TableAssignments = tableGuests,
                    SATFormula = formula,
                    TotalClauses = totalClauses,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    Message = "Successfully assigned all guests to tables"
                };
            }

            return new SATResponse
            {
                Success = false,
                SATFormula = formula,
                TotalClauses = totalClauses,
                ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                Message = "No valid assignment found"
            };
        }

        private bool SolveSAT(SATRequest request, Dictionary<int, int> assignment, 
            Dictionary<int, List<int>> tableGuests, int guestId)
        {
            if (guestId >= request.TotalGuests)
            {
                return CheckAllConstraints(request, tableGuests);
            }

            // Try assigning guest to each table
            for (int table = 0; table < request.TotalTables; table++)
            {
                if (tableGuests[table].Count >= request.SeatsPerTable)
                {
                    continue;
                }

                // Check immediate constraints before assigning
                if (!CanAssignGuestToTable(request, guestId, table, tableGuests))
                {
                    continue;
                }

                // Assign
                assignment[guestId] = table;
                tableGuests[table].Add(guestId);

                // Recurse
                if (SolveSAT(request, assignment, tableGuests, guestId + 1))
                {
                    return true;
                }

                // Backtrack
                assignment.Remove(guestId);
                tableGuests[table].Remove(guestId);
            }

            return false;
        }

        private bool CanAssignGuestToTable(SATRequest request, int guestId, int table, 
            Dictionary<int, List<int>> tableGuests)
        {
            var currentGuests = tableGuests[table];

            // Check "cannot sit together" constraints
            if (request.CannotSitTogether != null)
            {
                foreach (var pair in request.CannotSitTogether)
                {
                    if ((pair.Guest1 == guestId && currentGuests.Contains(pair.Guest2)) ||
                        (pair.Guest2 == guestId && currentGuests.Contains(pair.Guest1)))
                    {
                        return false;
                    }
                }
            }

            // Check "must sit together" constraints
            if (request.MustSitTogether != null)
            {
                foreach (var pair in request.MustSitTogether)
                {
                    if (pair.Guest1 == guestId || pair.Guest2 == guestId)
                    {
                        int other = pair.Guest1 == guestId ? pair.Guest2 : pair.Guest1;
                        
                        // If other guest is already assigned to a different table, reject
                        foreach (var kvp in tableGuests)
                        {
                            if (kvp.Key != table && kvp.Value.Contains(other))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            // Check gender balance constraint
            if (request.Guests != null && currentGuests.Count > 0)
            {
                var currentGuest = request.Guests.FirstOrDefault(g => g.Id == guestId);
                if (currentGuest != null)
                {
                    int maleCount = currentGuests.Count(g => 
                        request.Guests.Any(guest => guest.Id == g && guest.Gender == "M"));
                    int femaleCount = currentGuests.Count - maleCount;

                    if (currentGuest.Gender == "M")
                    {
                        maleCount++;
                    }
                    else
                    {
                        femaleCount++;
                    }

                    if (Math.Abs(maleCount - femaleCount) > request.MaxGenderDifference)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckAllConstraints(SATRequest request, Dictionary<int, List<int>> tableGuests)
        {
            // Check that all tables respect seat limit
            foreach (var table in tableGuests.Values)
            {
                if (table.Count > request.SeatsPerTable)
                {
                    return false;
                }
            }

            // Check "cannot sit together" constraints
            if (request.CannotSitTogether != null)
            {
                foreach (var pair in request.CannotSitTogether)
                {
                    foreach (var guests in tableGuests.Values)
                    {
                        if (guests.Contains(pair.Guest1) && guests.Contains(pair.Guest2))
                        {
                            return false;
                        }
                    }
                }
            }

            // Check "must sit together" constraints
            if (request.MustSitTogether != null)
            {
                foreach (var pair in request.MustSitTogether)
                {
                    bool togetherInSomeTable = false;
                    foreach (var guests in tableGuests.Values)
                    {
                        if (guests.Contains(pair.Guest1) && guests.Contains(pair.Guest2))
                        {
                            togetherInSomeTable = true;
                            break;
                        }
                    }
                    if (!togetherInSomeTable)
                    {
                        return false;
                    }
                }
            }

            // Check gender balance
            if (request.Guests != null)
            {
                foreach (var guests in tableGuests.Values)
                {
                    int maleCount = guests.Count(g => 
                        request.Guests.Any(guest => guest.Id == g && guest.Gender == "M"));
                    int femaleCount = guests.Count - maleCount;

                    if (Math.Abs(maleCount - femaleCount) > request.MaxGenderDifference)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private (string formula, int totalClauses) GenerateSATFormula(SATRequest request)
        {
            var sb = new StringBuilder();
            int clauseCount = 0;

            sb.AppendLine("SAT Formula for Seating Arrangement:");
            sb.AppendLine("=======================================");
            sb.AppendLine();
            
            sb.AppendLine("Variables:");
            sb.AppendLine("x_g_t: Guest g sits at table t (where g ∈ [0, 99], t ∈ [0, 9])");
            sb.AppendLine();

            // Clause 1: Each guest must sit at exactly one table
            sb.AppendLine("1. Each guest sits at exactly one table:");
            sb.AppendLine("   For each guest g:");
            sb.AppendLine("   (x_g_0 ∨ x_g_1 ∨ ... ∨ x_g_9)");
            sb.AppendLine("   ∧ (¬x_g_i ∨ ¬x_g_j) for all i ≠ j");
            clauseCount += request.TotalGuests * (1 + (request.TotalTables * (request.TotalTables - 1)) / 2);
            sb.AppendLine();

            // Clause 2: Each table has at most SeatsPerTable guests
            sb.AppendLine("2. Each table has at most 10 seats:");
            sb.AppendLine("   At-most-k constraint using sequential encoding");
            clauseCount += request.TotalTables * request.TotalGuests;
            sb.AppendLine();

            // Clause 3: Guests who cannot sit together must be at different tables
            sb.AppendLine("3. Cannot sit together constraints:");
            if (request.CannotSitTogether != null && request.CannotSitTogether.Any())
            {
                sb.AppendLine($"   Total pairs: {request.CannotSitTogether.Count}");
                sb.AppendLine("   For each pair (g1, g2):");
                sb.AppendLine("   For each table t: (¬x_g1_t ∨ ¬x_g2_t)");
                clauseCount += request.CannotSitTogether.Count * request.TotalTables;
            }
            else
            {
                sb.AppendLine("   No constraints");
            }
            sb.AppendLine();

            // Clause 4: Guests who must sit together must be at the same table
            sb.AppendLine("4. Must sit together constraints:");
            if (request.MustSitTogether != null && request.MustSitTogether.Any())
            {
                sb.AppendLine($"   Total pairs: {request.MustSitTogether.Count}");
                sb.AppendLine("   For each pair (g1, g2):");
                sb.AppendLine("   For each table t: (x_g1_t ⟺ x_g2_t)");
                sb.AppendLine("   Which translates to: (¬x_g1_t ∨ x_g2_t) ∧ (x_g1_t ∨ ¬x_g2_t)");
                clauseCount += request.MustSitTogether.Count * request.TotalTables * 2;
            }
            else
            {
                sb.AppendLine("   No constraints");
            }
            sb.AppendLine();

            // Clause 5: Gender balance constraint
            sb.AppendLine("5. Gender balance constraint:");
            sb.AppendLine($"   For each table t: |males_t - females_t| ≤ {request.MaxGenderDifference}");
            sb.AppendLine("   This is encoded using cardinality constraints");
            clauseCount += request.TotalTables * 100; // Approximate
            sb.AppendLine();

            sb.AppendLine($"Total estimated clauses: ~{clauseCount}");

            return (sb.ToString(), clauseCount);
        }
    }
}

