import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  problems = [
    {
      title: 'Blocked N-Queens',
      icon: '♛',
      description: 'Solve the classic N-Queens problem with blocked positions using the powerful A* search algorithm.',
      route: '/nqueens',
      color: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      features: ['A* Algorithm', '3 Heuristics', 'Real-time Viz']
    },
    {
      title: 'SAT Problem',
      icon: '🎭',
      description: 'Arrange wedding guests at tables while satisfying complex boolean constraints and preferences.',
      route: '/sat',
      color: 'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
      features: ['SAT Solving', 'Constraints', 'Formula Gen']
    },
    {
      title: 'Killer Sudoku',
      icon: '🔪',
      description: 'Solve advanced Sudoku puzzles with cage constraints using CSP backtracking techniques.',
      route: '/killer-sudoku',
      color: 'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
      features: ['CSP', 'Backtracking', 'Forward Check']
    }
  ];
}
