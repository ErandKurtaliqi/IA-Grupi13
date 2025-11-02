import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { SocialGolfersComponent } from './components/social-golfers/social-golfers.component';
import { SudokuComponent } from './components/sudoku/sudoku.component';
import { LatinSquareComponent } from './components/latin-square/latin-square.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'social-golfers', component: SocialGolfersComponent },
  { path: 'sudoku', component: SudokuComponent },
  { path: 'latin-square', component: LatinSquareComponent }
];
