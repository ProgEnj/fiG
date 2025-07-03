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

  previewSrc = '#';

  isAuthFormShown: Boolean = false;

  uploadForm = this.formBuilder.group({
    name: ['', Validators.required],
    file: ['', Validators.required],
    tags: ['', Validators.required],
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

  UpdatePreview(file: File) {
    this.previewSrc = URL.createObjectURL(file);
  }

  onImagePicked(event: Event) {
    const file = (event.target as HTMLInputElement).files![0];
    this.UpdatePreview(file);
  }
}
