export interface ChessAnalysisRequest {
  fen: string;
  depth: number;
}

export interface ChessAnalysisResponse {
  currentPosition: string;
  isWhiteTurn: boolean;
  evaluation: number;
  bestMove: string | null;
  bestMoveNotation: string | null;
  principalVariation: string[];
  nodesExplored: number;
  nodesPruned: number;
  computationTimeMs: number;
  topMoves: MoveAnalysis[];
  resultMessage: string;
}

export interface MoveAnalysis {
  move: string;
  notation: string;
  evaluation: number;
  evaluationDescription: string;
}

