using API.Models;

namespace API.Services
{
    public interface INQueensService
    {
        Task<NQueensResponse> SolveAsync(NQueensRequest request);
    }
}

