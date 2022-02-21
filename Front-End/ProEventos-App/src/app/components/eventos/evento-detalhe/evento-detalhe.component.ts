import { Component, OnInit, TemplateRef } from '@angular/core';
import {
  AbstractControl,
  FormArray,
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
import { Lote } from '@app/models/Lote';
import { LoteService } from '@app/services/lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { DateFormatPipe } from '@app/helpers/date-format.pipe';
import { getDate } from 'ngx-bootstrap/chronos/utils/date-getters';
import { normalizeObjectUnits } from 'ngx-bootstrap/chronos/units/aliases';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
  providers: [DateFormatPipe]
})
export class EventoDetalheComponent implements OnInit {
  form: FormGroup;
  evento = {} as Evento; //Objeto vazio do tipo evento.
  saveMode = 'post';
  eventId: number;
  modalRef?: BsModalRef;
  currentBatch = {id: 0, name: '', index: 0};
  imageUrl = 'assets/svg/upload.svg';
  file: File;

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

  get bsConfigNotTime(): any {
    return{
      adaptivePosition: true,
      isAnimated: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    }
  }

  get lotes(): FormArray {
    //Get que retorna os lotes, que são um item no Formulario, e dizendo que é um FormArray
    return this.form.get('lotes') as FormArray;
  }

  constructor(
    private formbuilder: FormBuilder,
    private localeService: BsLocaleService,
    private routerActivated: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService,
    private datePipe: DateFormatPipe
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
      imagemURL: [''],
      lotes: this.formbuilder.array([]), //Adicionando mais uma matriz de formbuilder, e passando um array vazio como valor, pois esse array vai ser populado em tempo de execução
    });
  }

  saveEvent(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.evento =
        this.saveMode === 'post'
          ? { ...this.form.value }
          : { id: this.evento.id, ...this.form.value };
      this.eventoService[this.saveMode](this.evento).subscribe({
        next: (result: any) => {
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso!');
          this.router.navigate([`/eventos/detalhe/${result.id}`]);
          console.log(result);
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar o evento.', 'Erro!');
        },
        complete: (result: Evento) => {
          this.spinner.hide();
        },
      });
    }
  }


  createBatch(lote: Lote): FormGroup {
    return this.formbuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
      quantidade: [lote.quantidade, Validators.required],
    });
  }


  changingTheBatchDateValue(value: Date, index: number, field: string): void {
    this.lotes[index][field] = value;
  }

  loadEvent(): void {
    this.eventId = +this.routerActivated.snapshot.paramMap.get('id');

    if (this.eventId !== null && this.eventId !== 0) {
      this.spinner.show();
      this.saveMode = 'put';
      this.eventoService.getEventoById(this.eventId).subscribe({
        // simbolo de "+" converte string em int
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          if(this.evento.imagemURL !== '') {
            this.imageUrl = environment.apiURL + 'resources/images/' + this.evento.imagemURL;
          }
          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.createBatch(lote));
          });
        },
        error: (error: any) => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento', 'Erro!');
          this.router.navigate(['/eventos']);
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  addBatch(): void {
    this.lotes.push(this.createBatch({ id: 0 } as Lote)); // Para cada lote
  }

  removeBatch(index: number, template: TemplateRef<any>): void {

    this.currentBatch.id = this.lotes.get(index + '.id').value;
    this.currentBatch.name = this.lotes.get(index + '.nome').value;
    this.currentBatch.index = index;
    console.log(this.lotes.get(this.currentBatch.index + '.id').value);
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'})


  }

  saveBatch(): void {
    if (this.form.controls.lotes.valid) {
      this.spinner.show();
      this.loteService.postLote(this.eventId, this.form.value.lotes).subscribe({
        next: (result: Lote[]) => {
          console.log(result)
          console.log(this.form.controls.lotes.value);
          this.toastr.success('Lotes salvo com sucesso!', 'Sucesso!');
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar os lotes.', 'Erro!');
        },
        complete:() => {
          this.spinner.hide();
        }
      });
    }
  }

  loadBatch(): void {
    this.loteService.getLotesById(this.eventId).subscribe({
      next: (result: Lote[]) => {
        if(result !== null) {
          console.log(result)
          result.forEach(lote => this.lotes.push(this.createBatch(lote)) )
        }
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error('Erro ao tentar carregar os lotes', 'Erro!');
        this.spinner.hide();
      },
      complete: ()=> {
        this.spinner.hide();
      }
    })
  }

  declineDelete(): void {
    this.modalRef?.hide();
  }

  confirmDelete(): void {
    this.modalRef?.hide();
    //
    this.spinner.show();
    this.loteService.deleteLote(this.eventId, this.currentBatch.id).subscribe({
      next:(result: any) => {
        console.log(result);
        this.toastr.success('Lotes salvo com sucesso!', 'Sucesso!');
        this.lotes.removeAt(this.currentBatch.index);

      },
      error:(error: any) => {
        console.error(error);
        this.toastr.error(`Erro ao tentar excluir o lote ${this.currentBatch.id}`, 'Erro!');
        this.spinner.hide();
      },
      complete:() => {
        this.spinner.hide();
      }
    })
  }

  cancelingTheChange(): void {
    this.router.navigate(['/eventos']);
  }

  validateInvalidField(formField: FormControl | AbstractControl): any {
    return { 'is-invalid': formField.errors && formField.touched };
  }

  returnsTheBatchTitle(nome: string): string {
    return nome === null || nome === '' ? 'Nome do lote' : nome
  }


  applyLocaleDatePicker(): void {
    this.localeService.use('pt-br');
  }


  onFileChange(event: any):void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imageUrl = event.target.result;

    this.file = event.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  uploadImage(): void{
    this.spinner.show();
    this.eventoService.postUpload(this.eventId, this.file).subscribe({
      next: () =>{
        this.loadEvent();
        this.toastr.success('Imagem atualizada com sucesso', 'Sucesso!')
      },
      error: (error: any) => {
        console.error(error);
        this.spinner.hide();
        this.toastr.error('Erro ao salvar a imagem.', 'Erro!');
      },
      complete:() => {
        this.spinner.hide();
      }
    });
  }
}
