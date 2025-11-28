using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChessController : ControllerBase
    {
        private readonly ChessService _chessService;
        private readonly ILogger<ChessController> _logger;

        public ChessController(ChessService chessService, ILogger<ChessController> logger)
        {
            _chessService = chessService;
            _logger = logger;
        }

        [HttpPost("analyze")]
        public ActionResult<ChessAnalysisResponse> AnalyzePosition([FromBody] ChessAnalysisRequest request)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Parse the FEN and create board
                var board = new ChessBoard();
                board.InitializeFromFEN(request.FEN);

                // Create MiniMax service
                var miniMaxService = new MiniMaxService(_chessService);

                // Find best move
                var (bestMove, evaluation, nodesExplored, nodesPruned, pv) = 
                    miniMaxService.FindBestMove(board, request.Depth, board.IsWhiteTurn);

                // Analyze top moves
                var topMovesAnalysis = miniMaxService.AnalyzeAllMoves(board, request.Depth, board.IsWhiteTurn);

                stopwatch.Stop();

                var response = new ChessAnalysisResponse
                {
                    CurrentPosition = board.ToFEN(),
                    IsWhiteTurn = board.IsWhiteTurn,
                    Evaluation = evaluation,
                    BestMove = bestMove?.ToString(),
                    BestMoveNotation = bestMove?.ToNotation(),
                    PrincipalVariation = pv.Select(m => m.ToNotation()).ToList(),
                    NodesExplored = nodesExplored,
                    NodesPruned = nodesPruned,
                    ComputationTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    TopMoves = topMovesAnalysis.Take(5).Select(m => new MoveAnalysis
                    {
                        Move = m.move.ToString(),
                        Notation = m.move.ToNotation(),
                        Evaluation = m.evaluation,
                        EvaluationDescription = GetEvaluationDescription(m.evaluation)
                    }).ToList(),
                    ResultMessage = GetResultMessage(evaluation, board.IsWhiteTurn, bestMove)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing chess position");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("default-position")]
        public ActionResult<string> GetDefaultPosition()
        {
            return Ok(new { fen = "2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1" });
        }

        [HttpPost("validate-fen")]
        public ActionResult<object> ValidateFEN([FromBody] string fen)
        {
            try
            {
                var board = new ChessBoard();
                board.InitializeFromFEN(fen);
                
                var whiteMoves = _chessService.GenerateAllMoves(board, true);
                var blackMoves = _chessService.GenerateAllMoves(board, false);

                return Ok(new
                {
                    valid = true,
                    isWhiteTurn = board.IsWhiteTurn,
                    whiteLegalMoves = whiteMoves.Count,
                    blackLegalMoves = blackMoves.Count,
                    position = board.ToFEN()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { valid = false, error = ex.Message });
            }
        }

        [HttpPost("analyze-both")]
        public ActionResult<object> AnalyzeBothScenarios([FromBody] ChessAnalysisRequest request)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Parse the FEN and create board
                var boardWhite = new ChessBoard();
                boardWhite.InitializeFromFEN(request.FEN);

                var boardBlack = new ChessBoard();
                boardBlack.InitializeFromFEN(request.FEN);

                // Ensure we analyze from both perspectives
                var fenParts = request.FEN.Split(' ');
                fenParts[1] = "w"; // White to move
                var fenWhite = string.Join(' ', fenParts);
                boardWhite.InitializeFromFEN(fenWhite);

                fenParts[1] = "b"; // Black to move
                var fenBlack = string.Join(' ', fenParts);
                boardBlack.InitializeFromFEN(fenBlack);

                // Create MiniMax service
                var miniMaxService = new MiniMaxService(_chessService);

                // Analyze White to move
                var (bestMoveWhite, evalWhite, nodesWhite, prunedWhite, pvWhite) = 
                    miniMaxService.FindBestMove(boardWhite, request.Depth, true);
                var topMovesWhite = miniMaxService.AnalyzeAllMoves(boardWhite, request.Depth, true);

                // Analyze Black to move
                var (bestMoveBlack, evalBlack, nodesBlack, prunedBlack, pvBlack) = 
                    miniMaxService.FindBestMove(boardBlack, request.Depth, false);
                var topMovesBlack = miniMaxService.AnalyzeAllMoves(boardBlack, request.Depth, false);

                stopwatch.Stop();

                var response = new
                {
                    whiteToMove = new
                    {
                        currentPosition = fenWhite,
                        isWhiteTurn = true,
                        evaluation = evalWhite,
                        bestMove = bestMoveWhite?.ToString(),
                        bestMoveNotation = bestMoveWhite?.ToNotation(),
                        principalVariation = pvWhite.Select(m => m.ToNotation()).ToList(),
                        nodesExplored = nodesWhite,
                        nodesPruned = prunedWhite,
                        topMoves = topMovesWhite.Take(5).Select(m => new
                        {
                            move = m.move.ToString(),
                            notation = m.move.ToNotation(),
                            evaluation = m.evaluation,
                            evaluationDescription = GetEvaluationDescription(m.evaluation)
                        }).ToList(),
                        resultMessage = GetResultMessage(evalWhite, true, bestMoveWhite)
                    },
                    blackToMove = new
                    {
                        currentPosition = fenBlack,
                        isWhiteTurn = false,
                        evaluation = evalBlack,
                        bestMove = bestMoveBlack?.ToString(),
                        bestMoveNotation = bestMoveBlack?.ToNotation(),
                        principalVariation = pvBlack.Select(m => m.ToNotation()).ToList(),
                        nodesExplored = nodesBlack,
                        nodesPruned = prunedBlack,
                        topMoves = topMovesBlack.Take(5).Select(m => new
                        {
                            move = m.move.ToString(),
                            notation = m.move.ToNotation(),
                            evaluation = m.evaluation,
                            evaluationDescription = GetEvaluationDescription(m.evaluation)
                        }).ToList(),
                        resultMessage = GetResultMessage(evalBlack, false, bestMoveBlack)
                    },
                    computationTimeMs = stopwatch.Elapsed.TotalMilliseconds,
                    summary = new
                    {
                        positionFEN = request.FEN.Split(' ')[0],
                        whiteAdvantage = evalWhite > 0 ? "White has advantage" : evalWhite < 0 ? "Black has advantage" : "Equal",
                        blackAdvantage = evalBlack > 0 ? "White has advantage" : evalBlack < 0 ? "Black has advantage" : "Equal",
                        totalNodesExplored = nodesWhite + nodesBlack,
                        totalNodesPruned = prunedWhite + prunedBlack,
                        overallPruningEfficiency = ((double)(prunedWhite + prunedBlack) / (nodesWhite + nodesBlack + prunedWhite + prunedBlack) * 100).ToString("F1") + "%"
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing both scenarios");
                return BadRequest(new { error = ex.Message });
            }
        }

        private string GetEvaluationDescription(int evaluation)
        {
            if (evaluation > 900) return "Winning for White";
            if (evaluation > 300) return "White is better";
            if (evaluation > 100) return "White is slightly better";
            if (evaluation > -100) return "Equal position";
            if (evaluation > -300) return "Black is slightly better";
            if (evaluation > -900) return "Black is better";
            return "Winning for Black";
        }

        private string GetResultMessage(int evaluation, bool isWhiteTurn, Move? bestMove)
        {
            if (bestMove == null)
            {
                return isWhiteTurn ? "Checkmate! Black wins." : "Checkmate! White wins.";
            }

            var player = isWhiteTurn ? "White" : "Black";
            var evalDesc = GetEvaluationDescription(evaluation);
            
            return $"{player} to move. Best move: {bestMove.ToNotation()}. Position: {evalDesc}";
        }
    }
}

