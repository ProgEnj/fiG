import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ClickStopPropagationDirective } from '../../shared/directives/click-stop-propagation.directive';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-authentication-form',
  imports: [ClickStopPropagationDirective, ReactiveFormsModule],
  templateUrl: './authentication-form.component.html',
  styleUrl: './authentication-form.component.scss'
})
export class AuthenticationFormComponent implements OnInit, OnDestroy {
  private _formBuilder = inject(FormBuilder);
  private _authenticationService = inject(AuthenticationService);
  private _router = inject(Router);
  private _location = inject(Location);
  private bodyElement = document.body;

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

  ngOnInit(): void {
    this.bodyElement.classList.add('no-scroll');
  }

  ngOnDestroy(): void {
    this.bodyElement.classList.remove('no-scroll');
  }

  authFormToggle() {
    this._location.back();
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
