import { Component } from '@angular/core';
import { ClickStopPropagationDirective } from '../click-stop-propagation.directive';

@Component({
  selector: 'app-authentication-form',
  imports: [ClickStopPropagationDirective],
  templateUrl: './authentication-form.component.html',
  styleUrl: './authentication-form.component.scss'
})
export class AuthenticationFormComponent {

  isAuthFormShown: Boolean = true;

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

}
