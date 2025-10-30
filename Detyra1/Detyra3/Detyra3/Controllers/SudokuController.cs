using Microsoft.AspNetCore.Mvc;
using Detyra3.Interfaces;

namespace Detyra3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SudokuController : ControllerBase
{
    private readonly ISudokuService _service;
    public SudokuController(ISudokuService service)
    {
        _service = service;
    }

    [HttpGet("solve")]
    public IActionResult Solve([FromQuery] string difficulty = "Easy")
    {
        var result = _service.GenerateAndSolve(difficulty);
        return Ok(result);
    }

    [HttpPost("solve-custom")]
    public IActionResult SolveCustom([FromBody] int[][] grid)
    {
        if (grid.Length != 9 || grid.Any(r => r.Length != 9))
            return BadRequest("Grid must be 9x9.");

        var solved = _service.SolveBfs(grid);
        return Ok(new { Original = grid, Solved = solved });
    }
}
