import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NQueensRequest, NQueensResponse } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class NQueensService {
  private apiUrl = 'https://localhost:7140/api/NQueens';

  constructor(private http: HttpClient) { }

  solve(request: NQueensRequest): Observable<NQueensResponse> {
    return this.http.post<NQueensResponse>(`${this.apiUrl}/solve`, request);
  }

  getHeuristics(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/heuristics`);
  }
}

