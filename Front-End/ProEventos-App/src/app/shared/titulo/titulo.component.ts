import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss'],
})
export class TituloComponent implements OnInit, OnDestroy {
  @Input() titulo: string;
  @Input() subtitulo: string;
  @Input() iconClass: string = 'fa fa-sticky-note';
  @Input() botaoListar: boolean = false;
  @Input() rota: string;

  constructor(private router: Router) {}

  ngOnInit() {}

  ngOnDestroy(): void {}

  listEntitiesButton(): void {
    this.router.navigate([`/${this.rota.toLowerCase()}/lista`]);
  }
}
