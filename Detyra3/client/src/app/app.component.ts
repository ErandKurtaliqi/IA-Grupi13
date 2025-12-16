import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
import { ChessService } from './services/chess.service';
import { ChessAnalysisResponse } from './models/chess.models';

interface LegalMove {
  fromRow: number;
  fromCol: number;
  toRow: number;
  toCol: number;
  notation: string;
  evaluation?: number;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Chess MiniMax with Alpha-Beta Pruning';
  
  // Chess board state
  board: string[][] = [];
  fen: string = '2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1';
  depth: number = 4;
  
  // Analysis results
  analysis: ChessAnalysisResponse | null = null;
  bothAnalysis: any = null;
  loading: boolean = false;
  error: string | null = null;
  viewMode: 'single' | 'both' = 'single';

  // Drag and drop states
  selectedSquare: { row: number, col: number } | null = null;
  draggingPiece: { row: number, col: number } | null = null;
  legalMoves: LegalMove[] = [];
  highlightedSquares: { row: number, col: number, type: 'selected' | 'legal' | 'capture' }[] = [];

  constructor(private chessService: ChessService) {}

  ngOnInit() {
    this.parseFEN(this.fen);
    this.analyzePosition();
  }

  parseFEN(fen: string) {
    const position = fen.split(' ')[0];
    const rows = position.split('/');
    
    this.board = [];
    for (let i = 0; i < 8; i++) {
      this.board[i] = [];
      let col = 0;
      for (const char of rows[i]) {
        if (char >= '1' && char <= '8') {
          const emptySquares = parseInt(char);
          for (let j = 0; j < emptySquares; j++) {
            this.board[i][col++] = '.';
          }
        } else {
          this.board[i][col++] = char;
        }
      }
      // Fill remaining squares
      while (col < 8) {
        this.board[i][col++] = '.';
      }
    }
  }

  generateFENFromBoard(): string {
    let fen = '';
    for (let i = 0; i < 8; i++) {
      let emptyCount = 0;
      for (let j = 0; j < 8; j++) {
        if (this.board[i][j] === '.') {
          emptyCount++;
        } else {
          if (emptyCount > 0) {
            fen += emptyCount.toString();
            emptyCount = 0;
          }
          fen += this.board[i][j];
        }
      }
      if (emptyCount > 0) {
        fen += emptyCount.toString();
      }
      if (i < 7) fen += '/';
    }
    
    // Get the turn from current FEN
    const currentTurn = this.fen.split(' ')[1] || 'w';
    fen += ` ${currentTurn} - - 0 1`;
    return fen;
  }

