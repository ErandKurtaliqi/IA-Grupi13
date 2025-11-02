namespace Detyra3.Interfaces;

/// <summary>
/// Service për zgjidhjen e Social Golfers Problem
/// 
/// MODELI I PROBLEMIT:
/// - Variablat: Çdo javë (week) është një variabël që përmban grupet e lojtarëve
/// - Domain: Të gjitha kombinimet e mundshme të grupeve ku çdo lojtar shfaqet saktësisht një herë për javë
/// - Constraints:
///   1. Çdo lojtar duhet të shfaqet saktësisht në një grup për javë
///   2. Dy lojtarë nuk mund të jenë në të njëjtin grup më shumë se një herë gjatë të gjitha javëve
///   3. Çdo grup duhet të ketë saktësisht p lojtarë (në rastin tonë 4)
/// 
/// REPREZENTIMI:
/// - State: int[][] - çdo rresht përfaqëson një javë, çdo element është ID e lojtarit
/// - Grupe: Organizohen si sekuenca e lojtarëve (p.sh. [0,1,2,3] është grupi 1 i javës 1)
/// </summary>
public interface ISocialGolfersService
{
    /// <summary>
    /// Zgjidh problemin duke përdorur Depth First Search (DFS) me Backtracking
    /// </summary>
    /// <param name="numPlayers">Numri total i lojtarëve (n = g × p)</param>
    /// <param name="groupSize">Madhësia e grupit (p lojtarë në grup)</param>
    /// <param name="numWeeks">Numri i javëve (w)</param>
    /// <returns>Rezultati me zgjidhjen ose mesazh gabimi</returns>
    object SolveWithDfs(int numPlayers, int groupSize, int numWeeks);

    /// <summary>
    /// Zgjidh problemin duke përdorur Depth Limited Search me Backtracking
    /// </summary>
    /// <param name="numPlayers">Numri total i lojtarëve (n = g × p)</param>
    /// <param name="groupSize">Madhësia e grupit (p lojtarë në grup)</param>
    /// <param name="numWeeks">Numri i javëve (w)</param>
    /// <param name="depthLimit">Limiti i thellësisë së kërkimit</param>
    /// <returns>Rezultati me zgjidhjen ose mesazh gabimi</returns>
    object SolveWithDepthLimitedSearch(int numPlayers, int groupSize, int numWeeks, int depthLimit);
}

