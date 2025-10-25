using Detyra3.Interfaces;

namespace Detyra3.Services
{
    public class LatinSquareSolverService : ILatinSquareSolverService
    {
        public int[,] SolveBacktracking(int n)
        {
            if (n < 1) throw new ArgumentException("n duhet të jetë >= 1");

            var grid = new int[n, n];

            // Optimizim: mbajmë set-et e përdorimit për çdo rresht dhe kolonë.
            var rowUsed = new bool[n, n + 1]; // rowUsed[r, val]
            var colUsed = new bool[n, n + 1]; // colUsed[c, val]

            bool Backtrack(int idx)
            {
                if (idx == n * n) return true; // të gjitha qelizat u mbushën

                int r = idx / n;
                int c = idx % n;

                // Heuristikë e lehtë: provojmë një zhvendosje ciklike që prodhon shpejt një Latin Square
                // por prapë kontrollojmë kufizimet.
                for (int val = 1; val <= n; val++)
                {
                    if (!rowUsed[r, val] && !colUsed[c, val])
                    {
                        grid[r, c] = val;
                        rowUsed[r, val] = true;
                        colUsed[c, val] = true;

                        if (Backtrack(idx + 1)) return true;

                        // backtrack
                        rowUsed[r, val] = false;
                        colUsed[c, val] = false;
                        grid[r, c] = 0;
                    }
                }
                return false;
            }

            // Trik për ta nisur me një rresht bazë që shpesh e bën kërkimin trivial:
            // Rreshti i parë: 1..n
            for (int c = 0; c < n; c++)
            {
                int v = c + 1;
                grid[0, c] = v;
                rowUsed[0, v] = true;
                colUsed[c, v] = true;
            }

            // Backtracking fillon nga rreshti 1 (idx = n)
            if (!Backtrack(n))
                throw new InvalidOperationException("Zgjidhja nuk u gjet (e pazakontë për Latin Square).");

            return grid;
        }

        public int[,] SolveIddfs(int n)
        {
            if (n < 1) throw new ArgumentException("n duhet të jetë >= 1");

            var grid = new int[n, n];
            var rowUsed = new bool[n, n + 1];
            var colUsed = new bool[n, n + 1];

            // Rreshti i parë pre-inicializohet si më sipër për ta reduktuar hapësirën e kërkimit.
            for (int c = 0; c < n; c++)
            {
                int v = c + 1;
                grid[0, c] = v;
                rowUsed[0, v] = true;
                colUsed[c, v] = true;
            }

            bool Dls(int limit, int depth)
            {
                if (depth == limit)
                    return depth == n * n; // sukses vetëm kur jemi në fund të plotësimit

                int r = depth / n;
                int c = depth % n;

                // nëse jemi në rreshtin 0, qeliza është tashmë e vendosur nga inicializimi
                if (r == 0)
                {
                    return Dls(limit, depth + 1);
                }

                for (int val = 1; val <= n; val++)
                {
                    if (!rowUsed[r, val] && !colUsed[c, val])
                    {
                        grid[r, c] = val;
                        rowUsed[r, val] = true;
                        colUsed[c, val] = true;

                        if (Dls(limit, depth + 1)) return true;

                        rowUsed[r, val] = false;
                        colUsed[c, val] = false;
                        grid[r, c] = 0;
                    }
                }
                return false;
            }

            // IDDFS: rrit gradualisht limitin e thellësisë
            for (int limit = 0; limit <= n * n; limit++)
            {
                if (Dls(limit, depth: 0))
                    return grid;
            }

            throw new InvalidOperationException("Zgjidhja nuk u gjet me IDDFS.");
        }
    }
}
