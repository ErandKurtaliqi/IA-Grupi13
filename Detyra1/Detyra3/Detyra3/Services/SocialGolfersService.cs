using Detyra3.Interfaces;
using System.Diagnostics;

namespace Detyra3.Services;

/// <summary>
/// Implementimi i Social Golfers Problem duke përdorur DFS dhe Depth Limited Search
/// </summary>
public class SocialGolfersService : ISocialGolfersService
{
    private HashSet<string> _pairingsUsed = new HashSet<string>();
    private int _nodesExplored;
    private const int MAX_NODES = 100_000;

    public object SolveWithDfs(int numPlayers, int groupSize, int numWeeks)
    {
        var stopwatch = Stopwatch.StartNew();
        _pairingsUsed = new HashSet<string>();
        _nodesExplored = 0;

        if (numPlayers % groupSize != 0)
            return new { Error = "Numri i lojtarëve duhet të jetë i plotpjesëtueshëm me madhësinë e grupit" };

        int numGroups = numPlayers / groupSize;
        var solution = new List<List<List<int>>>();

        bool found = SolveRecursive(solution, numPlayers, groupSize, numGroups, numWeeks, 0);
        stopwatch.Stop();

        if (found)
        {
            return new
            {
                Algorithm = "DFS with Backtracking",
                Success = true,
                NumPlayers = numPlayers,
                GroupSize = groupSize,
                NumGroups = numGroups,
                NumWeeks = numWeeks,
                NodesExplored = _nodesExplored,
                TimeMs = stopwatch.ElapsedMilliseconds,
                Solution = FormatSolution(solution)
            };
        }

        return new
        {
            Algorithm = "DFS with Backtracking",
            Success = false,
            Message = $"Nuk u gjet zgjidhje për {numWeeks} javë. Nodes explored: {_nodesExplored}",
            TimeMs = stopwatch.ElapsedMilliseconds
        };
    }

    public object SolveWithDepthLimitedSearch(int numPlayers, int groupSize, int numWeeks, int depthLimit)
    {
        var stopwatch = Stopwatch.StartNew();
        _nodesExplored = 0;

        if (numPlayers % groupSize != 0)
            return new { Error = "Numri i lojtarëve duhet të jetë i plotpjesëtueshëm me madhësinë e grupit" };

        int numGroups = numPlayers / groupSize;

        for (int limit = 1; limit <= depthLimit; limit++)
        {
            _pairingsUsed = new HashSet<string>();
            var solution = new List<List<List<int>>>();
            
            bool found = SolveRecursiveLimited(solution, numPlayers, groupSize, numGroups, numWeeks, 0, limit);
            
            if (found)
            {
                stopwatch.Stop();
                return new
                {
                    Algorithm = "Depth Limited Search with Backtracking",
                    Success = true,
                    NumPlayers = numPlayers,
                    GroupSize = groupSize,
                    NumGroups = numGroups,
                    NumWeeks = numWeeks,
                    DepthLimit = limit,
                    NodesExplored = _nodesExplored,
                    TimeMs = stopwatch.ElapsedMilliseconds,
                    Solution = FormatSolution(solution)
                };
            }
        }

        stopwatch.Stop();
        return new
        {
            Algorithm = "Depth Limited Search with Backtracking",
            Success = false,
            Message = $"Nuk u gjet zgjidhje brenda limitit të thellësisë {depthLimit}. Nodes explored: {_nodesExplored}",
            TimeMs = stopwatch.ElapsedMilliseconds
        };
    }

    /// <summary>
    /// Zgjidh rekursivisht - ndërton një javë në një kohë
    /// </summary>
    private bool SolveRecursive(List<List<List<int>>> solution, int numPlayers, int groupSize, int numGroups, int numWeeks, int week)
    {
        _nodesExplored++;
        if (_nodesExplored > MAX_NODES)
            return false;

        if (week == numWeeks)
            return true;

        // Provo të ndërtosh një javë me të gjitha ndarjet e mundshme
        var weekPartition = new List<List<int>>();
        var used = new bool[numPlayers];

        if (BuildWeek(weekPartition, used, numPlayers, groupSize, numGroups, 0, 0))
        {
            // Java u ndërtua me sukses, shto në zgjidhje
            solution.Add(weekPartition.Select(g => g.ToList()).ToList());
            AddPairings(weekPartition);

            // Provo javën tjetër
            if (SolveRecursive(solution, numPlayers, groupSize, numGroups, numWeeks, week + 1))
                return true;

            // Backtrack
            solution.RemoveAt(solution.Count - 1);
            RemovePairings(weekPartition);
        }

        return false;
    }

