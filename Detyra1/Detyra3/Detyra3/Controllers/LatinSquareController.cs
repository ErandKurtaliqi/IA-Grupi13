using Detyra3.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Detyra3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatinSquareController : ControllerBase
    {
        private readonly ILatinSquareSolverService _solver;

        public LatinSquareController(ILatinSquareSolverService solver)
        {
            _solver = solver;
        }

        // GET api/latinsquare?n=5&algo=backtracking|iddfs
        [HttpGet]
        public IActionResult Get([FromQuery] int n = 5, [FromQuery] string algo = "backtracking")
        {
            if (n < 1) return BadRequest("n duhet të jetë >= 1.");
            algo = (algo ?? "backtracking").Trim().ToLowerInvariant();

            int[,] result;
            try
            {
                result = algo switch
                {
                    "backtracking" => _solver.SolveBacktracking(n),
                    "iddfs" => _solver.SolveIddfs(n),
                    _ => throw new ArgumentException("Algoritmi i panjohur. Përdor 'backtracking' ose 'iddfs'.")
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // Kthejmë si jagged array për JSON miqsor
            var json = ToJagged(result);
            return Ok(new
            {
                n,
                algorithm = algo,
                matrix = json
            });
        }

        private static int[][] ToJagged(int[,] matrix)
        {
            int n0 = matrix.GetLength(0);
            int n1 = matrix.GetLength(1);
            var jagged = new int[n0][];
            for (int i = 0; i < n0; i++)
            {
                jagged[i] = new int[n1];
                for (int j = 0; j < n1; j++)
                    jagged[i][j] = matrix[i, j];
            }
            return jagged;
        }
    }
}
