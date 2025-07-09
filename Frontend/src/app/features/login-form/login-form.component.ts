import { Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ClickStopPropagationDirective } from '../../shared/directives/click-stop-propagation.directive';
import { AuthenticationService } from '../../core/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, ClickStopPropagationDirective],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss'
})
export class LoginFormComponent implements OnInit {

  private _formBuilder = inject(FormBuilder);
  private _authService = inject(AuthenticationService);
  private _router = inject(Router);

  isLoginFormShown: Boolean = false;

  loginForm = this._formBuilder.group({
    email: ['', Validators.compose([Validators.required, Validators.email])],
    password: ['', Validators.required],
  });

  ngOnInit(): void {
    console.log(this._authService.IsLoggedIn());
  }

  loginFormToggle() {
    const bodyElement = document.body;
    if(this.isLoginFormShown) {
      bodyElement.classList.remove('no-scroll');
      this.isLoginFormShown = false;
    }
    else {
      bodyElement.classList.add('no-scroll')
      this.isLoginFormShown = true;
    }
  }

  onSubmit() {
    let formValue = this.loginForm.value;
    this._authService.LogIn(
      { Email: formValue.email!, Password: formValue.password! })
      .subscribe(res => { 
        this.loginFormToggle();
        this._authService.SetToken(res.token);
        localStorage.setItem("username", res.username);
        this._router.navigate(['/refresh']).then(() => this._router.navigate(['/']));
      });
  }

}

