import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-social-golfers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './social-golfers.component.html',
  styleUrls: ['./social-golfers.component.css']
})
export class SocialGolfersComponent {
  numPlayers = signal(16);
  groupSize = signal(4);
  numWeeks = signal(5);
  depthLimit = signal(6);
  maxWeeksToTry = signal(15);
  
  algorithm = signal<'dfs' | 'depth-limited' | 'find-max'>('dfs');
  loading = signal(false);
  result = signal<any>(null);
  error = signal<string>('');

  constructor(private apiService: ApiService) {}

  solve() {
    this.loading.set(true);
    this.error.set('');
    this.result.set(null);

    const algo = this.algorithm();
    
    if (algo === 'dfs') {
      this.apiService.solveSocialGolfersDFS(
        this.numPlayers(), 
        this.groupSize(), 
        this.numWeeks()
      ).subscribe({
        next: (data) => {
          this.result.set(data);
          this.loading.set(false);
        },
        error: (err) => {
          this.error.set(err.error?.message || 'An error occurred');
          this.loading.set(false);
        }
      });
    } else if (algo === 'depth-limited') {
      this.apiService.solveSocialGolfersDepthLimited(
        this.numPlayers(), 
        this.groupSize(), 
        this.numWeeks(),
        this.depthLimit()
      ).subscribe({
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
      this.apiService.findMaxWeeks(
        this.numPlayers(), 
        this.groupSize(),
        this.maxWeeksToTry()
      ).subscribe({
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
}

