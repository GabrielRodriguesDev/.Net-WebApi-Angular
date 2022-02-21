import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss'],
})
export class EventoListaComponent implements OnInit {
  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];

  public exibirImagem: boolean = true;
  private _filtroLista: string = '';

  public eventId: number;

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this._filtroLista
      ? this.filtrarEventos(this._filtroLista)
      : this.eventos;
  }
  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getEventos();
    this.spinner.show();
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        console.error(error);
        this.spinner.hide();
        this.toastr.error(error.message, 'Erro');
        console.log(this.eventos);
      },
      complete: () => this.spinner.hide(),
    });
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string }) =>
        evento.tema.toLocaleLowerCase().includes(filtrarPor) !== false ||
        evento.local.toLocaleLowerCase().includes(filtrarPor) !== false
    );
  }

  public alterarImagem() {
    this.exibirImagem = !this.exibirImagem;
  }

  //Modal
  openModal(event: any, template: TemplateRef<any>, eventId: number): void {
    event.stopPropagation(); //Parando a propagação (não trocando a rota)
    this.eventId = eventId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmDelete(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService
      .delete(this.eventId)
      .subscribe({
        next: (result: any) => {
          console.log(result);
          if (result.message === 'Deletado') {
            this.toastr.success(
              'O evento foi deleado com sucesso',
              'Deletado!'
            );
            this.getEventos();
          }
        },
        error: (error: any) => {
          console.error(error);
          this.toastr.error(
            `Erro ao tentar deletar o evento ${this.eventId}`,
            'Erro!'
          );
        },
      })
      .add(() => {
        //Ao finalizar o processamento do subscribe, independente se deu erro ou sucesso. Ele cai aqui
        // pois estamos adicionando um "ultimo recurso" a chamada antes do unsubscribe.
        this.spinner.hide();
      });
  }

  declineDelete(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

  returnImage(imagemURL: string): string {
    return (imagemURL !== '')
    ? `${environment.apiURL}resources/images/${imagemURL}`
    :'assets/img/semimagem.png'
  }
}
