import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ClickStopPropagationDirective } from '../../shared/directives/click-stop-propagation.directive';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, ClickStopPropagationDirective],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss'
})
export class LoginFormComponent implements OnInit, OnDestroy {

  private _formBuilder = inject(FormBuilder);
  private _authService = inject(AuthenticationService);
  private _router = inject(Router);
  private _location = inject(Location);
  private bodyElement = document.body;

  loginForm = this._formBuilder.group({
    email: ['', Validators.compose([Validators.required])],
    password: ['', Validators.required],
  });

  ngOnInit(): void {
    this.bodyElement.classList.add('no-scroll');

  }

  ngOnDestroy(): void {
    this.bodyElement.classList.remove('no-scroll');
  }

  loginFormToggle() {
    this._location.back();
  }

  onSubmit() {
    let formValue = this.loginForm.value;
    this._authService.LogIn(
      { Email: formValue.email!, Password: formValue.password! })
      .subscribe(res => { 
        this.loginFormToggle();
        this._authService.SetToken(res.token);
        this._authService.SetUsername(res.username);
        this._router.navigate(['/refresh']).then(() => this._router.navigate(['/']));
      });
  }

}

