using Detyra3.Interfaces;
using System.Collections.Generic;

namespace Detyra3.Services;

public class SudokuService : ISudokuService
{
    private const int MAX_STATES = 500_000; // safety limit to avoid infinite expansion

    public object GenerateAndSolve(string difficulty)
    {
        var grid = Generate(difficulty);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var solved = SolveBfs(GridToArray(grid));
        stopwatch.Stop();

        return new
        {
            Difficulty = difficulty,
            TimeMs = stopwatch.ElapsedMilliseconds,
            Original = GridToArray(grid),
            Solved = solved
        };
    }

    //// ----- BFS SOLVER -----
    //public int[][] SolveBfs(int[][] grid)
    //{
    //    var startState = Copy(grid);
    //    var queue = new Queue<int[][]>();
    //    queue.Enqueue(startState);
    //    int stateCount = 0;

    //    while (queue.Count > 0)
    //    {
    //        var current = queue.Dequeue();
    //        stateCount++;
    //        if (stateCount > MAX_STATES)
    //        {
    //            // safety guard so Swagger doesn't hang forever
    //            return new int[][] { new[] { -1 } }; // indicates "cutoff"
    //        }

    //        var empty = FindEmpty(current);
    //        if (empty == null)
    //            return current; // solved!

    //        var (r, c) = empty.Value;

    //        for (int num = 1; num <= 9; num++)
    //        {
    //            if (IsValid(current, r, c, num))
    //            {
    //                var nextState = Copy(current);
    //                nextState[r][c] = num;
    //                queue.Enqueue(nextState);
    //            }
    //        }
    //    }

    //    return grid; // no solution found
    //}

    // ----- Helpers -----
    private (int, int)? FindEmpty(int[][] grid)
    {
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
                if (grid[i][j] == 0)
                    return (i, j);
        return null;
    }

    private bool IsValid(int[][] grid, int row, int col, int num)
    {
        for (int i = 0; i < 9; i++)
        {
            if (grid[row][i] == num) return false;
            if (grid[i][col] == num) return false;
        }

        int startRow = row - row % 3;
        int startCol = col - col % 3;
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                if (grid[startRow + r][startCol + c] == num)
                    return false;

        return true;
    }

    private int[][] Copy(int[][] grid)
    {
        var result = new int[9][];
        for (int i = 0; i < 9; i++)
        {
            result[i] = new int[9];
            for (int j = 0; j < 9; j++)
                result[i][j] = grid[i][j];
        }
        return result;
    }

    // ----- Generate puzzles -----
    private int[,] Generate(string difficulty)
    {
        return difficulty.ToLower() switch
        {
            "easy" => new int[9, 9]
            {
                {5,3,0,0,7,0,0,0,0},
                {6,0,0,1,9,5,0,0,0},
                {0,9,8,0,0,0,0,6,0},
                {8,0,0,0,6,0,0,0,3},
                {4,0,0,8,0,3,0,0,1},
                {7,0,0,0,2,0,0,0,6},
                {0,6,0,0,0,0,2,8,0},
                {0,0,0,4,1,9,0,0,5},
                {0,0,0,0,8,0,0,7,9}
            },
            "medium" => new int[9, 9]
            {
                {0,0,0,0,0,0,0,1,2},
                {0,0,0,0,0,3,0,0,0},
                {0,0,1,0,0,0,0,0,0},
                {0,0,0,5,0,0,4,0,7},
                {0,0,0,1,0,2,0,0,0},
                {7,0,3,0,0,9,0,0,0},
                {0,0,0,0,0,0,3,0,0},
                {0,0,0,9,0,0,0,0,0},
                {4,6,0,0,0,0,0,0,0}
            },
            "hard" => new int[9, 9]
            {
                {8,0,0,0,0,0,0,0,0},
                {0,0,3,6,0,0,0,0,0},
                {0,7,0,0,9,0,2,0,0},
                {0,5,0,0,0,7,0,0,0},
                {0,0,0,0,4,5,7,0,0},
                {0,0,0,1,0,0,0,3,0},
                {0,0,1,0,0,0,0,6,8},
                {0,0,8,5,0,0,0,1,0},
                {0,9,0,0,0,0,4,0,0}
            },
            _ => throw new ArgumentException("Use Easy, Medium, or Hard")
        };
    }

