using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NQueensController : ControllerBase
    {
        private readonly INQueensService _nQueensService;
        private readonly ILogger<NQueensController> _logger;

        public NQueensController(INQueensService nQueensService, ILogger<NQueensController> logger)
        {
            _nQueensService = nQueensService;
            _logger = logger;
        }

        [HttpPost("solve")]
        public async Task<ActionResult<NQueensResponse>> Solve([FromBody] NQueensRequest request)
        {
            try
            {
                _logger.LogInformation($"Solving N-Queens problem with N={request.N}, Heuristic={request.HeuristicType}");
                
                if (request.N < 4 || request.N > 20)
                {
                    return BadRequest(new { message = "N must be between 4 and 20" });
                }

                var result = await _nQueensService.SolveAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error solving N-Queens problem");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("heuristics")]
        public ActionResult<List<string>> GetHeuristics()
        {
            return Ok(new List<string> { "conflicts", "attacking", "combined" });
        }
    }
}

