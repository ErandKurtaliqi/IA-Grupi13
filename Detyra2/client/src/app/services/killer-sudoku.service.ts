import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { KillerSudokuRequest, KillerSudokuResponse } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class KillerSudokuService {
  private apiUrl = 'https://localhost:7140/api/KillerSudoku';

  constructor(private http: HttpClient) { }

  solve(request: KillerSudokuRequest): Observable<KillerSudokuResponse> {
    return this.http.post<KillerSudokuResponse>(`${this.apiUrl}/solve`, request);
  }

  getSample(): Observable<KillerSudokuRequest> {
    return this.http.get<KillerSudokuRequest>(`${this.apiUrl}/sample`);
  }
}