  analyzePosition() {
    this.loading = true;
    this.error = null;
    this.viewMode = 'single';
    this.bothAnalysis = null;
    this.clearHighlights();
    
    this.chessService.analyzePosition({
      fen: this.fen,
      depth: this.depth
    }).subscribe({
      next: (response) => {
        this.analysis = response;
        this.loading = false;
        this.extractLegalMoves();
        console.log('Analysis:', response);
      },
      error: (err) => {
        this.error = err.message || 'Failed to analyze position';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  extractLegalMoves() {
    // Extract legal moves from top moves analysis
    if (this.analysis?.topMoves) {
      this.legalMoves = this.analysis.topMoves.map(move => {
        const parsed = this.parseMoveNotation(move.notation);
        return {
          ...parsed,
          notation: move.notation,
          evaluation: move.evaluation
        };
      });
    }
  }

  parseMoveNotation(notation: string): { fromRow: number, fromCol: number, toRow: number, toCol: number } {
    // Parse notation like "Nc4 → d6" or "Qf4 → d6"
    const files: { [key: string]: number } = { 'a': 0, 'b': 1, 'c': 2, 'd': 3, 'e': 4, 'f': 5, 'g': 6, 'h': 7 };
    
    // Remove piece letter and split by arrow
    const parts = notation.split(' → ');
    if (parts.length !== 2) return { fromRow: -1, fromCol: -1, toRow: -1, toCol: -1 };
    
    const from = parts[0].substring(1); // Remove piece letter
    const to = parts[1];
    
    const fromCol = files[from[0]];
    const fromRow = 8 - parseInt(from[1]);
    const toCol = files[to[0]];
    const toRow = 8 - parseInt(to[1]);
    
    return { fromRow, fromCol, toRow, toCol };
  }

  analyzeBothScenarios() {
    this.loading = true;
    this.error = null;
    this.viewMode = 'both';
    this.analysis = null;
    this.clearHighlights();
    
    this.chessService.analyzeBothScenarios({
      fen: this.fen,
      depth: this.depth
    }).subscribe({
      next: (response) => {
        this.bothAnalysis = response;
        this.loading = false;
        console.log('Both scenarios analysis:', response);
      },
      error: (err) => {
        this.error = err.message || 'Failed to analyze both scenarios';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  switchTurn() {
    const parts = this.fen.split(' ');
    parts[1] = parts[1] === 'w' ? 'b' : 'w';
    this.fen = parts.join(' ');
    this.parseFEN(this.fen);
    this.clearHighlights();
    this.analyzePosition();
  }

  resetToDefault() {
    this.fen = '2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1';
    this.depth = 4;
    this.parseFEN(this.fen);
    this.clearHighlights();
    this.analyzePosition();
  }

  // ============ Drag and Drop Functionality ============

  onSquareClick(row: number, col: number) {
    const piece = this.board[row][col];
    const isWhiteTurn = this.fen.split(' ')[1] === 'w';
    
    // If we have a selected piece and clicking on a valid destination
    if (this.selectedSquare) {
      const isLegalMove = this.highlightedSquares.some(
        sq => sq.row === row && sq.col === col && (sq.type === 'legal' || sq.type === 'capture')
      );
      
      if (isLegalMove) {
        this.makeMove(this.selectedSquare.row, this.selectedSquare.col, row, col);
        return;
      }
    }
    
    // If clicking on own piece, select it
    if (piece !== '.' && this.isOwnPiece(piece, isWhiteTurn)) {
      this.selectPiece(row, col);
    } else {
      this.clearHighlights();
    }
  }

  isOwnPiece(piece: string, isWhiteTurn: boolean): boolean {
    if (piece === '.') return false;
    const isWhitePiece = piece === piece.toUpperCase();
    return isWhitePiece === isWhiteTurn;
  }

  selectPiece(row: number, col: number) {
    this.selectedSquare = { row, col };
    this.highlightedSquares = [{ row, col, type: 'selected' }];
    
    // Calculate legal moves for this piece
    this.calculateLegalMovesForPiece(row, col);
  }

  calculateLegalMovesForPiece(row: number, col: number) {
    const piece = this.board[row][col];
    if (piece === '.') return;
    
    const isWhite = piece === piece.toUpperCase();
    const lowerPiece = piece.toLowerCase();
    
    const moves: { row: number, col: number }[] = [];
    
    switch (lowerPiece) {
      case 'k':
        this.getKingMoves(row, col, isWhite, moves);
        break;
      case 'q':
        this.getQueenMoves(row, col, isWhite, moves);
        break;
      case 'r':
        this.getRookMoves(row, col, isWhite, moves);
        break;
      case 'b':
        this.getBishopMoves(row, col, isWhite, moves);
        break;
      case 'n':
        this.getKnightMoves(row, col, isWhite, moves);
        break;
      case 'p':
        this.getPawnMoves(row, col, isWhite, moves);
        break;
    }
    
    // Add legal move highlights
    for (const move of moves) {
      const targetPiece = this.board[move.row][move.col];
      const type = targetPiece !== '.' ? 'capture' : 'legal';
      this.highlightedSquares.push({ row: move.row, col: move.col, type });
    }
  }

  getKingMoves(row: number, col: number, isWhite: boolean, moves: { row: number, col: number }[]) {
    const directions = [[-1,-1], [-1,0], [-1,1], [0,-1], [0,1], [1,-1], [1,0], [1,1]];
    for (const [dr, dc] of directions) {
      const newRow = row + dr;
      const newCol = col + dc;
      if (this.isValidTarget(newRow, newCol, isWhite)) {
        moves.push({ row: newRow, col: newCol });
      }
    }
  }

  getQueenMoves(row: number, col: number, isWhite: boolean, moves: { row: number, col: number }[]) {
    this.getRookMoves(row, col, isWhite, moves);
    this.getBishopMoves(row, col, isWhite, moves);
  }

  getRookMoves(row: number, col: number, isWhite: boolean, moves: { row: number, col: number }[]) {
    const directions = [[0,1], [0,-1], [1,0], [-1,0]];
    for (const [dr, dc] of directions) {
      for (let i = 1; i < 8; i++) {
        const newRow = row + dr * i;
        const newCol = col + dc * i;
        if (!this.isInBounds(newRow, newCol)) break;
        
        const target = this.board[newRow][newCol];
        if (target === '.') {
          moves.push({ row: newRow, col: newCol });
        } else {
          if (this.isEnemy(target, isWhite)) {
            moves.push({ row: newRow, col: newCol });
          }
          break;
        }
      }
    }
  }

  getBishopMoves(row: number, col: number, isWhite: boolean, moves: { row: number, col: number }[]) {
    const directions = [[1,1], [1,-1], [-1,1], [-1,-1]];
    for (const [dr, dc] of directions) {
      for (let i = 1; i < 8; i++) {
        const newRow = row + dr * i;
        const newCol = col + dc * i;
        if (!this.isInBounds(newRow, newCol)) break;
        
        const target = this.board[newRow][newCol];
        if (target === '.') {
          moves.push({ row: newRow, col: newCol });
        } else {
          if (this.isEnemy(target, isWhite)) {
            moves.push({ row: newRow, col: newCol });
          }
          break;
        }
      }
    }
  }

  getKnightMoves(row: number, col: number, isWhite: boolean, moves: { row: number, col: number }[]) {
    const offsets = [[-2,-1], [-2,1], [-1,-2], [-1,2], [1,-2], [1,2], [2,-1], [2,1]];
    for (const [dr, dc] of offsets) {
      const newRow = row + dr;
      const newCol = col + dc;
      if (this.isValidTarget(newRow, newCol, isWhite)) {
        moves.push({ row: newRow, col: newCol });
      }
    }
  }

  getPawnMoves(row: number, col: number, isWhite: boolean, moves: { row: number, col: number }[]) {
    const direction = isWhite ? -1 : 1;
    const startRow = isWhite ? 6 : 1;
    
    // Forward move
    const newRow = row + direction;
    if (this.isInBounds(newRow, col) && this.board[newRow][col] === '.') {
      moves.push({ row: newRow, col: col });
      
      // Double move from start
      const doubleRow = row + 2 * direction;
      if (row === startRow && this.board[doubleRow][col] === '.') {
        moves.push({ row: doubleRow, col: col });
      }
    }
    
    // Captures
    for (const dc of [-1, 1]) {
      const captureCol = col + dc;
      if (this.isInBounds(newRow, captureCol)) {
        const target = this.board[newRow][captureCol];
        if (target !== '.' && this.isEnemy(target, isWhite)) {
          moves.push({ row: newRow, col: captureCol });
        }
      }
    }
  }

  isInBounds(row: number, col: number): boolean {
    return row >= 0 && row < 8 && col >= 0 && col < 8;
  }

  isValidTarget(row: number, col: number, isWhite: boolean): boolean {
    if (!this.isInBounds(row, col)) return false;
    const target = this.board[row][col];
    if (target === '.') return true;
    return this.isEnemy(target, isWhite);
  }

  isEnemy(piece: string, isWhite: boolean): boolean {
    if (piece === '.') return false;
    const isPieceWhite = piece === piece.toUpperCase();
    return isPieceWhite !== isWhite;
  }

  clearHighlights() {
    this.selectedSquare = null;
    this.highlightedSquares = [];
  }

  makeMove(fromRow: number, fromCol: number, toRow: number, toCol: number) {
    const piece = this.board[fromRow][fromCol];
    
    // Update board
    this.board[toRow][toCol] = piece;
    this.board[fromRow][fromCol] = '.';
    
    // Switch turn
    const parts = this.fen.split(' ');
    parts[1] = parts[1] === 'w' ? 'b' : 'w';
    
    // Generate new FEN
    this.fen = this.generateFENFromBoard();
    const newParts = this.fen.split(' ');
    newParts[1] = parts[1];
    this.fen = newParts.join(' ');
    
    this.clearHighlights();
    this.analyzePosition();
  }

  // ============ Drag Events ============

  onDragStart(event: DragEvent, row: number, col: number) {
    const piece = this.board[row][col];
    const isWhiteTurn = this.fen.split(' ')[1] === 'w';
    
    if (piece === '.' || !this.isOwnPiece(piece, isWhiteTurn)) {
      event.preventDefault();
      return;
    }
    
    this.draggingPiece = { row, col };
    this.selectPiece(row, col);
    
    if (event.dataTransfer) {
      event.dataTransfer.effectAllowed = 'move';
      event.dataTransfer.setData('text/plain', `${row},${col}`);
    }
  }

  onDragOver(event: DragEvent, row: number, col: number) {
    const isLegalMove = this.highlightedSquares.some(
      sq => sq.row === row && sq.col === col && (sq.type === 'legal' || sq.type === 'capture')
    );
    
    if (isLegalMove) {
      event.preventDefault();
      if (event.dataTransfer) {
        event.dataTransfer.dropEffect = 'move';
      }
    }
  }

  onDrop(event: DragEvent, toRow: number, toCol: number) {
    event.preventDefault();
    
    if (this.draggingPiece) {
      const isLegalMove = this.highlightedSquares.some(
        sq => sq.row === toRow && sq.col === toCol && (sq.type === 'legal' || sq.type === 'capture')
      );
      
      if (isLegalMove) {
        this.makeMove(this.draggingPiece.row, this.draggingPiece.col, toRow, toCol);
      }
    }
    
    this.draggingPiece = null;
  }

  onDragEnd(event: DragEvent) {
    this.draggingPiece = null;
  }

  // ============ UI Helpers ============

  getPieceSymbol(piece: string): string {
    const symbols: { [key: string]: string } = {
      'K': '♔', 'Q': '♕', 'R': '♖', 'B': '♗', 'N': '♘', 'P': '♙',
      'k': '♚', 'q': '♛', 'r': '♜', 'b': '♝', 'n': '♞', 'p': '♟'
    };
    return symbols[piece] || '';
  }

  isWhitePiece(piece: string): boolean {
    return piece === piece.toUpperCase() && piece !== '.';
  }

  getSquareColor(row: number, col: number): string {
    return (row + col) % 2 === 0 ? 'light' : 'dark';
  }

  getSquareClass(row: number, col: number): string {
    const baseClass = this.getSquareColor(row, col);
    const highlight = this.highlightedSquares.find(sq => sq.row === row && sq.col === col);
    
    if (highlight) {
      return `${baseClass} ${highlight.type}`;
    }
    return baseClass;
  }

  isHighlighted(row: number, col: number): boolean {
    return this.highlightedSquares.some(sq => sq.row === row && sq.col === col);
  }

  getHighlightType(row: number, col: number): string {
    const highlight = this.highlightedSquares.find(sq => sq.row === row && sq.col === col);
    return highlight?.type || '';
  }

  isDraggable(row: number, col: number): boolean {
    const piece = this.board[row][col];
    if (piece === '.') return false;
    const isWhiteTurn = this.fen.split(' ')[1] === 'w';
    return this.isOwnPiece(piece, isWhiteTurn);
  }

  getEvaluationBar(): number {
    if (!this.analysis) return 50;
    
    // Convert evaluation to percentage (0-100)
    // Normalize between -2000 to +2000
    const normalized = Math.max(-2000, Math.min(2000, this.analysis.evaluation));
    return ((normalized + 2000) / 4000) * 100;
  }

  getEvaluationColor(): string {
    if (!this.analysis) return '#888';
    
    if (this.analysis.evaluation > 300) return '#4caf50';
    if (this.analysis.evaluation > 100) return '#8bc34a';
    if (this.analysis.evaluation > -100) return '#ff9800';
    if (this.analysis.evaluation > -300) return '#ff5722';
    return '#f44336';
  }

  formatEvaluation(value: number): string {
    if (value > 10000) return 'White wins';
    if (value < -10000) return 'Black wins';
    return (value / 100).toFixed(2);
  }

  // Make best move automatically
  makeBestMove() {
    if (!this.analysis?.bestMoveNotation) return;
    
    const parsed = this.parseMoveNotation(this.analysis.bestMoveNotation);
    if (parsed.fromRow >= 0) {
      this.makeMove(parsed.fromRow, parsed.fromCol, parsed.toRow, parsed.toCol);
    }
  }
}
