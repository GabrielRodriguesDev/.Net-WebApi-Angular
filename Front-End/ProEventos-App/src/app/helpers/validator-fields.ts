import { AbstractControl, FormGroup } from '@angular/forms';

export class ValidatorFields {
  static mustMatch(controlName: string, matchingControlname: string): any {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlname];

      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
        // Se caso o formGroup já tiver erros, retorna null
        return null;
      }

      if (control.value != matchingControl.value) {
        //Validando as duas senhas não são iguais
        matchingControl.setErrors({ mustMatch: true }); // Setando o um "errors " {mustMatch}
      } else {
        matchingControl.setErrors(null); // Setando null em erros
      }

      return null; // Caso não passe em nada retorna null
    };
  }
}
