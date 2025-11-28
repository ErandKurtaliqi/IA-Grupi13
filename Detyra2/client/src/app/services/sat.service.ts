import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SATRequest, SATResponse } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class SATService {
  private apiUrl = 'https://localhost:7140/api/SAT';

  constructor(private http: HttpClient) { }

  solve(request: SATRequest): Observable<SATResponse> {
    return this.http.post<SATResponse>(`${this.apiUrl}/solve`, request);
  }

  generateFormula(request: SATRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/generate-formula`, request);
  }
}

