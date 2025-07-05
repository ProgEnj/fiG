import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ClickStopPropagationDirective } from '../../shared/directives/click-stop-propagation.directive';

@Component({
  selector: 'app-login-form',
  imports: [ReactiveFormsModule, ClickStopPropagationDirective],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss'
})
export class LoginFormComponent {

  private formBuilder = inject(FormBuilder);

  isLoginFormShown: Boolean = false;

  loginForm = this.formBuilder.group({
    username: ['', Validators.required],
    email: ['', Validators.compose([Validators.required, Validators.email])],
    password: ['', Validators.required],
  });

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
  }

}