    private bool SolveRecursiveLimited(List<List<List<int>>> solution, int numPlayers, int groupSize, int numGroups, int numWeeks, int week, int depthLimit)
    {
        _nodesExplored++;
        if (_nodesExplored > MAX_NODES)
            return false;

        if (week > depthLimit)
            return false;

        if (week == numWeeks)
            return true;

        var weekPartition = new List<List<int>>();
        var used = new bool[numPlayers];

        if (BuildWeek(weekPartition, used, numPlayers, groupSize, numGroups, 0, 0))
        {
            solution.Add(weekPartition.Select(g => g.ToList()).ToList());
            AddPairings(weekPartition);

            if (SolveRecursiveLimited(solution, numPlayers, groupSize, numGroups, numWeeks, week + 1, depthLimit))
                return true;

            solution.RemoveAt(solution.Count - 1);
            RemovePairings(weekPartition);
        }

        return false;
    }

    /// <summary>
    /// Ndërton një javë - grup për grup
    /// </summary>
    private bool BuildWeek(List<List<int>> partition, bool[] used, int numPlayers, int groupSize, int numGroups, int groupIdx, int startFrom)
    {
        if (groupIdx == numGroups)
        {
            // Kontrollo që të gjithë lojtarët janë përdorur
            for (int i = 0; i < numPlayers; i++)
                if (!used[i])
                    return false;
            return true;
        }

        // Siguro që grupi ekziston
        if (partition.Count <= groupIdx)
            partition.Add(new List<int>());

        // Ndërto grupin aktual
        return BuildGroup(partition, used, numPlayers, groupSize, numGroups, groupIdx, startFrom);
    }

    /// <summary>
    /// Ndërton një grup - lojtar për lojtar
    /// </summary>
    private bool BuildGroup(List<List<int>> partition, bool[] used, int numPlayers, int groupSize, int numGroups, int groupIdx, int startFrom)
    {
        var group = partition[groupIdx];

        // Nëse grupi është i plotë, vazhdo me grupin tjetër
        if (group.Count == groupSize)
        {
            // Për grupin tjetër, fillo nga 0 (do të kontrollojmë used në loop)
            return BuildWeek(partition, used, numPlayers, groupSize, numGroups, groupIdx + 1, 0);
        }

        // Përcakto nga ku të fillosh
        int start = startFrom;
        if (group.Count > 0)
        {
            start = group[group.Count - 1] + 1;
        }

        // Provo të shtosh çdo lojtar të disponueshëm
        for (int player = start; player < numPlayers; player++)
        {
            if (used[player])
                continue;

            // Kontrollo çiftet
            bool valid = true;
            var pairsToAdd = new List<string>();
            foreach (var p in group)
            {
                string pair = GetPairKey(p, player);
                if (_pairingsUsed.Contains(pair))
                {
                    valid = false;
                    break;
                }
                pairsToAdd.Add(pair);
            }

            if (!valid)
                continue;

            // Shto lojtarin
            group.Add(player);
            used[player] = true;
            foreach (var pair in pairsToAdd)
                _pairingsUsed.Add(pair);

            // Rekurro
            if (BuildGroup(partition, used, numPlayers, groupSize, numGroups, groupIdx, startFrom))
                return true;

            // Backtrack
            group.RemoveAt(group.Count - 1);
            used[player] = false;
            foreach (var pair in pairsToAdd)
                _pairingsUsed.Remove(pair);
        }

        return false;
    }

    private bool IsValidPartition(List<List<int>> partition)
    {
        foreach (var group in partition)
        {
            for (int i = 0; i < group.Count; i++)
            {
                for (int j = i + 1; j < group.Count; j++)
                {
                    string pair = GetPairKey(group[i], group[j]);
                    if (_pairingsUsed.Contains(pair))
                        return false;
                }
            }
        }
        return true;
    }

    private void AddPairings(List<List<int>> partition)
    {
        foreach (var group in partition)
        {
            for (int i = 0; i < group.Count; i++)
            {
                for (int j = i + 1; j < group.Count; j++)
                {
                    string pair = GetPairKey(group[i], group[j]);
                    _pairingsUsed.Add(pair);
                }
            }
        }
    }

    private void RemovePairings(List<List<int>> partition)
    {
        foreach (var group in partition)
        {
            for (int i = 0; i < group.Count; i++)
            {
                for (int j = i + 1; j < group.Count; j++)
                {
                    string pair = GetPairKey(group[i], group[j]);
                    _pairingsUsed.Remove(pair);
                }
            }
        }
    }

    private string GetPairKey(int player1, int player2)
    {
        int min = Math.Min(player1, player2);
        int max = Math.Max(player1, player2);
        return $"{min}-{max}";
    }

    private object FormatSolution(List<List<List<int>>> solution)
    {
        var weeks = new List<object>();
        
        for (int w = 0; w < solution.Count; w++)
        {
            var groups = new List<object>();
            for (int g = 0; g < solution[w].Count; g++)
            {
                groups.Add(new
                {
                    GroupNumber = g + 1,
                    Players = solution[w][g]
                });
            }
            weeks.Add(new
            {
                WeekNumber = w + 1,
                Groups = groups
            });
        }

        return weeks;
    }
}
