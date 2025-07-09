import { Component, inject } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ClickStopPropagationDirective } from '../../shared/directives/click-stop-propagation.directive';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication-form',
  imports: [ClickStopPropagationDirective, ReactiveFormsModule],
  templateUrl: './authentication-form.component.html',
  styleUrl: './authentication-form.component.scss'
})
export class AuthenticationFormComponent {

  private _formBuilder = inject(FormBuilder);
  private _authenticationService = inject(AuthenticationService);
  private _router = inject(Router);

  isAuthFormShown: Boolean = false;

  authForm = this._formBuilder.group({
    username: ['', Validators.required],
    email: ['', Validators.compose([Validators.required, Validators.email])],
    password: ['', Validators.compose([Validators.required, Validators.pattern(new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/))])],
    confirmPassword: ['', Validators.compose([Validators.required, Validators.pattern(new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$/))])],
  },
  {
    validators: this.confirmPasswordValidator('password', 'confirmPassword')
  });

  authFormToggle() {
    const bodyElement = document.body;
    if(this.isAuthFormShown) {
      bodyElement.classList.remove('no-scroll');
      this.isAuthFormShown = false;
    }
    else {
      bodyElement.classList.add('no-scroll')
      this.isAuthFormShown = true;
    }
  }

  onSubmit() {
    let formValue = this.authForm.value;
    this._authenticationService.Register(
      { Email: formValue.email!, Username: formValue.username!, Password: formValue.password! })
      .subscribe(res => {
        this.authFormToggle();
        this._router.navigate(['/refresh']).then(() => this._router.navigate(['/']));
      });
  }

  confirmPasswordValidator(passwordControl: string, confirmPasswordControlName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const password = control.get(passwordControl);
      const confirmPassword = control.get(confirmPasswordControlName);

      if(confirmPassword?.errors && !confirmPassword!.errors?.['confirmedPasswordValidator']) {
        return null;
      }

      if(password?.value !== confirmPassword?.value) {
        const error = { confirmedPasswordValidator: "Passwords do not match"};
        confirmPassword?.setErrors(error);
        return error;
      }
      else {
        confirmPassword?.setErrors(null);
        return null;
      }
    }
  }
}
