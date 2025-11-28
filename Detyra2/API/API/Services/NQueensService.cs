using API.Models;
using System.Diagnostics;

namespace API.Services
{
    public class NQueensService : INQueensService
    {
        public async Task<NQueensResponse> SolveAsync(NQueensRequest request)
        {
            return await Task.Run(() => Solve(request));
        }

        private NQueensResponse Solve(NQueensRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            var blockedSet = new HashSet<string>();
            
            if (request.BlockedPositions != null)
            {
                foreach (var pos in request.BlockedPositions)
                {
                    blockedSet.Add($"{pos.Row},{pos.Col}");
                }
            }

            var initialState = new NQueensState
            {
                Queens = new int[request.N],
                N = request.N,
                BlockedPositions = blockedSet,
                Row = 0,
                GCost = 0,
                HCost = 0
            };

            for (int i = 0; i < request.N; i++)
            {
                initialState.Queens[i] = -1;
            }

            initialState.HCost = CalculateHeuristic(initialState, request.HeuristicType);

            var result = AStar(initialState, request.HeuristicType, out int nodesExplored);
            stopwatch.Stop();

            if (result != null)
            {
                var solution = new List<Position>();
                for (int i = 0; i < result.Queens!.Length; i++)
                {
                    if (result.Queens[i] >= 0)
                    {
                        solution.Add(new Position { Row = i, Col = result.Queens[i] });
                    }
                }

                return new NQueensResponse
                {
                    Success = true,
                    Solution = solution,
                    NodesExplored = nodesExplored,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    HeuristicUsed = request.HeuristicType,
                    Message = $"Solution found using {request.HeuristicType} heuristic"
                };
            }

            return new NQueensResponse
            {
                Success = false,
                NodesExplored = nodesExplored,
                ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                HeuristicUsed = request.HeuristicType,
                Message = "No solution found"
            };
        }

        private NQueensState? AStar(NQueensState initialState, string heuristicType, out int nodesExplored)
        {
            var openSet = new PriorityQueue<NQueensState, int>();
            var closedSet = new HashSet<string>();
            
            openSet.Enqueue(initialState, initialState.FCost);
            nodesExplored = 0;

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                nodesExplored++;

                if (IsGoal(current))
                {
                    return current;
                }

                closedSet.Add(current.GetKey());

                // If we've placed all queens, but it's not a goal, skip
                if (current.Row >= current.N)
                {
                    continue;
                }

                // Try placing a queen in each column of the current row
                for (int col = 0; col < current.N; col++)
                {
                    var newState = current.Clone();
                    newState.Queens![current.Row] = col;
                    newState.Row = current.Row + 1;
                    newState.GCost = current.GCost + 1;

                    // Check if this position is valid
                    if (!IsValidPosition(newState, current.Row, col))
                    {
                        continue;
                    }

                    var key = newState.GetKey();
                    if (closedSet.Contains(key))
                    {
                        continue;
                    }

                    newState.HCost = CalculateHeuristic(newState, heuristicType);
                    openSet.Enqueue(newState, newState.FCost);
                }
            }

            return null;
        }

        private bool IsGoal(NQueensState state)
        {
            if (state.Row < state.N)
            {
                return false;
            }

            for (int i = 0; i < state.N; i++)
            {
                if (state.Queens![i] < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidPosition(NQueensState state, int row, int col)
        {
            // Check if blocked
            if (state.BlockedPositions != null && state.BlockedPositions.Contains($"{row},{col}"))
            {
                return false;
            }

            // Check conflicts with previously placed queens
            for (int i = 0; i < row; i++)
            {
                int placedCol = state.Queens![i];
                if (placedCol < 0) continue;

                // Same column
                if (placedCol == col)
                {
                    return false;
                }

                // Diagonal
                if (Math.Abs(i - row) == Math.Abs(placedCol - col))
                {
                    return false;
                }
            }

            return true;
        }

        private int CalculateHeuristic(NQueensState state, string heuristicType)
        {
            return heuristicType.ToLower() switch
            {
                "conflicts" => ConflictsHeuristic(state),
                "attacking" => AttackingPairsHeuristic(state),
                "combined" => CombinedHeuristic(state),
                _ => ConflictsHeuristic(state)
            };
        }

        // Heuristic 1: Number of conflicts (rows without queens)
        private int ConflictsHeuristic(NQueensState state)
        {
            int remainingQueens = state.N - state.Row;
            return remainingQueens;
        }

        // Heuristic 2: Count potential attacking pairs
        private int AttackingPairsHeuristic(NQueensState state)
        {
            int attacks = 0;
            
            for (int row = state.Row; row < state.N; row++)
            {
                for (int col = 0; col < state.N; col++)
                {
                    if (state.BlockedPositions != null && state.BlockedPositions.Contains($"{row},{col}"))
                    {
                        continue;
                    }

                    // Count how many placed queens attack this position
                    for (int i = 0; i < state.Row; i++)
                    {
                        int placedCol = state.Queens![i];
                        if (placedCol < 0) continue;

                        if (placedCol == col || Math.Abs(i - row) == Math.Abs(placedCol - col))
                        {
                            attacks++;
                        }
                    }
                }
            }

            return attacks + (state.N - state.Row);
        }

        // Heuristic 3: Combined approach (admissible)
        // This is designed to be admissible by not overestimating the cost
        private int CombinedHeuristic(NQueensState state)
        {
            int remainingQueens = state.N - state.Row;
            
            // Count available safe positions for remaining queens
            int minConflicts = 0;
            for (int row = state.Row; row < state.N; row++)
            {
                int safePositions = 0;
                for (int col = 0; col < state.N; col++)
                {
                    if (state.BlockedPositions != null && state.BlockedPositions.Contains($"{row},{col}"))
                    {
                        continue;
                    }

                    bool safe = true;
                    for (int i = 0; i < state.Row; i++)
                    {
                        int placedCol = state.Queens![i];
                        if (placedCol < 0) continue;

                        if (placedCol == col || Math.Abs(i - row) == Math.Abs(placedCol - col))
                        {
                            safe = false;
                            break;
                        }
                    }

                    if (safe)
                    {
                        safePositions++;
                    }
                }

                // If no safe positions, add penalty
                if (safePositions == 0)
                {
                    minConflicts += state.N; // Heavy penalty
                }
            }

            return remainingQueens + minConflicts;
        }
    }
}

