using API.Models;

namespace API.Services
{
    public interface IKillerSudokuService
    {
        Task<KillerSudokuResponse> SolveAsync(KillerSudokuRequest request);
    }
}

