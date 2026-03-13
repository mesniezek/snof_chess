import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChessService {
  constructor(private http: HttpClient) {}

  getBoard(): Observable<string[][]> {
    return this.http.get<string[][]>('https://localhost:7081/api/chess/board');
  }

  movePiece(fromRow: number, fromCol: number, toRow: number, toCol: number): Observable<string[][]> {
    const body = { fromRow, fromCol, toRow, toCol };
    return this.http.post<string[][]>('https://localhost:7081/api/chess/move', body);
  }
}
