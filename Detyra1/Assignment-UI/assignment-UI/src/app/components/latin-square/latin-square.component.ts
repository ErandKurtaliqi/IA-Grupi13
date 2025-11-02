import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-latin-square',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './latin-square.component.html',
  styleUrls: ['./latin-square.component.css']
})
export class LatinSquareComponent {
  n = signal(5);
  algorithm = signal<'backtracking' | 'iddfs'>('iddfs');
  loading = signal(false);
  result = signal<any>(null);
  error = signal<string>('');

  constructor(private apiService: ApiService) {}

  solve() {
    this.loading.set(true);
    this.error.set('');
    this.result.set(null);

    this.apiService.solveLatinSquare(this.n(), this.algorithm()).subscribe({
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

  getColorForValue(value: number, max: number): string {
    const hue = (value - 1) * (360 / max);
    return `hsl(${hue}, 70%, 85%)`;
  }
}

