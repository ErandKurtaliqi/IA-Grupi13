using API.Models;
using System.Diagnostics;

namespace API.Services
{
    public class KillerSudokuService : IKillerSudokuService
    {
        private int backtrackCount = 0;

        public async Task<KillerSudokuResponse> SolveAsync(KillerSudokuRequest request)
        {
            return await Task.Run(() => Solve(request));
        }

        private KillerSudokuResponse Solve(KillerSudokuRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            backtrackCount = 0;

            if (request.Grid == null)
            {
                request.Grid = new int[9][];
                for (int i = 0; i < 9; i++)
                {
                    request.Grid[i] = new int[9];
                }
            }

            // Create domains for each cell
            var domains = new Dictionary<string, HashSet<int>>();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (request.Grid[row][col] == 0)
                    {
                        domains[$"{row},{col}"] = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    }
                }
            }

            // Initial constraint propagation
            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (var key in domains.Keys.ToList())
                {
                    var parts = key.Split(',');
                    int row = int.Parse(parts[0]);
                    int col = int.Parse(parts[1]);

                    var original = domains[key].Count;
                    ApplyConstraints(request, domains, row, col);
                    
                    if (domains[key].Count < original)
                    {
                        changed = true;
                    }

                    if (domains[key].Count == 1)
                    {
                        request.Grid[row][col] = domains[key].First();
                        domains.Remove(key);
                        changed = true;
                    }
                }
            }

            bool success = SolveCSP(request, domains);
            stopwatch.Stop();

            if (success)
            {
                return new KillerSudokuResponse
                {
                    Success = true,
                    Solution = request.Grid,
                    BacktrackCount = backtrackCount,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    Message = "Solution found"
                };
            }

            return new KillerSudokuResponse
            {
                Success = false,
                BacktrackCount = backtrackCount,
                ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                Message = "No solution exists"
            };
        }

        private bool SolveCSP(KillerSudokuRequest request, Dictionary<string, HashSet<int>> domains)
        {
            if (domains.Count == 0)
            {
                return IsValidSolution(request);
            }

            // Select variable with minimum remaining values (MRV heuristic)
            var selectedKey = domains.OrderBy(kvp => kvp.Value.Count).First().Key;
            var parts = selectedKey.Split(',');
            int row = int.Parse(parts[0]);
            int col = int.Parse(parts[1]);

            var possibleValues = new List<int>(domains[selectedKey]);

            foreach (var value in possibleValues)
            {
                // Try assigning value
                request.Grid![row][col] = value;
                
                if (IsConsistent(request, row, col))
                {
                    // Save state
                    var oldDomains = new Dictionary<string, HashSet<int>>();
                    foreach (var kvp in domains)
                    {
                        oldDomains[kvp.Key] = new HashSet<int>(kvp.Value);
                    }
                    
                    domains.Remove(selectedKey);

                    // Forward checking - update domains
                    bool valid = true;
                    foreach (var key in domains.Keys.ToList())
                    {
                        var p = key.Split(',');
                        int r = int.Parse(p[0]);
                        int c = int.Parse(p[1]);

                        ApplyConstraints(request, domains, r, c);
                        
                        if (domains[key].Count == 0)
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid && SolveCSP(request, domains))
                    {
                        return true;
                    }

                    // Backtrack
                    backtrackCount++;
                    domains.Clear();
                    foreach (var kvp in oldDomains)
                    {
                        domains[kvp.Key] = kvp.Value;
                    }
                }

                request.Grid[row][col] = 0;
            }

            return false;
        }

        private void ApplyConstraints(KillerSudokuRequest request, 
            Dictionary<string, HashSet<int>> domains, int row, int col)
        {
            var key = $"{row},{col}";
            if (!domains.ContainsKey(key))
            {
                return;
            }

            var toRemove = new List<int>();

            foreach (var value in domains[key])
            {
                // Check row constraint
                for (int c = 0; c < 9; c++)
                {
                    if (c != col && request.Grid![row][c] == value)
                    {
                        toRemove.Add(value);
                        break;
                    }
                }

                // Check column constraint
                for (int r = 0; r < 9; r++)
                {
                    if (r != row && request.Grid![r][col] == value)
                    {
                        toRemove.Add(value);
                        break;
                    }
                }

                // Check 3x3 box constraint
                int boxRow = (row / 3) * 3;
                int boxCol = (col / 3) * 3;
                for (int r = boxRow; r < boxRow + 3; r++)
                {
                    for (int c = boxCol; c < boxCol + 3; c++)
                    {
                        if ((r != row || c != col) && request.Grid![r][c] == value)
                        {
                            toRemove.Add(value);
                            break;
                        }
                    }
                }
            }

            foreach (var value in toRemove)
            {
                domains[key].Remove(value);
            }

            // Check cage constraints
            if (request.Cages != null)
            {
                foreach (var cage in request.Cages)
                {
                    if (cage.Cells != null && cage.Cells.Any(c => c.Row == row && c.Col == col))
                    {
                        ApplyCageConstraint(request, domains, cage, row, col);
                    }
                }
            }
        }

        private void ApplyCageConstraint(KillerSudokuRequest request, 
            Dictionary<string, HashSet<int>> domains, Cage cage, int row, int col)
        {
            if (cage.Cells == null) return;

            var key = $"{row},{col}";
            if (!domains.ContainsKey(key)) return;

            // Calculate current sum and count filled cells
            int currentSum = 0;
            int filledCount = 0;
            var usedValues = new HashSet<int>();

            foreach (var cell in cage.Cells)
            {
                var cellValue = request.Grid![cell.Row][cell.Col];
                if (cellValue != 0)
                {
                    currentSum += cellValue;
                    filledCount++;
                    usedValues.Add(cellValue);
                }
            }

            int emptyCells = cage.Cells.Count - filledCount;
            int remainingSum = cage.Sum - currentSum;

            // Remove values that are already used in this cage
            foreach (var used in usedValues)
            {
                domains[key].Remove(used);
            }

            // Remove values that would make it impossible to reach the target sum
            var toRemove = new List<int>();
            foreach (var value in domains[key])
            {
                // If this is the last empty cell, it must equal the remaining sum
                if (emptyCells == 1)
                {
                    if (value != remainingSum || value < 1 || value > 9)
                    {
                        toRemove.Add(value);
                    }
                }
                else
                {
                    // Check if remaining sum is achievable
                    int minPossible = value + (emptyCells - 1);
                    int maxPossible = value + (9 * (emptyCells - 1));
                    
                    if (remainingSum < minPossible || remainingSum > maxPossible)
                    {
                        toRemove.Add(value);
                    }
                }
            }

            foreach (var value in toRemove)
            {
                domains[key].Remove(value);
            }
        }

        private bool IsConsistent(KillerSudokuRequest request, int row, int col)
        {
            int value = request.Grid![row][col];

            // Check row
            for (int c = 0; c < 9; c++)
            {
                if (c != col && request.Grid[row][c] == value)
                {
                    return false;
                }
            }

            // Check column
            for (int r = 0; r < 9; r++)
            {
                if (r != row && request.Grid[r][col] == value)
                {
                    return false;
                }
            }

            // Check 3x3 box
            int boxRow = (row / 3) * 3;
            int boxCol = (col / 3) * 3;
            for (int r = boxRow; r < boxRow + 3; r++)
            {
                for (int c = boxCol; c < boxCol + 3; c++)
                {
                    if ((r != row || c != col) && request.Grid[r][c] == value)
                    {
                        return false;
                    }
                }
            }

            // Check cage constraints
            if (request.Cages != null)
            {
                foreach (var cage in request.Cages)
                {
                    if (cage.Cells != null && cage.Cells.Any(c => c.Row == row && c.Col == col))
                    {
                        if (!CheckCageConstraint(request, cage))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool CheckCageConstraint(KillerSudokuRequest request, Cage cage)
        {
            if (cage.Cells == null) return true;

            int sum = 0;
            var values = new HashSet<int>();
            bool allFilled = true;

            foreach (var cell in cage.Cells)
            {
                var value = request.Grid![cell.Row][cell.Col];
                if (value == 0)
                {
                    allFilled = false;
                }
                else
                {
                    sum += value;
                    if (values.Contains(value))
                    {
                        return false; // Duplicate in cage
                    }
                    values.Add(value);
                }
            }

            if (allFilled)
            {
                return sum == cage.Sum;
            }

            // Partial check: current sum shouldn't exceed target
            return sum <= cage.Sum;
        }

        private bool IsValidSolution(KillerSudokuRequest request)
        {
            // Check all cells are filled
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (request.Grid![i][j] == 0)
                    {
                        return false;
                    }
                }
            }

            // Check all constraints
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!IsConsistent(request, i, j))
                    {
                        return false;
                    }
                }
            }

            // Check all cages
            if (request.Cages != null)
            {
                foreach (var cage in request.Cages)
                {
                    if (!CheckCageConstraint(request, cage))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

