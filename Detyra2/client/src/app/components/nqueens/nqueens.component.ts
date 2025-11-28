import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NQueensService } from '../../services/nqueens.service';
import { Position } from '../../models/models';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-nqueens',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './nqueens.component.html',
  styleUrls: ['./nqueens.component.css']
})
export class NQueensComponent implements OnInit {
  n: number = 4;
  heuristicType: string = 'combined';
  heuristics: string[] = ['conflicts', 'attacking', 'combined'];
  blockedPositions: Position[] = [];
  solution: Position[] = [];
  nodesExplored: number = 0;
  executionTime: number = 0;
  message: string = '';
  isLoading: boolean = false;
  showSolution: boolean = false;
  
  // For adding blocked positions
  blockRow: number = 0;
  blockCol: number = 0;

  constructor(private nQueensService: NQueensService) {}

  ngOnInit(): void {
    // Load with solvable sample - but don't auto-solve
    // This way users can see the parameters and click solve themselves
    this.blockedPositions = [];
  }

  addBlockedPosition(): void {
    if (this.blockRow >= 0 && this.blockRow < this.n && this.blockCol >= 0 && this.blockCol < this.n) {
      const exists = this.blockedPositions.some(p => p.row === this.blockRow && p.col === this.blockCol);
      if (!exists) {
        this.blockedPositions.push({ row: this.blockRow, col: this.blockCol });
      }
    }
  }

  removeBlockedPosition(index: number): void {
    this.blockedPositions.splice(index, 1);
  }

  clearBlocked(): void {
    this.blockedPositions = [];
  }

  solve(): void {
    this.isLoading = true;
    this.showSolution = false;
    this.message = '';

    const request = {
      n: this.n,
      blockedPositions: this.blockedPositions.length > 0 ? this.blockedPositions : undefined,
      heuristicType: this.heuristicType
    };

    this.nQueensService.solve(request).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.showSolution = true;
        this.solution = response.solution || [];
        this.nodesExplored = response.nodesExplored;
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

  isQueen(row: number, col: number): boolean {
    return this.solution.some(p => p.row === row && p.col === col);
  }

  isBlocked(row: number, col: number): boolean {
    return this.blockedPositions.some(p => p.row === row && p.col === col);
  }

  getRange(n: number): number[] {
    return Array.from({ length: n }, (_, i) => i);
  }
}

