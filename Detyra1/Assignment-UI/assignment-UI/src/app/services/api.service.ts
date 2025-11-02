import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7116/api'; // ASP.NET Core API (HTTP - recommended for dev)

  constructor(private http: HttpClient) { }

  solveSocialGolfersDFS(numPlayers: number, groupSize: number, numWeeks: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/socialgolfers/dfs`, {
      params: { numPlayers, groupSize, numWeeks }
    });
  }

  solveSocialGolfersDepthLimited(numPlayers: number, groupSize: number, numWeeks: number, depthLimit: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/socialgolfers/depth-limited`, {
      params: { numPlayers, groupSize, numWeeks, depthLimit }
    });
  }

  findMaxWeeks(numPlayers: number, groupSize: number, maxWeeksToTry: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/socialgolfers/find-max-weeks`, {
      params: { numPlayers, groupSize, maxWeeksToTry }
    });
  }

  // Sudoku endpoints
  solveSudoku(difficulty: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/sudoku/solve`, {
      params: { difficulty }
    });
  }

  solveCustomSudoku(grid: number[][]): Observable<any> {
    return this.http.post(`${this.apiUrl}/sudoku/solve-custom`, grid);
  }

  // Latin Square endpoints
  solveLatinSquare(n: number, algo: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/latinsquare`, {
      params: { n, algo }
    });
  }
}

