using API.Models;

namespace API.Services
{
    public interface ISATService
    {
        Task<SATResponse> SolveAsync(SATRequest request);
    }
}

