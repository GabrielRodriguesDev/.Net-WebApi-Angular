import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService, Spinner } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  form: FormGroup;
  evento = {} as Evento; //Objeto vazio do tipo evento.
  saveMode = 'post';

  get f(): any {
    return this.form.controls;
  }
  get bsConfig(): any {
    return {
      adaptivePosition: true,
      isAnimated: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      withTimepicker: true,
      showWeekNumbers: false,
    };
  }

  constructor(
    private formbuilder: FormBuilder,
    private localeService: BsLocaleService,
    private routerActivated: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.validation();
    this.loadEvent();
    this.applyLocaleDatePicker();
  }

  public validation(): void {
    this.form = this.formbuilder.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(60),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: ['', Validators.required],
    });
  }

  loadEvent(): void {
    const eventIdParam = this.routerActivated.snapshot.paramMap.get('id');

    if (eventIdParam !== null) {
      this.spinner.show();
      this.saveMode = 'put';
      this.eventoService.getEventoById(+eventIdParam).subscribe({
        // simbolo de "+" converte string em int
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
        },
        error: (error: any) => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento', 'Erro!');
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  saveChanges(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.evento =
        this.saveMode === 'post'
          ? { ...this.form.value }
          : { id: this.evento.id, ...this.form.value };
      this.eventoService[this.saveMode](this.evento).subscribe({
        next: (result: any) => {
          console.log(result);
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso!');
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar o evento.', 'Erro!');
        },
        complete: () => {
          this.spinner.hide();
          this.router.navigate(['/eventos']);
        },
      });
    }
  }

  resetForm(): void {
    this.form.reset();
  }

  validateInvalidField(formField: FormControl): any {
    return { 'is-invalid': formField.errors && formField.touched };
  }

  applyLocaleDatePicker(): void {
    this.localeService.use('pt-br');
  }
}
