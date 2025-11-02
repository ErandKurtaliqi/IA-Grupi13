using Microsoft.AspNetCore.Mvc;
using Detyra3.Interfaces;

namespace Detyra3.Controllers;

/// <summary>
/// Controller për Social Golfers Problem
/// 
/// PËRDORIMI:
/// GET /api/socialgolfers/dfs?numPlayers=32&groupSize=4&numWeeks=10
/// GET /api/socialgolfers/depth-limited?numPlayers=16&groupSize=4&numWeeks=5&depthLimit=10
/// 
/// SHEMBULL:
/// Për problemin origjinal: 32 lojtarë, 4 në grup, sa javë mund të luajnë?
/// Testo me vlera të ndryshme të numWeeks për të gjetur maksimumin
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SocialGolfersController : ControllerBase
{
    private readonly ISocialGolfersService _service;

    public SocialGolfersController(ISocialGolfersService service)
    {
        _service = service;
    }

    /// <summary>
    /// Zgjidh Social Golfers Problem duke përdorur DFS me Backtracking
    /// </summary>
    /// <param name="numPlayers">Numri total i lojtarëve (default: 8)</param>
    /// <param name="groupSize">Madhësia e grupit (default: 4)</param>
    /// <param name="numWeeks">Numri i javëve (default: 2)</param>
    /// <returns>Zgjidhja ose mesazh gabimi</returns>
    [HttpGet("dfs")]
    public IActionResult SolveWithDfs(
        [FromQuery] int numPlayers = 16,
        [FromQuery] int groupSize = 4,
        [FromQuery] int numWeeks = 5)
    {
        if (numPlayers <= 0 || groupSize <= 0 || numWeeks <= 0)
            return BadRequest("Të gjitha parametrat duhet të jenë pozitive");

        if (numPlayers % groupSize != 0)
            return BadRequest("Numri i lojtarëve duhet të jetë i plotpjesëtueshëm me madhësinë e grupit");

        var result = _service.SolveWithDfs(numPlayers, groupSize, numWeeks);
        return Ok(result);
    }

    /// <summary>
    /// Zgjidh Social Golfers Problem duke përdorur Depth Limited Search me Backtracking
    /// </summary>
    /// <param name="numPlayers">Numri total i lojtarëve (default: 8)</param>
    /// <param name="groupSize">Madhësia e grupit (default: 4)</param>
    /// <param name="numWeeks">Numri i javëve (default: 2)</param>
    /// <param name="depthLimit">Limiti i thellësisë (default: 5)</param>
    /// <returns>Zgjidhja ose mesazh gabimi</returns>
    [HttpGet("depth-limited")]
    public IActionResult SolveWithDepthLimited(
        [FromQuery] int numPlayers = 16,
        [FromQuery] int groupSize = 4,
        [FromQuery] int numWeeks = 5,
        [FromQuery] int depthLimit = 6)
    {
        if (numPlayers <= 0 || groupSize <= 0 || numWeeks <= 0 || depthLimit <= 0)
            return BadRequest("Të gjitha parametrat duhet të jenë pozitive");

        if (numPlayers % groupSize != 0)
            return BadRequest("Numri i lojtarëve duhet të jetë i plotpjesëtueshëm me madhësinë e grupit");

        var result = _service.SolveWithDepthLimitedSearch(numPlayers, groupSize, numWeeks, depthLimit);
        return Ok(result);
    }

    /// <summary>
    /// Provon të gjejë numrin maksimal të javëve për një konfiguracion të dhënë
    /// Duke provuar nga 1 javë e duke rritur derisa nuk gjendet më zgjidhje
    /// </summary>
    /// <param name="numPlayers">Numri total i lojtarëve (default: 16)</param>
    /// <param name="groupSize">Madhësia e grupit (default: 4)</param>
    /// <param name="maxWeeksToTry">Numri maksimal i javëve për të provuar (default: 15)</param>
    /// <returns>Numri maksimal i javëve të mundshme</returns>
    [HttpGet("find-max-weeks")]
    public IActionResult FindMaxWeeks(
        [FromQuery] int numPlayers = 16,
        [FromQuery] int groupSize = 4,
        [FromQuery] int maxWeeksToTry = 15)
    {
        if (numPlayers <= 0 || groupSize <= 0 || maxWeeksToTry <= 0)
            return BadRequest("Të gjitha parametrat duhet të jenë pozitive");

        if (numPlayers % groupSize != 0)
            return BadRequest("Numri i lojtarëve duhet të jetë i plotpjesëtueshëm me madhësinë e grupit");

        int maxWeeks = 0;
        var attempts = new List<object>();

        // Provoj me numra të ndryshëm javësh
        for (int weeks = 1; weeks <= maxWeeksToTry; weeks++)
        {
            var result = _service.SolveWithDfs(numPlayers, groupSize, weeks);
            
            // Kontrollo nëse është dictionary dhe ka Success
            var resultDict = result as dynamic;
            bool success = false;
            
            try
            {
                success = resultDict?.Success == true;
            }
            catch
            {
                // Nëse ka error, ndalojmë
                break;
            }

            attempts.Add(new
            {
                Weeks = weeks,
                Success = success
            });

            if (success)
                maxWeeks = weeks;
            else
                break; // Nëse dështon për këtë numër, nuk ka kuptim të vazhdojmë
        }

        return Ok(new
        {
            NumPlayers = numPlayers,
            GroupSize = groupSize,
            MaxWeeksFound = maxWeeks,
            Attempts = attempts,
            Message = maxWeeks > 0 
                ? $"Lojtarët mund të luajnë për maksimumi {maxWeeks} javë pa përsëritur çifte."
                : "Nuk u gjet zgjidhje për asnjë javë."
        });
    }

    /// <summary>
    /// Shembull i thjeshtë për testing (8 lojtarë, 4 në grup, 2 javë)
    /// </summary>
    [HttpGet("example-simple")]
    public IActionResult SimpleExample()
    {
        var result = _service.SolveWithDfs(numPlayers: 8, groupSize: 4, numWeeks: 2);
        return Ok(result);
    }

    /// <summary>
    /// Shembull i problemit origjinal (32 lojtarë, 4 në grup)
    /// VINI RE: Kjo mund të jetë shumë e ngadaltë për 32 lojtarë!
    /// Provoni me numër më të vogël javësh fillimisht
    /// </summary>
    [HttpGet("example-original")]
    public IActionResult OriginalExample([FromQuery] int numWeeks = 5)
    {
        var result = _service.SolveWithDfs(numPlayers: 32, groupSize: 4, numWeeks: numWeeks);
        return Ok(result);
    }
}

