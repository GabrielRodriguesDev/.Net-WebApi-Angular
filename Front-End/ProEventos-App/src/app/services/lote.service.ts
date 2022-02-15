import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable()
export class LoteService {
  baseURL = 'https://localhost:5001/lotes';
  constructor(private http: HttpClient) {}

  public getLotesById(evenotId: number): Observable<Lote[]> {
    return this.http.get<Lote[]>(`${this.baseURL}/${evenotId}`);
  }

  public getLoteById(eventoId: number, loteId: number): Observable<Lote> {
    return this.http.get<Lote>(`${this.baseURL}/${eventoId}/${loteId}`);
  }

  public postLote(eventoId: number, lotes: Lote[]): Observable<Lote[]> {
    return this.http.post<Lote[]>(`${this.baseURL}/${eventoId}`, lotes);
  }

  public putLote(eventoId: number, lotes: Lote[]): Observable<Lote[]> {
    return this.http.post<Lote[]>(`${this.baseURL}/${eventoId}`, lotes);
  }

  public deleteLote(eventoId: number, loteId: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/${eventoId}/${loteId}`);
  }
}
