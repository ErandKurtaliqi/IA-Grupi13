using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SATController : ControllerBase
    {
        private readonly ISATService _satService;
        private readonly ILogger<SATController> _logger;

        public SATController(ISATService satService, ILogger<SATController> logger)
        {
            _satService = satService;
            _logger = logger;
        }

        [HttpPost("solve")]
        public async Task<ActionResult<SATResponse>> Solve([FromBody] SATRequest request)
        {
            try
            {
                _logger.LogInformation($"Solving SAT problem with {request.TotalGuests} guests and {request.TotalTables} tables");
                
                if (request.TotalGuests > request.TotalTables * request.SeatsPerTable)
                {
                    return BadRequest(new { message = "Not enough seats for all guests" });
                }

                var result = await _satService.SolveAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error solving SAT problem");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("generate-formula")]
        public ActionResult<string> GenerateFormula([FromBody] SATRequest request)
        {
            try
            {
                // This will just generate and return the formula without solving
                var service = new SATService();
                var result = service.SolveAsync(request).Result;
                return Ok(new { formula = result.SATFormula, totalClauses = result.TotalClauses });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating SAT formula");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}

