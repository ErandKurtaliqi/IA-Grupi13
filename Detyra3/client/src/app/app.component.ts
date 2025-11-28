import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
import { ChessService } from './services/chess.service';
import { ChessAnalysisResponse } from './models/chess.models';

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
  viewMode: 'single' | 'both' = 'single'; // 'single' or 'both'

  // UI states
  selectedSquare: { row: number, col: number } | null = null;

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

  analyzePosition() {
    this.loading = true;
    this.error = null;
    this.viewMode = 'single';
    this.bothAnalysis = null;
    
    this.chessService.analyzePosition({
      fen: this.fen,
      depth: this.depth
    }).subscribe({
      next: (response) => {
        this.analysis = response;
        this.loading = false;
        console.log('Analysis:', response);
      },
      error: (err) => {
        this.error = err.message || 'Failed to analyze position';
        this.loading = false;
        console.error('Error:', err);
      }
    });
  }

  analyzeBothScenarios() {
    this.loading = true;
    this.error = null;
    this.viewMode = 'both';
    this.analysis = null;
    
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
    this.analyzePosition();
  }

  resetToDefault() {
    this.fen = '2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1';
    this.depth = 4;
    this.parseFEN(this.fen);
    this.analyzePosition();
  }

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
}
