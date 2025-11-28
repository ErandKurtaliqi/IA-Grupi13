import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { NQueensComponent } from './components/nqueens/nqueens.component';
import { SATComponent } from './components/sat/sat.component';
import { KillerSudokuComponent } from './components/killer-sudoku/killer-sudoku.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'nqueens', component: NQueensComponent },
  { path: 'sat', component: SATComponent },
  { path: 'killer-sudoku', component: KillerSudokuComponent }
];
