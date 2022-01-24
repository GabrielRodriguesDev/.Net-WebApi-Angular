import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorFields } from '@app/helpers/validator-fields';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss'],
})
export class PerfilComponent implements OnInit {
  form: FormGroup;

  get f() {
    // Conveniente para pegar um FormField apenas com a letra F
    return this.form.controls;
  }
  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.startingInteractionWithTheForm();
  }

  startingInteractionWithTheForm(): void {
    this.form = this.formBuilder.group(
      {
        titles: ['', [Validators.required]],
        firstName: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(20),
          ],
        ],
        lastName: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(50),
          ],
        ],
        email: ['', [Validators.required, Validators.email]],
        telephone: ['', [Validators.required]],
        occupation: ['', [Validators.required]],
        description: ['', [Validators.required]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
      },
      {
        validator: ValidatorFields.mustMatch('password', 'confirmPassword'),
      }
    );
  }
  onSubmit(): void {
    // Vai parar aqui se o form estiver inválido
    if (this.form.invalid) {
      return;
    }
  }
  resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }
}
