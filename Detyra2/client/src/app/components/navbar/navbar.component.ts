import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="navbar">
      <div class="navbar-container">
        <a routerLink="/" class="navbar-brand">
          <span class="brand-icon">🧠</span>
          <span class="brand-text">AI Algorithms</span>
        </a>
        <div class="navbar-links">
          <a routerLink="/nqueens" routerLinkActive="active" class="nav-link">
            <span class="link-icon">♛</span>
            <span>N-Queens</span>
          </a>
          <a routerLink="/sat" routerLinkActive="active" class="nav-link">
            <span class="link-icon">🎭</span>
            <span>SAT Problem</span>
          </a>
          <a routerLink="/killer-sudoku" routerLinkActive="active" class="nav-link">
            <span class="link-icon">🔪</span>
            <span>Killer Sudoku</span>
          </a>
          <a routerLink="/" class="nav-link home-link">
            <span class="link-icon">🏠</span>
            <span>Home</span>
          </a>
        </div>
      </div>
    </nav>
  `,
  styles: [`
    .navbar {
      background: rgba(10, 14, 39, 0.95);
      backdrop-filter: blur(10px);
      border-bottom: 1px solid rgba(255, 255, 255, 0.1);
      padding: 1rem 0;
      position: sticky;
      top: 0;
      z-index: 1000;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
    }

    .navbar-container {
      max-width: 1400px;
      margin: 0 auto;
      padding: 0 2rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .navbar-brand {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      text-decoration: none;
      color: white;
      font-size: 1.5rem;
      font-weight: 800;
      transition: transform 0.3s;
    }

    .navbar-brand:hover {
      transform: scale(1.05);
    }

    .brand-icon {
      font-size: 2rem;
    }

    .brand-text {
      background: linear-gradient(135deg, #fff 0%, #a8b7ff 100%);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      background-clip: text;
    }

    .navbar-links {
      display: flex;
      gap: 1rem;
    }

    .nav-link {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.75rem 1.5rem;
      border-radius: 12px;
      text-decoration: none;
      color: rgba(255, 255, 255, 0.7);
      font-weight: 600;
      transition: all 0.3s;
      border: 1px solid transparent;
    }

    .nav-link:hover {
      background: rgba(255, 255, 255, 0.1);
      color: white;
      border-color: rgba(255, 255, 255, 0.2);
    }

    .nav-link.active {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
      border-color: rgba(255, 255, 255, 0.3);
    }

    .home-link {
      background: rgba(255, 255, 255, 0.05);
    }

    .link-icon {
      font-size: 1.2rem;
    }

    @media (max-width: 768px) {
      .navbar-container {
        flex-direction: column;
        gap: 1rem;
      }

      .navbar-links {
        flex-wrap: wrap;
        justify-content: center;
        gap: 0.5rem;
      }

      .nav-link span:not(.link-icon) {
        display: none;
      }

      .brand-text {
        display: none;
      }
    }
  `]
})
export class NavbarComponent {}

