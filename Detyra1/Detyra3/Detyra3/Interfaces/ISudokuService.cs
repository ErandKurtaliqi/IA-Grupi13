namespace Detyra3.Interfaces;

public interface ISudokuService
{
    object GenerateAndSolve(string difficulty);
    int[][] SolveBfs(int[][] grid);
}