    // ----- Convert 2D -> jagged -----
    private int[][] GridToArray(int[,] grid)
    {
        var result = new int[9][];
        for (int i = 0; i < 9; i++)
        {
            result[i] = new int[9];
            for (int j = 0; j < 9; j++)
                result[i][j] = grid[i, j];
        }
        return result;
    }

    // ZËVENDËSO SolveBfs me këtë qasje "smart" (DFS + MRV + singles)
    public int[][] SolveBfs(int[][] grid)
    {
        var g = Copy(grid);

        // 1) Propagim i thjeshtë: mbush qelizat me 1 kandidat derisa të mos ketë më
        if (!FillSingles(g))
            return grid; // konflikt i pashmangshëm -> pa zgjidhje

        // 2) Backtracking me MRV
        if (SolveDfsWithMrv(g))
            return g;

        return grid; // pa zgjidhje
    }

    // Faza e propagimit: mbush qelizat që kanë vetëm 1 kandidat.
    // Kthen false nëse detekton konflikt (0 kandidatë).
    private bool FillSingles(int[][] grid)
    {
        bool changed;
        int safety = 0;

        do
        {
            changed = false;
            safety++;
            if (safety > 100) break; // mbrojtje minimale nga loop-e të pafundme

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (grid[r][c] != 0) continue;

                    var cand = GetCandidates(grid, r, c);
                    if (cand.Count == 0) return false;     // konflikt
                    if (cand.Count == 1)
                    {
                        grid[r][c] = cand[0];
                        changed = true;
                    }
                }
            }
        } while (changed);

        return true;
    }

    // MRV: gjej qelizën bosh me më pak kandidatë
    private (int r, int c, List<int> cand)? FindBestCell(int[][] grid)
    {
        int bestR = -1, bestC = -1;
        List<int> bestCand = null!;
        int bestCount = int.MaxValue;

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (grid[r][c] != 0) continue;
                var cand = GetCandidates(grid, r, c);
                int k = cand.Count;
                if (k == 0) return null;      // konflikt
                if (k < bestCount)
                {
                    bestCount = k;
                    bestR = r; bestC = c; bestCand = cand;
                    if (k == 1) return (bestR, bestC, bestCand); // më e mira e mundshme
                }
            }
        }

        if (bestR == -1) return null; // s’ka bosh -> e zgjidhur
        return (bestR, bestC, bestCand);
    }

    private bool SolveDfsWithMrv(int[][] grid)
    {
        // Nëse s’ka bosh, kemi mbaruar
        var pick = FindBestCell(grid);
        if (pick == null)
        {
            // në këtë pikë s’ka qeliza bosh -> e zgjidhur
            return IsComplete(grid);
        }

        var p = pick.Value;
        // Nëse MRV ktheu null nga konfliktet (0 kandidatë diku), backtrack
        if (p.cand == null) return false;

        foreach (var v in p.cand)
        {
            if (!IsValid(grid, p.r, p.c, v)) continue;

            grid[p.r][p.c] = v;

            // Propagim pas vendosjes
            var snapshot = Copy(grid);
            if (FillSingles(grid) && SolveDfsWithMrv(grid))
                return true;

            // rikthe gjendjen
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grid[i][j] = snapshot[i][j];
        }

        // s'funksionoi asnjë kandidat
        grid[p.r][p.c] = 0;
        return false;
    }

    private List<int> GetCandidates(int[][] grid, int r, int c)
    {
        if (grid[r][c] != 0) return new List<int>();

        bool[] bad = new bool[10];

        // rresht & kolonë
        for (int i = 0; i < 9; i++)
        {
            bad[grid[r][i]] = true;
            bad[grid[i][c]] = true;
        }

        // 3x3 kuti
        int sr = r - r % 3, sc = c - c % 3;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                bad[grid[sr + i][sc + j]] = true;

        var cand = new List<int>(9);
        for (int v = 1; v <= 9; v++)
            if (!bad[v]) cand.Add(v);

        return cand;
    }

    private bool IsComplete(int[][] grid)
    {
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                if (grid[r][c] == 0) return false;
        return true;
    }

}
