using API.Models;
using System.Diagnostics;

namespace API.Services
{
    public class MiniMaxService
    {
        private readonly ChessService _chessService;
        private int _nodesExplored;
        private int _nodesPruned;
        private List<Move> _principalVariation;

        public MiniMaxService(ChessService chessService)
        {
            _chessService = chessService;
            _principalVariation = new List<Move>();
        }

        public (Move? bestMove, int evaluation, int nodesExplored, int nodesPruned, List<Move> pv) 
            FindBestMove(ChessBoard board, int depth, bool maximizingPlayer)
        {
            _nodesExplored = 0;
            _nodesPruned = 0;
            _principalVariation = new List<Move>();

            var stopwatch = Stopwatch.StartNew();
            
            var moves = _chessService.GenerateAllMoves(board, maximizingPlayer);
            
            if (moves.Count == 0)
            {
                return (null, _chessService.EvaluateBoard(board), 0, 0, new List<Move>());
            }

            Move? bestMove = null;
            int bestValue = maximizingPlayer ? int.MinValue : int.MaxValue;
            List<Move> bestPV = new List<Move>();

            foreach (var move in moves)
            {
                _chessService.MakeMove(board, move);
                
                var currentPV = new List<Move> { move };
                int value = AlphaBeta(board, depth - 1, int.MinValue, int.MaxValue, !maximizingPlayer, currentPV);
                
                _chessService.UndoMove(board, move);

                if (maximizingPlayer)
                {
                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestMove = move;
                        bestPV = new List<Move>(currentPV);
                    }
                }
                else
                {
                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestMove = move;
                        bestPV = new List<Move>(currentPV);
                    }
                }
            }

            stopwatch.Stop();
            
            return (bestMove, bestValue, _nodesExplored, _nodesPruned, bestPV);
        }

        private int AlphaBeta(ChessBoard board, int depth, int alpha, int beta, bool maximizingPlayer, List<Move> principalVariation)
        {
            _nodesExplored++;

            if (depth == 0 || _chessService.IsGameOver(board))
            {
                return _chessService.EvaluateBoard(board);
            }

            var moves = _chessService.GenerateAllMoves(board, maximizingPlayer);

            if (moves.Count == 0)
            {
                // Checkmate or stalemate
                if (_chessService.IsCheckmate(board, maximizingPlayer))
                {
                    return maximizingPlayer ? -999999 : 999999;
                }
                return 0; // Stalemate
            }

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                Move? bestMove = null;
                List<Move> bestPV = new List<Move>();

                foreach (var move in moves)
                {
                    _chessService.MakeMove(board, move);
                    
                    var currentPV = new List<Move>();
                    int eval = AlphaBeta(board, depth - 1, alpha, beta, false, currentPV);
                    
                    _chessService.UndoMove(board, move);

                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        bestMove = move;
                        bestPV = new List<Move> { move };
                        bestPV.AddRange(currentPV);
                    }

                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        _nodesPruned++;
                        break; // Beta cutoff
                    }
                }

                if (bestPV.Count > 0)
                {
                    principalVariation.Clear();
                    principalVariation.AddRange(bestPV);
                }

                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                Move? bestMove = null;
                List<Move> bestPV = new List<Move>();

                foreach (var move in moves)
                {
                    _chessService.MakeMove(board, move);
                    
                    var currentPV = new List<Move>();
                    int eval = AlphaBeta(board, depth - 1, alpha, beta, true, currentPV);
                    
                    _chessService.UndoMove(board, move);

                    if (eval < minEval)
                    {
                        minEval = eval;
                        bestMove = move;
                        bestPV = new List<Move> { move };
                        bestPV.AddRange(currentPV);
                    }

                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        _nodesPruned++;
                        break; // Alpha cutoff
                    }
                }

                if (bestPV.Count > 0)
                {
                    principalVariation.Clear();
                    principalVariation.AddRange(bestPV);
                }

                return minEval;
            }
        }

        public List<(Move move, int evaluation)> AnalyzeAllMoves(ChessBoard board, int depth, bool forWhite)
        {
            var moves = _chessService.GenerateAllMoves(board, forWhite);
            var moveEvaluations = new List<(Move, int)>();

            foreach (var move in moves)
            {
                _chessService.MakeMove(board, move);
                
                var dummyPV = new List<Move>();
                int eval = AlphaBeta(board, depth - 1, int.MinValue, int.MaxValue, !forWhite, dummyPV);
                
                _chessService.UndoMove(board, move);

                moveEvaluations.Add((move, eval));
            }

            // Sort by evaluation (best first)
            if (forWhite)
            {
                moveEvaluations = moveEvaluations.OrderByDescending(x => x.Item2).ToList();
            }
            else
            {
                moveEvaluations = moveEvaluations.OrderBy(x => x.Item2).ToList();
            }

            return moveEvaluations;
        }
    }
}

