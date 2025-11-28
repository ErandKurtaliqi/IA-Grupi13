import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { KillerSudokuService } from '../../services/killer-sudoku.service';
import { Cage, CellPosition } from '../../models/models';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-killer-sudoku',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './killer-sudoku.component.html',
  styleUrls: ['./killer-sudoku.component.css']
})
export class KillerSudokuComponent implements OnInit {
  grid: number[][] = [];
  solution: number[][] = [];
  cages: Cage[] = [];
  
  // Results
  backtrackCount: number = 0;
  executionTime: number = 0;
  message: string = '';
  isLoading: boolean = false;
  showSolution: boolean = false;
  
  // For adding cages
  cageSum: number = 15;
  cageCells: string = '0,0;0,1';

  constructor(private killerSudokuService: KillerSudokuService) {}

  ngOnInit(): void {
    this.initializeGrid();
    this.loadSamplePuzzle();
    // Don't auto-solve - let users click the button
  }

  initializeGrid(): void {
    this.grid = Array(9).fill(0).map(() => Array(9).fill(0));
    this.solution = Array(9).fill(0).map(() => Array(9).fill(0));
  }

  loadSamplePuzzle(): void {
    this.initializeGrid();
    
    // Pre-fill some cells to make it easier to solve
    this.grid[0][0] = 1;
    this.grid[0][1] = 2;
    this.grid[1][0] = 3;
    this.grid[1][1] = 4;
    
    // Very simple cages for a 4x4 mini sudoku-like puzzle
    // We'll use a minimal set that's guaranteed solvable
    this.cages = [
      // Top-left 2x2
      { sum: 3, cells: [{row: 0, col: 0}, {row: 0, col: 1}] },
      { sum: 7, cells: [{row: 1, col: 0}, {row: 1, col: 1}] },
      
      // Some more simple cages
      { sum: 12, cells: [{row: 0, col: 2}, {row: 0, col: 3}] },
      { sum: 11, cells: [{row: 1, col: 2}, {row: 1, col: 3}] },
      { sum: 10, cells: [{row: 2, col: 0}, {row: 2, col: 1}] },
      { sum: 14, cells: [{row: 2, col: 2}, {row: 2, col: 3}] },
      { sum: 13, cells: [{row: 3, col: 0}, {row: 3, col: 1}] },
      { sum: 9, cells: [{row: 3, col: 2}, {row: 3, col: 3}] }
    ];
  }

  addCage(): void {
    try {
      const cells: CellPosition[] = this.cageCells.split(';').map(cell => {
        const [row, col] = cell.split(',').map(n => parseInt(n.trim()));
        return { row, col };
      });
      
      if (cells.length > 0 && cells.every(c => c.row >= 0 && c.row < 9 && c.col >= 0 && c.col < 9)) {
        this.cages.push({ sum: this.cageSum, cells });
        this.cageCells = '';
      }
    } catch (e) {
      console.error('Invalid cage format', e);
    }
  }

  removeCage(index: number): void {
    this.cages.splice(index, 1);
  }

  clearCages(): void {
    this.cages = [];
  }

  solve(): void {
    this.isLoading = true;
    this.showSolution = false;
    this.message = '';

    const request = {
      grid: this.grid,
      cages: this.cages
    };

    this.killerSudokuService.solve(request).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.showSolution = true;
        this.solution = response.solution || this.grid;
        this.backtrackCount = response.backtrackCount;
        this.executionTime = response.executionTimeMs;
        this.message = response.message || '';
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Full error:', error);
        if (error.error) {
          console.error('Error details:', error.error);
        }
        this.message = 'Error: ' + (error.error?.message || error.message || 'Failed to solve. Check console for details.');
      }
    });
  }

  getCellValue(row: number, col: number): number {
    return this.showSolution ? this.solution[row][col] : this.grid[row][col];
  }

  getCageForCell(row: number, col: number): Cage | undefined {
    return this.cages.find(cage => 
      cage.cells.some(cell => cell.row === row && cell.col === col)
    );
  }

  getCageSum(row: number, col: number): number | null {
    const cage = this.getCageForCell(row, col);
    if (cage) {
      // Only show sum on the first cell (top-left) of the cage
      const firstCell = cage.cells.sort((a, b) => a.row - b.row || a.col - b.col)[0];
      if (firstCell.row === row && firstCell.col === col) {
        return cage.sum;
      }
    }
    return null;
  }

  getCageId(row: number, col: number): number {
    return this.cages.findIndex(cage => 
      cage.cells.some(cell => cell.row === row && cell.col === col)
    );
  }

  hasBorderRight(row: number, col: number): boolean {
    if (col === 8) return true;
    const cage = this.getCageForCell(row, col);
    const nextCage = this.getCageForCell(row, col + 1);
    return cage !== nextCage;
  }

  hasBorderBottom(row: number, col: number): boolean {
    if (row === 8) return true;
    const cage = this.getCageForCell(row, col);
    const nextCage = this.getCageForCell(row + 1, col);
    return cage !== nextCage;
  }

  getRange(n: number): number[] {
    return Array.from({ length: n }, (_, i) => i);
  }
}

