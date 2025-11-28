using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KillerSudokuController : ControllerBase
    {
        private readonly IKillerSudokuService _killerSudokuService;
        private readonly ILogger<KillerSudokuController> _logger;

        public KillerSudokuController(IKillerSudokuService killerSudokuService, ILogger<KillerSudokuController> logger)
        {
            _killerSudokuService = killerSudokuService;
            _logger = logger;
        }

        [HttpPost("solve")]
        public async Task<ActionResult<KillerSudokuResponse>> Solve([FromBody] KillerSudokuRequest request)
        {
            try
            {
                _logger.LogInformation("Solving Killer Sudoku problem");
                
                if (request.Grid == null || request.Grid.Length != 9)
                {
                    return BadRequest(new { message = "Grid must be 9x9" });
                }

                var result = await _killerSudokuService.SolveAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error solving Killer Sudoku problem");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("sample")]
        public ActionResult<KillerSudokuRequest> GetSample()
        {
            // Return a sample Killer Sudoku puzzle
            var sample = new KillerSudokuRequest
            {
                Grid = new int[9][]
                {
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                },
                Cages = new List<Cage>
                {
                    new Cage { Sum = 3, Cells = new List<CellPosition> { new() { Row = 0, Col = 0 }, new() { Row = 0, Col = 1 } } },
                    new Cage { Sum = 15, Cells = new List<CellPosition> { new() { Row = 0, Col = 2 }, new() { Row = 0, Col = 3 } } },
                    new Cage { Sum = 22, Cells = new List<CellPosition> { new() { Row = 0, Col = 4 }, new() { Row = 0, Col = 5 }, new() { Row = 1, Col = 4 } } }
                }
            };

            return Ok(sample);
        }
    }
}

