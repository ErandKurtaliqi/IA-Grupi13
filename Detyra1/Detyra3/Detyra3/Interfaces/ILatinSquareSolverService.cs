namespace Detyra3.Interfaces
{
    public interface ILatinSquareSolverService
    {
        int[,] SolveBacktracking(int n);
        int[,] SolveIddfs(int n);
    }
}
