import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Evento } from '../models/Evento';

@Injectable()
export class EventoService {
  baseUrl = environment.apiURL + 'eventos';
  constructor(private http: HttpClient) {}

  public getEventos(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseUrl).pipe(take(1));
  }

  public getEventosByTema(tema: string): Observable<Evento[]> {
    return this.http
      .get<Evento[]>(`${this.baseUrl}/tema/${tema}`)
      .pipe(take(1));
    //Usamos o pipe e take para permitir que só seja feito uma inscrição por subscribe,
    //Podemos configurar mais com o take({numero});
  }

  public getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento> {
    return this.http.post<Evento>(this.baseUrl, evento).pipe(take(1));
  }

  public put(evento: Evento): Observable<Evento> {
    return this.http.put<Evento>(this.baseUrl, evento).pipe(take(1));
  }

  public delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload);

    return this.http
    .post<Evento>(`${this.baseUrl}/upload-image/${eventoId}`, formData)
    .pipe(take(1));
  }
}
