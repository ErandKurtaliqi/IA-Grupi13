import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-sudoku',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './sudoku.component.html',
  styleUrls: ['./sudoku.component.css']
})
export class SudokuComponent {
  difficulty = signal<'Easy' | 'Medium' | 'Hard'>('Easy');
  loading = signal(false);
  result = signal<any>(null);
  error = signal<string>('');
  customMode = signal(false);
  customGrid = signal<number[][]>([]);

  constructor(private apiService: ApiService) {
    this.initializeCustomGrid();
  }

  initializeCustomGrid() {
    const grid: number[][] = [];
    for (let i = 0; i < 9; i++) {
      grid[i] = Array(9).fill(0);
    }
    this.customGrid.set(grid);
  }

  solve() {
    this.loading.set(true);
    this.error.set('');
    this.result.set(null);

    if (this.customMode()) {
      this.apiService.solveCustomSudoku(this.customGrid()).subscribe({
        next: (data) => {
          this.result.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set(err.error?.message || 'An error occurred');
          this.loading.set(false);
        }
      });
    } else {
      this.apiService.solveSudoku(this.difficulty()).subscribe({
        next: (data) => {
          this.result.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set(err.error?.message || 'An error occurred');
          this.loading.set(false);
        }
      });
    }
  }

  updateCell(row: number, col: number, value: string) {
    const num = parseInt(value) || 0;
    const grid = [...this.customGrid()];
    grid[row][col] = num;
    this.customGrid.set(grid);
  }

  clearCustomGrid() {
    this.initializeCustomGrid();
  }

  toggleMode() {
    this.customMode.set(!this.customMode());
    this.result.set(null);
  }
}

