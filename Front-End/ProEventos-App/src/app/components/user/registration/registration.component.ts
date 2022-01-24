import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  AbstractControlOptions,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ValidatorFields } from '@app/helpers/validator-fields';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
})
export class RegistrationComponent implements OnInit {
  form: FormGroup;

  get f(): any {
    console.log(this.form.controls);
    return this.form.controls;
  }

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.startingInteractionWithTheForm();
  }

  startingInteractionWithTheForm(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorFields.mustMatch('password', 'confirmPassword'),
    };
    this.form = this.formBuilder.group(
      {
        firstName: [
          '',
          [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(20),
          ],
        ],
        lastName: [
          '',
          [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(50),
          ],
        ],
        email: ['', [Validators.required, Validators.email]],
        user: [
          '',
          [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(50),
          ],
        ],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
      },
      formOptions
    );
  }
}
