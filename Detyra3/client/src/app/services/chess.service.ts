import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChessAnalysisRequest, ChessAnalysisResponse } from '../models/chess.models';

@Injectable({
  providedIn: 'root'
})
export class ChessService {
  private readonly apiUrl = 'https://localhost:7140/api/chess';

  constructor(private http: HttpClient) { }

  analyzePosition(request: ChessAnalysisRequest): Observable<ChessAnalysisResponse> {
    return this.http.post<ChessAnalysisResponse>(`${this.apiUrl}/analyze`, request);
  }

  analyzeBothScenarios(request: ChessAnalysisRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/analyze-both`, request);
  }

  getDefaultPosition(): Observable<{ fen: string }> {
    return this.http.get<{ fen: string }>(`${this.apiUrl}/default-position`);
  }

  validateFEN(fen: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/validate-fen`, JSON.stringify(fen), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}

