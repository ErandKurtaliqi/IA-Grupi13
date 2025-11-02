import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  problems = [
    {
      title: 'Social Golfers Problem',
      description: 'Schedule golfers into groups so no two golfers play together more than once.',
      algorithms: ['DFS with Backtracking', 'Depth Limited Search'],
      route: '/social-golfers',
      icon: '⛳',
      color: '#7702FF'
    },
    {
      title: 'Sudoku Solver',
      description: 'Solve 9×9 Sudoku puzzles using systematic search algorithms.',
      algorithms: ['BFS with Backtracking'],
      route: '/sudoku',
      icon: '🔢',
      color: '#CC26D5'
    },
    {
      title: 'Latin Square Generator',
      description: 'Generate n×n grids where each symbol appears once per row and column.',
      algorithms: ['IDDFS', 'Backtracking'],
      route: '/latin-square',
      icon: '📐',
      color: '#F0060B'
    }
  ];
}

