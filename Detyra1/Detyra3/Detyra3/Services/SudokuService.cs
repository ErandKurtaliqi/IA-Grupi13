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

    // ----- BFS SOLVER -----
    public int[][] SolveBfs(int[][] grid)
    {
        var startState = Copy(grid);
        var queue = new Queue<int[][]>();
        queue.Enqueue(startState);
        int stateCount = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            stateCount++;
            if (stateCount > MAX_STATES)
            {
                // safety guard so Swagger doesn't hang forever
                return new int[][] { new[] { -1 } }; // indicates "cutoff"
            }

            var empty = FindEmpty(current);
            if (empty == null)
                return current; // solved!

            var (r, c) = empty.Value;

            for (int num = 1; num <= 9; num++)
            {
                if (IsValid(current, r, c, num))
                {
                    var nextState = Copy(current);
                    nextState[r][c] = num;
                    queue.Enqueue(nextState);
                }
            }
        }

        return grid; // no solution found
    }

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
}
