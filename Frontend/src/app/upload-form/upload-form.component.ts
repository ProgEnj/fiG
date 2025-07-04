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

  uploadedFile?: File = undefined;

  isAuthFormShown: Boolean = false;

  uploadForm = this.formBuilder.group({
    name: ['', Validators.required],
    file: [File.prototype , Validators.required],
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
    let uploadFormData = this.uploadForm.value;
    let formData = new FormData();
    let tags = uploadFormData.tags!.split(' ');

    formData.append("File", this.uploadedFile!);
    formData.append("Name", uploadFormData.name!);
    formData.append("Tags", JSON.stringify(tags));
  }

  UpdatePreview(file: File) {
    this.uploadedFile = file;
    this.previewSrc = URL.createObjectURL(file);
  }

  onImagePicked(event: Event) {
    const uploadedFile = (event.target as HTMLInputElement).files![0];
    this.UpdatePreview(uploadedFile);
  }
}
