import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss'],
})
export class EventosComponent implements OnInit {
  public eventos: any = [];
  exibirImage: boolean = true;
  private _filtroLista: string = '';
  public eventosFiltrados: any = '';

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this._filtroLista
      ? this.filtrarEventos(this._filtroLista)
      : this.eventos;
    console.log(this.eventos);
  }

  public filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string }) =>
        evento.tema.toLocaleLowerCase().includes(filtrarPor) !== false ||
        evento.local.toLocaleLowerCase().includes(filtrarPor) !== false
    );
  }
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getEventos();
  }

  public getEventos(): void {
    this.http.get('https://localhost:5001/eventos').subscribe(
      (response) => {
        this.eventos = response;
        this.eventosFiltrados = response;
      },
      (error) => console.log(error)
    );
  }
}
