import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-upload-form',
  imports: [ReactiveFormsModule],
  templateUrl: './upload-form.component.html',
  styleUrl: './upload-form.component.scss'
})
export class UploadFormComponent {
  private formBuilder = inject(FormBuilder);

  isAuthFormShown: Boolean = false;

  uploadForm = this.formBuilder.group({
    username: ['', Validators.required],
    email: ['', Validators.compose([Validators.required, Validators.email])],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
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

  }
}
